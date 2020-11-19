<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ChangeUserName" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto" Codebehind="ChangeUserName.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">

    <script language ="javascript">
    function Compare()
    {       
         //alert(window.document.getElementById(txtNewUserName));
    }
    function capLock(e)
    {
             kc = e.keyCode?e.keyCode:e.which;
             sk = e.shiftKey?e.shiftKey:((kc == 16)?true:false);
             if(((kc >= 65 && kc <= 90) && !sk)||((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
             else
                document.getElementById('divMayus').style.visibility = 'hidden';
    }
    </script>
     <%-- <script language="javascript" type="text/javascript">
          function blockspecialcharacters(e) {
              var keynum
              var keychar
              var numcheck
              if (window.event) {
                  keynum = e.keyCode
              }
              else if (e.which) {
                  keynum = e.which
              }
              keychar = String.fromCharCode(keynum)
              if (keychar == "@" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+") {
                  e.keyCode = '';
                  return false;
              }
              else {
                  return true;
              }
          }
</script>--%>
    <asp:Panel ID ="pnlLogin" runat ="server" DefaultButton="btnChange">
    <table align =center  width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Change User Name
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
      <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   Width="1px"></asp:Label>
                &nbsp;<asp:Label ID="Label4" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label>
    <Table cellSpacing="0" cellPadding="0" width="558" align="center"  border="0">
    <tbody>
        <tr>
            <td width="10"height="10"><img height="17" src="Images/tbl_topLeft.gif" width="10"></td>
            <td background="Images/tbl_top.gif"height="10">
            <img height="17" src="Images/tbl_top.gif" width="10"></td>
            <td width="10"height="10"><img height="17" src="Images/tbl_topRight.gif" width="10"></td>
        </tr>
        <tr>
              <td width="10"  background="Images/tbl_left.gif">
  	            <img height="10" src="Images/tbl_left.gif" width="10">
              </td>
              <td>

                 <table id ="tblBase" align ="center" width="100%" border ="0" cellpadding="3" cellspacing="0" >
			            <tr> 
			                <td align ="center">
                                <asp:Label ID="lblError" class="lblErrorSkin" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td  valign ="middle" align ="center" >
                                    <table id="LoginTable"  align="center"  border="0" class="BaseTblBorder" cellpadding ="3" cellspacing ="0" width="450px">
                                        
                                        <tr>
                                            <td class="tx_6" colspan ="3" style="height:20px;background:url(images/17.gif);"><asp:Label id="lblHead" runat="server" meta:resourcekey="LblChangePwd" ></asp:Label></td>
			                            </tr>
                                        <tr>
                                             <td><asp:Label ID="Label20" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label></td>
                                             <td align="left" style="width: 123px">
                                                <asp:Label ID="lblOldUserName" Class="lblStaticSkin" runat="server" meta:resourcekey="LblOldPwd"></asp:Label>
                                                </td>
                                            <td  align="left" style="height: 30px">
                                                <asp:TextBox  autocomplete="off"  ID="txtOldUserName" Class="textSkin"  MaxLength ="10" runat="server" onkeypress="capLock(event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvOldPwd" class="vldRequiredSkin" ControlToValidate ="txtOldUserName" runat="server" meta:resourcekey="rfvOldPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                             <td><asp:Label ID="Label1" runat="server" Class="lblRequiredSkin"  meta:resourcekey="LblStar"  ></asp:Label></td>
                                             <td align="left" style="width: 123px" >
                                                <asp:Label ID="lblNewUserName" Class="lblStaticSkin" runat="server" meta:resourcekey="LblNewPwd" ></asp:Label>
                                                
                                            <td  align="left">
                                                <asp:TextBox  autocomplete="off"  ID="txtNewUserName" Class="textSkin" MaxLength ="10" runat="server"  Font-Underline="False" onkeypress="capLock(event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNewPwd" Class="vldRequiredSkin" ControlToValidate ="txtNewUserName" runat="server" meta:resourcekey="rfvNewPwd"  ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                         <tr>
                                             <td style="height: 30px"><asp:Label ID="Label2" runat="server" Class="lblRequiredSkin"  meta:resourcekey="LblStar"  ></asp:Label></td>
                                             <td align="left" style="height: 30px; width: 123px;">
                                                <asp:Label ID="lblConfirmUserName" Class="lblStaticSkin" runat="server"  meta:resourcekey="lblConfirmPwd" ></asp:Label>
                                             </td>
                                            <td align="left" style="height: 30px">
                                                <asp:TextBox  autocomplete="off"  ID="txtConfirmUserName" Class="textSkin"  MaxLength ="10" runat="server"  onkeypress="capLock(event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvConPwd"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmUserName" runat="server" meta:resourcekey="rfvConPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align ="center" colspan ="4">
                                                <table align="center" >
                                                    <tr>
                                                        <td>
                                                        <br />
                                                            <asp:Button ID="btnChange" runat ="Server" meta:resourcekey="btnChangePwd" Class ="btnNormalSkin" OnClick="btnChange_Click" OnClientClick ="javascript:Compare()" ValidationGroup="Mandatory"/>
                                                        <br />
                                                         </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <div id="divMayus" style="visibility:hidden; color: red;" align="center"><b>Caps Lock is on.</b></div>
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
              <td width="10" background="Images/tbl_right.gif" height ="350">
          	    <img height="10" src="Images/tbl_right.gif" width="10">
              </td>
        </tr>
        <tr>
              <td width="10"height="10"><img height="10"  src="Images/tbl_bottomLeft.gif" width="10"></td>
              <td background="Images/tbl_bottom.gif"height="10">
          	    <img height="10" src="Images/tbl_bottom.gif" width="10"></td>
              <td width="10"height="10"><img height="10" src="Images/tbl_bottomRight.gif" width="10"></td>
         </tr>
         </tbody>
     </Table>     
</asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>
