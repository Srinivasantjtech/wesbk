<TD height="270px" class="box_pro_high1" style="width: 162px;height: 305px;">
  <div align="center"  >
      $if(TBT_SUB_FAMILY)$
      <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$" class="hoverblack">
        <!--<img  alt=""  src="prodimages$ATTRIBUTE_ID_452$" />-->
        <img  alt=""  data-original="prodimages$ATTRIBUTE_ID_452$" class="lazy" />
      </a>
      $else$
      <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="hoverblack">
        <!--<img  alt="" src="prodimages$ATTRIBUTE_ID_452$" />-->
        <img  alt=""  data-original="prodimages$ATTRIBUTE_ID_452$" class="lazy"/>
      </a>
      $endif$
          <div class="familyheader" style="height:35px;width: 170px;">
            $if(TBT_SUB_FAMILY)$
            <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$">
              $FAMILY_NAME$
            </a>
            $else$
            <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
              $FAMILY_NAME$
            </a>
            $endif$
          </div>
      
 
    $if(ATTRIBUTEDESC_GV)$
    <p class="gvdescription">
      $ATTRIBUTEDESC$...
    </p>
    $else$
    <p class="gvdescription">
      $ATTRIBUTEDESC$
    </p>
    $endif$

    $if(TBT_BUY_PRODUCT)$

    $if(TBT_WITH_LOGIN)$
   
    <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$" class="hoverblack_1">
                <div class="viewprodetailsps" style="font-size:11px;font-weight:normal;" >
                
                  <strong> Order No:$ATTRIBUTE_ID_1$ </strong><br/>
                  $if(TBT_SUB_PRODUCT)$
                  $else$
                  <strong>
                    Cost:&#36; $ATTRIBUTE_ID_5$
                </strong><br/>
                  $endif$
                  
                </div>
              </a> <br/>
            <div class="clear"></div>

    $endif$
    $if(TBT_ECOMENABLED)$
    $if(TBT_HIDE_BUY)$
    <div style="width:150px;margin:auto;">
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
    <div style="width:120px;margin-top:-12px;">
      <input name="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$" value="1" onkeyup="callproductqtykeypress(event)"  size="2" type="text" style="float:left;height: 21px;width:40px;background-color:white;border:1px solid #86B7CD;text-align: center;"  maxlength="6" onkeydown="return keyct(event)" id="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$"/>
        <div class="costable">
          <div class="pricepopup">
            <div class="popupouterdiv1">
              <div class="popupaero"></div>
              $TBT_PRODUCT_PRICE_TABLE$
            </div>
              <a style="cursor:pointer;margin:-1px;"  onClick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');" class="btnbuy button smallsiz btngreen">
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
           <div class="viewprodetailsps" style="font-size:11px;font-weight:normal;width:180px;color:#33333;word-wrap:break-word;margin-top:5px;margin-bottom:5px;overflow: hidden;">
             $if(TBT_SUB_FAMILY)$           

             <a href="family.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$FAMILY_EA_PATH$"  style="font-size:11px;font-weight:bold;">
               <strong  >View</strong>:
               $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
             </a>
             $else$
             <img src="images/ico_details.gif" width="17" height="14" border="0" align="absmiddle" />
             <strong class="tx_3" style="font-weight:normal;text-decoration:none;">View</strong>: 

             <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$&cid=$CATEGORY_ID$&path=$PRODUCT_EA_PATH$"  style="font-size:11px;font-weight:bold;">
               $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$
             </a>
             $endif$
             <br/>
             <strong class="hoverblack_1">Starting From:$TBT_MIN_PRICE$</strong><br/>
             
           </div >
              $endif$
              <br/>
      <div class="clear"></div>
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
          </a>

</TD>