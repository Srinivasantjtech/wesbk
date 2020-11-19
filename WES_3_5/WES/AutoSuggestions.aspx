<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoSuggestions.aspx.cs" Inherits="AutoSuggestions" %>
<%--
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html>
<body>
<asp:repeater runat="server" id="repeaterSearch">    
    <ItemTemplate>
<table>
<tr>
<td valign="top" style="width:75%">

<asp:repeater runat="server" id="RepeaterAutosuggestion"  DataSource='<%# GetRepeaterAutosuggestion()%>'>    
<HeaderTemplate>
<div class="clear"></div>
<div class="viewmoresmall"><strong>Search suggestions for "<%# Eval("input") %>"</strong></div>
<div class="clear"></div>
<ul>
</HeaderTemplate>
<ItemTemplate>
<li>
<a href='/powersearch.aspx?srctext=<%# Eval("val") %>' ><%# Eval("val") %></a>
</li>
</ItemTemplate>
<FooterTemplate>
</ul>
</FooterTemplate>
</asp:repeater>

<asp:repeater runat="server" id="RepeaterProduct" DataSource='<%# GetRepeaterProduct()  %>'>    
<HeaderTemplate>
<div class="viewmoresmall"><strong>Products</strong></div>
</HeaderTemplate>
<ItemTemplate>
<div class="drop_products">
<img width="80" height="80" alt="img" src='<%# GetImagePath(Eval("Family_Thumbnail"),Eval("Prod_Thumbnail")) %>'>
<a href='/productdetails.aspx?pid=<%#Eval("Prod_Id") %>&amp;fid=<%#Eval("Family_Id") %>&amp;cid=<%# Eval("CATEGORY_ID")%>&amp;path=<%# GetEAPath(Eval("CATEGORY_PATH"),Eval("Family_Id"),"")%>'><strong><%# Eval("Family_name")%></strong></a>
<p> <%# Eval("Prod_Description") %> </p>
<div style="Color:red">Code :<strong><%# Eval("Prod_Code")%></strong> &nbsp;&nbsp;&nbsp;&nbsp;Price :<strong style="red">$ <%# Eval("Price")%></strong>
</div>
<div class="clear"></div>
</div>
</ItemTemplate>
</asp:repeater>
</td>
<td valign="top" style="width:60%">
<asp:repeater runat="server" id="RepeaterCategory" DataSource='<%# GetRepeaterAttr()  %>' >    
<HeaderTemplate>
</HeaderTemplate>
<ItemTemplate>
<div class="viewmoresmall"><strong><%# Eval("name") %></strong></div>
<asp:repeater runat="server" id="RepeaterAttrDetail" DataSource='<%# GetRepeaterAttrDetail(Eval("attribute_id")) %>'>    
<HeaderTemplate>
<ul>
</HeaderTemplate>
<ItemTemplate>
<li>
<a href='powersearch.aspx?&amp;id=0&amp;searchstr=<%# Eval("input") %>&amp;type=Category&amp;value=<%# Eval("name") %>&amp;bname=&amp;byp=2&amp;Path=<%# GetEAPath("","","Cat") %>' style="font-size:10px"><%# Eval("name") %></a>
</li>
</ItemTemplate>
<FooterTemplate>
</ul>
</FooterTemplate>
</asp:repeater>
<a style='color:#0099FF;text-decoration:none;font-size:9px;display:<%# GetRepeaterAttrFooter(Eval("attribute_id"))%>;' href='PowerSearch.aspx?srctext=<%# Eval("input") %>'>View All Results</a>
</ItemTemplate>

</asp:repeater>
</td>
</tr>
<tr>
<td colspan="2">
<%--<div class="clear"></div>--%>
<a class="viewmore" href='PowerSearch.aspx?srctext=<%# Eval("input") %>'>View All Results</a>
<%--<div class="clear"></div>--%>
</td>
</tr>
</table>
</ItemTemplate>
</asp:repeater>
</body>
</html>