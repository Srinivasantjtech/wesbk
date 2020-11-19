<%@ Control Language="C#" AutoEventWireup="true" Inherits="search_searchparametricfilter" Codebehind="searchparametricfilter.ascx.cs" %>
<%
 Response.Write(ST_ParametricFilters());
     %>
<!--Added js by M/A-->
<input type="hidden" name="hidAttrIds" runat="server" id="hidAttrIds" />
<input type="hidden" name="hdnFilterIds" runat="server" id="hdnFilterIds" />
<input type="hidden" name="hdnFilterType" runat="server" id="hdnFilterType" />

<script language="javascript">
    document.getElementById("<%=hidAttrIds.ClientID%>").value = "<%=sAttrIds%>";
    function GetSelectedItems() {

        var AttrIds = document.getElementById("<%=hidAttrIds.ClientID%>").value
        var mySplitResult = AttrIds.split(",");
        var SelAttrStr = '';
        for (var i = 0; i < mySplitResult.length - 1; i++) {
            for (var j = 0; j < document.getElementById(mySplitResult[i]).options.length; j++) {
                if (document.getElementById(mySplitResult[i]).options[j].selected) {
                    temp = document.getElementById(mySplitResult[i]).options[j].value;                    
                    SelAttrStr = SelAttrStr + mySplitResult[i] + "^" + temp + "|";
                }
            }
        }
        SelAttrStr = SelAttrStr.substring(0, SelAttrStr.length - 1);        
        if(SelAttrStr.length > 0)
        {        
          __doPostBack("PARAMETRICFILTER", SelAttrStr);
        }
        else
        {  
         alert("Please select atleast one item for filtering!"); 
            return false;
        }
    }
    
    function GetSelectedItemsFilter() {

        var AttrIds = document.getElementById("<%=hidAttrIds.ClientID%>").value
        var mySplitResult = AttrIds.split(",");
        var SelAttrStr = '';
        for (var i = 0; i < mySplitResult.length - 1; i++) {
            for (var j = 0; j < document.getElementById(mySplitResult[i]).options.length; j++) {
                if (document.getElementById(mySplitResult[i]).options[j].selected) {
                    temp = document.getElementById(mySplitResult[i]).options[j].value;
                    SelAttrStr = SelAttrStr + mySplitResult[i] + "^" + temp + "|";
                }
            }
        }
        SelAttrStr = SelAttrStr.substring(0, SelAttrStr.length - 1)        
        __doPostBack("PARAMETRICFILTER", SelAttrStr);
    }
    
    function __doPostBack(eventTarget, eventArgument) {
        
//            document.getElementById("__EVENTTARGET").value = eventTarget;
//            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            //alert(document.getElementById("__EVENTTARGET").value);
            document.getElementById("<%=hdnFilterType.ClientID%>").value = eventTarget;
            document.getElementById("<%=hdnFilterIds.ClientID%>").value = eventArgument;
            document.forms[0].submit();
        }


function GetSelItems(field) {

        var count=0;
        var sCategoryIds='';
        for (var j = 0; j < document.getElementsByName(field).length; j++)
        {            
            if(document.getElementsByName(field).item(j).checked==true)
            {
                sCategoryIds = sCategoryIds + document.getElementsByName(field).item(j).value + ",";
            }
        }

        sCategoryIds = sCategoryIds.substring(0, sCategoryIds.length - 1)
        if (sCategoryIds.length > 0) 
            __doPostBack('CATEGORYFILTER', sCategoryIds);
        else
            alert('Please select atleast one category for filtering');
 }

    function Reset() {
        var AttrIds = document.getElementById("<%=hidAttrIds.ClientID%>").value
        var mySplitResult = AttrIds.split(",");
        //alert(AttrIds);
        var SelAttrStr = '';
        for (var i = 0; i < mySplitResult.length - 1; i++) {
            for (var j = 0; j < document.getElementById(mySplitResult[i]).options.length; j++) {
                if (document.getElementById(mySplitResult[i]).options[j].selected) {
                    document.getElementById(mySplitResult[i]).options[j].selected=false;
                }
            }
        }
    }
</script>
