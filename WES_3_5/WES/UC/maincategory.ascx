<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_maincategory" EnableTheming="True" Codebehind="maincategory.ascx.cs" %>
  <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />
     <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />
     <input type="hidden" name="HidsubcatIds1" runat="server" id="HidsubcatIds1" />
     <input id="HidItemPage" type="hidden" runat="server" />
<input id="Hidcat" type="hidden" runat="server" />
 <script language="javascript">
     function GetSelectedItems(field) {        
         var SelAttrStr = '';
         for (var j = 0; j < document.getElementById(field).options.length; j++) {
             if (document.getElementById(field).options[j].selected) {
                 if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
                     SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                     if (field == 1)
                         document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
                     else if (field == 2) {
                         document.getElementById("<%=HidsubcatIds.ClientID%>").value = SelAttrStr;
                     }
                     else if (field == 3) {
                         document.getElementById("<%=HidsubcatIds1.ClientID%>").value = SelAttrStr;
                     }

                     if (field <= 3) {
                         document.forms[0].submit();
                     }
                 }
             }

         }
     }
     
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(window).scroll(function () {
                if ($(this).scrollTop() > 200) {
                    $('#toTop').fadeIn();
                } else {
                    $('#toTop').fadeOut();
                }
            });
            $('#toTop').click(function () {
                $("html, body").animate({ scrollTop: 0 }, 600);
                return false;
            });
        });
</script>



<table width="180" border="0" cellspacing="0" cellpadding="0" >    
  <tr>
    <td>
                           
           

<%
    //if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3") && Request.QueryString["cid"] != null)
    //{
        %>
       <%-- <uc1:browsebyproductWES ID="browsebyproductWES1" runat="server" />--%>
        <%
    //}
    //else
    //{
                     
    //Response.Write(ST_Browsebycategory());
   
    
            
%>
                
                         
                        <%
                        //} 
                            
                            %>
           
<%--</ul>
            
            </td>
        </tr>
      </table>      
      </div>--%>
       
           
  
       <% 
           
           Response.Write(ST_Categories());
            %>
       
    </td>
  </tr>
</table>
</br>               

               