<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_browsebyproductWES" Codebehind="browsebyproductWES.ascx.cs" %>
<%
    Response.Write(ST_Browsebycategory());
%>
 <div align="left" class="treeview" style="background-color:#F7F7F7">                       
                         <asp:TreeView ID="TVProduct" runat="server" Font-Bold="true" OnTreeNodePopulate="PopulateNode" OnLoad="ConstructRootTree"
                        NodeStyle-HorizontalPadding="0" RootNodeStyle-HorizontalPadding="12" 
                            RootNodeStyle-ChildNodesPadding="0" LeafNodeStyle-HorizontalPadding="0" ParentNodeStyle-HorizontalPadding="0"  
                          ExpandImageUrl="~/images/right_arrow1f.jpg" CollapseImageUrl="~/images/down_ arrow1f.jpg"    
                        ExpandDepth="0" NodeWrap="true" BorderWidth="0px" ExpandImageToolTip=""
                            Font-Names ="Arial, Helvetica, sans-serif" Font-Size="11px" 
                            OnSelectedNodeChanged="TVProduct_SelectedNodeChanged" NodeStyle-Height="22" HoverNodeStyle-ForeColor="Gray"  Width="180" >
                            <ParentNodeStyle HorizontalPadding="0px" Width="100%" />
                            <HoverNodeStyle ForeColor="Gray" Width="100%" />
                            <SelectedNodeStyle />
                            <RootNodeStyle ChildNodesPadding="0px" HorizontalPadding="11px" Width="100%" />
                            <NodeStyle Height="22px"
                                HorizontalPadding="0px" Width="100%" />
                            <LeafNodeStyle HorizontalPadding="0px" Width="100%" />                      
                        </asp:TreeView>
                        </div>
 
 
            