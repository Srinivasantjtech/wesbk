<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_productlist" Codebehind="productlist.ascx.cs" %>
<%--<%@ Register Src="~/search/search.ascx" TagName="search" TagPrefix="uc1" %>--%>
<%@ Register Src="~/search/searchrsltproductfamily.ascx" TagName="searchrsltproductfamily" TagPrefix="uc4" %>
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

    </script>
    <%-- <script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>--%>
    <%-- <script type="text/javascript" src="../scripts/jquery-1.10.2.min.js"></script>--%>
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
            $('#tblload').toggle();
            $('#tblload').show();
            var eapath = $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_htmleapath").val();          
            var iTotalPages = $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_htmltotalpages").val();            
            var iTpages = parseInt(iTotalPages);            
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            var ViewMode = $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_htmlviewmode").val();
            var irecords = $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_htmlirecords").val();
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "product_list.aspx/DynamicPag",
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
            var x = $("#" + '<%= HiddenField1.ClientID %>').val();
            var y = $(window).scrollTop();
            var z = $('#tblmain').outerHeight();
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            var eapath = $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_htmleapath").val();
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
    }

</script>
  <table id="tblmain" align="left" style="width: 790px;height:auto">
    <tr>
    <td align="left" style="margin:0 5px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" onload="Getfidfromurl();">
        <tr>
          <td align="left" class="tx_1">
          <div class="br">
            <%
                Response.Write(Bread_Crumbs());
            %>
         </div>
          </td>
        </tr>
        <tr>
        <td height="3px"></td>
        </tr>   
      </table>         
             <div class="box1" >
             <table>
             <tr>
             <td>
                                          <uc4:searchrsltproductfamily ID="searchrsltproductfamily1" runat="server" />                                                 
                                 </td>
                              </tr>   
                              <tr>                              
                              <td>                         
                               <div class="divLoadData">
                                    </div>                                  
       </td></tr>        
          <tr>
      <td align="center">
      <table id="tblload" style="display:none;" width="325px"" >
   <tr>
      <td align="center">
      
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

                      </table>                 
             
                                            
         </div>

          </td>
      </tr>
    
      <tr>
        <td width="760" height="35" vAlign="top">
   
          <table id="data" width="100%" bgColor="#f2f2f2" border="0" cellSpacing="0" cellPadding="0" style="display:none;" class="box2">
            <tr>
              <TD height="35" width="156" align="left">
                <div class="listingmenu">
                
                </div >
              </TD>
              <td width="303" align="middle">
                <div class="listingmenu" style="width:232px;">
              
                </div >
              </td>
              <td width="281" align="right">
                <div class="listingmenu push_right listingnave" style=" float: right;">
                  <table  style="VERTICAL-ALIGN: top;float:right;">
                    <tr>
                      <td class="">
                       
                      </td>
                    </tr>
                  </table>
                </div >
              </td>
            </tr>
          </table>   
      </td>
      </tr>
    </table>
    <table >
    <tr>
    <td>
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
