﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_login" EnableTheming="true" Codebehind="login.ascx.cs"  %>
<%@ Register Src="~/UC/newproductsnav.ascx" TagName="newproductsnav" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<input id="hidpwd" name="hidpwd" type="hidden" runat="server" />
<script type="text/javascript">
    function txtpassword_onFocus() {
        if (document.forms[0].elements["<%=hidpwd.ClientID%>"] != null) {
            if (document.forms[0].elements["<%=hidpwd.ClientID%>"].value.length != 0) {
             //   document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
            }
        }
    }
    function passwordcheck() {
        if (document.forms[0].elements["<%=txtUserName.ClientID%>"].value.length != 0) {
            if (document.forms[0].elements["<%=txtPassword.ClientID%>"] != null) {
                if (document.forms[0].elements["<%=txtPassword.ClientID%>"].value.length == 0) {
                   // document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
                    //  alert("Enter the password");
                    //return false;
                }
            }
            else {
                document.forms[0].elements["<%=txtPassword.ClientID%>"].value = document.forms[0].elements["<%=hidpwd.ClientID%>"].value;
                return false;
            }
        }
    }


    function ForgotLinkPage() {
        var mUser = document.getElementById("<%=txtUserName.ClientID%>");
        window.location.href = "ForgotPassword.aspx?loginName=" + mUser.value;
    }
    function ForgotLinkPageUserID() {
        window.location.href = "ForgotUserName.aspx";
    }

//    function Restrictpasslenth() {
//        var text = document.forms[0].elements["<%=txtPassword.ClientID%>"].value;
//        if (text.length >= 15) {
//            alert('Password Length should not be greater than 15.');
//        }       
    //    }


</script>
 <script type="text/javascript" >
      function keyboardActions(event) {
         if (event.keyCode == 13) {

             eval($("#<%=cmdLogin.ClientID %>").trigger('click'));
             return false;
         }

     }
     $(document).ready(function () {
         if ($.browser.mozilla) {
             $("#<%=txtUserName.ClientID %>").keypress(keyboardActions);
             $("#<%=txtPassword.ClientID %>").keypress(keyboardActions);
             $("#<%=chkKeepme.ClientID %>").keypress(keyboardActions);
         } else {
             $("#<%=txtUserName.ClientID %>").keydown(keyboardActions);
             $("#<%=txtPassword.ClientID %>").keydown(keyboardActions);
             $("#<%=chkKeepme.ClientID %>").keydown(keyboardActions);
         }

     });
  </script>


<asp:UpdatePanel ID="updpnllogin" runat="server" UpdateMode="Conditional">
    <contenttemplate>
              <table width="96%" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td>
                    <div align="center">
                    <asp:Label ID="RegSucess" Visible="false" runat="server" Text ="<%$ Resources:login,lbRegSucess%>" Class="lblResultSkin"></asp:Label>
                    </div>
                    <%
                        string strActionType ="";
                        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
                        if (!string.IsNullOrEmpty(Request.QueryString["ActionType"]))
                            strActionType = objSecurity.StringDeCrypt(Request.QueryString["ActionType"].ToString());
                        if (!string.IsNullOrEmpty(Request.QueryString["ActionType"])  && strActionType == "RequireLogin")
                       {
                     %>
                    <div class="alert" style="background-color:#FFD52B;height:50px;margin-left: 5px;margin-right: 6px;">
                     <div class="img_left" style="margin-left: 40px;margin-top: -2px;"><img  alt="img" src="images/info2.png"/></div>
                       <p style="text-align:left;margin-left: 145px;font: italic bold 13pt arial;"> Please Login To Access WES Online With Prices - Login / Register Below </p> 
                    </div>
                    <a  href="OnlineCatalogue.aspx">
                       <div class="catelogueicon" style="background-color:#62b21d;height:67px;margin-left: 5px;margin-right: 6px;">
                      <%-- <div class="img_left" style="margin-left: -135px;margin-top: 5px;"><img width="72"  height="57" alt="img" src="images/catalogue-icon.png"/></div>--%>
                        <p style="text-align:left;margin-left:-20px;margin-top: 26px;font: italic bold 13pt arial;color:#FFFFFF;">  View Our Online Catalogue (No Price Edition) Click Here </p> 
                       </div>
                    </a>
                        <%
                        }                            
                        %>
                    </td>
                  </tr>
                  <tr>
                    <td>
                <div class="container_cmn" style="height:376px;">
                      <div class="container12" >
                          <%
                              
                             // TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();                                                               
                              if (!string.IsNullOrEmpty(Request.QueryString["ActionType"]) && strActionType == "RequireLogin")
                              {
                         %>
                            <div class="span8 box1" style="margin-left:6px;">
                          <h3 class="title1" align="left">Create An Account</h3>
                           <div class="img_left"><img width="106" height="80" alt="img" src="images/page_icon1.png"/></div>
                          <div class="box4">
          <p class="p2">WES is a wholesale distributor of electronic parts and accessories. Please note we are a business to business supplier including government departments, schools and learning institutions.</p>
        <p class="p2">To establish an account with WES you will be required fill in an account application form to us that will be reviewed by our Sales Team. Once approved you will receive a confirmation email from us with further information.</p>
        <p class="p2">Questions? Contact us between the hours of 8:30 a.m. and 5:00 p.m. Monday - Friday<br>
          Email: <a href="mailto:sales@wes.net.au">sales@wes.net.au</a> or Phone: +61 2 9797-9866<br><br>
          Please select an option below.</p>           
        <p class="p2"><br>
        <a style="margin:0 20px 0 0;" class="buttonreg bigsiz btnbluebig fleft alainc" href="DealerRegistration.aspx"><strong>NEW CUSTOMER</strong><span >NEW customers who have never<br> puchased from us before</span></a>
        <a style="margin:0 20px 0 0;" class="buttonreg bigsiz btnbluebig fleft alainc" href="ExistCustomerRegistration.aspx" ><strong>EXISTING CUSTOMER</strong><span >If you already have an account<br> and would like online access</span></a>
     
        </p>
      </div>
                          </div>

                           <%
                              }
                             %>

                             <%
                              else
                              {
                                 %>
                                 <div style="margin-left:7px;margin-bottom:6px;margin-top:-1px;">
                                 <a href="http://www.wes.com.au/mediapub/ebook/casioNP/" target="_blank">
                                 <img src="images/img_flash/beforelogin.gif" alt="casio banner" width="965px" height="190px" />
                              
                                 </a>
                                 </div>
                                 <div class="span8 box1" style="margin-left:6px;">
                          <h3 class="title2" align="left">Welcome to WES AUSTRALASIA! </h3>
                          <br />
                          <div class="box_1 push_left">
                            <p class="p2">WES Australasia is a leading wholesale distributor for electronic components and accessories. </p>
                            <p class="p2">Our products are sourced globally supplying Engineers, Installers and Retailers throughout many industries in the Australasia Pacific Region.</p>
                            <p class="p2">We pride ourselves on fast delivery, great customer service and our extensive product range that extends over the following categories:</p>
                            <p class="p2">- Data / Cabling / Racking<br>
                              - Audio / Visual Installation<br>
                              - Electronic Components<br>
                              - Security and Surveillance<br>
                              - Communications<br>
                              - Tools and Test Equipment<br>
                              - AC / DC Power<br>
                              - Plus More!</p>
                            <p class="p2linkL" style="text-align:left;"><a href="../CreateanAccount.aspx"  >Learn more about WES...</a></p>
                          </div>
                          <div class="box_2 push_left" style="margin-top: -16px;"> 
                          <%--<img alt="alt text" src="images/categorie1.png"> <img alt="alt text" src="images/categorie2.png"> <img alt="alt text" src="images/categorie3.png"> <img alt="alt text" src="images/categorie4.png"> <img alt="alt text" src="images/categorie5.png"> <img alt="alt text" src="images/categorie6.png"> <img alt="alt text" src="images/categorie7.png"> <img alt="alt text" src="images/categorie8.png"> <img alt="alt text" src="images/categorie9.png"> <img alt="alt text" src="images/categorie10.png"> <img alt="alt text" src="images/categorie11.png"> <img alt="alt text" src="images/categorie12.png"> <img alt="alt text" src="images/categorie13.png"> <img alt="alt text" src="images/categorie14.png"> <img alt="alt text" src="images/categorie15.png"> <img alt="alt text" src="images/categorie16.png"> <img alt="alt text" src="images/categorie17.png"> <img alt="alt text" src="images/categorie18.png"> <img alt="alt text" src="images/categorie19.png"> <img alt="alt text" src="images/categorie20.png">--%> 
                           <span class="box_2 push_left imgspan spanimg1"></span>
                           <span class="box_2 push_left imgspan spanimg2"></span>
                           <span class="box_2 push_left imgspan spanimg3"></span>
                           <span class="box_2 push_left imgspan spanimg4"></span>
                           <span class="box_2 push_left imgspan spanimg5"></span>
                           <span class="box_2 push_left imgspan spanimg6"></span>
                           <span class="box_2 push_left imgspan spanimg7"></span>
                           <%--<span class="box_2 push_left imgspan spanimg8"></span>--%>
                           <span class="box_2 push_left imgspan spanimg22"></span>
                          <span class="box_2 push_left imgspan spanimg9"></span>
                          <span class="box_2 push_left imgspan spanimg10"></span>
                          <span class="box_2 push_left imgspan spanimg11"></span>
                          <span class="box_2 push_left imgspan spanimg12"></span>
                          <span class="box_2 push_left imgspan spanimg13"></span>
                          <span class="box_2 push_left imgspan spanimg14"></span>
                          <span class="box_2 push_left imgspan spanimg15"></span>
                          <span class="box_2 push_left imgspan spanimg16"></span>
                           <span class="box_2 push_left imgspan spanimg17"></span>
                           <span class="box_2 push_left imgspan spanimg18"></span>
                           <span class="box_2 push_left imgspan spanimg19"></span>
                           <span class="box_2 push_left imgspan spanimg20"></span>
                            <span class="box_2 push_left imgspan spanimg21"></span>
                           <%--<span class="box_2 push_left imgspan spanimg22"></span>--%>
                           <span class="box_2 push_left imgspan spanimg8"></span>
                           <span class="box_2 push_left imgspan spanimg23"></span>
                           <span class="box_2 push_left imgspan spanimg24"></span>
                        
                          </div>
                        </div>

                                 <%
                                     
                              } %>

                        
                        <div class="span4 box2" style="height:420px;width:300px;">
                          <h3 class="title1 alaincenter">Customer Login</h3>
                          
                          <p class="p1">Please Sign In using your Email ID or  User Name and Password below:</p>
                          <p class="p1">User Login
                            <%--<input type="text" style="width:170px" class=" push_right" name="">--%>
                            <asp:TextBox ID="txtUserName"  autocomplete="off" runat="server" class=" input1" style="width:170px"  size="25" MaxLength="50"  ></asp:TextBox>
                          </p>
                          <p class="p1">Password
                       <%--     <input type="text" style="width:170px" class=" push_right" name="">--%>
                      <%-- <asp:TextBox ID="txtPassword"  style="width:170px" class=" input1" runat="server" TextMode ="Password"  size="25" MaxLength="15" onkeypress="capLock(event)" onFocus="txtpassword_onFocus();"  ></asp:TextBox>--%>
                       <%--<asp:TextBox ID="txtPassword"  style="width:170px" class=" input1" runat="server" TextMode ="Password"  size="25" MaxLength="15" onkeyup="Restrictpasslenth()" onFocus="txtpassword_onFocus();"  ></asp:TextBox>--%>
                       <asp:TextBox ID="txtPassword"  style="width:170px" class=" input1" runat="server" TextMode ="Password"  size="25" MaxLength="40"  onFocus="txtpassword_onFocus();"  ></asp:TextBox>
                          </p>
                          <p class="p1">
                            <%--<input type="checkbox">--%>
                            <input type="checkbox"  id="chkKeepme" name="chkKeepme" style="border-style:none;" runat="server" />
                            Keep me logged in this computer</p>
                            <input type="checkbox" name="chkShopCart" id="chkShopCart"  visible="false"
                                                    style="border-style:none;" runat="server" />
                          <%--<p class="p1">Forgot <a href="#">Username?</a> or <a href="#">Password?</a></p>--%>



                            <p class="p1" >  Forgot <asp:HyperLink ID="lnkForgotNamePage" runat="server" CssClass="HyperLinkStyle" >Username </asp:HyperLink>Or
                                                    <asp:HyperLink ID="lnkForgotPWDPage" runat="server" CssClass="HyperLinkStyle">Password </asp:HyperLink>?</p>
                          <p class="p1" style="height:30px;">
                         
                            <%--<input type="submit" value="LOGIN" class="button normalsiz btngreen fullwidth" name="Submit">--%>
                            <asp:Button ID="cmdLogin" class="buttonlogin" text="LOGIN" runat ="server" OnClick="cmdLogin_Click"  ValidationGroup="LoginValidation" OnClientClick="return passwordcheck();"  />
                          </p>

                           <%
                               string Hlksgp = "";
                               if (!string.IsNullOrEmpty(Request.QueryString["ActionType"]))
                                 Hlksgp = objSecurity.StringDeCrypt(Request.QueryString["ActionType"].ToString());

                               if (Hlksgp == null || Hlksgp != "RequireLogin")
                              {
                         %>
                                 <p class="p1" style="height:30px;text-align:center;line-height: 30px;">
                                   <asp:HyperLink ID="CmdSignuphp" NavigateUrl="../CreateanAccount.aspx" runat="server"  CssClass="buttonsignup" >CUSTOMER SIGN UP / REGISTER</asp:HyperLink>
                          </p>

                           <%
                              }
                             %>

                            

                       
                          <p class="p1" style="text-align: center;height:32px;font-size:11px;">
                           
                           <asp:Label ID="lblErrMsg" runat="server"  Text="" Class="lblErrorSkin" ></asp:Label>
                           <br />
                            <asp:HyperLink ID="lnkResetPassword" CssClass="HyperLinkStyle" runat="server" Visible="false">Please Reset Your Password</asp:HyperLink>
                             </p>
                   
                        </div>
                      </div>

                   </div>

                    </td>
                  </tr>
                </table>
                                              <asp:Label ID="lblhid" runat="server" Text="" ></asp:Label>
                                            <asp:Label ID="lblhid1" runat="server" Text="" ></asp:Label>
                                            <asp:Panel ID="pnlChgPassword" runat="server" Width="300px" Style="display:none">
                                                <table border="0" width="100%" cellspacing="0" cellpadding="3" bgcolor="black" style="border:1px;border-color:Black">
                                                            <tr>
                                                                <td height="28" colspan="2" style="background-color:#5CBBE9;"><table width="260" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" height="30" class="tx_6" width="90%">&nbsp;&nbsp;&nbsp;CHANGE PASSWORD</td>
                                                                    <td align="right"><img src="images/ico_11.gif" width="14" height="17"></td>
                                                                    </tr>
                                                                    </table></td>
                                                            </tr>
                                                            <tr>
                                                                <td bgcolor="white" valign="top"><asp:Label ID="lblNewPassword" Class="lblStaticSkin" runat="server" Text="New Password" ></asp:Label></td>
                                                                <td bgcolor="white">
                                                                    <asp:TextBox ID="txtchgPassnew1" Class="textSkin" MaxLength="50" runat="server" TextMode="Password" /><br />
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Invalid Password" ControlToValidate="txtchgPass">*</asp:RequiredFieldValidator>--%>
                                                                      <%--<asp:RequiredFieldValidator ID="txtchgPassRequired" runat="server" ControlToValidate="txtchgPass" ErrorMessage="Invalid Password" ToolTip="Invalid Password"></asp:RequiredFieldValidator>--%>
                                                                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtchgPassnew1"   ValidationExpression="^[a-zA-Z0-9\s]{6,24}$"
                                                        runat="server" ErrorMessage="Password must contain alphabet and numeric,and length should be 6 to 24"></asp:RegularExpressionValidator>
                                                   
                                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"
                                                        ControlToValidate="txtchgPassnew1"   ValidationExpression=".*[0-9].*"
                                                        runat="server" ErrorMessage="Password must contain one numeric"></asp:RegularExpressionValidator>
                                                   
                                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"
                                                        ControlToValidate="txtchgPassnew1"   ValidationExpression=".*[a-zA-Z].*"
                                                        runat="server" ErrorMessage="Password must contain one alphabet"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="New Password Required"
                 ControlToValidate="txtchgPassnew1" ></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        <tr>
                                                            <td bgcolor="white" valign="top"><asp:Label ID="lblConfirmPassword" Class="lblStaticSkin" runat="server"  Text="Confirm Password"></asp:Label></td>
                                                            <td bgcolor="white">
                                                                <asp:TextBox ID="txtchgCPass" Class="textSkin" MaxLength="50" Textmode="Password" runat="server" /><br />
                                                                    <%--<asp:CompareValidator ID="CompareValidator1" runat="server"  ControlToCompare="txtchgPass" ControlToValidate="txtchgCPass"    ValueToCompare="String"></asp:CompareValidator><br /> --%>
                                                               <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Confirm Password does not match"  ControlToValidate="txtchgCPass"></asp:RequiredFieldValidator>--%>
                                                              <%-- <asp:RequiredFieldValidator ID="txtchgCPassRequired" runat="server" ControlToValidate="txtchgCPass" ErrorMessage="Invalid Password" ToolTip="Invalid Password"></asp:RequiredFieldValidator>--%>
                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required"
                                                               ControlToValidate="txtchgCPass" ></asp:RequiredFieldValidator><br />
                                                              <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"
                                                                ControlToCompare="txtchgPassnew1" ControlToValidate="txtchgCPass" ValueToCompare="String"></asp:CompareValidator>
                                                                   
                                                              <asp:Label ID="lblCMsg1"  runat="server" /><br/>
                                                               
                                                                
                                                            </td>
                                                            </tr>
                                                        <tr>
                                                        <td align="center" bgcolor="white">
                                                            <asp:Button ID="btnOk" class="chg_boton" UseSubmitBehavior="true" runat="server" Text="Ok" OnClick="btnOk_Click"  CausesValidation="true"/></td>
                                                        <td align="center" bgcolor="white">
                                                            <asp:Button ID="btnCancel" class="chg_boton"  UseSubmitBehavior="false" 
                                                                runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" /></td>
                                                        </tr>
                                                        <tr>
                                                        <td align="center" bgcolor="white" colspan="2">
                                                            <asp:Label ID="lblCMsg" runat="server" Class="lblErrorSkin"  Font-Bold="true" 
                                                             Text=""></asp:Label>
                                                        </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            <asp:ModalPopupExtender ID="ChgPassPop" PopupControlID="pnlChgPassword" BackgroundCssClass="modalBackground" 
                                                    DropShadow="true" runat="server" TargetControlID="lblhid">
                                            </asp:ModalPopupExtender>
                                      <asp:Panel ID="PnlAdmin" runat="server" Style="display:none">
                                      <table width="514" border="0" cellpadding="0" cellspacing="0" style="border:2px solid #CCCCCC;background-color:#ffffff;">
  
  <tr>
    <td colspan="2" valign="top" style="padding:20px;"><strong class="txt_18 blue1">Welcome to WES online store.</strong>
      <p class="blue1 txt_14"><strong>New Online Account User Setup</strong></p>
      <p>We have detected that your account has multiple email addresses associated to it.</p>
      <p>WES online web site supports multiple user logins from your company. Your account will require at least one
      person in your company to be set as an Admin user so that user permission levels and adding, editing and
      deleting user’s can be configured by the Admin user.</p>
      <p>Until this defined you will not be able to place orders, however you will be able to browse the website.</p>
      <p>Please download the PDF form from below and Fax this back to us so that an Admin user from your company
      can be assigned. Once done, the Admin user will be able to configure user login’s for people within your own
    company if it is required.</p></td>
  </tr>
  <tr>
    <td width="248" height="49" valign="top" style="padding-left:20px;"><a href="#"><img src="images/dld_pdf_form.png" width="197" alt="" height="40" /></a></td>
    <td width="266" valign="middle"><b>
        <asp:HyperLink ID="hlink" CssClass="txt_12 blue" runat="server" NavigateUrl="~/Home.aspx">Continue Browsing Web Site</asp:HyperLink>
    </b></td>
  </tr>
</table>
                                      </asp:Panel>
                                      <asp:ModalPopupExtender ID="ShowAdminAlert" 
                                      PopupControlID="pnlAdmin" BackgroundCssClass="modalBackground" 
                                                    DropShadow="true" TargetControlID="lblhid1"
                                      runat="server">
                                      </asp:ModalPopupExtender>
                                      <asp:Label ID="lblConfirm" Text="" runat="server"></asp:Label>

                                      

                                                </contenttemplate>
    <triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmdLogin" EventName="Click"/>
                                                <asp:AsyncPostBackTrigger ControlID="btnOk" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            </triggers>
</asp:UpdatePanel>

<table>
<tbody>
  <tr>
                    <td>
                      <div class="container_cmn" >
                      <div class="container12" >
                        <div class="span12 box2" style="margin-left:6px;width: 946px;">
                           <h3 class="titletemp" align="left" >Product Highlights </h3>
                           <uc1:newproductsnav ID="newproductsnav1" runat="server" />
                          
                        </div> 
                      </div> 
                      </div> 
                    </td>
                  </tr>
</tbody>
</table>