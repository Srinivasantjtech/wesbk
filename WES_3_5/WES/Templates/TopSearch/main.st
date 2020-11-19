<table><tr><td valign="top" style="width:75%"><div class="clear"></div>$if(TBT_SEARCH_DISPLAY)$<div class="viewmoresmall"><strong>Search suggestions for "$TBT_SEARCH_TEXT$"</strong></div>$endif$
<div class="clear"></div><ul>$TBWDataList:{$it.TBWDataListItem$}$</ul><div class="viewmoresmall"><strong>Products</strong></div>$TBWDataList1:{$it.TBWDataListItem1$}$
</td>$TBT_SEARCH_ATTR$</tr><tr><td colspan="2"><div class="clear"></div><a href="PowerSearch.aspx?srctext=$TBT_SEARCH_TEXT$" class="viewmore">View All Results</a>
<div class="clear"></div></td></tr></table>