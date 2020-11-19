
  <div class="cat_list even">
    <!--<table width="100%" border="0" cellspacing="0" cellpadding="6">
     
      <tr>
        <td width="" >-->
          $if(TBT_SUB_FAMILY)$
          <div class="cat_list_img">
            <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="display:table-cell;vertical-align:middle;text-align:center;height:100px;width:100px;background: none repeat scroll 0 0 #FFFFFF;" class="">
              <!--<img src="prodimages$ATTRIBUTE_ID_747$" style="max-height:100px; max-width:100px;" border="0"/>-->
              <img  alt="" src="prodimages$ATTRIBUTE_ID_747$">
          </a>
          </div >
          $else$
          <div class="cat_list_img">
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="display:table-cell;vertical-align:middle;text-align:center;height:100px;width:100px;background: none repeat scroll 0 0 #FFFFFF;" class="">
              <!--<img src="prodimages$ATTRIBUTE_ID_747$" style="max-height:100px; max-width:100px;" border="0"/>-->
              <img    alt="" src="prodimages$ATTRIBUTE_ID_747$">
          </a>
          </div >
          $endif$
        <!--</td>
        <td   align="left" width="450px">-->
          <div class="cat_list_txt">
            $if(TBT_SUB_FAMILY)$
            <strong>
              <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="text-decoration:none;">$FAMILY_NAME$</a>
            </strong>
            $else$
            <strong>
              <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">$FAMILY_NAME$</a>
            </strong>
            $endif$
            <p>
              $ATTRIBUTE_ID_13$
              $if(TBT_MORE_13)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" class="tx_3">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" class="tx_3">More Information</a>
              $endif$
              $endif$
            </p>
            <p>
              $ATTRIBUTE_ID_90$
              $if(TBT_MORE_90)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" class="tx_3">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" class="tx_3">More Information</a>
              $endif$
              $endif$
            </p>
            <p>
              $ATTRIBUTE_ID_91$
              $if(TBT_MORE_91)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $endif$
              $endif$
            </p>
            <p>
              $ATTRIBUTE_ID_377$
              $if(TBT_MORE_377)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $endif$
              $endif$
            </p>
            <p>
              $ATTRIBUTE_ID_379$
              $if(TBT_MORE_379)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $endif$
              $endif$
            </p>
            <p>
              $ATTRIBUTE_ID_4$
              $if(TBT_MORE_4)$
              $if(TBT_SUB_FAMILY)$
              ...<a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $else$
              ...<a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="color:#0099FF;text-decoration:none;">More Information</a>
              $endif$
              $endif$
            </p>
            <!--<br/>-->
            <!--<strong>View</strong>:-->
            <div class="test123">
              $if(TBT_SUB_FAMILY)$
              <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$"  class="tx_3new">
                <img src="images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong style="font-weight:normal;text-decoration:none;">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
              </a>
              $else$
              <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$"  class="tx_3new">
                <img src="images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong style="font-weight:normal;text-decoration:none;">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
              </a>
              $endif$
            </div>
            <!--$if(TBT_SUB_FAMILY)$
            <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="color:#0099FF;" class="tx_3">
              $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
            </a>
            $else$
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="color:#0099FF;" class="tx_3">
              $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
            </a>
            $endif$-->
          </div >

        <!--</td>
        <td  width="160px">-->

    <div class="cat_list_right" style="width:160px;">
      $if(TBT_BUY_PRODUCT)$
      $if(TBT_SUB_PRODUCT)$
      $else$
      <div class="cat_list_cost">
        <a class="hoverblack_1" href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;font-weight:normal;">
              <strong id="divcost$divcount$">
                Cost: &#36;$TBT_USER_PRICE$
            </strong>
              <br/>
              <strong>
                Order No: $ATTRIBUTE_ID_1$
              </strong> 
        </a>
              <br/>
            </div >
      $endif$
      
            
            <!--$if(TBT_WITH_LOGIN)$
            <div id="pid$TBT_PRODUCT_ID$" class="popupouterdiv" >$TBT_PRODUCT_PRICE_TABLE$</div>
            <strong>
              <div onMouseOver="javascript:ShowPriceTable('pid$TBT_PRODUCT_ID$')" onMouseOut="javascript:ClosePriceTable('pid$TBT_PRODUCT_ID$')" style="position:relative"> </div>
            </strong>
      $endif$-->

      $if(TBT_ECOMENABLED)$
      $if(TBT_HIDE_BUY)$
      <div style="width:150px;margin:auto;">
        <p class="ordercode">
          $PRODUCT_STATUS$
        </p>
      </div>
      $elseif(TBT_SUB_PRODUCT)$
      <div class="substitute_list">
		  <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;font-weight:normal;" class="hoverblack_1">
			

			  <strong>
          Order No: $ATTRIBUTE_ID_1$
        </strong>
		  </a>
        <!--<div class="substitute_head">

          <img src="images/subproduct_icon.jpg">

            <h5> SUBSTITUTE PRODUCT </h5>

          </div>-->
        <div class="substitute_details">
          <!--<b class="pink">
            <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="hoverblack_1" > $ATTRIBUTE_ID_1$</a>
          </b>-->
          <p> Product is unavailable and replaced with code: </p>
          <b class="green">$TBT_REP_NIL_CODE$</b>
          <a href="$TBT_REP_EA_PATH$">View Substitute Product</a>
        </div>

      </div>
      $else$
      <div >
      <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)" style="float:left;height: 21px;margin: 12px 0 13px 38px;width:40px;background-color:white;border:1px solid #86B7CD;text-align: center;" size="2" type="text" maxlength="6" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"/>
      <div class="costable">
                <div class="pricepopup">
                  <div id="popupouterdiv$divcount$">
                      <!--<div class="popupaero"></div>
                        $TBT_PRODUCT_PRICE_TABLE$-->
                     </div>                  
                        <a style="cursor:pointer;"  class="btnbuy button smallsiz btngreen costable" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" onmouseover="mouseOver('$PRODUCT_CODE$','$TBT_PRODUCT_ID$','$divcount$')">
                          Buy
                        </a>
                </div>
              </div>
      </div>
      $endif$
      <!--<a style="cursor:pointer;" onMouseOut="MM_swapImgRestore();ClosePriceTable('pid$TBT_PRODUCT_ID$')" onMouseOver="MM_swapImage('Image$TBT_PRODUCT_ID$_$TBT_FAMILY_ID$_pp','','images/but_buy2.gif',1);ShowPriceTable('pid$TBT_PRODUCT_ID$')" class="btnbuy button smallsiz btngreen" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">
        Buy
      </a>-->


              <!--</div >-->
            $else$
                <!--<div class="cat_list_right" >-->   
                $if(TBT_SUB_FAMILY)$
                        <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="btnbuy button smallsiz btnblue">
              More Info             
            </a>
                $else$
                        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="btnbuy button smallsiz btnblue">
              More Info            
            </a>
                $endif$

            $endif$
         $else$

            $if(TBT_WITH_LOGIN)$
            <div class="cat_list_cost">
            <strong>
              Starting From: $TBT_MIN_PRICE$
            </strong>
              <br/>
              <strong>
                Order No: $ATTRIBUTE_ID_1$
              </strong>
              <br/>
          </div>
            $endif$
           
            $if(TBT_SUB_FAMILY)$
            <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="btnbuy button smallsiz btnblue">
              More Info            
            </a>
            $else$
            <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$"  class="btnbuy button smallsiz btnblue">
              More Info            
            </a>
            $endif$
              
      $endif$
    </div>

          <!--</div >-->
        <!--</td>
      </tr>
     
    </table>-->
  </div >
