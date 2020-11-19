<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_categorylist" Codebehind="categorylist.ascx.cs" %>
<%@ Register Src="newproducts.ascx" TagName="newproducts" TagPrefix="uc1" %>
<input id="HidItemPage" type="hidden" runat="server" />

<%--<script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>
<script type="text/javascript" src="../scripts/jquery-1.10.2.min.js"></script>--%>
<script type="text/javascript">
    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }
 </script>

<script type="text/javascript">

    $(document).ready(function () {

        //var pageshow = 'pageshow';

        $(window).bind('pageshow', function () {
       // $(window).load('pageshow', function () {
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
                jQuery(document).ready(function () {
                    $("img.lazy").lazyload();
                });
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

            var eapath = $("#ctl00_maincontent_Categorylist1_htmleapath").val();
            var iTotalPages = $("#ctl00_maincontent_Categorylist1_htmltotalpages").val();
            var ViewMode = $("#ctl00_maincontent_Categorylist1_htmlviewmode").val();
            var irecords = $("#ctl00_maincontent_Categorylist1_htmlirecords").val();
            var iTpages = parseInt(iTotalPages);
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "CategoryList.aspx/DynamicPag",
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

                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });


                    }

                    else {

                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#databottom').toggle();
                        $('#databottom').show();

                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                }
            })
        };

        $(window).scroll(function () {

            var x = $("#" + '<%= HiddenField1.ClientID %>').val();

            var y = $(window).scrollTop();

            var z = $('#tblmain').outerHeight();
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            z = z - (z / 2);
            //            alert("y" + y);
            //            alert("z" + z);
            if ($(window).scrollTop() >= z) {

                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    //alert("inside scroll");
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        // alert("inside x");
                        if (checkload == "0") {
                            //alert("inside checkload");
                            lastPostFunc();
                        }
                    }
                }
            }
            else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                // alert("inside scroll");
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        if (checkload == "0") {
                            lastPostFunc();
                        }
                    }
                }

            }
        });
    });   
</script>
<script  type="text/javascript">

    function ImgLoad(v) {
        var img2 = document.getElementById("imgCable2");
        img2.style.display = v;
    } 
    function GetRecords(attv) {
        ImgLoad("block");
        $.ajax({
            type: "POST",
            url: "categorylist.aspx/FindAnotherEnd",
            data: "{'strvalue':'" + attv + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            failure: function (response) {
                ImgLoad("none");
                //alert(response.d);
            },
            error: function (response) {
                ImgLoad("none");
                //alert(response.d);
            }
        });

    }
    function OnSuccess(response) {
        

        var Cables;
        var CableList = document.getElementById("Cable2");
        for (var option in CableList) {
            CableList.remove(option);
        }
        AddOption(CableList, "Select All");
        ImgLoad("none");
        if (response.d != null) {
            var Cables = response.d.split("#####");

            for (var i = 0; i < Cables.length; i++)
                AddOption(CableList, Cables[i]);
        }
        
      /*  var xmlDoc = $.parseXML(response.d);
        var xml = $(xmlDoc);
        var CableList = document.getElementById("Cable2");

        for (var option in CableList) {
            CableList.remove(option);
        }
      
        AddOption(CableList,"Select All");

        var Cables = xml.find("Plug 2");
   
        Cables.each(function () {
      
            var Cable = $(this);
      
            AddOption(CableList, Cable.find("Plug 2").text());

        });*/
    }
    function AddOption(CableList,xvalue) {
        var opt = document.createElement('option');
        opt.value = xvalue;
        opt.innerHTML = xvalue;

        CableList.appendChild(opt);

    }
 

    function GetSelectedCable(field) {
        var SelAttrStr = '';
        if (field == "Cable1") {
            for (var j = 0; j < document.getElementById(field).options.length; j++) {
                if (document.getElementById(field).options[j].selected) {
                    if (document.getElementById(field).options[j].value != 'Select Plug1' && document.getElementById(field).options[j].value != 'Select All') {
                        SelAttrStr = document.getElementById(field).options[j].value
                        GetRecords(SelAttrStr);

                    }
                }

            }
        }
    }
    function CalbeRedirect(response) {
        if( response.d!=null && response.d!="")
            window.location.href = response.d;  

    }
    function GetCableFinderURL() {

        var cable1 = "";
        var cable2 = "";
        var f1;
        var f2;

            f1 = document.getElementById("Cable1");
            for (var j = 0; j < f1.options.length; j++) {
                if (f1.options[j].selected) {
                    if (f1.options[j].value != 'Select Plug1' && f1.options[j].value != 'Select All') {
                        cable1 = f1.options[j].value


                    }
                }

            }
        

            f2 = document.getElementById("Cable2");
            for (var j = 0; j < f2.options.length; j++) {
                if (f2.options[j].selected) {
                    if (f2.options[j].value != 'Select Plug1' && f2.options[j].value != 'Select All') {
                        cable2 = f2.options[j].value


                    }
                }

            }
       

        $.ajax({
            type: "POST",
            url: "categorylist.aspx/GetFindMyCableURL",
            data: "{'Cable1':'" + cable1 + "','Cable2':'" + cable2 + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: CalbeRedirect,
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });

    }
    
</script>
    <% 
    if ((Request.Url.ToString().ToLower().Contains("categorylist.aspx")) && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
    {%>

        <table width="764px" align="center" >
  <%}
    else
    {%>        
      <table width="782px" align="center" >         
  <%}%>  
    <tr>
    <td align="center">
         <table width="100%" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
        <tr>
          <td align="left" class="tx_1">   
           <div class="br" style="margin-left:-3px;" >         
            <%
                Response.Write(Bread_Crumbs());
            %>
            </div> 
          </td>
        </tr>   
      </table>
      </td>
      </tr>
      <tr>
       <td height="3px">
       </td>
      </tr>
     </table>
      
<% Response.Write(ST_CategoryList()); %>

<% 
    if ((Request.Url.ToString().ToLower().Contains("categorylist.aspx")) && Request.QueryString["tsb"] == null )
    {
        %>
<div style="margin: 0 0 0 4px;" class="box1"> 
       <table id="tblmain">              
               <tr>
               <td >
                 <%   Response.Write(ST_CategoryProductList());  %>
                  <div class="divLoadData">
                                    </div> 
               </td>
               </tr>
               <tr>
               <td>
               
               <table id="tblload" style="display:none;" width="325px" align="center">
    <tr>
      <td>
      
      <div width="300px" align="center"><img src="images/bigLoader.gif" width="12%" height ="12%"></div>
      </td>
      </tr>
      <tr>
      <td align="center">
      <asp:Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT"
              Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" ></asp:Label> 
      </td>
      </tr>
      </table> 
               </td>
               
               </tr>

               <tr>
               <td>
              <table id="databottom" width="775px" bgColor="#f2f2f2" border="0" cellSpacing="0" cellPadding="0" style="display:none;" class="box2">
          <tr>
            <td height="35" width="156" align="left">
              
            </td>
            <td width="303" align="middle">
             
            </td>
            <td width="175" align="right">
             
            </td>
          </tr>
        </table>
        <asp:HiddenField ID="HiddenField2" runat="server" />
         <asp:HiddenField ID="HiddenField1" runat="server" />
          <asp:HiddenField ID="HFcnt" runat="server" />
                    <asp:HiddenField ID="hfcheckload" runat="server" />
                    <asp:HiddenField ID="hforgurl" runat="server" />
                     <input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
                     <input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpages" runat="server" style="display:none;"/>
             <asp:HiddenField ID="hfback" runat="server" />
                       <asp:HiddenField ID="hfbackdata" runat="server" />
                <asp:HiddenField ID="hfscrollpos" runat="server" />
               </td> 
               </tr> 
               </table>


     
</div> 
<table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" >
  <tr>
    <td width="70%">&nbsp;</td>
    <td width="28%" align="right">
      <a href="#" class="scrollup" >Top</a>
    </td>
    <td width="2%">&nbsp;</td>
  </tr>
</table>
<%    }
  
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

             <script type="text/javascript" language="javascript">


                 function SetSortOrder(orderVal) {
                     var url = window.location.href;
                     $.ajax({
                         type: "POST",
                         url: "/GblWebMethods.aspx/SetSortOrder",
                         data: "{'orderVal':'" + orderVal + "','url':'" + url + "'}",
                         contentType: "application/json;charset=utf-8",
                         dataType: "json",
                         success: function (result) {
                             if (result.d == "1") {
                                 window.location = url;
                             }
                         },
                         error: function (result) {
                             //rtn = false;
                         }


                     });
                     return false;
                     //OnCaptchaSuccess,
                 }

</script>

<script>


    $(document).ready(function () {

        $(document).scroll(function () {

            var footerSelector = '.container_cmn';
            var socialBarSelector = '.fixed';

            var bottomViewPort = $(window).scrollTop() + $(window).height();
            var footerTop = $(footerSelector).offset().top;



            if (bottomViewPort >= footerTop + 200) {
                //$(socialBarSelector).fadeOut();
                //alert ( bottomViewPort);
                //alert ( footerTop);

                $(".fixed").css("display", 'none');
                //$(".fixed").css("overflow",'auto' );
                //$('.fixed').css("top",'25px'); 


            } else {
                //$(socialBarSelector).fadeIn();
                $(".fixed").css("display", 'block');
                // $(".fixed").css("height",'auto' );

                $(".fixed").css("overflow-y", 'auto');
            }
        });

    });


    /*
    function sticky_relocate() {




    var window_top = jQuery(this).scrollTop();
    var div_top = jQuery('.leftwrapper').offset().top;
    var footer_top = jQuery('.container_cmn').offset().top;

    if (window_top > div_top) {


    $(".fixed").css("height",'450px' );
    $(".fixed").css("overflow-y",'hidden' );


    //jQuery('#fixed-toolbar').addClass('fixed');
    } else {


    $(".fixed").css("overflow-y",'visible' );

    //jQuery('#fixed-toolbar').removeClass('fixed');
    }

    if (window_top > footer_top) {

    $(".fixed").css("overflow-y",'visible' );

    //jQuery('#fixed-toolbar').removeClass('fixed');
    }
	

    }

    jQuery(function () {
    jQuery(window).scroll(sticky_relocate);
    sticky_relocate();
    });			  


    */

    $(function () {

        var $window = $(window);
        var lastScrollTop = $window.scrollTop();
        var wasScrollingDown = true;

        var $sidebar = $("#Category");
        if ($sidebar.length > 0) {

            var initialSidebarTop = $sidebar.position().top;

            $window.scroll(function (event) {

                var windowHeight = $window.height();
                var sidebarHeight = $sidebar.outerHeight();
                //alert(windowHeight +"  "+sidebarHeight);

                var scrollTop = $window.scrollTop();
                var scrollBottom = scrollTop + windowHeight;

                var sidebarTop = $sidebar.position().top;
                var sidebarBottom = sidebarTop + sidebarHeight;

                var heightDelta = Math.abs(windowHeight - sidebarHeight);
                var scrollDelta = lastScrollTop - scrollTop;

                var isScrollingDown = (scrollTop > lastScrollTop);
                var isWindowLarger = (windowHeight > sidebarHeight);

                if ((isWindowLarger && scrollTop > initialSidebarTop) || (!isWindowLarger && scrollTop > initialSidebarTop + heightDelta)) {
                    $sidebar.addClass('fixed');
                } else if (!isScrollingDown && scrollTop <= initialSidebarTop) {
                    $sidebar.removeClass('fixed');
                }

                var dragBottomDown = (sidebarBottom <= scrollBottom && isScrollingDown);
                var dragTopUp = (sidebarTop >= scrollTop && !isScrollingDown);

                if (dragBottomDown) {
                    if (isWindowLarger) {
                        $sidebar.css('top', 15);
                    } else {
                        $sidebar.css('top', -heightDelta);
                    }
                } else if (dragTopUp) {
                    $sidebar.css('top', 15);
                } else if ($sidebar.hasClass('fixed')) {



                    var lastScroll = 0;
                    $(window).scroll(function (event) {


                        //alert($(".fixed").get(0).scrollHeight);

                        /* Sets the current scroll position
                        var st = $(this).scrollTop();
	
                        //Determines up-or-down scrolling
                        //if (st > lastScroll){
                        //$(".fixed").css("display",'inline')
                        //} 
                        if(st == 0){
                        $(".fixed").css("display",'none')
                        }
                        //Updates scroll position
                        lastScroll = st;*/
                    });

                    /*jQuery(function($) {
                    $('.fixed').on('scroll', function() {
                    if($(this).scrollTop() + $(this).innerHeight() == $(this)[0].scrollHeight) {
                    $(".fixed").css("overflow-y",'hidden')
                    }
                    else {
                    $(".fixed").css("overflow-y",'visible')
                    }
                    })
                    });*/


                    $(window).scroll(function () {
                        if ($(this).scrollTop() > 200) {
                            $('.fixed').css("top", '15px');
                        }


                        /*else { 
                        $('.fixed').stickySidebar({
                        sidebarTopMargin: 180,
                        footerThreshold: 100
                        }); 
                        }*/
                    });



                    /*			  
			  
                    $(window).scroll(function() { 
                    if($(window).scrollTop() + $(window).height() == $(document).height()) {
                    $(".fixed").css("height",'250px' );
                    $(".fixed").css("overflow-y",'hidden' );
                    }
                    else {
                    $(".fixed").css("height",'250px' );
                    $(".fixed").css("overflow-y",'visible')
                    }
                    });
	
                    */

                    $(window).scroll(function () {
                        if ($(window).scrollTop() >= (($(document).height() - $(window).height()) - $('.fixed').innerHeight())) {
                            console.log('div reached');
                        }
                    });

                    var currentTop = parseInt($sidebar.css('top'), 10);

                    var minTop = -heightDelta;
                    var scrolledTop = currentTop + scrollDelta;

                    var isPageAtBottom = (scrollTop + windowHeight >= $(document).height());
                    var newTop = (isPageAtBottom) ? minTop : scrolledTop;

                    $sidebar.css('top', newTop);
                }

                lastScrollTop = scrollTop;
                wasScrollingDown = isScrollingDown;
            });
        }
    });

</script>
<%--<script language="javascript" type="text/javascript">


    $(document).ready(function () {

        $(document).scroll(function () {

            var footerSelector = '.container_cmn';
            var socialBarSelector = '.fixed';

            var bottomViewPort = $(window).scrollTop() + $(window).height();
            var footerTop = $(footerSelector).offset().top;

            if (bottomViewPort >= footerTop) {
                //$(socialBarSelector).fadeOut();

                $(".fixed").css("height", '250px');
                $(".fixed").css("overflow", 'hidden');
                //$('.fixed').css("top",'25px'); 


            } else {
                //$(socialBarSelector).fadeIn();
                $(".fixed").css("height", 'auto');

                $(".fixed").css("overflow", 'visible');
            }
        });

    });



    $(function () {

        var $window = $(window);
        var lastScrollTop = $window.scrollTop();
        var wasScrollingDown = true;

        var $sidebar = $("#Category");
        if ($sidebar.length > 0) {

            var initialSidebarTop = $sidebar.position().top;

            $window.scroll(function (event) {

                var windowHeight = $window.height();
                var sidebarHeight = $sidebar.outerHeight();
                //alert(windowHeight +"  "+sidebarHeight);

                var scrollTop = $window.scrollTop();
                var scrollBottom = scrollTop + windowHeight;

                var sidebarTop = $sidebar.position().top;
                var sidebarBottom = sidebarTop + sidebarHeight;

                var heightDelta = Math.abs(windowHeight - sidebarHeight);
                var scrollDelta = lastScrollTop - scrollTop;

                var isScrollingDown = (scrollTop > lastScrollTop);
                var isWindowLarger = (windowHeight > sidebarHeight);

                if ((isWindowLarger && scrollTop > initialSidebarTop) || (!isWindowLarger && scrollTop > initialSidebarTop + heightDelta)) {
                    $sidebar.addClass('fixed');
                } else if (!isScrollingDown && scrollTop <= initialSidebarTop) {
                    $sidebar.removeClass('fixed');
                }

                var dragBottomDown = (sidebarBottom <= scrollBottom && isScrollingDown);
                var dragTopUp = (sidebarTop >= scrollTop && !isScrollingDown);

                if (dragBottomDown) {
                    if (isWindowLarger) {
                        $sidebar.css('top', 15);
                    } else {
                        $sidebar.css('top', -heightDelta);
                    }
                } else if (dragTopUp) {
                    $sidebar.css('top', 15);
                } else if ($sidebar.hasClass('fixed')) {



                    var lastScroll = 0;
                    $(window).scroll(function (event) {


                        //alert($(".fixed").get(0).scrollHeight);

                        /* Sets the current scroll position
                        var st = $(this).scrollTop();
	
                        //Determines up-or-down scrolling
                        //if (st > lastScroll){
                        //$(".fixed").css("display",'inline')
                        //} 
                        if(st == 0){
                        $(".fixed").css("display",'none')
                        }
                        //Updates scroll position
                        lastScroll = st;*/
                    });

            


                    $(window).scroll(function () {
                        if ($(this).scrollTop() > 200) {
                            $('.fixed').css("top", '15px');
                        }


                        /*else { 
                        $('.fixed').stickySidebar({
                        sidebarTopMargin: 180,
                        footerThreshold: 100
                        }); 
                        }*/
                    });


                    $(window).scroll(function () {
                        if ($(window).scrollTop() >= (($(document).height() - $(window).height()) - $('.fixed').innerHeight())) {
                            console.log('div reached');
                        }
                    });

                    var currentTop = parseInt($sidebar.css('top'), 10);

                    var minTop = -heightDelta;
                    var scrolledTop = currentTop + scrollDelta;

                    var isPageAtBottom = (scrollTop + windowHeight >= $(document).height());
                    var newTop = (isPageAtBottom) ? minTop : scrolledTop;

                    $sidebar.css('top', newTop);
                }

                lastScrollTop = scrollTop;
                wasScrollingDown = isScrollingDown;
            });
        }
    });

</script>--%>

<%--<script type="text/javascript" language="javascript">
    (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);
</script>--%>