<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_searchbybrand" Codebehind="searchbybrand.ascx.cs" %>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<table width="560" cellspacing="0" cellpadding="0"  class="SearchResultsPaging1">
	<tr>
		<td valign="middle" align="left" colspan="3" class="SearchResultsPaging2">
			<table   >
				<tbody>
					<tr>
						<td style="width: 5px">
							&nbsp;
						</td>
						<td  align="left" colspan="2" height="20" class="tx_7A1">
							<img src="../Images/search-icon.jpg" width="21" height="21" border="0" align="middle" alt="" />&nbsp;1. QUICK FIND BY BRAND AND MODEL
						</td>
					</tr>
				</tbody>
			</table>
		</td>
	</tr>
	<tr>
		<td valign="middle" align="left" height="55px">
			<table cellspacing="8" valign="top" >
				<tr>
                <td>
                <asp:DropDownList ID="dd1" runat="server" AutoPostBack="true"
                        ontextchanged="dd1_TextChanged" Width="140px"></asp:DropDownList>
                    </td>
                   <td><asp:DropDownList ID="dd2" runat="server" AutoPostBack="true" OnTextChanged="dd2_TextChanged"  Width="140px"> 
                    </asp:DropDownList></td>
                   <td><asp:DropDownList ID="dd3" runat="server" AutoPostBack="true" OnTextChanged="dd3_TextChanged"  Width="140px">
                    </asp:DropDownList></td>
					<!--<td > PLACE HOLDER
						<img style="cursor:pointer;" src="images/tosuit_search.jpg"  onclick="urlredirectB()" />

					</td>-->
					<td>&nbsp;</td>
				</tr>

			</table>
		</td>
	</tr>
</table>

<%//=ST_bybrand()
%>
<% if (Request.Url.OriginalString.Contains("categorylist.aspx"))
   {
       //Response.Write(updateNavigation());
       %>
       &nbsp;
       <%
       Response.Write(ST_bybrand());
       %>
   <%}
   else
   {  Response.Write(ST_bybrand());
       } %>
     
    </ContentTemplate>
</asp:UpdatePanel> 
     <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />
     <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />
     <input type="hidden" name="HidsubcatIds1" runat="server" id="HidsubcatIds1" />
    
     <script language="javascript">
     function GetSelectedItems(field) {
         var SelAttrStr = '';                  
            for (var j = 0; j < document.getElementById(field).options.length; j++) {
               if (document.getElementById(field).options[j].selected) {
                    if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
                        SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                            if (field == 1)
                            document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
                            else if (field == 2)
                            {
                            document.getElementById("<%=HidsubcatIds.ClientID%>").value = SelAttrStr;
                            }
                            else if (field == 3)
                            {
                            document.getElementById("<%=HidsubcatIds1.ClientID%>").value = SelAttrStr;
                            }
                       
                        if (field <= 3) {                        
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
