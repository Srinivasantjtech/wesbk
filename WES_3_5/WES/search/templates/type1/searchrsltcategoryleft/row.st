
<h3 class="title3aLeft">$TBT_ATTRIBUTE_TITLE$</h3>
<div class="submenu" style="word-break: break-all" >
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
