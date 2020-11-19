<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_browsebybrand" Codebehind="browsebybrand.ascx.cs" %>
<%
    Response.Write(ST_Browsebycategory());
%>

<div align="left" class="treeview" style="background-color:#F7F7F7">
                     <%--   <asp:TreeView ID="TVBrand" runat="server" Font-Bold="true" 
                        NodeStyle-HorizontalPadding="0" RootNodeStyle-HorizontalPadding="8" RootNodeStyle-ChildNodesPadding="0" LeafNodeStyle-HorizontalPadding="0" ParentNodeStyle-HorizontalPadding="0"  
                        ExpandImageUrl="~/images/rightarrow.GIF" CollapseImageUrl="~/images/leftarrow.GIF"   
                        ExpandDepth="0" NodeWrap="true" BorderWidth="0px" Font-Names ="Arial, Helvetica, sans-serif" Font-Size="12px" NodeStyle-BackColor="#F7F7F7" NodeStyle-ForeColor="#0157A7">                                    
                        <SelectedNodeStyle ForeColor="Black" Font-Bold="true" Font-Size="Small" Font-Italic="true"/>
                        </asp:TreeView>--%>
                        <asp:TreeView ID="TVBrand" runat="server" Font-Bold="true" OnTreeNodePopulate="PopulateNode" OnLoad="ConstructRootTree"
                        NodeStyle-HorizontalPadding="0" RootNodeStyle-HorizontalPadding="12" 
                            RootNodeStyle-ChildNodesPadding="0" LeafNodeStyle-HorizontalPadding="0" ParentNodeStyle-HorizontalPadding="0"  
                          ExpandImageUrl="~/images/right_arrow1f.jpg" CollapseImageUrl="~/images/down_ arrow1f.jpg" ExpandImageToolTip=""  
                        ExpandDepth="0" NodeWrap="true" BorderWidth="0px" 
                            Font-Names ="Arial, Helvetica, sans-serif" Font-Size="11px" 
                            OnSelectedNodeChanged="TVBrand_SelectedNodeChanged" NodeStyle-Height="22" HoverNodeStyle-ForeColor="Gray"   Width="180">
                            <ParentNodeStyle HorizontalPadding="0px" Width="100%" />
                            <HoverNodeStyle ForeColor="Gray" Width="100%" />
                            <SelectedNodeStyle />
                            <RootNodeStyle ChildNodesPadding="0px" HorizontalPadding="11px" Width="100%" />
                            <NodeStyle Height="22px"
                                HorizontalPadding="0px" Width="100%" />
                            <LeafNodeStyle HorizontalPadding="0px" Width="100%" />                      
                        </asp:TreeView>
                        </div>
           
 </ul>
            </td>
        </tr>
      </table></div>
    </td>
  </tr>
</table>
</br>            