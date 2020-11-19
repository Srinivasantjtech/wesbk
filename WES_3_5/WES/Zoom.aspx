<%@ Page Language="C#" AutoEventWireup="true" Inherits="Zoom" Codebehind="Zoom.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>    
<style type="text/css">
<!--
.drag{position:relative;}
-->
</style>
<script language="JavaScript1.2" type="text/javascript">

var dragapproved=false
function move(){
if (event.button==1&&dragapproved){
return false
}
}
function drags(){
if (!document.all)
return false
if (event.srcElement.className=="drag"){
dragapproved=true
document.onmousemove=move
}
}
document.onmousedown=drags
document.onmouseup=new Function("dragapproved=false")
//-->
</script>

</head>
<body>
<form id="form1" runat="server" >
    <div>
    <table>
        <tr>
            <td>
                
                <asp:Image ID = "imgPF" runat ="server" CssClass="drag"/>
                <%
                    if (Session["USER_NAME"] != null && Session["USER_NAME"].ToString() != "")
                    {
                        //chk = 0;
                    }
                    else
                    {
                        //chk = 1;
                        Response.Redirect("Login.aspx",false);
                    }
                    
                     %>
            </td>
        </tr>
    </table>
     </div>
    </form>
</body>
</html>
