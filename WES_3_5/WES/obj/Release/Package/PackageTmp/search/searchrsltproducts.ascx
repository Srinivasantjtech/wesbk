<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="search_searchrsltproducts" Codebehind="searchrsltproducts.ascx.cs" %>
  
<input id="HidItemPage" type="hidden" runat="server" />
<input id="Hidcat" type="hidden" runat="server" />
<% 
    string strval=null;
    string requrl = Request.Url.OriginalString.ToString().ToUpper();
    if (requrl.Contains("BYBRAND.ASPX"))
    {
        strval = ST_BrandAndModelProductListNewJson();
    }
    else
        strval = ST_ProductListJson();
    
    if (strval != null && strval != string.Empty)
    {
        if (strval != "CLEAR")
        {
            Response.Write(strval);
        }
    }
    else
    {
        if (requrl.Contains("PRODUCT_LIST.ASPX") || requrl.Contains("BYBRAND.ASPX") || requrl.Contains("BYPRODUCT.ASPX"))
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">Right now no products for sale from this category.</div>");
        else
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">No Products were found that match your selection.</div>");
    }
   
   
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
</script>
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }
        function productbuy(buyvalue,pid){   
        var qtyval = document.forms[0].elements[buyvalue].value;
        var qtyavail= document.forms[0].elements[buyvalue].name;
        qtyavail=qtyavail.toString().split('_')[1];        
        var minordqty= document.forms[0].elements[buyvalue].name;
        minordqty = minordqty.toString().split('_')[2];

        var fid = document.forms[0].elements[buyvalue].name;
        fid = fid.toString().split('_')[3];
        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";
        if( isNaN(qtyval) || qtyval =="" || qtyval <= 0 || qtyval.indexOf(".")!=-1)
          {    
                alert('Invalid Quantity!');
                window.document.forms[0].elements[buyvalue].style.borderColor = "red";
                document.forms[0].elements[buyvalue].focus();
                return false;
          } 
         else
         {
            var tOrderID = '<%=Session["ORDER_ID"]%>';

            if (tOrderID != null && parseInt(tOrderID) > 0) 
            {
                //window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid='+pid+'&Qty=' + qtyval + "&ORDER_ID=" + tOrderID;
                CallProductPopup(orgurl,buyvalue, pid, qtyval, tOrderID, fid);
            }
            else 
            {
                //window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid='+pid+'&Qty=' + qtyval;
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
<input type="text" name="pseapath" id="htmlpseapath" runat="server" style="display:none;"/>
<input type="text" name="psbceapath" id="htmlpsbceapath" runat="server" style="display:none;"/>
<input type="text" name="psTotalpages" id="htmlpstotalpages" runat="server" style="display:none;"/>
<input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
<input type="text" name="bceapath" id="htmlbceapth" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpage" runat="server" style="display:none;"/>
<input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="psViewMode" id="htmlpsviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>
<input type="text" name="psirecords" id="htmlpsirecords" runat="server" style="display:none;"/>