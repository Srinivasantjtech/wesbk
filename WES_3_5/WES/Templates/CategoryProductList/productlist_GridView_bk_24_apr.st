<TD bgcolor="" class="box_pro_high1" style="width: 162px;height: 305px;">
  <div align="center">
      $if(TBT_SUB_FAMILY)$
      <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$"  class="hoverblack">
        <!--<img  alt="" src="prodimages$ATTRIBUTE_ID_747$"/>-->
        <img  alt="" data-original="prodimages$ATTRIBUTE_ID_747$" class="lazy"/>
     </a>    
      $else$
      <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$"  class="hoverblack">
        <!--<img  alt="" src="prodimages$ATTRIBUTE_ID_747$"/>-->
        <img  alt="" data-original="prodimages$ATTRIBUTE_ID_747$" class="lazy"/>
      </a>    
      $endif$   
        <div class="familyheader" style="height:30px;">
          $if(TBT_SUB_FAMILY)$
          <strong>
            <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" style="text-decoration:none;">$FAMILY_NAME$</a>
          </strong>
          $else$
          <strong>
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">$FAMILY_NAME$</a>
          </strong>
          $endif$
        </div>
         


    $if(ATTRIBUTEDESC_GV)$
    <p style="white-space: inherit;margin-left: -10px;font-size:11px;height:67px;width:180px;word-wrap:break-word">
      $ATTRIBUTEDESC$...
    </p>
    $else$
    <p style="white-space: inherit;margin-left: -10px;font-size:11px;height:67px;width:180px;word-wrap:break-word">
      $ATTRIBUTEDESC$
    </p>
    $endif$
    $if(TBT_BUY_PRODUCT)$
    <div class="viewprodetails12" >
          <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">
          <strong>
            Order No: $ATTRIBUTE_ID_1$
          </strong><br/>
            <strong>
              Cost: &#36;$TBT_USER_PRICE$
            </strong>
            </a>                       
          </div>
        $if(TBT_WITH_LOGIN)$     
      <div style="margin-top:0px;">
        <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"  value="1" onkeyup="callproductqtykeypress(event)"   style="float:left;height: 24px;margin: 0px 0px 0px 15px;width:40px;background-color:white;border:1px solid #86B7CD;text-align: center;" size="2" type="text" maxlength="6" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"/>
        <div class="costable">
          <div class="pricepopup">
            <div class="popupouterdiv1">
              <div class="popupaero"></div>
              $TBT_PRODUCT_PRICE_TABLE$
            </div>
            <a style="cursor:pointer;margin: -1px 28px 0px 0px;"  class="btnbuy button smallsiz btngreen costable" onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');">
              Buy
            </a>
          
          </div>
        </div>
      </div>
        $else$
        $if(TBT_SUB_FAMILY)$
        <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">
          More Info 
        </a>
        $else$
        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">
          More Info 
        </a>
        $endif$
        $endif$
        $else$
        $if(TBT_WITH_LOGIN)$
        <div class="viewprodetails12" >         
          $if(TBT_SUB_FAMILY)$
          <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$"  >
           </a>
          $else$
          <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" >
          </a>
          $endif$
          $if(TBT_SUB_FAMILY)$
          <a href="family.aspx?&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&ProductResult=SUCCESS&pcr=$PARENT_CATEGORY_ID$&Path=$FAMILY_EA_PATH$" >
            <strong>View</strong>:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
          </a>
          $else$
          <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" >
            <strong>View</strong>:$PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
          </a>
          $endif$
          <br/>
          <strong>
          Starting From: $TBT_MIN_PRICE$<br/>
        </strong>
      </div >
        $endif$
          $if(TBT_SUB_FAMILY)$
              <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">More Info</a>
          $else$
              <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">More Info</a>
          $endif$
        $endif$
  </div>
</TD>

