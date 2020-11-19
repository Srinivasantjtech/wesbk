<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="search_ProductFinderPL" Codebehind="ProductFinderPL.ascx.cs" %>
<input id="HidItemPage" type="hidden" runat="server" />
<input type="hidden" id="hdnFamilyId" runat="server"/>
    <input type="hidden" id="__EVENTTARGET" name="__EVENTTARGET" value="" runat="server"/>
    <input type="hidden" id="__EVENTARGUMENT" name="__EVENTARGUMENT" value="" runat="server" />
<script type="text/javascript">
    function __doPostBack1(eventTarget, eventArgument) {
        document.getElementById("<%=__EVENTTARGET.ClientID%>").value = eventTarget;
        document.getElementById("<%=__EVENTARGUMENT.ClientID%>").value = eventArgument;
        document.forms[0].submit();
    }

    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }
    

//    function GotoAttPage(attid,Attname) {
//        var val = document.getElementById(attid).value;
//        if (val != Attname) {
//            location = form.select.options[index].value;
//            document.getElementById('res').value = "Thanks for selecting cat";
//        } else {
//            document.getElementById('res').value = "please select CAT";
//        }
//        var index = form.select.selectedIndex
//        if (form.select.options[index].value != "0") {
//            location = form.select.options[index].value;
//        }
//    }

    </script>
    <%-- <script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>--%>
    <%-- <script type="text/javascript" src="../scripts/jquery-1.10.2.min.js"></script>--%>
<script type="text/javascript">
    $(document).ready(function () {


        var className = $("#progrid_wrapper .product-items").attr('class');
        if (className.indexOf("list-group-item") != -1) {

            $('#progrid_wrapper .product-items').addClass('list-group-item');
            $('#grid').removeClass('lightblue');
            $('#list').addClass('lightblue');

        }
        else {

            $('#progrid_wrapper .product-items').removeClass('list-group-item');
            $('#list').removeClass('lightblue');
            $('#grid').addClass('lightblue');
        }

        $(window).bind('pageshow', function () {
            // update hidden input field

            $("#" + '<%= HiddenField2.ClientID %>').val("0");
            $("#" + '<%= HiddenField1.ClientID %>').val("0");
            $("#" + '<%= hfcheckload.ClientID %>').val("0");

            // update hidden input field
            var hfback = $("#" + '<%= hfback.ClientID %>').val();

            if ($.browser.msie) {
                if ($.browser.version != "11.0") {

                    $("#" + '<%= HFcnt.ClientID %>').val("1");
                    hfback = "0";
                }
            }
            if (hfback == 1) {
                var hfbackdata = $("#" + '<%= hfbackdata.ClientID %>').val();

                $('.divLoadData:last').before(hfbackdata);
                //                jQuery(document).ready(function () {
                //                    $("img.lazy").lazyload();
                //                });
                var scrollpos = $("#" + '<%= hfscrollpos.ClientID %>').val();
                $(window).scrollTop(scrollpos);
            }
            else {

                $("#" + '<%= HFcnt.ClientID %>').val("1");
                var pgno = $("#" + '<%= HFcnt.ClientID %>').val();


            }

        });

        function lastPostFunc() {
            $("#" + '<%= hfcheckload.ClientID %>').val("1");
            $('#tblload').toggle();
            $('#tblload').show();
           // var eapath = $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmleapath").val();
           // var iTotalPages = $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmltotalpages").val();
            var eapath = $("#" + '<%= htmleapath.ClientID %>').val();
            var iTotalPages = $("#" + '<%= htmltotalpages.ClientID %>').val();
            var iTpages = parseInt(iTotalPages);
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            //var ViewMode = $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmlviewmode").val();
            //var irecords = $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmlirecords").val();
            var ViewMode = $("#" + '<%= htmlviewmode.ClientID %>').val();
            var irecords = $("#" + '<%= htmlirecords.ClientID %>').val();

            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "ProductFinder.aspx/DynamicPag",
                data: "{'strvalue':'" + hforgurl + "','ipageno':" + hfpageno + ",'iTotalPages':" + iTpages + ",'eapath':'" + eapath + "','ViewMode':'" + ViewMode + "','irecords':'" + irecords + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "LOGIN") {

                        window.location.href = "Login.aspx";
                    }
                    else if (data.d != "") {

                        $('.divLoadData:last').before(data.d);
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");



                        $("#" + '<%= hfback.ClientID %>').val("1");
                        var hfpageno1 = $("#" + '<%= HFcnt.ClientID %>').val();
                        var data1 = "";
                        if (hfpageno1 > 2) {
                            var data1 = $("#" + '<%= hfbackdata.ClientID %>').val();
                            data1 = data1 + data.d;
                        }
                        else {
                            data1 = data.d;
                        }


                        $("#" + '<%= hfbackdata.ClientID %>').val(data1);
                        //                        jQuery(document).ready(function () {
                        //                            $("img.lazy").lazyload();
                        //                        });

                        var className = $("#progrid_wrapper .product-items").attr('class');
                        if (className.indexOf("list-group-item") != -1) {

                            $('#progrid_wrapper .product-items').addClass('list-group-item');
                            $('#grid').removeClass('lightblue');
                            $('#list').addClass('lightblue');

                        }
                        else {

                            $('#progrid_wrapper .product-items').removeClass('list-group-item');
                            $('#list').removeClass('lightblue');
                            $('#grid').addClass('lightblue');
                        }
                    }
                    else {

                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#data').toggle();
                        $('#data').show();
                       
                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    // alert(err);
                }
            })
        };
        $(window).scroll(function () {
            var className = $("#progrid_wrapper .product-items").attr('class');
            if (className.indexOf("list-group-item") != -1) {

                $('#progrid_wrapper .product-items').addClass('list-group-item');
                $('#grid').removeClass('lightblue');
                $('#list').addClass('lightblue');

            }
            else {

                $('#progrid_wrapper .product-items').removeClass('list-group-item');
                $('#list').removeClass('lightblue');
                $('#grid').addClass('lightblue');
            }

            var x = $("#" + '<%= HiddenField1.ClientID %>').val();
            var y = $(window).scrollTop();
            var z = $('#tblmain').outerHeight();
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            //var eapath = $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmleapath").val();
            var eapath = $("#" + '<%= htmleapath.ClientID %>').val();
            z = z - (z / 2);
            if ($(window).scrollTop() >= z) {
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        if (checkload == "0") {
                            if (eapath != "") {
                                lastPostFunc();
                            }
                        }
                    }
                }
            }
            else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        if (checkload == "0") {
                            if (eapath != "") {
                                lastPostFunc();
                            }
                        }
                    }
                }
            }
        });
    });
</script>

<div id="tblmain" align="left" style="width: 790px;height:auto;display:table;">
  <%--<tr>
    <td align="left" style="margin:0 5px;">--%>
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
     


<table id="tblload" style="display:none;" width="800px" >
   <tr>
      <td align="center">
      
      <div width="300px" align="center"><img src="images/bigLoader.gif" width="40px" height ="40px"></div>
     </td>
      </tr>
      <tr>
      <td align="center">
      <asp:Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT" 
              Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" ></asp:Label> 
      </td>
      </tr>
      </table>

      <%--    </td>
  </tr>--%>
</div>

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

 <asp:HiddenField ID="HiddenField2" runat="server" />
         <asp:HiddenField ID="HiddenField1" runat="server" />
          <asp:HiddenField ID="HFcnt" runat="server" />
                    <asp:HiddenField ID="hfcheckload" runat="server" />
                    <asp:HiddenField ID="hforgurl" runat="server" />
   <asp:HiddenField ID="hfback" runat="server" />
                       <asp:HiddenField ID="hfbackdata" runat="server" />
                    <asp:HiddenField ID="hfscrollpos" runat="server" />