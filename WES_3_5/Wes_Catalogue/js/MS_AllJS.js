
function productbuy(buyvalue, pid) {
        try {


            var qtyval = document.forms[0].elements[buyvalue].value;
          
            var qtyavail = document.forms[0].elements[buyvalue].name;
            qtyavail = qtyavail.toString().split('_')[1];
            var minordqty = document.forms[0].elements[buyvalue].name;
            minordqty = minordqty.toString().split('_')[2];

            var fid = document.forms[0].elements[buyvalue].name;
            fid = fid.toString().split('_')[3];

            // var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";
            //   var orgurl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
            var pathname = window.location.pathname; // Returns path only
          
            var orgurl = window.location.href.replace(pathname, "") + "/";

            if ((isNaN(qtyval)
             || qtyval == ""
             || qtyval <= 0
             || qtyval.indexOf(".") != -1) && (pathname.indexOf("/mct/") == -1)) {
                alert('Invalid Quantity!');
                //  window.document.forms[0].elements[buyvalue].style.borderColor = "red";
                document.forms[0].elements[buyvalue].focus();
                return false;
            }
            else {
                var tOrderID = '<%=Session["ORDER_ID"]%>';
                if (pathname.indexOf("/mct/") != -1) {

                    qtyval = 1;
                }
                if (tOrderID != null && parseInt(tOrderID) > 0) {
                  
                    CallProductPopupMS(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
                else {
                    
                    CallProductPopupMS(orgurl, buyvalue, pid, qtyval, tOrderID, fid);
                }
            }
        }
        catch (e) {
            alert(e);
        }
    }






    $(".dropdown").hover(
        function () {
            $('.dropdown-menu', this).stop().fadeIn("fast");
        },
        function () {
            $('.dropdown-menu', this).stop().fadeOut("fast");
        });




    $('.navbar .dropdown').hover(function () {
        $(this).addClass('open').find('.dropdown-menu').first().stop(true, true).show();
    }, function () {
        $(this).removeClass('open').find('.dropdown-menu').first().stop(true, true).hide();
    });


function submenuClick(SUBmenu)
{    
     var className = $(SUBmenu).attr('class');     
     $("#MsubProducts").children("li").removeClass("open")     
     if( className=="brands open" )
     {
         $(SUBmenu).removeClass("open")
     }
     else
     {
          $(SUBmenu).addClass("open")
     }


}





 $("#Mproducts").click(function () {
        
       var className = $("#Mproducts").attr('class');     
       if( className=="products open" )
         {
             hideallmenu();
             $("#MsubProducts").children("li").addClass("open")
         }
         else
         {
                  hideallmenu();
            $("#Mproducts").removeClass("products")
            $("#Mproducts").addClass("products open")
            $("#products_menu").addClass("open")
           
         }
       
       
    });






     $("#products_menu").mouseover(function () {
        hideallmenu();
        $("#Mproducts").removeClass("products")
        $("#Mproducts").addClass("products open")
        $("#products_menu").addClass("open")
    });
   
   
   $("#products_menu").mouseout(function () {
        hideallmenu();
        
    });
    $("#MAccount").click(function () {


          var className = $("#MAccount").attr('class');     
       if( className=="stores open" )
         {
             hideallmenu();
         }
         else
         {
                 hideallmenu();
        $("#MAccount").removeClass("stores")
        $("#MAccount").addClass("stores open")
        $("#my_account").addClass("open")
         }

       
    });

     $("#my_account").mouseover(function () {
        hideallmenu();
        $("#MAccount").removeClass("stores")
        $("#MAccount").addClass("stores open")
        $("#my_account").addClass("open")
    });
     $("#my_account").mouseout(function () {
        hideallmenu();
        
    });
     $("#MAboutus").mouseover(function () {
        hideallmenu();
        
    });
     $("#MAboutus").mouseout(function () {
        hideallmenu();
        
    });
      $("#MContact").mouseover(function () {
        hideallmenu();
        
    });
     $("#MContact").mouseout(function () {
        hideallmenu();
        
    });
     $("#MCart").click(function () {


          var className = $("#MCart").attr('class');     
       if( className=="cart open" )
         {
             hideallmenu();
         }
         else
         {
                 hideallmenu();
        $("#MCart").removeClass("cart")
        $("#MCart").addClass("cart open")
        $("#cart").addClass("open")
         }

       
    });
   
     $("#cart").mouseover(function () {
        hideallmenu();
        $("#MCart").removeClass("cart")
        $("#MCart").addClass("cart open")
        $("#cart").addClass("open")
    });
     $("#cart").mouseout(function () {
        hideallmenu();
        
    });
    function hideallmenu()
    {
              $("#Mproducts").removeClass("products open")
              $("#Mproducts").addClass("products")

              $("#products_menu").removeClass()
              

              $("#MAccount").removeClass("stores open")
              $("#MAccount").addClass("stores")

              $("#my_account").removeClass()

              $("#MCart").removeClass("cart open")
              $("#MCart").addClass("cart")

              $("#cart").removeClass()
    }


    $(".amenu .category").mouseover(function () {
        $(".amenu .sub-menu").hide();
        $($(this).children(".sub-menu")).show();
    });

    $(".amenu .sub-menu").mouseout(function () {
        $(this).hide();
    });
    $(".amenu .category hidemenu-list").mouseover(function () {
        $(".amenu .sub-menu").hide();
        $($(this).children(".sub-menu")).show();
    });



    $('.def-html').darkTooltip({
        opacity: 1,
        gravity: 'south'
    });



   function goBack() {
    window.history.back()
}





     jQuery(document).ready(function () {
         $("img.lazy").lazyload();
     });

     (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "load", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);






    $(document).ready(function () {

        $('.scrollup').click(function () {
            $("html, body").animate({ scrollTop: 100 }, 600);
            return false;
        });
    });
  function ttrim(stringToTrim) {
	   return stringToTrim.replace(" ","");
    }
    function GetDeal() {
       
        var m = document.getElementById('txtMail1').value;    

        window.location.href = '/mGetDeal.aspx?mail=' + m;
    }
 function urlredirectKMS(e) {

        var keynum
        var keychar
        var numcheck
        if (window.event) {
            keynum = e.keyCode
        }
        else if (e.which) {
            keynum = e.which
        }
        keychar = String.fromCharCode(keynum)
       

        var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");
         var supName = document.getElementById('SupplierName').value;
        if (ddlattrvalue != "") {
            if (ddlattrvalue != "Search") {

                if (e.keyCode == 13) {
                 
                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplaceMS",                        
                        data: "{'strvalue':'" + ddlattrvalue + "','suppName':'" + supName + "'}",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                window.location.href = "/" + data.d + "/mps/";
                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });


                    return false;
                }
            }
        }
    }

     function urlredirectMS() {
    try{
        var ddlattrvalue = document.getElementById('srcfield').value.replace("\"", "`~");
        var supName = document.getElementById('SupplierName').value;


        if (ddlattrvalue != "") {
            if (ddlattrvalue != "Search..") {
                if (ttrim(ddlattrvalue) != "") {




                 
                    $.ajax({
                        type: "POST",
                        url: "/GblWebMethods.aspx/stringreplaceMS",                        
                        data: "{'strvalue':'" + ddlattrvalue + "','suppName':'" + supName + "'}",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                
                                window.location.href = "/" + data.d + "/mps/";

                            }
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                        }



                    });

                   

                   
                }
            }
        }
    }
    catch (e) {
        alert(e.ToString());
    }
    }


    $('.dropdown li').click(function (e) {
        e.stopPropagation();
    });
    $('.dropdown-inner').click(function (e) {
        e.stopPropagation();
        $(this).toggleClass('open').trigger('shown.bs.dropdown');
    });


    $(document).ready(function () {


        $("#topnavbar").mouseover(function () {
            $("#megadropmenu").css({ 'display': 'none' });
        });

        $("#main-menu li").mouseover(function () {
            var className = $(this).attr('class');
            if (className == "category hidemenu-list") {

                $("#megadropmenu").removeClass("dropdown-menu mega-dropdown-menu row open_menu");
                $("#megadropmenu").addClass("dropdown-menu-nominwidth mega-dropdown-menu-nowidth row border_none");
                $("#main_drop_menu").addClass("megamenu_border");
                $("#menu_heading").addClass("mar_right_none");
          
            }
            else {
                $("#megadropmenu").removeClass("dropdown-menu-nominwidth mega-dropdown-menu-nowidth row border_none");
                $("#megadropmenu").addClass("dropdown-menu mega-dropdown-menu row");
                $("#main_drop_menu").removeClass("megamenu_border");
                $("#menu_heading").removeClass("mar_right_none");
            }

        });

        $("#megadropmenu").hover(function () {
            $("#megadropmenu").css({ 'display': 'block' });
        },
       function () {
           $("#megadropmenu").css({ 'display': 'none' });
       });
        $("#mdd").mouseover(function () {
            $("#megadropmenu").css({ 'display': 'block' });
        });
    });


    


    
     function keyct(e) {
         var keyCode = (e.keyCode ? e.keyCode : e.which);
         if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

         }
         else {
             e.preventDefault();
         }
     }



    
