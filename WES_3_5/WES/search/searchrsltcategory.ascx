<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_searchrsltcategory" Codebehind="searchrsltcategory.ascx.cs" %>
<% Response.Write(ST_Categories());%>

<script language="javascript">
function GetSelectedCat() {
    var ind = document.getElementById("category_filter1").selectedIndex;    
    window.document.location = document.getElementById("category_filter1").value;
}
</script>