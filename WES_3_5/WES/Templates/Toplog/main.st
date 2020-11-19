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
          window.document.location="powersearch.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26");
          }
          }
          }
          }

         function FillVal(ctl)
          {
          if(ctl.value=='' || ctl.value==null || ttrim(ctl.value) == '')
          {
          ctl.value="Search WES! Enter Keywords or Part No's";
          }
          }

          function Foc(ctl)
          {
          if(ctl.value=="Search WES! Enter Keywords or Part No's")
          {
          ctl.value='';
          }
          }
          function Focout(ctl)
          {
         
          }
          function urlredirectK(e)
          {
          var ddlattrvalue=document.getElementById('srcfield').value;
          if(ddlattrvalue!="")
          {
          if(ddlattrvalue!="Search WES! Enter Keywords or Part No's")
          {
          if(e.keyCode==13)
          {
          window.document.location="powersearch.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26");
          return false;
          }
          }
          }
          }
        </script>
<table width="100%" height="100%"  border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td style="font-family: Arial, Helvetica, sans-serif; font-size: 100%;">
      <div id="header">
        <div class="bg1">
          <div class="bg1inner">
            <div class="welcommsg"></div>
         
            <div class="toplinks">
              <a href="OnlineCatalogue_price.aspx" class="toplinkatest" >Online Catalogue</a>
              <!--<a href="http://www.wes.com.au/mediapub/ebook/wescat2017NP/" class="toplinkatest" >Online Catalogue</a>-->
              <a href="Agencies.aspx" class="toplinkatest">Agencies</a>              
              <a href="aboutus.aspx" class="toplinkatest">About Us</a>
              <a href="contactus.aspx" class="toplinkatest">Contact Us</a>
              <a href="login.aspx" class="toplinkatest">Login</a>
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
            <div class="search_outer" style="float:left; margin: 30px 0 0 60px;">
              <input name="srcfield" type="text" id="srcfield" style="font-size:12px;font-family:arial;" class="topsearch" value="Search WES! Enter Keywords or Part No's" onblur="FillVal(srcfield);" onfocus="Foc(srcfield);" onkeypress="return urlredirectK(event);" onfocusout="Focout(srcfield);" autocomplete="off"  />                                       
              <a style="cursor:pointer;"  class="hoverbtn searchbtn"  onclick="urlredirect()" ></a>
              
            </div>            
            <div class="user_outer">
              <a href="BulkOrder.aspx?txtcnt=25" class="iconbtn  ico1" ></a>
              <a href="myaccount.aspx" class="iconbtn  ico2" ></a>
              <a href="CatalogueDownload.aspx" class="iconbtn  ico3" ></a>
              <div class="clear"></div>
              <a class="hoverbtn chkout" href="shipping.aspx" ></a>
              <a class="cart2 hoverbtn"  href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=$TBT_ORDER_ID$">VIEW CART</a>
              <a class="cart1 hoverbtn"  href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=$TBT_ORDER_ID$">
                <span id="cartqty">($TBW_CART_ITEM$)</span>&nbsp;ITEM/S
              </a >
            </div>
            <div class="clear"></div>
          </div>
        </div>
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