<div class="product_top">
  <div class="family_img" id="lboximgpopup">
    <div id="product-image" class="gallery-view" style="height:210px:vertical-align: middle;" >
      <a href="/prodimages$TBT_TFWEB_LIMAGE$">
        <!--<img style="display: block;max-height:256px;max-width:256px" src="prodimages$TBT_TFWEB_IMAGE1$"  alt=""/>-->
        <img style="display: block;max-height:256px;max-width:256px" data-original="prodimages$TBT_TFWEB_IMAGE1$"  alt="" class="lazy" id="popupmaindel"/>
      </a>
    </div>

    <div class="product-info">
      <div class="brand"></div>
      <div class="gallery-thumbnails">
        <div id="gallery" style="margin-top:25px">
          <ul>
            $TBT_MULTIIMAGES:
            <li width="50" height="50">
              <a href="prodimages$it.LargeImage$">
                <!--<img src="prodimages$it.Thumpnail$" alt="" style="max-width:50px;max-height:50px"/>-->
                <img data-original="prodimages$it.Thumpnail$" alt="" style="max-width:50px;max-height:50px" class="lazy"/>
              </a>
            </li>}$
          </ul>
        </div>
      </div>
    </div>
  </div>
  <div class="family_txt">
    <h1 class="pad10" style=" color: #666666;font-size: 16px;text-align:left;" id="familyName">$TBT_FAMILY_NAME$</h1>
    <p>
      $TBT_SHORT_DESCRIPTION$
    </p>
    <div id="div1" style="color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0;min-height:162px;">
      <p>
        $TBT_MORE$
      </p>
    </div>
    <div id="div2" style="display:none;color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0;min-height:162px;">
      <p>
        $TBT_DESCALL$
      </p>
    </div>
    $if(TBT_MORE_SHOW)$
    <div id="preview" >
      <div id="smo" class="show_hide12">
        <a class="redmoew" >Read More! Show Full Description </a>
      </div>
      <div id="lmo" class="show_hide12" style="margin:10px 0px;">
        <a class="redmoewless"  >Show Less Description</a>
      </div>
    </div>
    $endif$
    <br/>
    <a class="btnfamily" href="family.aspx?&amp;fid=$TBT_FAMILY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;Path=$TBT_EA_PATH$">$TBT_PROD_COUNT$ of $TBT_FAMILY_PROD_COUNT$ Products Shown | View All Products in this Family</a>
  </div>
  </div>
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