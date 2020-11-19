<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_bybrand" Codebehind="bybrand.ascx.cs" %>
<%@ Register Src="~/search/searchrsltproducts.ascx" TagName="searchrsltproducts" TagPrefix="uc4" %>
<input id="HidItemPage" type="hidden" runat="server" />
<input id="Hidcat" type="hidden" runat="server" />
<%--<script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>--%>
<script type="text/javascript">
    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }
 </script>
<script type="text/javascript">
    $(document).ready(function () {

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
            $('#tblload').show();
            var iTotalPages = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmltotalpage").val();
            var iTpages = parseInt(iTotalPages);
            var eapath = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmleapath").val();
            var ViewMode = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmlviewmode").val();
            var irecords = $("#ctl00_maincontent_bybrand1_searchrsltproducts2_htmlirecords").val();
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            var prevpageno = hfpageno;
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "bybrand.aspx/DynamicPag",
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
            // alert("scrollTop" + y);
            var z = $('#tblmain').outerHeight();
            // alert("outerHeight" + z);
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            z = z - (z / 2);

            if ($(window).scrollTop() >= z) {

                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();

                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);


                    if (x == "0") {

                        lastPostFunc();

                    }
                }



            }
            else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();

                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);


                    if (x == "0") {

                        lastPostFunc();
                    }
                }

            }


        });



    });
  
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
               }


         });
         return false;
     }

</script>
<script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }
    </script>
    <input type="hidden" id="hdnFamilyId" runat="server">   
    <input type="hidden" name="__EVENTTARGET" value="">
    <input type="hidden" name="__EVENTARGUMENT" value="">   
    <table id="tblmain" align="center" style="width: 558; height:auto">
    <tr>
    <td align="center">
    <% if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX")) { %>
        <table width="780" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
    <% } else{ %>        
        <table width="558" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
    
    <%} %>
    
        <tr>
            
          <td align="left" class="tx_1">            
           
                <div class="" style="margin:0 0 0 4px;">          
               <%
        Response.Write(Bread_Crumbs());
                %>
                </div> 
          </td>
        </tr>
        <tr>
        <td height="3px">
        
        </td>
        </tr>
     
     </table>
      </td>
      </tr>
        <tr>
            <td>
       
              <div class="box1" style=" margin: 0px 0px 0px 2px; height:100%;" >    
                <uc4:searchrsltproducts ID="searchrsltproducts2" runat="server" /> 
               <table>
              
             
               <tr>
               <td align="center">             
               <table id="tblload" style="display:none;" width="775px" >
    <tr>
      <td align="center">
      
      <div width="300px" align="center"><img src="images/bigLoader.gif" width="6%" height ="6%"></div>
      </td>
      </tr>
      <tr>
      <td align="center">
      <asp:Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT " 
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

               </td> 
               </tr> 
               </table>
                                           
              </div>
                 <asp:HiddenField ID="HiddenField2" runat="server" />
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                 <asp:HiddenField ID="HFcnt" runat="server" />
                                                <asp:HiddenField ID="hfcheckload" runat="server" />
                                                <asp:HiddenField ID="hforgurl" runat="server" />
            
             <asp:HiddenField ID="hfback" runat="server" />
                       <asp:HiddenField ID="hfbackdata" runat="server" />
             <asp:HiddenField ID="hfscrollpos" runat="server" />
              </td>
              </tr>
    
    </table>
   
   <table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" >
  <tr>
    <td width="70%">&nbsp;</td>
    <td width="28%" align="right">
      <a href="#" class="scrollup" >Top</a>
    </td>
    <td width="2%">&nbsp;</td>
  </tr>
</table>