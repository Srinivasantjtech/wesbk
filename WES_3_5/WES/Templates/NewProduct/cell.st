<div class="box_pro_high_new" style="height:328px;width:186px;float:left;padding-top:6px;padding-bottom:15px;padding-left:5px;display: table-cell;text-align: center;">
  <table width="175" border="0" cellpadding="0" cellspacing="0">
    <tr>

      <td  align="center" valign="top"  bgcolor="" style="width: 120px;height: 100px;">
       
          <div align="center" >
            <a class="hoverblack" href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
              <img src="prodimages$TBT_TWEB_IMAGE1$" name="" maxwidth="100px" />
            <!--<img data-original="prodimages$TBT_TWEB_IMAGE1$" name="" maxwidth="100px"  class="lazy" />-->
            </a>
              </div>
    
      </td>
    </tr>
    <tr>
      <td  style="height:35px" colspan="2" valign="top" align="center"  class="familyheader" >
        <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">$TBT_FAMILY_NAME$</a>
      </td>
    </tr>

    <tr>
      <td height="50" colspan="2" valign="top" align="center" class="tx_1">
        <p style="white-space: inherit;margin-left: -1px;font-size:11px;height:50px;width:180px;">
          $TBT_DESCRIPTIONS$
        </p>
      </td>
    </tr>
    <tr>
      <td width="101" align="center" valign="top">
        <table width="80" border="0" cellspacing="0" cellpadding="0">


          <tr>
            <td height="30" valign="bottom" align="center" >
              <div class="viewprodetailsps" style="font-weight:normal;width:180px;" >
                <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$&cid=$TBT_CATEGORY_ID$&path=$PRODUCT_EA_PATH$">
                  <strong> Order No:$TBT_CODE$</strong>
                </a>
                <br/>
                <strong>
                  Cost:&#36; $TBT_YOURCOST$
                </strong>
                <br/>
              </div >

            </td>
          </tr>

          <tr>
            <td valign="center" align="center">
              $if(TBT_ECOMENABLED)$
              $if(TBT_STOCK_STATUS_2)$
              <br/>
              <!--<a href="OrderDetails.aspx?&bulkorder=1&Pid=$TBT_PRODUCT_ID$&Qty=$TBT_MIN_ORD_QTY$&ORDER_ID=$TBT_ORDER_ID$" class="homebut_buy1">-->
              <a onClick="productbuy('txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$','$TBT_PRODUCT_ID$');" style="cursor: pointer;"  class="homebut_buy1">


                <!--<img src="images/but_buy2.gif" name="Image$TBT_PRODUCT_ID$_np" width="76" height="25" border="0" alt="test"/>-->
              </a>

              $else$

              $PRODUCT_STATUS$
              $endif$
              $endif$
            </td>
          </tr>
        </table>


      </td>

    </tr>
  </table>
</div>

