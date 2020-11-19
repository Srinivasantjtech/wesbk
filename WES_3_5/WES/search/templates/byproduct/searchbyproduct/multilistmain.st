<!--<table style="display:none;">
<tr>  
  <td height="10"></td>
</tr>
</table>
<table width="558" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="30" background="images/23.gif">
      <table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
          <td height="30" class="tx_1">
            <strong> PRODUCTS SEARCH</strong>
          </td>          
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="back_9">
      <div style="width:588px; height:150px; overflow:auto; ">
        <table width="550" border="0" align="center" cellpadding="10" cellspacing="0" class="tx_1">
          <tr>
          
            $TBWDataList:{
            $it.TBWDataListItem$}$
            
            <td width="115" valign="bottom">
              <input name="button2" type="button" class="form_boton" id="button2" value="Apply Filters"  onclick="GetSelectedItemsFilter();"/>
            </td>
          </tr>
        </table>
      </div>
    </td>
   
  </tr>
</table>-->
<script language="javascript">
  function urlredirectP() {
  var SelAttrStr = '';
  var field=1;
  var url=window.document.location.href;
  var Qarry=url.split('&');
  var param = 'byproduct.aspx?&id=0&' + Qarry[2];
  var param1='';

  while(field <=2)
		{		
		for (var j = 1; j < document.getElementById(field).options.length; j++)
		{
        if (document.getElementById(field).options[j].selected) 
				{
        
			 if(document.getElementById(field).options[j].value!='Select Type' && document.getElementById(field).options[j].value != 'All Models')
      {
      if(document.getElementById(field).options[j].value != 'All Models')
      {  
      SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
      if (field == 1)
      {
      param1 = '&sl2=' + document.getElementById(field).options[j].value;
            }
            else if (field == 2)
            {
            var hasQueryString = document.getElementById(field).options[j].value;
            var hasstr = hasQueryString.indexOf('|');
            var prodstr = hasQueryString.substring(0, hasstr);
            var prodstr1 = hasQueryString.substring(hasstr + 1, hasQueryString.length);
            param1 = param1 + '&fid='+prodstr1;
            }
       }
       else
       {
            break;
       }
      }
      else
      {
          break;
      }
      
      }
      
      }
      
      field++;
      }
      
      if(param1.length > 1){
      param=param+param1;
      window.document.location = param+'&byp=2';
      }
      
      }
    </script>

<table width="560px" cellspacing="0" cellpadding="0"  class="SearchResultsPaging1">
	<tr>
		<td valign="middle" align="left" colspan="3" class="SearchResultsPaging2">
			<table>
				<tbody>
					<tr>
						<td style="width: 5px">
							&nbsp;
						</td>
						<td  align="left" colspan="2" height="20" class="tx_7A1">
							<img src="images/search-icon.jpg" width="21" height="21" border="0" align="absmiddle"/>&nbsp;$TBW_CATEGORY_NAME$ QUICK FIND
						</td>
					</tr>
				</tbody>
			</table>
		</td>
	</tr>	
	<tr>
		<td valign="middle" align="left" height="55px">
			<table cellspacing="8" valign="top" >

				<tr>
          <td>&nbsp;</td>
					$TBWDataList:{
					$it.TBWDataListItem$}$
					<td >
						<img style="cursor:pointer;" src="images/tosuit_search.jpg"   onclick="urlredirectP();" />

					</td>
					<td>&nbsp;</td>
				</tr>

			</table>
		</td>
	</tr>
</table>