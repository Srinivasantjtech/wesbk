﻿<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="DealerRegistration"
    Title="Untitled Page"  Culture="en-US" Codebehind="DealerRegistration.aspx.cs" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">
  <script language="javascript" type="text/javascript">

    function Hidepopup() {
        $find('testTACpopup').hide();

    }

    function Showpopup() {
        $find('testTACpopup').hide();
    }
    $(function () {
        $('.input_dr').keyup(function (ev) {           
            var value = $(this).val();
            $(this).val(value.replace(/'/g, '`'));
        })
    });
</script>
    <script language="javascript" type="text/javascript">
        function checkEnableSubmit() {
            if (checkselectedlist() == true && (document.getElementById("ctl00_maincontent_chkterms").checked == true)) {
                document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                return true;
            }

            if (checkselectedlist() == false && (document.getElementById("ctl00_maincontent_chkterms").checked == true)) {
                document.getElementById("ctl00_maincontent_chkterms").checked = false;
                alert('Please complete the field ' + '\n' + 'What Describes you and/or your company the best!');
                return false;
            }

            if (checkselectedlist() == true && (document.getElementById("ctl00_maincontent_chkterms").checked == false)) {
                document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
                alert('To Submit form you must agree to the Sales Terms and Conditions');
                return false;
            }

            validateSelection();
        }


        function checkEnableOthers() {
            if ((document.getElementById("ctl00_maincontent_chkother").checked == true) || (document.getElementById("ctl00_maincontent_chkother").checked == 'true') || (document.getElementById("ctl00_maincontent_chkother").checked == 1)) {
                document.getElementById("ctl00_maincontent_txtothers").disabled = false;
            }
            else {
                document.getElementById("ctl00_maincontent_txtothers").disabled = true;
            }
        }

        function checkselectedlist() {
            if ((document.getElementById("ctl00_maincontent_chkautomotive").checked == true) || (document.getElementById("ctl00_maincontent_chkengineer").checked == true) || (document.getElementById("ctl00_maincontent_chkgovernment").checked == true) ||
                    (document.getElementById("ctl00_maincontent_Chkhobbyist").checked == true) || (document.getElementById("ctl00_maincontent_chkinstallation").checked == true) || (document.getElementById("ctl00_maincontent_chkretailer").checked == true) ||
                    (document.getElementById("ctl00_maincontent_chkschool").checked == true) || (document.getElementById("ctl00_maincontent_chkservice").checked == true) || (document.getElementById("ctl00_maincontent_chkother").checked == true)) {
                document.getElementById("ctl00_maincontent_ErrorStatusHiddenField").value = "1";
                return true;
            }
            else {
                document.getElementById("ctl00_maincontent_ErrorStatusHiddenField").value = "0";
                return false;
            }
        }

        function validateSelection() {
            if (checkselectedlist() == true) {

                if (document.getElementById("ctl00_maincontent_chkterms").checked == true) {
                    document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                    document.getElementById("ctl00_maincontent_btnsubmit").style.fontWeight = 'bold';
                    document.getElementById("ctl00_maincontent_btnsubmit").style.backgroundColor = "#ff0000";
                    //document.getElementById("ctl00_maincontent_btnsubmit").style.display = '';
                    //document.getElementById("ctl00_maincontent_btnsubmit").disabled = false;
                    document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                    return true;
                }
                else {
                    document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                    //document.getElementById("ctl00_maincontent_btnsubmit").style.display = 'none';
                    //document.getElementById("ctl00_maincontent_btnsubmit").disabled = true;
                    document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
                    return false;
                }
            }
            else {
                document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
                document.getElementById("ctl00_maincontent_chkterms").checked = false;
                //document.getElementById("ctl00_maincontent_btnsubmit").style.display = 'none';
                //document.getElementById("ctl00_maincontent_btnsubmit").disabled = true;
                return false;
            }
        }




       
    </script>
     

        <script language="javascript" type="text/javascript">
            function check(e) {
                var keynum
                var keychar
                var numcheck
                // For Internet Explorer
                if (window.event) {
                    keynum = e.keyCode
                }
                // For Netscape/Firefox/Opera
                else if (e.which) {
                    keynum = e.which
                }
             
                keychar = String.fromCharCode(keynum)
                //List of special characters you want to restrict

//                var evt = (document.all?window.event:e);
//                var keyCodeValue = (document.all?window.event.keyCode:e.which);                       

                if (keychar == "@" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")"   || keychar == "+" ) {
                    e.keyCode = '';
                    return false;
                }
//                else if (keynum == 39) {        
//                        
//                        if (e.keyCode) { //IE
//                          if (keyCodeValue == "112") {
//                            document.onhelp = function() { return (false); }
//                            window.onhelp = function() { return (false); }
//                          }
//                          e.returnValue = false;
//                          e.keyCode = 0;
//                        }else{  //not IE
//                          e.preventDefault();
//                          e.stopPropagation();
//                        } 
//                
//                    if (e.keyCode) {
//                      e.keyCode=96;
//                      return e.keyCode;
//                    }else if (e.which) {
//                      e.which=96; 
//                      return e.which;
//                    }
//                }
                else {
                    return true;
                }
            
           
}
</script>
      <script language="javascript" type="text/javascript">
          function Numbersonly(e) {
              var keynum
              var keychar
              var numcheck
              // For Internet Explorer
              if (window.event) {
                  keynum = e.keyCode
              }
              // For Netscape/Firefox/Opera
              else if (e.which) {
                  keynum = e.which
              }
              keychar = String.fromCharCode(keynum)
              //List of special characters you want to restrict
              if (keychar == "1" || keychar == "2" || keychar == "3" || keychar == "4" || keychar == "5" || keychar == "6" || keychar == "7" || keychar == "8" || keychar == "9" || keychar == "0") {
                    
                  return true;
              }
              else {
                  return false;
              }
          }
</script>
 <script language="javascript" type="text/javascript">
     function Email(e) {
         var keynum;
         var keychar;
         var numcheck;
         // For Internet Explorer
         if (window.event) {
            // var xc = window.event;
            // alert(xc);             
             keynum = e.keyCode;
         }
         

         // For Netscape/Firefox/Opera
         else if (e.which) {
            keynum = e.which;
     }

         keychar = String.fromCharCode(keynum);

         //List of special characters you want to restrict
         if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar=="{" || keychar=="}" || keychar=="[" || keychar=="]" ||  keychar=="|" || keychar=="'" || keychar=="/" ) {
            // alert(keychar);
            e.keyCode = '';
             return false;
         }
         else {
             return true;
             
         }
     }
</script>
    <asp:Panel ID="pnlnewProfile" runat="server" DefaultButton="btnsubmit">
      <table width="790px" border="0" cellpadding="0" cellspacing="0"> 
  <tr>
  <td align="left" style="text-align:left;margin-left:0px;"    >
      
     <%--   <div class="container">--%>
         <%--<div class="container12" style="width: 754px;">--%>
           <div class="span9 box1" style="width: 760px;margin-left:0px;">
               <h3 class="title1" align="left">New Customer Sign Up</h3>
               <div class="img_left">
                    <img width="106" height="80" alt="img" src="images/page_icon1.png">
               </div>
               <div class="box4">
                <p class="p3">New Customer sign form. Please enter all the details below and submit form to us for processing.</p>
                <p class="p3">After you have submitted the sign up form our Sales Team willl check and verify your details are correct. </p>
                <p class="p3">Once approved you will receive a confirmation email from us with further information.</p>
                <p class="p3">Questions? Contact us between the hours of 8:30 a.m. and 5:00 p.m. Monday - Friday<br>
               
                Email:
                <a href="mailto:sales@wes.net.au">sales@wes.net.au</a>
                or Phone: +61 2 9797-9866
                <br>
                <br>
                Please Fill Form in Below
                </p>
               </div> 
                 <table cellpadding="0" cellspacing="0" border="0" width="588px" valign="top">
            <%--<tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                        Width="1px"></asp:Label>
                    &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields"
                        Class="lblNormalSkin"></asp:Label>
                </td>
            </tr>--%>
          <%--  <tr>
                <td valign="top" align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                New Customer Sign Up
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                Thank you for your interest in WES<br />
                                <br />
                                WES is a wholesale distributor of electronic parts and accessories.<br />
                                Please note we are a business to business supplier including government departments,
                                schools and
                                <br />
                                learning institutions.Consumer sales available through Wagner Electronics - <a href="http://www.wagner.net.au"
                                    target="_blank" style="text-decoration: none; color: #0099FF;">www.wagner.net.au</a><br />
                                <br />
                                To establish an account with WES please fill in the details below and our Sales
                                Team will<br />
                                review your details, once approved you will receive a confirmation email from us<br />
                                with further information.<br />
                                <br />
                                <span style="color: #0099FF;"><i>Questions?</i></span> Contact us between the hours
                                of 8:30 a.m. and 5:00 p.m. Monday - Friday<br />
                                <br />
                                Email: <a href="mailto:sales@wes.net.au" class="tx_3">sales@wes.net.au</a><br />
                                Phone: +61 2 9797-9866<br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <%--<tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>--%>
            <tr>
                <td align="left">
                <div class="box1">
                  <h3 class="title3">Company Details</h3>
                    <table cellspacing="0" width="743px" cellpadding="0" >
                       
                       <%-- <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Company Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>--%>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                                 <%--<tr>
                          <td  width="200px">
                              
                            </td>
                             <td width="250px" >
                             
                            </td>
                            <td align="right" width="235px" >
             
                                <asp:Label ID="Label18" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Visible="true" Text="Required Fields" CssClass="red"
                                   ></asp:Label>
                    
                            </td>
                        </tr>--%>
                                    <tr>
                                        <td width="170px" align="left" >
                                        <span class="form_1">
                                            Company / Account Name:<asp:Label ID="Label1" runat="server" Class="lblRequiredSkin"
                                                Visible="true" Text="*" Width="1px"></asp:Label></span>
                                        </td>
                                        <td width="250px" align="left" ><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtcompname" Text=""   CssClass="input_dr"  
                                                MaxLength="50"></asp:TextBox></span>
                                        </td>
                                     
                                           <%-- <tr>--%>
                                                <td align="left" width="150px">
                                                    &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvcompname" runat="server" 
                                                        ControlToValidate="txtcompname" Display="Dynamic" ErrorMessage="Required" 
                                                        Class="vldRequiredSkin" Text="Enter Company Name" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" width="100px">
                                                    <asp:Label ID="Label18" runat="server" Class="lblRequiredSkin" Text="*" 
                                                        Visible="true" Width="1px"></asp:Label>
                                                    &nbsp;<asp:Label ID="Label19" runat="server" CssClass="red" Text="Required Fields" 
                                                        Visible="true"></asp:Label>
                                                </td>
                                           <%-- </tr>--%>
                                     
                                    </tr>
                                    
                                    <tr>
                                        <td align="left"><span class="form_1">
                                            ABN / ACN / Company Number:<asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label></span>
                                        </td>
                                        <td align="left"><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtcompno" Text="" CssClass="input_dr" 
                                                MaxLength="20"></asp:TextBox></span>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;
                                           </td>
                                    </tr>
                                     <tr>
                                        <td align="left"><span class="form_1">
                                            Company WebSite:<asp:Label ID="Label20" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label></span>
                                        </td>
                                        <td align="left"><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtCompanyWebSite" Text="" CssClass="input_dr"  
                                                MaxLength="100"></asp:TextBox></span>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;
                                              <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCompanyWebSite"
                                                ErrorMessage="Required" Text="Enter Valid Website" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                           </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div> 
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>

            <tr>
                <td align="left">
                 <div class="box1">
                 <h3 class="title3">Contact Details</h3>
                    <table cellspacing="0" width="680px" cellpadding="0" >
                        
                        <%--<tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Contact Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>--%>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <%--<tr>
                          <td  width="118px">
                              
                            </td>
                             <td width="235px" >
                             
                            </td>
                            <td align="right" width="235px">
             
                                <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                                &nbsp;<asp:Label ID="Label17" runat="server" Visible="true" Text="Required Fields"
                                    CssClass="red"></asp:Label>
                    
                            </td>
                        </tr>--%>
                                    <tr>
                                        <td width="180px"><span class="form_1">
                                            First Name<asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td width="250px"><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtfname" Text="" CssClass="input_dr" 
                                                MaxLength="30" ></asp:TextBox></span> 
                                        </td>
                                        <td align="left" width="150px">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvfname" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"
                                                ControlToValidate="txtfname"></asp:RequiredFieldValidator>
                                        </td>
                                         <td align="right" width="100px">
             
                                <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                                &nbsp;<asp:Label ID="Label17" runat="server" Visible="true" Text="Required Fields"
                                    CssClass="red"></asp:Label>
                    
                            </td>
                                    </tr>
                                  <%--  <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Last Name:<asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtlname" Text=""  CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvlname" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"
                                                ControlToValidate="txtlname"></asp:RequiredFieldValidator>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Postion:<asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtposition" Text="" CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="right">
                                            &nbsp;&nbsp;</td>
                                            <td></td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Phone:<asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtphone" Text="" CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"
                                                ControlToValidate="txtphone"></asp:RequiredFieldValidator>
                                            &nbsp;&nbsp;
                                            <asp:FilteredTextBoxExtender ID="ftePhone" runat="server" FilterMode="ValidChars"
                                                FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphone" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Mobile/Cell Phone:</span> 
                                           
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtMobile" Text="" CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars"
                                                FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />
                                        </td>
                                        <td></td>
                                    </tr>
                                   <%-- <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Fax:<asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtfax" Text="" CssClass="input_dr"  MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                                                ValidChars="1234567890" TargetControlID="txtfax" />
                                        </td>
                                        <td></td>
                                    </tr>
                                 <%--   <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Email:<asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtemail" Text="" CssClass="input_dr" 
                                                MaxLength="55"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                ControlToValidate="txtemail"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"
                                                ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                        </td>
                                        <td></td>
                                    </tr>
                                   <%-- <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Confirm Email:<asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtcemail" Text="" CssClass="input_dr" 
                                                MaxLength="55"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email"
                                                ControlToValidate="txtcemail"></asp:RequiredFieldValidator>
                                           
                                            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtcemail" ControlToCompare="txtemail"
                                                runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic"></asp:CompareValidator>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div> 
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                 <div class="box1">
                 <h3 class="title3">Address Details</h3>
                    <table cellspacing="0" width="745px" cellpadding="0" >
                       
                       <%-- <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Address Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>--%>
                        <%--<tr>
                <td align="right">
             
                    <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                        Width="1px"></asp:Label>
                    &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields"
                        Class="lblNormalSkin"></asp:Label>
                    
                </td>
            </tr>--%>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <%--<tr>
                          <td  width="118px">
                              
                            </td>
                             <td width="235px" >
                             
                            </td>
                            <td align="right" width="235px">
             
                                <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                                &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields"
                                    CssClass="red"></asp:Label>
                    
                            </td>
                        </tr>--%>
                                    <tr>
                                       
                                        <td width="170px"><span class="form_1">
                                            Street Address:<asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td width="250px"><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtsadd" Text="" CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span>
                                        </td>
                                        <td align="left" width="150px">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"
                                                ControlToValidate="txtsadd"></asp:RequiredFieldValidator>
                                        </td>
                                          <td width="100px" align="right" >
                                           <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                                &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields" CssClass="red"
                                   ></asp:Label>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Address Line 2:</span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtadd2" Text=""  CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td>
                                        </td>
                                         <td >
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Suburb/Town<asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txttown" Text=""  CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Suburb/Town"
                                                ControlToValidate="txttown"></asp:RequiredFieldValidator>
                                        </td>
                                         <td>
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            State/Province:<asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtstate" Text="" CssClass="input_dr" 
                                                MaxLength="30"></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province"
                                                ControlToValidate="txtstate"></asp:RequiredFieldValidator>
                                        </td>
                                         <td>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Post/Zip Code:<asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label></span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox runat="server" ID="txtzip" Text="" CssClass="input_dr" MaxLength="10"  ></asp:TextBox></span> 
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Post/Zip Code"
                                                ControlToValidate="txtzip"></asp:RequiredFieldValidator>
                                            &nbsp;&nbsp;
                                            <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtzip" />
                                        </td>
                                         <td>
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td><span class="form_1">
                                            Country:</span> 
                                        </td>
                                        <td><span class="form_2">
                                            <asp:DropDownList ID="drpCountry" runat="server"  Width="250px" Class="DropdownlistSkin"  >
                                            </asp:DropDownList></span> 
                                          
                                        </td>
                                        <td>
                                        </td>
                                         <td>
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </div> 
                </td>
            </tr>
          <%--  <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>--%>
            
            <tr>
                <td align="left">
                <div class="box1">
                <h3 class="title3">Business Type</h3>
                 <div class="form_3">
                    Please select options below for what best fits your organisation:
                    <span class="red"> * </span>
                 </div>
                    
                    <table cellspacing="0" width="680px" cellpadding="0" >
                        
                       <%-- <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                What Describes you and/or your company the best:
                                <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>--%>
                        <tr>
                           <%-- <td width="2%">
                                &nbsp;
                            </td>--%>
                            <td  align="left">
                           <%-- <div class="form_4">--%>
                                <table cellpadding="0" cellspacing="0" border="0" width="660px">
                                  
                                    <tr>
                                        <td>
                                      
                                            <table cellpadding="0" cellspacing="4" border="0" width="660px">
                                                <tr>
                                                    <td align="left" width="100px" style="font-size: 12px;" >
                                                    <label>
                                                        <asp:CheckBox ID="chkservice" Text="Service/Repair" runat="server"  />
                                                    </label>
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                      <label>
                                                        <asp:CheckBox ID="chkretailer" Text="Retailer" runat="server"  />
                                                          </label>
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                      <label>
                                                        <asp:CheckBox ID="chkengineer" Text="Engineer" runat="server" 
                                                            Checked="false" />
                                                              </label>
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                      <label>
                                                        <asp:CheckBox ID="Chkhobbyist" Text="Hobbyist" runat="server" 
                                                            Checked="false" />
                                                    </label>
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                      <label>
                                                        <asp:CheckBox ID="chkinstallation" Text="Installation Audio/Visual" runat="server"
                                                            Checked="false" />
                                                              </label>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="4" border="0" width="660px">
                                                <tr>
                                                    <td align="left" style="font-size: 12px;" >
                                                        <asp:CheckBox ID="chkautomotive" Text="Installation Automotive" runat="server"  />
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                        <asp:CheckBox ID="chkgovernment" Text="Government Department" runat="server"  />
                                                    </td>
                                                    <td align="left" style="font-size: 12px;" >
                                                        <asp:CheckBox ID="chkschool" Text="School/University/Technical College" runat="server"
                                                             Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="4" border="0" width="660px">
                                                <tr>
                                                    <td align="left" style="font-size: 12px;" >
                                                       
                                                        <input id="chkother" runat="server" type="checkbox" onchange="javascript:checkEnableOthers()" />
                                                        Other, Please Specify
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtothers" Text=""  class="input_dr" Width="300px" 
                                                            MaxLength="50" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            <%--</div> --%>
                            </td>
                        </tr>
                    </table>
                 
                    </div> 
                </td>
            </tr>
          <%--  <tr>
                <td align="center">
                
                    <table cellpadding="2" cellspacing="3" border="0">
                        <tr>
                            <td align="center">
                                <input id="chkterms" type="checkbox" runat="server" onclick="javascript:checkEnableSubmit();" />
                            </td>
                            <td class="tx_1">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse">
                                    <tr>
                                        <td>
                                           
                                                <div style="line-height: 14px;color:#333">
                                                <span class="txtnorm">
                                                 I have read and understand
                                                 <asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;" ToolTip="Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server"  />
                                                </span>
                                                </div> 
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
               
                </td>
            </tr>--%>
            <tr>
                <td align="left">
                 <div class="box1">
                  <h3 class="title3">Submit Form</h3>
                    <table cellpadding="10px" cellspacing="0" border="0">
                       
                        <tr>
                            <td class="tx_1" colspan="2" style="font-size: 12px;" >
                                Form Verify. Please enter text code shown below:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low"
                                    runat="server" CustomValidatorErrorMessage="Invalid Verification Code" CaptchaMaxTimeout="300" CaptchaMinTimeout="2" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="font-size: 12px;" >
                                            Text Code <span style="color: Red">*</span> &nbsp;
                                            <asp:TextBox ID="cText"  runat="server" value=""></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="cVerifyMsg" runat="server" ForeColor="Red" Text="" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>--%>
                          <tr>
                <td align="left">
                
                    <table cellpadding="2" cellspacing="3" border="0">
                        <tr>
                            <td align="center">
                                <input id="chkterms" type="checkbox" runat="server" onclick="javascript:checkEnableSubmit();" />
                            </td>
                            <td class="tx_1">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse">
                                    <tr>
                                        <td>
                                           
                                                <div style="line-height: 14px;color:#333">
                                                <span class="txtnorm">
                                                 I have read and understand
                                                 <asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;" ToolTip="Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server"  />
                                                </span>
                                                </div> 
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
               
                </td>
            </tr>
                    </table>
                    </div> 
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnsubmit" class="button normalsiz btngreen btnmain" Text="Submit" runat="server" ValidationGroup="Mandatory"
                                    OnClick="btnsubmit_Click" OnClientClick="return checkEnableSubmit()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />
           </div> 
           <%--<div class="span3 box2">

           </div>--%> 
       <%--  </div> --%>
       <%-- </div> --%>
      <%--  <table cellpadding="0" cellspacing="0" border="0" width="588px" valign="top">
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                        Width="1px"></asp:Label>
                    &nbsp;<asp:Label ID="Label4" runat="server" Visible="true" Text="Required Fields"
                        Class="lblNormalSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                New Customer Sign Up
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                Thank you for your interest in WES<br />
                                <br />
                                WES is a wholesale distributor of electronic parts and accessories.<br />
                                Please note we are a business to business supplier including government departments,
                                schools and
                                <br />
                                learning institutions.Consumer sales available through Wagner Electronics - <a href="http://www.wagner.net.au"
                                    target="_blank" style="text-decoration: none; color: #0099FF;">www.wagner.net.au</a><br />
                                <br />
                                To establish an account with WES please fill in the details below and our Sales
                                Team will<br />
                                review your details, once approved you will receive a confirmation email from us<br />
                                with further information.<br />
                                <br />
                                <span style="color: #0099FF;"><i>Questions?</i></span> Contact us between the hours
                                of 8:30 a.m. and 5:00 p.m. Monday - Friday<br />
                                <br />
                                Email: <a href="mailto:sales@wes.net.au" class="tx_3">sales@wes.net.au</a><br />
                                Phone: +61 2 9797-9866<br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Company Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                
                                    <tr>
                                        <td width="118px" align="left">
                                            Company / Account Name<asp:Label ID="Label1" runat="server" Class="lblRequiredSkin"
                                                Visible="true" Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td width="235px" align="left">
                                            <asp:TextBox runat="server" ID="txtcompname" Text="" Class="textSkin" Width="200px"
                                                MaxLength="40"></asp:TextBox>
                                        </td>
                                        <td align="left" width="235px">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvcompname" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Company Name"
                                                ControlToValidate="txtcompname"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            ABN / ACN /<br />
                                            Company Number<asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" ID="txtcompno" Text="" Class="textSkin" Width="200px"
                                                MaxLength="11"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;
                                           </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Address Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="118px">
                                            Street Address<asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td width="235px">
                                            <asp:TextBox runat="server" ID="txtsadd" Text="" Class="textSkin" Width="200px"
                                                MaxLength="30"></asp:TextBox><br />
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Street Address"
                                                ControlToValidate="txtsadd"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address Line 2
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtadd2" Text="" Class="textSkin" Width="200px"
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Suburb/Town<asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txttown" Text="" Class="textSkin" Width="200px"
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Suburb/Town"
                                                ControlToValidate="txttown"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            State/Province<asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtstate" Text="" Class="textSkin" Width="200px"
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter State/Province"
                                                ControlToValidate="txtstate"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Post/Zip Code<asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtzip" Text="" Class="textSkin" Width="100px" MaxLength="10" ></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvzip" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Post/Zip Code"
                                                ControlToValidate="txtzip"></asp:RequiredFieldValidator>
                                            &nbsp;&nbsp;
                                            <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtzip" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Country
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpCountry" runat="server" Width="215px" Class="DropdownlistSkin">
                                            </asp:DropDownList>
                                          
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                Contact Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="118px">
                                            First Name<asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td width="235px">
                                            <asp:TextBox runat="server" ID="txtfname" Text="" Class="textSkin" Width="200px"
                                                MaxLength="20" ></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvfname" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter First Name"
                                                ControlToValidate="txtfname"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Last Name<asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtlname" Text="" Class="textSkin" Width="200px"
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvlname" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Last Name"
                                                ControlToValidate="txtlname"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Postion<asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtposition" Text="" Class="textSkin" Width="200px"
                                                MaxLength="40"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Phone<asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtphone" Text="" Class="textSkin" Width="200px"
                                                MaxLength="16"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvphone" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Phone"
                                                ControlToValidate="txtphone"></asp:RequiredFieldValidator>
                                            &nbsp;&nbsp;
                                            <asp:FilteredTextBoxExtender ID="ftePhone" runat="server" FilterMode="ValidChars"
                                                FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphone" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mobile/Cell Phone
                                           
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMobile" Text="" Class="textSkin" Width="200px"
                                                MaxLength="16"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars"
                                                FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobile" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fax<asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtfax" Text="" Class="textSkin" Width="200px" MaxLength="16"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:FilteredTextBoxExtender ID="ftefax" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                                                ValidChars="1234567890" TargetControlID="txtfax" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Email<asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtemail" Text="" Class="textSkin" Width="200px" 
                                                MaxLength="55"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                ControlToValidate="txtemail"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtemail"
                                                ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Confirm Email<asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtcemail" Text="" Class="textSkin" Width="200px"
                                                MaxLength="55"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvcemail" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Confirm Email"
                                                ControlToValidate="txtcemail"></asp:RequiredFieldValidator>
                                           
                                            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtcemail" ControlToCompare="txtemail"
                                                runat="server" ErrorMessage="Confirm Email and Email should be same" ValidationGroup="Mandatory" Display="Dynamic"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="textdummy">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="textdummy">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table cellspacing="0" width="588px" cellpadding="0" style="background-color: #F2F2F2;
                        border: thin solid #E7E7E7">
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                What Describes you and/or your company the best:
                                <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                    Width="1px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tx_7_blue" align="left" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;
                            </td>
                            <td class="tx_1" align="left">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="4" border="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkservice" Text="Service/Repair" runat="server" Class="CheckBoxSkin1" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkretailer" Text="Retailer" runat="server" Class="CheckBoxSkin1" />
                                                    </td>
                                                    <td align="left" style="margin-left: 40px">
                                                        <asp:CheckBox ID="chkengineer" Text="Engineer" runat="server" Class="CheckBoxSkin1"
                                                            Checked="false" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="Chkhobbyist" Text="Hobbyist" runat="server" Class="CheckBoxSkin1"
                                                            Checked="false" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkinstallation" Text="Installation Audio/Visual" runat="server"
                                                            Class="CheckBoxSkin1" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="4" border="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkautomotive" Text="Installation Automotive" runat="server" Class="CheckBoxSkin1" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkgovernment" Text="Government Department" runat="server" Class="CheckBoxSkin1" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkschool" Text="School/University/Technical College" runat="server"
                                                            Class="CheckBoxSkin1" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="4" border="0">
                                                <tr>
                                                    <td align="left">
                                                       
                                                        <input id="chkother" runat="server" type="checkbox" onchange="javascript:checkEnableOthers()" />
                                                        Other, Please Specify
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" ID="txtothers" Text="" Class="textSkin" Width="300px"
                                                            MaxLength="50" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table cellpadding="2" cellspacing="3" border="0">
                        <tr>
                            <td align="center">
                                <input id="chkterms" type="checkbox" runat="server" onclick="javascript:checkEnableSubmit();" />
                            </td>
                            <td class="tx_1">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse">
                                    <tr>
                                        <td>
                                           
                                                <div style="line-height: 14px;color:#333">
                                                <span class="txtnorm">
                                                 I have read and understand
                                                 <asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;" ToolTip="Sales Terms and Conditions" Text="Sales Terms and Conditions" runat="server"  />
                                                </span>
                                                </div> 
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="lblCheckTerms" runat="server" ForeColor="Red" Text="*" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="tx_1" colspan="2">
                                Form Verify. Please enter text code shown below:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low"
                                    runat="server" CustomValidatorErrorMessage="Invalid Verification Code" CaptchaMaxTimeout="300" CaptchaMinTimeout="2" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            Text Code <span style="color: Red">*</span> &nbsp;
                                            <asp:TextBox ID="cText" runat="server" value=""></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Label ID="cVerifyMsg" runat="server" ForeColor="Red" Text="" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnsubmit" class="form_boton" Text="Submit" runat="server" ValidationGroup="Mandatory"
                                    OnClick="btnsubmit_Click" OnClientClick="return checkEnableSubmit()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="ErrorStatusHiddenField" runat="server" />--%>
        </td> 
        </tr> 
        </table> 
    </asp:Panel>


<asp:Panel ID="pnlTAC" runat="server" Width="650px"  Style="display:none; "    >
<a href = "javascript:Hidepopup()" class="testbutton" ></a>
<div class="boxfull" style="width:575px;height:500px;overflow:scroll;">
<table width="96%" border="0" cellpadding="0" cellspacing="0">

  <%--<tr>
    <td height="30" align="left" class="tx_7"  valign="top">TERMS AND CONDITIONS</td>
  </tr>--%>
 <%-- <tr>
    <td align="center" valign="top">&nbsp;</td>
  </tr>--%>
  <tr>
    <td align="center" valign="top">
      <table width="558" border="0" cellspacing="0" cellpadding="0">
      <%-- <tr>
          <td height="30"  background="images/17.gif">
             <table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
                 <tr>
                <td height="30" align="left" class="tx_6"  valign="top">TERMS AND CONDITIONS</td>
                </tr>
    
             </table>
          </td>
       </tr>--%>

        <tr>
          <td height="30" background="images/17.gif">
            <table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
                
              <tr>
                <td height="30" class="tx_6" align="left">TERMS AND CONDITIONS
                </td>
                <td align="right">
                  <img src="images/ico_11.gif" width="14" height="17"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td class="back_table_center3" valign="top">
            <table width="540" border="0" align="center" cellpadding="10" cellspacing="0" class="tx_1" >
              <tr>
                <td valign="top" align="left">
                  <p>
                    <strong>
                      <br/>
                      WES AUSTRALASIA:
                    </strong>                    
                     &nbsp;Is an independent Australian organisation dedicated to the Electronics Industry. Our aim is to give you the BEST possible service at COMPETITIVE prices.
                    <br/>
                    <br/>
                            </p>
                  <p>
                    <strong>THE PRODUCTS:</strong>

                    &nbsp;Goods offered are from reliable and internationally known manufacturers and are of the highest quality. Data and specifications are those quoted by our suppliers which we believe to be correct.
                  <br/>
                 <%-- <br/>--%>

                </p>
                  <p>
                    <strong>
                      <br/>
                      PRICES:
                    </strong>
                        &nbsp;This price list supersedes all previous price lists and although correct at the time of printing, prices are subject to change without notice. All prices INCLUDE GST.
                        <br/>
                      <%--  <br/>--%>
                </p>
                  <p>
                    <strong>
                      <br/>
                      DELIVERY:
                    </strong>
                     &nbsp;A delivery and handling charge applies to all orders. Please note that goods are sent at the purchasers risk. Minimum surface mail charge is &#36;6.90. Parcels over 500g attract parcel rates and in most cases the mail charges are more than courier. Therefore, for heavier or urgent orders we recommend that the courier service be used. In most cases they deliver on the next working day, thus courier can be both convenient and cost effective.


                    <br/>
                     <%-- <br/>--%>
                </p>

                  <p>
                    <strong>
                      <br/>
                      PICKUP:
                    </strong>
                    &nbsp;Wagner Electronics is open 8:30 to 5.00 Mon-Fri, 9:00 to 4:00 on Sat.
                    <br/>
                    <%--<br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      LOCAL:
                    </strong>
                    &nbsp;(Sydney Metropolitan Road Service) &#36;6.90. This is an overnight service.
                    <br/>
                    <%--<br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      LOCAL SAME DAY:
                    </strong>
                    &nbsp;(Sydney Metro') &#36;8.50. Orders must be received before 10AM. Delivery is between 2PM and 5.30PM. Please state clearly on your order "SAME DAY DELIVERY".
                    <br/>
                 <%--   <br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      COUNTRY and INTERSTATE:
                    </strong>
                    &nbsp;Air service is &#36;9.90 up to 3Kg.
                    <br/>
                   <%-- <br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      HEAVY PARCELS:
                    </strong>
                    &nbsp;(Above 3Kg) will be sent by road freight at no extra charge. Please allow 2-3 working days for delivery. Some country areas may take up to 7 days.
                    <br/>
                    <%--<br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      DANGEROUS GOODS:
                    </strong>
                    &nbsp; Orders with aerosols or isopropanol products must be sent by road.
                    <br/>
                   <%-- <br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      BACK ORDERS:
                    </strong>
                    &nbsp; Goods not in stock will be automatically back ordered unless otherwise requested.
                    <br/>
                    <br/>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      CLAIMS:
                    </strong>
                    &nbsp; The use of our products is totally beyond our control or supervision. We therefore cannot accept any responsibility for losses or consequential damage to any goods or equipment. A general Three month warranty applies to most products from date of invoice. Manufacturers, however, may vary this period at their discretion. PLEASE NOTE. No warranty applies to some parts as faults not correctly diagnosed or other faults in associated circuitry can damage newly fitted parts, e.g. semiconductors, etc.
                    <br/>
                  <%--  <br/>--%>
                  </p>
                  <p>
                    <strong>
                      <br/>
                      GOODS RETURNED:
                    </strong>
                    &nbsp; Please, it is essential you obtain authorization before returning any goods. All goods to be returned must be freight prepaid and accompanied with a copy of the original purchase invoice. Faulty goods will be replaced only. Not credited. Goods purchased incorrectly or no longer required will not be accepted for credit.
                    <br/>
                   <%-- <br/>--%>
                  </p> 
                  <p>
                    <strong>
                      <br/>
                      OWNERSHIP OF GOODS:
                    </strong>
                    &nbsp; Ownership of all goods and materials supplied by WES Components remains the property of WES Components, in accordance with the retention of title clause, until all goods supplied are paid for by the customer.
                    <br/>
                   <%-- <br/>--%>
                  </p> 
                  <p>
                    <strong>
                      <br/>
                      ERRORS/OMISSIONS:
                    </strong>
                    &nbsp; Due to the size of this catalogue and the thousands of products it contains, it is possible that mistakes exist. All care has been taken to eliminate errors, but no responsibility can be accepted. Therefore, we recommend that this catalogue be used as a guide only. All prices are correct at the time of printing, but may change without notice. Please tell us if you discover any discrepancies. We will advise you of corrections as they become available. See WESNEWS
                    <br/>
                    <br/>
                  </p>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>     
    </td>
  </tr>
</table>
</div> 
</asp:Panel>
<asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"
    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None"   >
</asp:ModalPopupExtender>

</asp:Content>