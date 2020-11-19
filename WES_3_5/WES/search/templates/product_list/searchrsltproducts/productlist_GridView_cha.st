<TD   class="box_pro_high1" style="width: 162px;height: 305px;">
  <div align="center">
    <!--<a class="hoverblack" href="#">-->
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
    <p class="gvdescription">
      $ATTRIBUTEDESC$...
    </p>
    $else$
    <pclass="gvdescription">
      $ATTRIBUTEDESC$
    </p>
    $endif$

    $if(TBT_BUY_PRODUCT)$
  
    <div class="viewprodetails12" >
          <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" style="text-decoration:none;">
          <strong>
            Order No: $ATTRIBUTE_ID_1$
          </strong><br/>
			  $if(TBT_SUB_PRODUCT)$
			  $else$
			  <strong>
              Cost: &#36;$TBT_USER_PRICE$
            </strong>
			  $endif$
            </a>
            
            
          </div>

        $if(TBT_WITH_LOGIN)$
      
   
        $if(TBT_HIDE_BUY)$
        <div style="margin-top:-10px;">
        <p class="ordercode">
          <span> Product No Longer Available.</span>
          <span> Please contact Us </span>
        </p>
      </div>
    $elseif(TBT_SUB_PRODUCT)$
    <div class="substitute_wrap">
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
    <div style="margin-top:0px;">
        <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"   value="1" style="float:left;height: 24px;margin: 0px 0px 0px 15px;width:40px;background-color:white;border:1px solid #86B7CD;text-align: center;" size="2" type="text" maxlength="6" onkeyup="callproductqtykeypress(event)" onkeydown="return keyct(event)" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$"/>
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
    $endif$
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
            <!--<img src="images/ico_details.gif" width="17" height="14" border="0" align="absmiddle" />-->
          </a>
          $else$
          <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$FAMILY_ID$&cid=$CATEGORY_ID$&pcr=$PARENT_CATEGORY_ID$&Path=$PRODUCT_EA_PATH$" >
            <!--<img src="images/ico_details.gif" width="17" height="14" border="0" align="absmiddle" />-->
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

              <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">
          More Info 
        </a>
          $else$
              <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="moreinfo button smallsiz btnblue" style="font-family:arial;">
          More Info 
        </a>

          $endif$

        $endif$
      <!--</a>-->
    <!--<a class="btnbuy1 button smallsiz btngreen">Buy</a>-->
  </div>

</TD>

