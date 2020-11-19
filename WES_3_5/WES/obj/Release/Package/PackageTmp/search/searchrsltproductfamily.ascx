<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="search_searchrsltproductfamily" Codebehind="searchrsltproductfamily.ascx.cs" %>
<input id="HidItemPage" type="hidden" runat="server" />
<%
    string st_productList = ST_ProductListJson();
    if (st_productList != null && st_productList != string.Empty)
        Response.Write(st_productList);
    else
        if (Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">Right now no products for sale from this category.</div>");
        else
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">No Products were found that match your selection.</div>");
%>
<script language="javascript">
    function GetSelectedIts() {
        var mySplitResult = "pppopt";
        var SelAttrStr = '';
        for (var j = 0; j < document.getElementById(mySplitResult).options.length; j++) {
            if (document.getElementById(mySplitResult).options[j].selected) {
                temp = document.getElementById(mySplitResult).options[j].value;
                document.getElementById("<%=HidItemPage.ClientID%>").value = temp;
                
            }
        }       
        document.forms[0].submit();
    }

    function productbuy(buyvalue, pid) {
       

        var qtyval = document.forms[0].elements[buyvalue].value;
        var qtyavail = document.forms[0].elements[buyvalue].name;
        qtyavail = qtyavail.toString().split('_')[1];
        var minordqty = document.forms[0].elements[buyvalue].name;
        minordqty = minordqty.toString().split('_')[2];

        var fid = document.forms[0].elements[buyvalue].name;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";

        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
            alert('Invalid Quantity!');
            window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            document.forms[0].elements[buyvalue].focus();
            return false;
        }
        else {
            var tOrderID = '<%=Session["ORDER_ID"]%>';

            if (tOrderID != null && parseInt(tOrderID) > 0) {
                //window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval + "&ORDER_ID=" + tOrderID;
                CallProductPopup(orgurl,buyvalue, pid, qtyval, tOrderID, fid);

            }
            else {
                //window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval;
                CallProductPopup(orgurl,buyvalue, pid, qtyval, tOrderID, fid);

            }

        }

    }
   
    function keyct(e) {
        var keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

        }
        else {
            e.preventDefault();
        }
    }
</script>
<input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpages" runat="server" style="display:none;"/>
<input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>