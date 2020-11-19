<TD>
  <table width="100%" border="0" cellspacing="0" cellpadding="6">
    <tr>

      <!--<td width="50" align="center" valign="top" >
        <table width="56%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td height="62" align="center" valign="top">
              <label class="tx_1">
                <input id="Chkbox_$PRODUCT_ID$" type="checkbox" name="CompareItem" value="$PRODUCT_ID$"  onclick="CheckCompareCount(document.forms[0].CompareItem,'Chkbox_$PRODUCT_ID$');" />
                <br/>
                <a href="#" onclick="GetCompareItems(document.forms[0].CompareItem,$FAMILY_ID$);">
                  Compare<br/>
                  Products
                </a>
              </label>
            </td>
          </tr>

          <tr>
            <td valign="top">-->
              <!--<%--$FAMILY_ID$--%><img src="<%--$FAMILY_ID$$ATTRIBUTE_ID_5337$$ATTRIBUTE_ID_5338$--%>" width="49" height="47"><%--/$FAMILY_ID$--%>-->
            <!--</td>
          </tr>
        </table>
      </td>-->
      <td width="25%" align="center" valign="top"  class="borderOdd">
        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">
          <img style="display: block;max-height:200px;max-width:200px" src="prodimages$ATTRIBUTE_ID_452$" />
        </a>
      </td>
      <td width="50%" valign="top" align="left" class="borderOdd">
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
          <tr>
            <td>
              <span class="tx_1">
                <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">
                  <b>$FAMILY_NAME$</b>
                </a>
              </span>
            </td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
            <tr>
            <td>
              <span class="tx_1">
                $ATTRIBUTE_ID_62$
              </span>
            </td>
            </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
            <tr>
            <td>
              <span class="tx_1">
              <strong>View</strong>: <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$"  class="tx_3">
                <img src="images/ico_details.gif" width="17" height="14" border="0" align="absmiddle" />
              </a>&nbsp;<a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$"  class="tx_3">
                $PRODUCT_COUNT$ of $FAMILY_PRODUCT_COUNT$ Products
              </a>
              </span>
            </td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>        
      </td>            
      <td width="25%" valign="top" class="borderOdd">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="top" class="tx_1" colspan="3">
              <strong>
                Item#: <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">$ATTRIBUTE_ID_1$</a>
              </strong>
            </td>
          </tr>
          <tr>
            <td height="37" valign="top" class="tx_1" colspan="3">
              <strong>
                Price: &#36; $ATTRIBUTE_ID_5$<br>
               </strong>
            </td>
          </tr>
          <tr>
            <td colspan="3">&nbsp;</td>
          </tr>
            <tr>
            <td width="26px">              
                <input name="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$" style="background-color:#FFFFFF;color: #000000;" size="2" type="text" onkeydown="return keyct(event)" id="txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$"/>
              </td>
              <td>&nbsp;</td>
              <td align="right" width="76px">
                <a style="cursor:pointer;" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$PRODUCT_ID$_$FAMILY_ID$_pp','','images/but_buy2.gif',1)">
                  <img src="images/but_buy1.gif" style="display:$TB_DISPLAY$" name="Image$PRODUCT_ID$_$FAMILY_ID$_pp" width="76px" height="28" border="0" onClick="productbuy('txt$PRODUCT_ID$_$TBT_QTY_AVAIL$_$TBT_MIN_ORD_QTY$_$FAMILY_ID$','$PRODUCT_ID$');"/>
                </a>
              <!--<a href="OrderDetails.aspx?Pid=$PRODUCT_ID$&Qty=$MIN_ORD_QTY$&byp=$BYP$" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$PRODUCT_ID$_$FAMILY_ID$','','images/but_buyitem2.gif',1)">
                <img src="images/but_buyitem1.gif" name="Image$PRODUCT_ID$_$FAMILY_ID$" width="100px" height="25" border="0"/>
              </a>-->
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</TD>