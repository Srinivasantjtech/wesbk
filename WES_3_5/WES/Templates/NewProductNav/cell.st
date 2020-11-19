<!--<td valign="top" width="128px">-->
  <div class="sixbox box1" style="height:180px;float:left;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tbody>
        <tr>
          <td>
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
              <div style="display:table-cell; vertical-align:middle; width:140px;height:110px;"  align="center">
                <!--<img src="prodimages$TBT_TWEB_IMAGE1$" name="" maxheight="105px"  maxwidth="105px" />-->
                <img data-original="prodimages$TBT_TWEB_IMAGE1$"  maxheight="105px"  maxwidth="105px" class="lazy" />
                
              </div>
            </a>
          </td>
        </tr>
        <tr>
          <td  valign="top" align="center"   >
            <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">$TBT_FAMILY_NAME$</a>
          </td>
        </tr>
               <tr  >
                <td  valign="top" align="center" >
                  <span class="tx_7B">
                    <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">$TBT_CODE$</a>
                  </span>
                </td>
              </tr>

              <tr>
                <td valign="top" align="center" >
                  $if(TBT_ECOMENABLED)$
                  $if(TBT_STOCK_STATUS_2)$
                  <br/>
                  <a href="OrderDetails.aspx?&bulkorder=1&Pid=$TBT_PRODUCT_ID$&Qty=$TBT_MIN_ORD_QTY$&ORDER_ID=$TBT_ORDER_ID$" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$TBT_PRODUCT_ID$_np','','images/but_buy2.gif',1)">
                    <img src="images/but_buy1.gif" name="Image$TBT_PRODUCT_ID$_np" width="76" height="25" border="0" alt="test"/>
                  </a>
                  $endif$
                  $endif$
                </td>
              </tr>
                   
      </tbody>
    </table>
  </div>
<!--</td>-->
        