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
  function geturl()
  {
  var rootPath = window.location.protocol + "//" + window.location.host + "/";
  var e = document.getElementById("ddlcategory");
  var strURL = e.options[e.selectedIndex].text;
  var strURL1=e.options[e.selectedIndex].value;
  var fullPath=rootPath+strURL1;
  if(e.options[e.selectedIndex].text!='Select Category')
  {

  window.location.href=fullPath;
  }

  }

  function geturlsubcat()
  {
  var rootPath = window.location.protocol + "//" + window.location.host + "/";
  var f = document.getElementById("ddlsubcategory");
  var strURL = f.options[f.selectedIndex].text;
  var strURL1=f.options[f.selectedIndex].value;

  var fullPath=rootPath+strURL1;
  if(f.options[f.selectedIndex].text!='Select Category')
  {
  window.location.href=fullPath;
  }

  }
</script>
<table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" class="">
  <tr>
    <td valign="bottom">
      <div class="box2" >
        <table width="100%" >
          <tr>
            <td >
              <!--style="background-color:#EDEDEE; border: 1px solid #CCCCCC;border-radius: 3px 3px 3px 3px;"-->
              <div class="bybrant_top">
                <div class="bybrant_img">
                  <!--<img src="ProdImages$TBT_TOSUITE_MODEL_IMAGE$" style="max-height:150px;max-width:150px;" alt=""/>-->
                  <img data-original="ProdImages$TBT_TOSUITE_MODEL_IMAGE$" style="max-height:150px;max-width:150px;" alt="" class="lazy"/>
                </div>
                <div class="bybrant_txt">
                  <h1 style ="color: #0071CF;font-size: 18px;text-align:left;">$TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$&nbsp;Accessories&nbsp;Product&nbsp;List </h1>
                  <div class="cat_list_info" style="text-align:left;">See Below for Cellink's range of Accessories to suit the $TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$</div>
                  <!--<div class="tips">Tip! Narrow products using filter options on the left!</div>-->
                  $if(TBT_SHOW_FT)$
                  <div class="CategoryFilter">
                    <div id="Rcatdiv">
                      <span class="CategoryFiltertxt">Filter Products By Category:       </span>
                      <select class="CategoryFilterdrop" id="ddlcategory"  name="lst1" onchange="geturl();">
                        $TBWDataList1:{$it.TBWDataListItem1$}$
                      </select>
                    </div>
                    $if(TBT_SHOW_FTSCat)$
                    <div class="cl" style="height:3px;"></div>
                    <div id="Scatdiv">
                      <span class="CategoryFiltertxt" id="span1">Filter Products By SubCategory:</span>
                      <select class="CategoryFilterdrop1" id="ddlsubcategory" name="lst2" onchange="geturlsubcat();">
                        $TBWDataList2:{$it.TBWDataListItem2$}$
                      </select>
                      <div class="cl"></div>
                    </div>
                    $endif$
                    <p style="text-align:left;">Note. See Addtional Filtering Options in left side Navigation.</p>
                  </div>
                  $endif$
                </div>
                <div class="clear"></div>
              </div>


              
              <!--</td>
        </tr>
        <tr>
          <td style="background-color:#EDEDEE; border: 1px solid #CCCCCC;border-radius: 3px 3px 3px 3px;" width="700px">-->
              <div class="title6" style="margin-left:5px;margin-right:5px;" >
                <div style="width:260px;text-align:left;" class="push_left">
                  <strong>$TBW_CATEGORY_NAME$</strong>
                </div>
                
                <div style="float:right;" class="push_left">
                  <!--<div class="push_left">Product Display View :</div>-->
                  $if(TBW_VIEWTYPE)$
                  <a href="bybrand.aspx?$TBW_URL$" class="gridview type1 now"></a>
                  <a href="bybrand.aspx?$TBW_URL1$" class="gridview type2"></a>
                  $else$
                  <a class="gridview type1" href="bybrand.aspx?$TBW_URL$"></a>
                  <a class="gridview type2 now" href="bybrand.aspx?$TBW_URL1$"></a>
                  $endif$
                </div>
                <!--<div style="margin-right:15px;" class="push_right">

                </div>-->
                <div style="float:right;margin-right: 10px;">
                  <span style="float:left;">Sort By :</span>
                  <div class="sort_drop" style="float:right;">
                    <ul class="drop_menu">
                      <li>
                        <a >$SortBy$</a>
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
              </div>
              <div class="clear"></div>

              <div class="listingmenu" style="width:232px;text-align:left;">

              </div>


              <div class="clear"></div>
            </td>
          </tr>
          <tr>
            <td height="3px"></td>
          </tr>

        </table>
      </div>
      <div class="clear"></div>
    </td>
  </tr>
  <tr>

    <td >
      <br/>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="">
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


