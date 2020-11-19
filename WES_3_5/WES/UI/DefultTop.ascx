<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_Defulttop" Codebehind="Defulttop.ascx.cs" %>
<%@ Register Src="CartItems.ascx" TagName="CartItems" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
<title></title>
<script language ="javascript" type ="text/jscript">
 //window.history.forward(1);
function Redirect()
    {
            var is_URL ="http://www.tradingbell.com";
            var is_Features = "left=0,Top=0,scrollbars=yes,width=990,height=750";
            window.open(is_URL,"WEBCAT",is_Features);
    }
    
    
</script>
<meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
</head>
<body style="padding:0; margin:0" onload="MM_preloadImages('~/Images/btn_schoolSuppliesOvr.jpg','~/Images/btn_officeSuppliesOvr.jpg','~/Images/btn_furnitureOvr.jpg','~/Images/btn_saleOvr.jpg','~/Images/btn_loginOvr.jpg')"  >
<asp:Panel ID="pnlSearch"  runat="server" > 
<script language ="javascript" type="text/javascript">
function ValidateSearchText(elemt)
{
             if(trim(window.document.getElementById("ctl00_TopPage_txtSearch").value) == "")
            {
               alert('Search text cannot be empty');
               window.document.getElementById("ctl00_TopPage_txtSearch").focus();
               return false;
            }
}
function trim(s) 
 {
            while (s.substring(0,1) == ' ')
            {
           s = s.substring(1,s.length);
            }
          while (s.substring(s.length-1,s.length) == ' ')
           {
           s = s.substring(0,s.length-1);
           }
           return s;
}
<!--
function MM_swapImgRestore() 
{ //v3.0
           var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages()
 { //v3.0
          var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
          var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
          if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d)
 { //v4.01
         var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
         d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
          if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
          for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
          if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() 
{ //v3.0
         var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
          if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
//-->
</script>
     <table id ="tblTop" width="800" border="0" align="center"  cellpadding="0" cellspacing="0" runat="server">
      <tr id ="rowImg">
        <td  id="cellImg" height="102" style="width: 822px">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
          <td width="55%" rowspan="2"><asp:Image ID="imgLog" runat="server"  ImageUrl="~/images/logo.jpg" Width="236" Height="102" /></td>
            <td valign="top" style="width: 500px">
            <div align="right">
                 <table  id="tblchkOut" border ="0" align="right" cellpadding="0" cellspacing="0">
         <tr valign="top">    
              <td><asp:Image ID="imgViewCart" runat="server" ImageUrl="~/images/icon_cart.jpg" Width="28" Height="30" /><asp:LinkButton ID="lbViewCart" runat="server" SkinID="TopLevelSkin"  align="right" Text="<%$ Resources:Top, lbViewCart %>" PostBackUrl="~/OrderDetails.aspx" ></asp:LinkButton>
            </td>
               <td><asp:Image ID="imgCheckout" runat="server" ImageUrl="~/images/icon_cart.jpg" Width="28" Height="30"/> <asp:LinkButton ID="lblCheckout" align="right" runat="server"  Text="<%$ Resources:Top,lblCheckout  %>" PostBackUrl="~/OrderDetails.aspx"  SkinID="TopLevelSkin" ></asp:LinkButton>
           </td>
             <td><uc1:CartItems ID="ucCartItems" runat="server" />
           </td>
       </tr>
  </table>
            </div>
            </td>
          </tr>
          <tr>
            <td colspan="2" style="text-align: right; background-color: #fafafa;" valign="top">
                <table id="SearchTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblSearch" runat="server"  SkinID ="SearchLinkSkin" Text="<%$ Resources:Top,lblSearch %>" ></asp:Label></td>
                        <td>
                            <asp:TextBox  autocomplete="off"  ID="txtSearch" runat="server" SkinID="textSkin" Width="100px" Font-Size="Smaller"></asp:TextBox></td>
                        <td>
                           <asp:ImageButton ID="cmdSearch" runat ="server" OnClientClick="return ValidateSearchText(this);" ImageUrl ="~/images/go.gif" OnClick="cmdSearch_Click"/></td>
                        <td>
                           <asp:LinkButton ID="lbHome" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lblHome %>"  PostBackUrl="~/Default.aspx"></asp:LinkButton></td>
                        <td>                
                          <%--<asp:LinkButton ID="lbEmail" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lbEmail %>"></asp:LinkButton>--%>
                        
                        <%
                                string Pid;
                                Pid = Request["Pid"];
                                string url;
                                url = "<a href='mailto:Your Eamil ID?body=" + Page.Request.Url.AbsoluteUri.ToString()+ "'";
                                url = url + "class=\"aref\">Email|</a>";
                                Response.Write(url);
                              
                             %>
                             </td>
                        <td>                
                          <asp:LinkButton ID="lbtnSignOut" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lbtnSignOut %>" OnClick="lbtnSignOut_Click1" ></asp:LinkButton></td>

                      </tr>
                </table>
                </td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td style="width: 822px"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('schoolSupplies','','Images/btn_schoolSuppliesOvr.jpg',1)"> <asp:ImageButton ID="ibtnScoolSupplies" runat="server" ImageUrl="~/images/btn_schoolSupplies.jpg" Width="159" Height="102" BorderStyle="None" PostBackUrl="~/Family.aspx?Cat=PP033" />   </a></td>
            <td><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('officeSupplies','','Images/btn_officeSuppliesOvr.jpg',1)"> <asp:ImageButton ID="ibtnOfficeSupplies" runat="server" ImageUrl ="~/images/btn_officeSupplies.jpg" Width="161" Height ="105" BorderStyle="none" PostBackUrl="~/Family.aspx?Cat=PP018" /></a></td>
            <td><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('furniture','','Images/btn_furnitureOvr.jpg',1)"> <asp:ImageButton ID="ibtnfurniture" runat="server" ImageUrl="~/images/btn_furniture.jpg" Width="160" Height ="102" BorderStyle="none" PostBackUrl="~/Family.aspx?Cat=PP003"/> </a></td>
            <td><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('sale','','Images/btn_saleOvr.jpg',1)"><asp:ImageButton ID="ibtnsale" runat="server" ImageUrl="~/images/btn_sale.jpg" Width="160" Height ="105" BorderStyle="none" PostBackUrl="~/Family.aspx?Cat=PP005"/></a></td>
            <td><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('login','','~/Images/btn_loginOvr.jpg',1)"><asp:ImageButton ID="ibtnlogin" runat="server" ImageUrl="~/images/btn_login.jpg" Width="161" Height ="105" BorderStyle="none" PostBackUrl="~/Login.aspx"/></a></td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td style="width: 822px">
            <asp:Image ID="imgmainimage" runat="server" ImageUrl="~/images/mainImage.jpg" Width="800" Height ="273" />   
        </td>
       </tr>
      </table>
  </asp:Panel>
</body>
</html>