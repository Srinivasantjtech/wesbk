<div class="product-items $LISTVIEW_CLASS$">

  <div class="pro_grid_final $CLASS_CLEAR$">
    <div class="image-block">
      <div style="height:35px;">
      <h6 class="proname">
        $if(TBT_SUB_FAMILY)$
        <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
        $else$
        <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
        $endif$
      </h6>
        </div>
      $if(TBT_SUB_FAMILY)$
      <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$"  >


        <img  src="prodimages$ATTRIBUTE_ID_747$" />
      </a>

      $else$
      <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$"  style="text-decoration:none;color:white;" >


        <img  alt="" src="prodimages$ATTRIBUTE_ID_747$" />
      </a>

      $endif$

    </div>
    <div class="pro-detail">
     
        $if(TBT_BUY_PRODUCT)$
        <div class="detail-left">
          
          <div class="pro-discroption">
            <p class="pro-discroption">
              $if(ATTRIBUTEDESC_GV)$
            
                $ATTRIBUTEDESC$...
            
              $else$
           
                $ATTRIBUTEDESC$
             
              $endif$

            </div>
          
          <!--<h6 class="proname" id="list-productname">
            $if(TBT_SUB_FAMILY)$
            <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
            $else$
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
            $endif$
          </h6>-->
          <div class="priceonly">
            <p class="priceblue1">
           
                <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color:#2cbbfe;">
                  <strong> &#36;$TBT_USER_PRICE$  </strong>
                </a>
             
            </p>
          </div>
          <div class="pro-ordercode">
            <p class="darkgray" style="color: #333;">
              <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color: #333;">
                Order Code: $ATTRIBUTE_ID_1$
              </a>
            </p>
          </div>
        </div>
        <div class="detail-right">
         
             <input type="text" class="quantity" name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"    size="2" type="text" maxlength="6" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"/>
         
          <a style="cursor:pointer;"  class="pro-detail_a" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">Add to Cart</a>
        </div>
        $else$
        <div class="detail-top clearfix">
          <div class="pro-discroption clearfix">
            <p class="">
              $if(ATTRIBUTEDESC_GV)$
              
                $ATTRIBUTEDESC$...
            
              $else$
           
                $ATTRIBUTEDESC$
             
              $endif$
            </p>
            <div class="pfclear"></div>
          </div>
          
          <!--<h6 class="proname" id="list-productname">
            $if(TBT_SUB_FAMILY)$
            <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
            $else$
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;color: #333;">$FAMILY_NAME$</a>
            $endif$
          </h6>-->
          <div class="startingfrom">
            <p class="lightblue" style="color: #333;">
              <strong>Starting From</strong>
            </p>
          </div>
          <div class="pro-price">
            <p	 class="lightblue">
              <strong>$TBT_MIN_PRICE$</strong>
            </p>
          </div>
          <div class="view4of4">
            <p class="darkgray" style="color: #333;">
              $if(TBT_SUB_FAMILY)$
              <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" class="darkgray" style="text-decoration:none;" >
                View $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
              </a>
              $else$
              <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" class="darkgray"  style="text-decoration:none;" >
                View $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
              </a>
              $endif$
            </p>
          </div>
        <div class="detail-bottom" id="viewmore">
          $if(TBT_SUB_FAMILY)$

          <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="pro-detail_a" style="font-family:arial;">
            More Info
          </a>
          $else$
          <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="pro-detail_a" style="font-family:arial;">
            More Info
          </a>

          $endif$


        </div>
         
        </div>  
        $endif$
      </div>
    </div>
  </div>
