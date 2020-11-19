
  <div class="cat_list">
    <div class="cat_list_img">
      $if(TBT_SUB_FAMILY)$
      <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" style="display:table-cell;vertical-align:middle;text-align:center;height:100px;width:100px;background: none repeat scroll 0 0 #FFFFFF;">
        <!--<img  alt=""  src="prodimages$ATTRIBUTE_ID_452$" />-->
        <img  alt=""  data-original="prodimages$ATTRIBUTE_ID_452$" class="lazy" />
      </a>
      $else$
      <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" style="display:table-cell;vertical-align:middle;text-align:center;height:100px;width:100px;background: none repeat scroll 0 0 #FFFFFF;">
        <!--<img  alt="" src="prodimages$ATTRIBUTE_ID_452$" />-->
        <img  alt=""  data-original="prodimages$ATTRIBUTE_ID_452$" class="lazy" />
      </a>
      $endif$
    </div >

    <div class="cat_list_txt">
      <h6>
        $if(TBT_SUB_FAMILY)$
        <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" style="text-decoration:none;">
          $FAMILY_NAME$
        </a>
        $else$
        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" style="text-decoration:none;">
          $FAMILY_NAME$
        </a>
        $endif$
      </h6 >
      <p>
        $ATTRIBUTE_ID_62$
        $if(TBT_MORE_62)$
        $if(TBT_SUB_FAMILY)$
        ...<a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="tx_3">More Information</a>
        $else$
        ...<a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="tx_3">More Information</a>
        $endif$
        $endif$

        $ATTRIBUTE_ID_4$
        $if(TBT_MORE_4)$
        $if(TBT_SUB_FAMILY)$
        ...<a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$"  class="tx_3">More Information</a>
        $else$
        ...<a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="tx_3">More Information</a>
        $endif$
        $endif$
      </p>

      $if(TBT_SUB_FAMILY)$
      
       <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$"  class="tx_3new">
         <img src="images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong  style="font-weight:normal;text-decoration:none;">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
       </a>
      <!--<a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$"  class="tx_3">
        $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
      </a>-->
      $else$
      
      <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$"  class="tx_3new">
        <img src="images/ico_details.gif"  border="0" align="absmiddle" />&nbsp;&nbsp;<strong style="font-weight:normal;text-decoration:none;">View:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products</strong>
      </a>
      <!--<a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$"  class="tx_3">
        $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
      </a>-->
      $endif$
    </div >

    <div class="cat_list_right11">

      $if(TBT_BUY_PRODUCT)$

      $if(TBT_WITH_LOGIN)$
      $if(TBT_SUB_PRODUCT)$
      $else$
      <div class="cat_list_cost">

        <!--<div id="pid$PRODUCT_ID$" class="popupouterdiv1" > $TBT_PRODUCT_PRICE_TABLE$</div>-->
        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="hoverblack_1">
          <!--<div onMouseOver="javascript:ShowPriceTable('pid$PRODUCT_ID$')" onMouseOut="javascript:ClosePriceTable('pid$PRODUCT_ID$')" style="position:relative">-->
            <strong id="divcost$divcount$">
              Cost: &#36;$ATTRIBUTE_ID_5$
            </strong><br/>
          <!--</div>-->
        
          <strong> Order No: $ATTRIBUTE_ID_1$</strong>

        </a>
      </div>
      $endif$
      $endif$


      $if(TBT_ECOMENABLED)$
      $if(TBT_HIDE_BUY)$
      <div style="width:150px;margin:auto;">
        <p class="ordercode">
          $PRODUCT_STATUS$
        </p>
      </div>
      $elseif(TBT_SUB_PRODUCT)$
      <div class="substitute_list">
        <!--<div class="substitute_head">

          <img src="images/subproduct_icon.jpg">

            <h5> SUBSTITUTE PRODUCT </h5>

          </div>-->
         <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="hoverblack_1">
          <!--<div onMouseOver="javascript:ShowPriceTable('pid$PRODUCT_ID$')" onMouseOut="javascript:ClosePriceTable('pid$PRODUCT_ID$')" style="position:relative">-->
         
          <strong> Order No: $ATTRIBUTE_ID_1$ </strong>

        </a>
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
      <div style="float:left;width:100%;">
        <input name="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)"   size="2" type="text" style="float:left;height: 21px;margin: 13px 12px 13px 33px;width:40px;background-color:white;border:1px solid #86B7CD;text-align: center;" maxlength="6" onkeydown="return keyct(event)" id="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$"/>
        <div class="costable">
          <div class="pricepopup">
            <div id="popupouterdiv$divcount$">
              <!--<div class="popupaero"></div>
              $TBT_PRODUCT_PRICE_TABLE$-->
              </div>
              <a style="cursor:pointer;" onClick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');" class="btnbuy button smallsiz btngreen costable" onmouseover="mouseOver('$PRODUCT_CODE$','$PRODUCT_ID$','$divcount$')">
                Buy
              </a>
            
          </div>
        </div>
      </div>
      $endif$
      $else$

      $if(TBT_SUB_FAMILY)$

      <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="btnbuy button smallsiz btnblue">
        More Info<!--<img src="Images/moreinfo1.png" width="76" height="28"  name="Image$PRODUCT_ID$_$FAMILY_ID$_pp"/>-->
      </a>
      $else$
      <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="btnbuy button smallsiz btnblue">
        More Info<!--<img src="Images/moreinfo1.png" width="76" height="28" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp"/>-->
      </a>

      $endif$

      $endif$

      $else$
      <div class="cat_list_cost">
        $if(TBT_WITH_LOGIN)$

        <strong>Starting From: $TBT_MIN_PRICE$</strong><br/>
        <strong> Order No: $ATTRIBUTE_ID_1$</strong>

        $endif$
      </div>
      $if(TBT_SUB_FAMILY)$

      <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="btnbuy button smallsiz btnblue">
        More Info<!--<img src="Images/moreinfo1.png" width="76" height="28" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp"/>-->
      </a>
      $else$
      <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="btnbuy button smallsiz btnblue">
        More Info<!--<img src="Images/moreinfo1.png" width="76" height="28" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp"/>-->
      </a>

      $endif$

      $endif$

    </div >
  </div >
