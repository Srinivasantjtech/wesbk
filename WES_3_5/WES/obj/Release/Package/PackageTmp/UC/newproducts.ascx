<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_newpeoducts" Codebehind="newproducts.ascx.cs" %>
<script src="/Scripts/AC_RunActiveContent.js" type="text/javascript"></script>
<script language="javascript" >
    function productbuy(buyvalue, pid) {
        var qtyval = 1;

//        var qtyavail = document.forms[0].elements[buyvalue].name;
//        qtyavail = qtyavail.toString().split('_')[1];
//        var minordqty = document.forms[0].elements[buyvalue].name;
//        minordqty = minordqty.toString().split('_')[2];

        var fid = buyvalue;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";

        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 ) {
            alert('Invalid Quantity!');
            //window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            //document.forms[0].elements[buyvalue].focus();
            return false;
        }
        else {
            // window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval;
            CallProductPopup(orgurl, buyvalue, pid, qtyval, 0, fid);
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
<%=ST_Newproduct()%>