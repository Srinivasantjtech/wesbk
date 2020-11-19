<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_CatalogueDownload" Codebehind="CatalogueDownload.ascx.cs" %>
<% Response.Write(ST_PDFDownload()); %>


<%--<script type="text/javascript">
    function ebookpopitup(url) {
        alert(url);
        var str = url;
        if (str.search("attachments") != -1) {

            url = url;
            
        }
        else {
            url = "/attachments" + url;
           
        }
        newwindow = window.open(url, 'name', 'height=400,width=600,left=400,top=200');
        if (window.focus) { newwindow.focus() }
        return false;
    }
</script>--%>