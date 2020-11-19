<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_specialproducts" Codebehind="Productspl.ascx.cs" %>
<%--<% Response.Write(ST_PDFDownload()); %>--%>

<script language="javascript">
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
    </script>
  <table>
       
        <tr>
            <td >
              <% Response.Write(ST_Specialproduct()); %>
            </td>
        </tr>
    </table>