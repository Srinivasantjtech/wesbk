<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_browsebybrandWES" Codebehind="browsebybrandWES.ascx.cs" %>
     <input type="hidden" name="hidcatIds" runat="server" id="hidcatIds" />
     <input type="hidden" name="HidsubcatIds" runat="server" id="HidsubcatIds" />
     <input type="hidden" name="HidsubcatIds1" runat="server" id="HidsubcatIds1" />
    
   <script language="javascript">
//       function GetSelectedItems(field) {
//      
//             var SelAttrStr = '';
//             for (var j = 0; j < document.getElementById(field).options.length; j++) {
//                 if (document.getElementById(field).options[j].selected) {
//                     if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
//                         SelAttrStr = document.getElementById(field).options[j].value + '^' + field;                    
//                         if (field == 1)
//                             document.getElementById("<%=hidcatIds.ClientID%>").value = SelAttrStr;
//                         else if (field == 2) {
//                             document.getElementById("<%=HidsubcatIds.ClientID%>").value = SelAttrStr;
//                         }
//                         else if (field == 3) {
//                             document.getElementById("<%=HidsubcatIds1.ClientID%>").value = SelAttrStr;
//                         }

//                         if (field <= 3) {                            
//                             document.forms[0].submit();                             
//                         }
//                     }
//                 }

//             }
//             function urlredirect() {
//                 alert('1');
//                 var catid = document.getElementById("<%=hidcatIds.ClientID%>").value;
//                 var subcatid = document.getElementById("<%=HidsubcatIds.ClientID%>").value;
//                 var param = '';
//                 if (ttrim(ddlattrvalue) != "" && ttrim(subcatid) != "") {
//                     param = "bybrand.aspx?&cid=" + catid.replace(/#/, "%23").replace(/&/g, "%26") + "byp=2";
//                 }
//                 if (ttrim(subcatid) != "") {
//                     param = "bybrand.aspx?&cid=" + subcatid.replace(/#/, "%23").replace(/&/g, "%26") + "byp=2";
//                 }
//                 window.document.location = param;
//             }


  //       }
    </script>

    <table  border="0" cellspacing="0" cellpadding="0" >  
  <tr>
    <td>
      
    <%
        //Response.Write(ST_Browsebycategory());
    %>
 
   <%-- <div align="left" class="treeview" style="background-color:#F7F7F7">--%>
                      
                        <%
                            //if (Request.QueryString["tsb"] == null && Request.QueryString["tsm"] == null)
                            //{
                        %>   
                           <%-- <asp:TreeView ID="TVBrand" runat="server" Font-Bold="true"  OnLoad="ConstructRootTree"
                            NodeStyle-HorizontalPadding="0" RootNodeStyle-HorizontalPadding="12" 
                                RootNodeStyle-ChildNodesPadding="0" LeafNodeStyle-HorizontalPadding="0" ParentNodeStyle-HorizontalPadding="0"  
                               ExpandImageToolTip=""
                               ShowExpandCollapse=false   
                            ExpandDepth="0" NodeWrap="true" BorderWidth="0px" 
                                Font-Names ="Arial, Helvetica, sans-serif" Font-Size="11px" 
                                NodeStyle-Height="22" HoverNodeStyle-ForeColor="Gray"   Width="180">
                                <ParentNodeStyle HorizontalPadding="0px" Width="100%" />
                                <HoverNodeStyle ForeColor="Gray" Width="100%" />
                                <SelectedNodeStyle />
                                <RootNodeStyle ChildNodesPadding="0px" HorizontalPadding="11px" Width="100%" />
                                <NodeStyle Height="22px"
                                    HorizontalPadding="0px" Width="100%" />
                                <LeafNodeStyle HorizontalPadding="0px" Width="100%" />                      
                            </asp:TreeView>--%>
                             <%
                           // }
                           //else if(Request.QueryString["tsb"] != null && Request.QueryString["tsm"] == null)
                           //{
                               %>
                                <%--<asp:TreeView ID="TVModel" runat="server" Font-Bold="true"  OnLoad="ConstructRootTreeModel"
                            NodeStyle-HorizontalPadding="0" RootNodeStyle-HorizontalPadding="12" 
                                RootNodeStyle-ChildNodesPadding="0" LeafNodeStyle-HorizontalPadding="0" ParentNodeStyle-HorizontalPadding="0"  
                               ExpandImageToolTip=""
                               ShowExpandCollapse=false   
                            ExpandDepth="0" NodeWrap="true" BorderWidth="0px" 
                                Font-Names ="Arial, Helvetica, sans-serif" Font-Size="11px" 
                                NodeStyle-Height="22" HoverNodeStyle-ForeColor="Gray"   Width="180">
                                <ParentNodeStyle HorizontalPadding="0px" Width="100%" />
                                <HoverNodeStyle ForeColor="Gray" Width="100%" />
                                <SelectedNodeStyle />
                                <RootNodeStyle ChildNodesPadding="0px" HorizontalPadding="11px" Width="100%" />
                                <NodeStyle Height="22px"
                                    HorizontalPadding="0px" Width="100%" />
                                <LeafNodeStyle HorizontalPadding="0px" Width="100%" />                      
                            </asp:TreeView>--%>
                               <%
                               //} 
           
                             %>
                          <%--  </div>--%>

    <%-- </ul>
                </td>
            </tr>
          </table></div>--%>
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <% //if (Request.Url.OriginalString.ToLower().Contains("categorylist.aspx") || Request.Url.OriginalString.ToLower().Contains("bybrand.aspx") ||  Request.Url.OriginalString.ToLower().Contains("product_list.aspx"))
           // {
             
        %>
            <%--   &nbsp;--%>
           <%
          // Response.Write(ST_bybrand());
           %>
       <%//}
       //else
       //{  Response.Write(ST_bybrand());
         //  } %>

       <% 
           
           Response.Write(ST_BrandAndModel()); 
            %>
        </ContentTemplate>
    </asp:UpdatePanel>
        </td>
      </tr>
    </table>
    </br>  
   