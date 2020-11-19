<tr>
  <td valign="Middle" align="center" style="width: 200px;   " class="FamilyPageTableCell">
    <div class="pro_thum_outer">
      $if(SHOW_DIV)$
      <div id="pro_img_popup$PRODUCT_ID$" style=" visibility:hidden; " class="pro_img_popup">
        <img data-original="$TWEB_LargeImg$" class="lazy" src="$TWEB_LargeImg$" alt="$PROD_CODE$ $PROD_DESCRIPTION$" />
      </div>
      $endif$
      <img style="max-height: 50px; max-width: 50px; display: inline;" onmouseover="javascript:Moverimgtag('$PRODUCT_ID$');" onmouseout="javascript:Moutimgtag('$PRODUCT_ID$');" data-original="$TWEB_Image$" class="lazy" alt="$PROD_CODE$ $PROD_DESCRIPTION$" />
    </div>
  </td>


  <td valign="Middle" align="center" style="width: 200px;   " class="FamilyPageTableCell"> $PROD_CODE$ </td>
$ATTRIBUTE_VALUES$
  <td valign="Middle" align="center" style="width: 200px;  " class="FamilyPageTableCell">
    <a class="tx_3" rel="pd.aspx?pid=$PRODUCT_ID$&amp;fid=$PARENT_FAMILY_ID$&amp;cid=$CAT_ID$&amp;path=$EA_PATH$" href="productdetails.aspx?pid=$PRODUCT_ID$&amp;fid=$PARENT_FAMILY_ID$&amp;cid=$CAT_ID$&amp;path=$EA_PATH$">See More Details </a>
  </td>

  
  <td valign="Middle" align="center" style="width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " class="costable" >


    <div class="costable">
      <div class="pricepopup">
        <div class="popupouterdiv1">
          <div class="popupaero"></div>
          $PRICE_TABLE$
        </div>
        <a style="text-decoration:none;"  class="costable" >
          &#36; $COST$
        </a>
      </div>
    </div>
  </td>

  
  
  
  <td valign="Middle" align="Center" style="width: 120px;  " class="FamilyPageTableCell">
    $if(SHOW_BUY)$
    <table width="100px" border="0" cellspacing="0" cellpadding="0">
      <tbody>
        <tr>
          <td>
            <div>
              <input type="text" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;text-align: center;" value="1" onkeyup="callproductqtykeypress(event)" id="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" size="1" maxlength="6" onkeydown="return keyct(event)" name="txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$" valign="middle"/>
                <div class="costable">
                  <div class="pricepopup">
                    <div id="popupouterdiv$PRODUCT_ID$" class="popupouterdivnone">
                      <div class="popupaero"></div>
                      $PRICE_TABLE$
                    </div>
                    <a onclick="productbuy('txt$PRODUCT_ID$_$AVIL_QTY$_$MIN_ORDER_QTY$_$BUY_FAMILY_ID$','$PRODUCT_ID$');" class="btnbuy2 button smallsiz btngreen costable" valign="middle" id="$PRODUCT_ID$" onmouseover="javascript:test('$PRODUCT_ID$');" onmouseout="javascript:Mouseout('$PRODUCT_ID$');" style="cursor:pointer;margin: 0 0 0 5px;">Buy  </a>
                  </div>
                </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    $endif$
  </td>

  
</tr>