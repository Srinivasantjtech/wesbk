<%@ Page Language="C#" MasterPageFile="~/MainPage.master" AutoEventWireup="true" Inherits="Login" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="Login.aspx.cs"   %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
 

</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
<script type="text/javascript">
    function init() {
        var url = document.location;
        SetUrl(url)
    }    
    window.onload = init;
    function SetUrl(x) {
        if (x != "") {
            $.ajax({
                type: "POST",
                url: "GblWebMethods.aspx/SetURL",
                data: '{"url":"' + x + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnajaxSuccess1,
                error: OnajaxFailure1
            });
        }

    }
    function OnajaxSuccess1(result) {}
    function OnajaxFailure1(result) {       }   
</script>
</asp:Content>