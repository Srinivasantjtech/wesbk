<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_searchbyproduct" Codebehind="searchbyproduct.ascx.cs" %>
<%
 Response.Write(ST_byproduct());
     %>
     
     <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />
     <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />
     <input type="hidden" name="HidfamIds" runat="server" id="HidfamIds" />
     <script language="javascript">
     function GetSelectedItems(field) {
         var SelAttrStr = '';  
                
            for (var j = 0; j < document.getElementById(field).options.length; j++) {
                if (document.getElementById(field).options[j].selected) {
                    if (document.getElementById(field).options[j].value != 'Select Type' && document.getElementById(field).options[j].value != 'Select Model' && document.getElementById(field).options[j].value != 'All Models') {
                        SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                        if (field == 1)
                            document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
                        else if (field == 2)
                            document.getElementById("<%=HidfamIds.ClientID%>").value = SelAttrStr;
//                        else if (field == 3) {
//                         
//                            document.getElementById("<%=HidfamIds.ClientID%>").value = SelAttrStr;
//                        }
                        if (field != 2) {
                            document.forms[0].submit();
                        }
                    }
                }

            }
            function urlredirect() {               
                var catid = document.getElementById("<%=hidcatIds.ClientID%>").value;
                var subcatid = document.getElementById("<%=HidsubcatIds.ClientID%>").value;
                var famid = document.getElementById("<%=HidfamIds.ClientID%>").value;
                var param = '';
                if (ttrim(ddlattrvalue) != "" && ttrim(subcatid) != "") {
                    param = "byproduct.aspx?&cid=" + catid.replace(/#/, "%23").replace(/&/g, "%26");
                }
                if (ttrim(subcatid) != "") {
                    param = "byproduct.aspx?&cid=" + subcatid.replace(/#/, "%23").replace(/&/g, "%26");
                }
                if (ttrim(famid) != "") {
                    param = param + "&&fid="+famid.replace(/#/, "%23").replace(/&/g, "%26");
                }
                window.document.location = param;
            }
        
        
    }
    </script>