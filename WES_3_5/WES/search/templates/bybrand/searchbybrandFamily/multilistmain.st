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
  function urlredirectB() {
  var SelAttrStr = '';
  var field=1;
  var url=window.document.location.href;  
  var Qarry=url.split('&');  
	var param = 'product_list.aspx?&id=0&' + Qarry[2];
	var param1='';	
	
	while(field <=3)
		{		
		for (var j = 0; j < document.getElementById(field).options.length; j++)
		{
      if (document.getElementById(field).options[j].selected) 
			{      
		  if(document.getElementById(field).options[j].value!='Select Brand' && document.getElementById(field).options[j].value!='List all models' && document.getElementById(field).options[j].value!='List all products')
      {

      if(document.getElementById(field).options[j].value!='List all models' && document.getElementById(field).options[j].value!='List all products')
      {
        SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
              if (field == 1)
              {
              param1 = '&sb=' + document.getElementById(field).options[j].value;
              break;
              }
              else if (field == 2)
              {
              param1 = param1 + '&sm=' + escape(document.getElementById(field).options[j].value);
              break;
              }
              }
              else
              {
              break;
              }
      
      }else
      {
      break;
      }
      }
      }field++;
      }
      if(param1.length>1){
      param=param+param1;    
      window.document.location = param+'&byp=2';
			}
			}
		</script>

<table width="563" cellspacing="0" cellpadding="0">
		<tr>
    <td width="20px">&nbsp;</td>
    <td align="left" colspan="1" height="20" class="tx_7A">$TOSUITE_DISPLAY$</td>
		<td valign="middle" align="left" height="55px">
			<table cellspacing="8" valign="top" >

				<tr>
					$TBWDataList:{
					$it.TBWDataListItem$}$
					<td >
					</td>					
				</tr>

			</table>
		</td>
	</tr>
</table>