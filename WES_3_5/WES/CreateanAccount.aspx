<%@ Page Language="C#" MasterPageFile="~/MainPage.master" AutoEventWireup="true" Inherits="Login" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="CreateanAccount.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">

<%--<div class="container" style="width:790px;" >--%>
 <%-- <div class="container12" >--%>
  <table width="790px" border="0" cellpadding="0" cellspacing="0"> 
  <tr>
  <td align="left" style="text-align:left;" >
  
    <div class="span9 box1" style="width:760px; margin-left:0px; ">
      <h3 class="title1" align="left">Create An Account</h3>
      <div class="img_left"><img width="106" height="80" alt="img" src="images/page_icon1.png"></div>
<div class="box4">
  <p class="p2">Thank you for your interest in WES</p>
        <p class="p2">WES is a wholesale distributor of electronic parts and accessories. Please note we are a business to business supplier including government departments, schools and learning institutions.</p>
        <p class="p2">To establish an account with WES you will be required fill in an account application form to us that will be reviewed by our Sales Team. Once approved you will receive a confirmation email from us with further information.</p>
        <p class="p2">Questions? Contact us between the hours of 8:30 a.m. and 5:00 p.m. Monday - Friday<br>
          Email: <a href="mailto:sales@wes.net.au">sales@wes.net.au</a> or Phone: +61 2 9797-9866<br><br>
          Please select an option below.</p>           
        <p class="p2"><br>
        <a style="margin:0 20px 0 0;" class="buttonreg bigsiz btnbluebig fleft alainc" href="DealerRegistration.aspx"><strong>NEW CUSTOMER</strong><span >NEW customers who have never<br> puchased from us before</span></a>
        <a style="margin:0 20px 0 0;" class="buttonreg bigsiz btnbluebig fleft alainc" href="ExistCustomerRegistration.aspx" ><strong>EXISTING CUSTOMER</strong><span >If you already have an account<br> and would like online access</span></a>
      <%--  <a class="buttonreg bigsiz btnbluebig fleft alainc" href="RetailerRegistration.aspx" ><strong>RETAILER CUSTOMER</strong><span >NEW customers who have never<br> puchased from us before</span></a>--%>
        </p>
      </div>
    </div>
    <%--<div class="span3 box2">

 <%-- </div>--%>
  
<%--</div>--%>
</td>
  </tr>
  </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>