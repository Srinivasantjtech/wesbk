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


<table align="center" width="775px" border="0" cellpadding="0" cellspacing="0" class="">

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
        <table width="100%">
          <tr >
            <td align="left">
              <div class="title3" style="margin-top:5px;margin-left:6px;margin-right:6px;">
                <div class="push_left" style="width:268px;">
                  $TBW_CATEGORY_NAME$
                </div >
           
                <div style="margin-left:0px;float:right;" class="push_left">
                  <!--<div class="push_left">Product Display View :</div>-->
                  $if(TBW_VIEWTYPE)$
                  <a href="categorylist.aspx?$TBW_URL$" class="gridview type1 now"></a>
                  <a href="categorylist.aspx?$TBW_URL1$" class="gridview type2"></a>
                  $else$
                  <a class="gridview type1" href="categorylist.aspx?$TBW_URL$"></a>
                  <a class="gridview type2 now" href="categorylist.aspx?$TBW_URL1$"></a>
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
               
              </div >
            </td>
          </tr>
         
        </table>
      </div >
      <div class="clear"></div>
    </td>
  </tr>
  <tr>
    <td>
      <br/>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="">
      <TABLE border="0" cellSpacing="0" cellPadding="0" width="775px">
        <tr>
          <td >
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
