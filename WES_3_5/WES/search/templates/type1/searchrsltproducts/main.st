
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
  function searchurl() {
  //alert("yes");
  var ddlattrvalue = document.getElementById('txtSearch').value;
  // alert(ddlattrvalue);
  if (ddlattrvalue != "") {
  if (ttrim(ddlattrvalue) != "") {
  //alert("11");
  //document.getElementById('txtSearch').value = ddlattrvalue;
  window.document.location = 'powersearch.aspx?srctext='+ ddlattrvalue;
  }
  }
  else {
  // alert("Keyword cannot be empty !");
  return false;
  }

  }

  function searchKeyPress(e)
  {
  // look for window.event in case event isn't passed in
  if (typeof e == 'undefined' && window.event) { e = window.event; }
  if (e.keyCode == 13)
  {
  searchurl();
  }
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
<div>
  $TBT_SPLCORRECTION$
</div>
<div class="box2" style="width:762px;">
  <div class="searchpagesearch"  >
    <input type="text" id="txtSearch" Class="topsearch1" placeholder="Search WES! Enter Keywords or Part No's" value="$TBT_TXTSEARCH$" onkeypress="searchKeyPress(event);"></input>
    <input type="Button" ID="btnSearch" runat="server" Class="hoverbtn searchbtn" style="border:0px;cursor:pointer;"  OnClick="return searchurl();"></input>
  </div>
  $if(TBT_SHOW_FT)$
  <div class="" style="margin-left: 61px;width:480px;">
    <div id="Rcatdiv">
      <span class="psfiltertxt">Filter Products By Category :</span>
      <select class="psfilter1" id="ddlcategory"  name="lst1" onchange="geturl();" >
        $TBWDataList1:{$it.TBWDataListItem1$}$
      </select>
    </div>
    $if(TBT_SHOW_FTSCat)$
    <div class="cl" style="height:4px;"></div>
    <div id="Scatdiv">
      <span class="psfiltertxt1" id="span1" >Filter Products By SubCategory :</span>
      <select class="psfilter2" id="ddlsubcategory" name="lst2" onchange="geturlsubcat();" >
        $TBWDataList2:{$it.TBWDataListItem2$}$
      </select>
      <div class="cl"></div>
    </div>
    $endif$
    <p style="text-align:left;margin-left: 5px;">Note. See Addtional Filtering Options in left side Navigation.</p>
  </div>
  $endif$
  <div style="height:5px;"></div>
  <!--<table width="760px" >
    <tr>
      <td style="background-color:#EDEDEE;" width="760px">-->
        <div class="title6" style="margin-left:4px;margin-right:4px;" >

          <div style="width:260px" class="push_left">
            <strong>Search Results</strong>
          </div>

          
          <div style="float:right;" class="">
            <!--<div class="push_left">Product Display View :</div>-->
            $if(TBW_VIEWTYPE)$
            <a href="powersearch.aspx?$TBW_URL$" class="gridview type1 now"></a>
            <a href="powersearch.aspx?$TBW_URL1$" class="gridview type2"></a>
            $else$
            <a class="gridview type1" href="powersearch.aspx?$TBW_URL$"></a>
            <a class="gridview type2 now" href="powersearch.aspx?$TBW_URL1$"></a>
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
                      <a onclick="SetSortOrder('relevance');">Relevance</a>
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

        <div class="listingmenu push_right listingnave" style="float:right;width:273px;margin-top:7px 0px;">
          <table  style="VERTICAL-ALIGN: top;float:right;font-size:12px;">
            <tr>
              <td class="">

              </td>
            </tr>
          </table>
        </div>

        <div class="clear"></div>
      <!--</td>
    </tr>
    <tr>
      <td height="3px"></td>
    </tr>

  </table>-->
</div>
<table align="left" width="775px" border="0" cellpadding="0" cellspacing="0" class="">
  <!--<tr>
    <td valign="bottom">
      <div class="box2"  style="margin-top:4px;">
        <table width="760px" >
          <tr>
            <td style="background-color:#EDEDEE;" width="760px">
              <div class="title6" style="margin-left:4px;margin-right:4px;" >
             
                <div style="width:181px" class="push_left">
                  <strong>Search Results</strong>
                </div>
                <div style="margin-left:3px" class="push_left">
                  <div class="push_left">Product Display View :</div>
                  $if(TBW_VIEWTYPE)$
                  <a href="powersearch.aspx?$TBW_URL$" class="gridview type1 now"></a>
                  <a href="powersearch.aspx?$TBW_URL1$" class="gridview type2"></a>
                  $else$
                  <a class="gridview type1" href="powersearch.aspx?$TBW_URL$"></a>
                  <a class="gridview type2 now" href="powersearch.aspx?$TBW_URL1$"></a>
                  $endif$
                 
                </div>
               
              </div>
              <div class="clear"></div>

              <div class="listingmenu push_right listingnave" style="float:right;width:273px;margin-top:7px 0px;">
                <table  style="VERTICAL-ALIGN: top;float:right;font-size:12px;">
                  <tr>
                    <td class="">

                    </td>
                  </tr>
                </table>
              </div>

              <div class="clear"></div>
            </td>
          </tr>
          <tr>
            <td height="3px"></td>
          </tr>
    
        </table>
      </div>
    </td>
  </tr>-->
  <tr>
    <td>
      <br/>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="">
      <TABLE border="0" cellSpacing="0" cellPadding="0" width="774px">
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
  <tr>
    <td>
      <br/>
    </td>
  </tr>
  <tr>
  </tr>
</table>