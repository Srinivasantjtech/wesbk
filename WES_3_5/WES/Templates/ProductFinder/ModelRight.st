
<div class="selectsecond $SELECT_CLASS$">
  <div class="numbox">
    2
  </div>
  <span class="opensans600">Select Model Of Device</span>
  <div class="select-wrap pfclear">
    <select onchange="location = this.options[this.selectedIndex].value;"  class="filter-option hasCustomSelect" style="width: 168px; position: absolute; opacity: 0; height: 38px; font-size: 17px;">
      <option Value="" >&#60;$SELECTED_MODEL$&#62;</option>
      $MODEL_LI_LIST$
    </select>
    <span class="customSelect filter-option" style="display: inline-block;">
      <span class="customSelectInner" style="width: 152px; display: inline-block;">&#60;$SELECTED_MODEL$&#62;</span>
    </span>
  </div>
</div>