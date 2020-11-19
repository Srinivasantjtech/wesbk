<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Top" Codebehind="Top.ascx.cs" %>
<%@ Register Src="CartItems.ascx" TagName="CartItems" TagPrefix="uc1" %>

<script type='text/javascript'>





   /* function init() {
        key_count_global = 0; 
        var vartime = null;

        document.getElementById("srcfield").onkeypress = function (e) {
            if (e == undefined)
                e = event;
   
            if (e != null) {

                key_count_global++;
                var id = document.getElementById("PSearchDiv");
                id.innerHTML = '<div ><img src="images/Invloading.gif"></img></div>';
                id.style.display = "block";
                vartime = setTimeout("lookup(" + key_count_global + "," + e.keyCode + ")", 1000); 
            }
        }
    }
    window.onload = init; 

    function lookup(key_count, keyCode) {

        if (key_count == key_count_global) { 
       
            if (keyCode != 13) {
                var SearchId = document.getElementById("srcfield")
                GetSearchProducts(SearchId)
            }
            else {
                urlredirect();
            }
        }

    }*/

</script>
<script>
    function GetSearchProducts(SearchId) {
        if (SearchId.value != "") {

            
            $.ajax({
                type: "POST",
                url: "GblWebMethods.aspx/GetSearchResultNew1",
                data: '{"Strvalue":"' + SearchId.value + '"}',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnajaxSuccess,
                error: OnajaxFailure


            });
        }
        else {
            var id = document.getElementById("PSearchDiv");
            toggleDiv(id, "none");
        }
    }



    function toggleDiv(Divid, dis) {
        Divid.style.display = dis;
    }

    function OnajaxSuccess(result) {

        var id = document.getElementById("PSearchDiv");
        if (result.d != "") {

            var sid = document.getElementById("srcfield");
            var s = result.d;
            id.innerHTML = result.d;

            toggleDiv(id, "block")
        }
        else {
            id.innerHTML = "No Result";
            toggleDiv(id, "block")
        }


    }
    function OnajaxFailure(result) {

        id.innerHTML = "Please try again later";
    }
    
  </script>

  <script language="javascript" >
      function ProductFilter() {
          
             // CallProductPopup(orgurl, buyvalue, pid, qtyval, 0, fid);
          // alert(orgurl);


          var url = "";
          var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";

              url = orgurl + "ProductFilter.aspx?popup=true&modal=true&width=970&height=420";

              tb_show(null, url, null);
      }
</script>
<%=ST_top() %>