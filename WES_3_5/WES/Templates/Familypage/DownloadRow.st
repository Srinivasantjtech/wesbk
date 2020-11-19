$if(TBT_FAMILY_HEAD)$                 
<div class="title7_prod">$TBT_FAMILY_NAME$-Family </div>
$endif$
<table class="table table-bordered" width="760px">
<thead>
  <tr>
    <th class="familydownload_td">Description</th>
    <th class="familydownload_td">File</th>
    <th class="familydownload_td">Size</th>
  </tr>
</thead>
<tbody>
  $TBWDataList:{
  $it.TBWDataListItem$}$
</tbody>
</table>