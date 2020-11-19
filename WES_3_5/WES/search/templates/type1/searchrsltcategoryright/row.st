<a class="menuitem submenuheader" href="">$TBT_ATTRIBUTE_TITLE$</a>
<div class="submenu">
  <ul>   
      $TBWDataList:{
      $it.TBWDataListItem$}$    
  </ul>
  <div class="submenuhide" id="submenuhide$TBT_MENU_ID$"  >
    <ul>
        $TBWDataList1:{
        $it.TBWDataListItem1$}$
    </ul>
  </div>
  <div  class="show_hide" id="show1" onclick="showsubmenu($TBT_MENU_ID$);dsp(this)" >
    <span class="dspchar">+ Show more options</span>
  </div>
</div>
<div class="clearall"></div>
