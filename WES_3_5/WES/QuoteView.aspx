<%@ Page Language="C#" AutoEventWireup="true"  Inherits="QuoteView" Codebehind="QuoteView.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="Invoice" Src ="~/UI/QuoteInvoice.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Quote Report</title>
</head>
<body>
    <form id="form1" runat="server">
        <table id ="tblBase" align ="center" width="650px" border ="0px" cellpadding="3" cellspacing="0">
        <tr>
            <td>
             <table id ="tblConfirm"  align ="left" width="650px" border ="0px" cellpadding="3" cellspacing="0">
    	           <tr class="tablerow">
                    <td class="StaticText" align="left" >                        
                        <b><asp:Label ID ="lblPageHead" runat ="server" meta:resourcekey="lblPageHead"></asp:Label></b>
                    </td>
                         <td align ="right"><img src="Images/Print.gif" id="imgPrint" runat ="server"/><asp:LinkButton ID="lbtnPrint" meta:resourcekey="lbtnPrint" runat ="Server" class="CompanylinkSkin" OnClientClick ="javascript:window.print();return false;"></asp:LinkButton></td>
    	           </tr>	           
	         </table>  
            </td>
        </tr>
        <tr>
           <td> 
              <WebCat:Invoice ID ="ucInvoice" runat ="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

       
