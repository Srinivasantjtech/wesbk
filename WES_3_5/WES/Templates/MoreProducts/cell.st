<td align="center" valign="top">
  <table width="175" border="0" cellpadding="2" cellspacing="0">
    <tr>
      <td height="30" colspan="2" align="left" valign="top" class="tx_7">
        <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$">$TBT_FAMILY_NAME$</a>
      </td>
    </tr>
    <tr>
      <td height="50" colspan="2" align="left" valign="top" class="tx_1">$TBT_DESCRIPTIONS1$</td>

    </tr>
    <tr>
      <td width="101" align="center" valign="top">
        <table width="80" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td height="30" valign="top" align="left" class="tx_9">
              <span class="tx_7B">
                <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$">$TBT_CODE$</a>
              </span>
              <br/>
              <span class="tx_9">&#36;&nbsp;$TBT_COST$</span>
            </td>
          </tr>
          <tr>
            <td>
              <br/>
              $if(TBT_ECOMENABLED)$
              $if(TBT_STOCK_STATUS_2)$
              <a href="OrderDetails.aspx?&bulkorder=1&Pid=$TBT_PRODUCT_ID$&Qty=$TBT_MIN_ORD_QTY$&ORDER_ID=$TBT_ORDER_ID$" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$TBT_PRODUCT_ID$_mp','','images/but_buy2.gif',1)">
                <img src="images/but_buy1.gif" name="Image$TBT_PRODUCT_ID$_mp" width="76" height="25" border="0"/>
              </a>
              $endif$
              $endif$
            </td>
          </tr>
        </table>
      </td>
      <td width="74px" align="right" valign="top">
        <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$">
          <img src="prodimages$TBT_TWEB_IMAGE1$" name="" width="65px" height="65px" class="border"/>
        </a>
      </td>
    </tr>
  </table>
</td>