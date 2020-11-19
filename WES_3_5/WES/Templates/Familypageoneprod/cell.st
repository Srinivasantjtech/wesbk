<tr>
  <td class ="tx_7">
    <strong>QTY</strong>
    <label>
      <input name="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" size="5" type="text" id="txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$" style="background-color:#FFFFFF;color: #000000;" />
    </label>
  </td>
</tr>
<tr>
  <td class ="tx_7">
    <strong>
      Product <a href="productdetails.aspx?&pid=$TBT_PRODUCT_ID$&fid=$TBT_FAMILY_ID$">$TBT_CODE$</a>
    </strong>
  </td>
</tr>
<tr>
  <td>
    <table width="100%">
      <tr>
        <td valign="top"  align="left" class="tx_9">
          <br/>
          PRICE: &#36; $TBT_COST$
        </td>
        <td align="right">
          <a onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$TBT_PRODUCT_ID$_$TBT_FAMILY_ID$fp','','images/but_buyitem2.gif',1)">
            <img src="images/but_buyitem1.gif" name="Image$TBT_PRODUCT_ID$_$TBT_FAMILY_ID$fp" width="145" height="25" border="0" onClick="productbuy(txt$TBT_PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$TBT_FAMILY_ID$,$TBT_PRODUCT_ID$);"/></a>
        </td>
      </tr>
    </table>
  </td>
</tr>
<tr>
  <td>
    $TBT_DESCRIPTION$
  </td>
</tr>