<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ForgotPassWord"  Culture="en-US"
    UICulture="en-US" Codebehind="ForgotPassWord.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
  <script type="text/javascript">
    function CheckCusType() {

        switch (document.getElementById("ctl00_maincontent_CusType").value) {
            case 'Retailer':
                ShowRetailer();
                break;
            case 'Dealer':
                ShowDealer();
                break;
           
        }
    }

    function ShowDealer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "block";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "block";
    }

    function ShowRetailer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "none";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "none";
    }
   
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td align="left" class="tx_1">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Forgot
                Password
            </td>
        </tr>
        <tr>
            <td class="tx_3">
                <hr>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblError" width="558" runat="server" align="left">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
       
        



        <div class="fpwd_panel" runat="server" id="divmain"> 
                    <h3 class="title1 alaincenter">Password Assistence</h3> 
                    <p class="alaincenter">To start resetting your password, type your email</p> 
                    <hr>
                    <p class="p1"><span class="label16">Email Address</span> 

                     

                        <asp:TextBox autocomplete="off" ID="txtUserMail" runat="server" Width="160px" 
                                   class="input_fpwd" size="25" AutoCompleteType="Disabled"></asp:TextBox>
                    </p> 
                   
            
            
             <p class="p1" runat="server" id="ploginname" visible="false" >
                 <asp:Label ID="lblLoginName" runat="server" Text="Login Name or WES Account No" Class="label16"
                                  ></asp:Label>
              
               <%--   <asp:TextBox autocomplete="off" ID="txtcompanyid" runat="server" 
                                     class="input_fpwd" size="25"  AutoCompleteType="Disabled"></asp:TextBox>
                   --%>
                     
             <%--    <asp:Label ID="lblsep" runat="server" Text="/" Class="lblStaticSkin" style="padding:0 2px"
                                    ></asp:Label>--%>

                 <asp:TextBox autocomplete="off" ID="txtLoginName" runat="server"  size="25"  MaxLength="10"  
                                   class="input_fpwd" AutoCompleteType="Disabled"></asp:TextBox>
                     
                    </p> 
                        
                    <p style="height:30px; width:200px; margin:0 auto; text-align:center;">  
               <asp:Button ID="btnUser" runat="server" text="SUBMIT"  OnClick="btnUser_Click"
                        class="buttonlogin" />
                 
                </div>
        
        
        
        
          <table width="558" height="30" border="0" id="tblusedidbtn" runat="server">
          
            <tr>
                <td>
                <asp:Label ID="lblsucess" runat="server" Text="" Visible="false" Font-Size ="14px" ForeColor="green"></asp:Label>
 <asp:Label ID="lblsucess1" runat="server" Text="" Visible="false" Font-Size ="14px" ForeColor="green"></asp:Label>
         </td>
            
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSQ" runat="server" DefaultButton="btnSubmit">
        <table id="tblSecurityQuestion" runat="Server" visible="false" class="BaseTblBorder"
            width="558" cellspacing="1">
            <tr >
                <td colspan="2" class="TableRowHead" align="left">
                    <asp:Label ID="lblSecurityHeader" runat="server" meta:resourcekey="lblSecurityHeader"></asp:Label>
                </td>
            </tr>
            <tr><td></td></tr>
            <tr>
                <td colspan="2" height="40" align="left">
                    <asp:Label ID="lblSecurityQuestion" runat="server" Class="lblNormalSkin"></asp:Label>
                </td>
            </tr>
            <tr >
                <td style="width: 107px" align="left">
                    <asp:Label ID="lblYourAnswer" runat="server" meta:resourcekey="lblYourAnswer" Class="lblStaticSkin"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox autocomplete="off" ID="txtYourAnswer" runat="server" Class="textSkin"
                        AutoCompleteType="Disabled"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table id="tblSecurityQuestionbtn" runat="server" width="558px" border="0" visible="false">
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click"
                        Class="btnNormalSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
