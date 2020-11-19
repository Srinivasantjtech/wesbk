<td valign="top" width="128px">
  <div class="box_pro_high" >

    <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
      <img width="100" height="100" alt="product" src="prodimages$TBT_TWEB_IMAGE1$" name="" />
    </a >
        <br/>
    <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
      <strong>$TBT_CODE$</strong>
    </a >
    <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
      <p style="margin: 0;padding:0;border: 0 none;font-size: 100%;">$TBT_FAMILY_NAME$ </p>
    </a >
          <a class="viewprodetails" href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">View Product Details</a>
    $if(TBT_ECOMENABLED)$
    $if(TBT_STOCK_STATUS_2)$
    <br/>
    <a href="OrderDetails.aspx?&bulkorder=1&Pid=$TBT_PRODUCT_ID$&Qty=$TBT_MIN_ORD_QTY$&ORDER_ID=$TBT_ORDER_ID$" class="btnbuy1 button smallsiz btngreen" style="margin:0 0 0 43px;">
      Buy
    </a>
    $endif$
    $endif$
  </div>
</td>
        