
<div class="filter-select">
  <select class="filter-option hasCustomSelect" style="width: 168px; position: absolute; opacity: 0; height: 38px; font-size: 17px;"
          onchange="this.options[this.selectedIndex].value && (window.location = this.options[this.selectedIndex].value);">
    <option Value="" >$SELECTED_ATT$</option>
    $TBWDataList:{
    $it.TBWDataListItem$}$

  </select>
  <span class="customSelect filter-option" style="display: inline-block;">
    <span class="customSelectInner" style="width: 152px; display: inline-block;$SELECT_COLOR$">$SELECTED_NAME$</span>
  </span>
</div>