<script type="text/javascript">

     function ttrim(stringToTrim) {
	   return stringToTrim.replace(" ","");
    }

     function urlredirect()
        {
            var ddlattrvalue=document.getElementById('srcfield').value;           
            if(ddlattrvalue!="")
            {
                if(ddlattrvalue!="Search WES! Enter Keywords or Part No's")
                {
                if(ttrim(ddlattrvalue)!="")
                {        
          var i=0;
          for(i=0;i<=ddlattrvalue.length;i++)
              ddlattrvalue= ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");
 
            window.document.location="powersearch.aspx?&srctext="+ ddlattrvalue.trim() + "";
            }
            }
            }
            }

            function FillVal(ctl)
            {

            if(ctl.value=='' || ctl.value==null || ttrim(ctl.value) == '')
            {
            ctl.value="Search WES! Enter Keywords or Part No's";

            var id = document.getElementById("PSearchDiv");
            id.style.display = "none";
            }
            }
            function Foc(ctl)
            {
            if(ctl.value=="Search WES! Enter Keywords or Part No's")
            {
            ctl.value='';
            }
            if(ctl.value!='')
            {

            var id = document.getElementById("PSearchDiv");
            //remove temp
            //id.style.display = "block";
            }
            }
            var hovered = false;
            function divMouseover(ctl)
            {
            hovered=true;
            }
            function divmouseout(ctl)
            {
            hovered=false;
            }
            function Focout(ctl)
            {
            if (!hovered) {
            var id = document.getElementById("PSearchDiv");
            id.style.display = "none";
            }


            }
            function Focout1(ctl)
            {

            var id = document.getElementById("PSearchDiv");
            id.style.display = "none";



            }

            function urlredirectK(e)
            {
           
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
            /* if (keychar == "@" || keychar == "!" || keychar == "~" || keychar == "`"  || keychar == "%" || keychar == "^"  || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == ";" || keychar == ":" || keychar == "," || keychar == "?" || keychar == "/" || keychar == "[" || keychar == "{" || keychar == "]" || keychar == "}" || keychar == "." || keychar == "*" || keychar == "\#" || keychar == "\$" || keychar == "\%" )
            {
            e.keyCode = '';
            return false;
            }
            else if (e.which == 64 || e.which == 126 || e.which == 33 || e.which == 64 || e.which == 35  || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 40 || e.which == 39 || e.which == 44 || e.which == 45 || e.which == 46 || e.which == 47 || e.which == 58 || e.which == 61 || e.which == 91 || e.which == 92 || e.which == 93 || e.which == 96 || e.which == 95 || e.which == 43 || e.which == 123 || e.which == 124 || e.which == 125 || e.which == 59 || e.which == 34 || e.which == 60 || e.which == 62 || e.which == 63 || e.which == 126)
            {

            return false;
            }*/

            var ddlattrvalue=document.getElementById('srcfield').value;

            if(ddlattrvalue!="")
            {
            if(ddlattrvalue!="Search WES! Enter Keywords or Part No's")
            {

            if(e.keyCode==13)
            {
            window.document.location="powersearch.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");
                          return false;
                  }
                }
              }
            }

          </script>
<table width="970px" height="100%"  border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="970px" align="center" valign="top" style="font-family: Arial, Helvetica, sans-serif; font-size: 100%; color: #666;">
      <div id="header">
        <div class="bg1">
          <div class="bg1inner">
            <div class="welcommsg">Welcome, &nbsp; $TBT_COMPANY_NAME$ &nbsp; $TBT_LOGIN_NAME$ &nbsp;</div>
            <div class="toplinks">
              
              <!--<a href="https://www.wes.com.au/media/ebook/wescat2017/" class="toplinkatest" >Online Catalogue</a>-->
              <a href="OnlineCatalogue_price.aspx" class="toplinkatest" >Online Catalogue</a>
              <a href="Agencies.aspx" class="toplinkatest" >Agencies</a>
              <a href="aboutus.aspx" class="toplinkatest" >About Us</a>
              <a href="contactus.aspx" class="toplinkatest" >Contact Us</a>
              <a href="logout.aspx" class="toplinkatest" >Log Out</a>
            </div>
          </div>
        </div >
        <div class="bg1">
          <div class="bg2inner">
            <div class="logo_outer" >
              <h1 class="brand" align="left">
                <a href="home.aspx" class="logomerge">
                  
                </a>
              </h1>
            </div>
             $if(TBT_STOP_ACCOUNT)$
            <div style="float:left; margin: 5px 0 0 0px;width: 685px;position: relative;display: inline-block;">
  <marquee scrollamount="5" style="font-family:Arial, Helvetica, sans-serif; color: #fa0101;display: inline-block;width: 685px;position: relative;overflow: hidden;">PLEASE NOTE YOUR ACCOUNT IS ON STOP DUE TO OUTSTANDING PAYMENTS.PLEASE CONTACT OUR ACCOUNTS DEPARTMENT ASAP TO RESOLVE THIS.EMAIL:accounts@wes.net.au PHONE:0297979866</marquee>
</div>
            $endif$ 
            <div class="search_outer" style="float:left; margin: 30px 0 0 60px;">
              <input name="srcfield" type="text" style="font-size:12px;font-family:arial;"  id="srcfield" class="topsearch"  value="Search WES! Enter Keywords or Part No's" onblur="FillVal(srcfield);Focout(srcfield)" onfocus="Foc(srcfield);"  onkeypress="return urlredirectK(event);"   autocomplete="off"/>
              <a style="cursor:pointer;"  class="hoverbtn searchbtn" onclick="urlredirect()"></a>
              <div class="searchdrop" id="PSearchDiv" style="Display:none" onmouseover="divMouseover();" onmouseout="divmouseout();" onmouseup="Focout()" >          
              </div>
              <!--onclick="ProductFilter()"-->
              <!--<a  href="/productfinder.aspx?tab=1" style="left: 142px;padding: 0;position: absolute;text-align: center;top: 42px;cursor: pointer;">Product Finder </a>-->  
            </div>                                
            <div class="user_outer">
              <a href="BulkOrder.aspx?txtcnt=27" class="iconbtn hoverbtn ico1" ></a>
              <a href="myaccount.aspx" class="iconbtn hoverbtn ico2" ></a>
              <a href="OnlineCatalogue_price.aspx" class="iconbtn hoverbtn ico3" ></a>
              <div class="clear"></div>
              <a class="hoverbtn chkout" id="cartcheckout"  href="$TBT_SHIP_URL$" ></a>
              <a class="cart2 hoverbtn" id="cartviewcart" href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=$TBT_ORDER_ID$">VIEW CART</a>
              <a class="cart1 hoverbtn"  href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=$TBT_ORDER_ID$">
                <span id="cartqty">($TBW_CART_ITEM$)</span>&nbsp;ITEM/S
              </a>
            </div>
            <div class="clear"></div>
          </div >
        </div >
        <div id="navcontainer">
          <ul>
            <!--$TBWDataList:{$it.TBWDataListItem$}$-->
            Cache_Top_Replace
            <li class="default">
              <a href="categorylist.aspx?&ld=0&cid=WESNEWS&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1jeqpQhfYbVHmDiGDdpCNZ" class="last redbtn">
                WES NEWS
                <br/>
                NEW PRODUCTS
              </a>
            </li >
           <li class="default">
              <a href="categorylist.aspx?&ld=0&cid=WES-CLR&byp=0" class="last yellowbtn">
                CLEARANCE <br/>
                PRODUCTS
              </a>
            </li >
            <li class="default">
              <a href="Productspl.aspx" class="last bluebtn">
                PRODUCT <BR/>
                SPECIALS
              </a>
            </li >
          </ul>
        </div>
        <div class="clear"></div>
      </div >
    </td>
  </tr>
</table>