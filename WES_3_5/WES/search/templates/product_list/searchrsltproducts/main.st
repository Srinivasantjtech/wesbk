<script language="javascript">
  var IE = document.all?true:false;
  var MouseX = 0;
  var MouseY = 0;
  if (!IE)
  {
  document.captureEvents(Event.MOUSEMOVE);
  document.onmousemove = getMouseXY;
  }
  function ShowPriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "visible";
  if (IE)
  {
  getMouseXY();
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = MouseY - (objDiv.clientHeight + 25) + 'px';
  }
  else
  {
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = (MouseY - (objDiv.offsetHeight + 50)) + 'px';
  }
  objDiv.style.zIndex = 1;
  objDiv.style.padding = "5px";
  }
  function getMouseXY(e)
  {
  if (IE) { // grab the x-y pos.s if browser is IE
  tempX = event.clientX + document.documentElement.scrollLeft;
  tempY = event.clientY + document.documentElement.scrollTop;
  } else {  // grab the x-y pos.s if browser is NS
  tempX = e.pageX;
  tempY = e.pageY;
  }
  MouseX = tempX;
  MouseY = tempY;
  }
  function ClosePriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "hidden";
  }
</script>
<table align="left" width="775px" border="0" cellpadding="0" cellspacing="0" class="">
  <tr>
    <td align="center" style="width:758px;">
      <div class="box1" style="height:108px;width:758px;">
        <div style="float:left;" >
          <!--<img src="prodimages$TBTC_IMAGE_FILE$" alt="" class="push_left imgstyle1"/>-->
          <img alt="" data-original="prodimages$TBTC_IMAGE_FILE$" style="background: none repeat scroll 0 0 #ffffff;border: 1px solid #e0e0e0;border-radius: 4px;margin: 0 10px 0 0;padding: 3px;float: left;" class="lazy"/>
          <div style="width:500px; float:left">
            <h6 style="font-size: 12px;text-align: left;">$TBTC_CATEGORY_NAME$</h6>
            <p class="p2 push_left" >$TBTC_SHORT_DESC_CAT$ </p>
          </div>
        </div >
        <div class="push_right"  style="width:120px;margin:20px 0 0 0;display:$TBT_DISPLAY_LINK$;" >
          <p class="p2 push_left" >
            <a href="$PDF$" target="_blank" class="downpdf" style="display:$TBT_DISPLAY_PDF_LINK$;"></a>
          </p>
          <p class="p2 push_left"  >
            <a href="$TBT_EBOOK_LINK$" target="_blank" class="viewpdf" style="display:$TBT_DISPLAY_EBOOK_LINK$;"></a >
          </p>
        </div>
        <div class="clear"></div>

      </div >
    </td>
  </tr>
    $if(TBW_SUB_CAT_IMAGE)$
    <tr style="display:block;">
        <td style="width:758px;">
            <div class="box1" style="height:108px;width:758px; border:none;">
                 $TBWDataList1:{
            $it.TBWDataListItem1$}$
            </div>
        </td>
    </tr>
     $endif$
  <tr>
    <td valign="bottom">
      <div class="box2">
        <table width="760px">
          <tr >
            <td align="center" style="width:758px;">
              <div class="title3" style="margin-top:5px;margin-left:6px;margin-right:6px;">
                <div class="push_left" style="width:300px;" align="left">
                  $TBW_CATEGORY_NAME$
                </div >
                
                <div style="margin-left:0px;float:right;" class="push_left" >
                  <!--<div class="push_left">Product Display View :</div>-->
                  $if(TBW_VIEWTYPE)$
                  <a href="product_list.aspx?$TBW_URL$" class="gridview type1 now"></a>
                  <a href="product_list.aspx?$TBW_URL1$" class="gridview type2"></a>
                  $else$
                  <a class="gridview type1" href="product_list.aspx?$TBW_URL$"></a>
                  <a class="gridview type2 now" href="product_list.aspx?$TBW_URL1$"></a>
                  $endif$                 
                </div>
                <div style="float:right;margin-right: 10px;">
                  <span style="float:left;">Sort By :</span>
                  <div class="sort_drop" style="float:right;">
                    <ul class="drop_menu">
                      <li>
                        <a>$SortBy$</a>
                        <ul>
                          <li>
                            <a onclick="SetSortOrder('catalog');">Relevance</a>
                          </li>
                          <li>
                            <a onclick="SetSortOrder('latest');">Latest</a>

                          </li>
                          <li>
                            <a onclick="SetSortOrder('ltoh');">Price Low To High</a>
                          </li>
                          <li>
                            <a onclick="SetSortOrder('htol');">Price High To Low</a>
                          </li>
                          <li>
                            <a onclick="SetSortOrder('popularity');">Popular</a>
                          </li>
                        </ul>
                      </li>


                    </ul>
                  </div>
                </div>
                <!--<div style="display:$TBT_DISPLAY_LINK$;">
                  <a href="$TBT_EBOOK_LINK$" class="gridview type3" target="_blank" style="display:$TBT_DISPLAY_EBOOK_LINK$;" ></a>
                  <a href="$PDF$" class="gridview type4" target="_blank" style="display:$TBT_DISPLAY_PDF_LINK$;"></a>
                </div>-->
              </div >
            </td>
          </tr>
        </table>
      </div >
    </td>
  </tr>
    
  <tr>
    <td height="126" align="left" class="">
      <TABLE border="0" cellSpacing="0" cellPadding="0" width="775px">
        <tr>
          <td>
            $TBWDataList:{
            $it.TBWDataListItem$}$
            <div class="divLoadData">
            </div>
          </td>
        </tr>
      </TABLE>
    </td>
  </tr>
</table>