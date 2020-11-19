<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_searchbybrandFamily" Codebehind="searchbybrandFamily.ascx.cs" %>
     <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />
     <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />
    
     <script language="javascript">
     function GetSelectedItems(field) {
         var SelAttrStr = '';                  
            for (var j = 0; j < document.getElementById(field).options.length; j++) {
               if (document.getElementById(field).options[j].selected) {
//                    if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
                   if (document.getElementById(field).options[j].value != 'List all products') {
                        SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                        if (field == 1)
                            document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
                        else if (field == 2)
                        {
                            document.getElementById("<%=HidsubcatIds.ClientID%>").value = SelAttrStr;
                            }
                       
                        if (field <= 2) {                        
                            document.forms[0].submit();
                        }
                    }
                }

            }
            function urlredirect() {  alert('1');             
                var catid = document.getElementById("<%=hidcatIds.ClientID%>").value;
                var subcatid = document.getElementById("<%=HidsubcatIds.ClientID%>").value;                
                var param = '';
                if (ttrim(ddlattrvalue) != "" && ttrim(subcatid) != "") {
                    param = "bybrand.aspx?&cid=" + catid.replace(/#/, "%23").replace(/&/g, "%26")+"byp=2";
                }
                if (ttrim(subcatid) != "") {
                    param = "bybrand.aspx?&cid=" + subcatid.replace(/#/, "%23").replace(/&/g, "%26")+"byp=2";
                }                
                window.document.location = param;
            }
        
        
    }
    </script>
<% =ST_bybrand()%>
    