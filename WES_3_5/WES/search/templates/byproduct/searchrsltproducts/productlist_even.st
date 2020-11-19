<TD>
  <table width="550" border="0" cellspacing="0" cellpadding="6">
    <tr>

      <td width="50" align="center" valign="top" bgcolor="#F9F9F9">
        <table width="90%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td height="62" align="center" valign="top">
              <label class="tx_1">
                <input id="Chkbox_$PRODUCT_ID$" type="checkbox" name="CompareItem" value="$PRODUCT_ID$"  onclick="CheckCompareCount(document.forms[0].CompareItem,'Chkbox_$PRODUCT_ID$');" />
                <br/>
                <a href="#" onclick="GetCompareItems(document.forms[0].CompareItem,$FAMILY_ID$);">                  
                    Compare<br/>
                      Products</a>
              </label>
            </td>
          </tr>

          <tr>
            <td valign="top">
              <!--<%--$FAMILY_ID$--%><img src="<%--$FAMILY_ID$$ATTRIBUTE_ID_5337$$ATTRIBUTE_ID_5338$--%>" width="49" height="47"><%--/$FAMILY_ID$--%>-->
                </td>
          </tr>
        </table>
      </td>
      <td  width="100" valign="top" bgcolor="#F9F9F9">
        <span class="tx_1">
          <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">
            <b>$FAMILY_NAME$</b>
          </a>
        </span>
      </td>
      <td  width="100" valign="top" bgcolor="#F9F9F9">
        <span class="tx_1">
          $ATTRIBUTE_ID_62$
          <br /><strong>View</strong>: <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$"  class="tx_3">
        <img src="images/ico_details.gif" width="17" height="14" border="0" align="absmiddle" /><br/>Details
        </a>          
        </span>
      </td>
      <td  width="150" align="center" valign="top" bgcolor="#F9F9F9" class="tx_1">
        <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">
          <img src="prodimages$ATTRIBUTE_ID_452$" />
        </a>
    </td>
      <td  width="150" valign="top" bgcolor="#F9F9F9">
        <table width="150" border="0" cellspacing="0" cellpadding="0">

          <tr>
            <td valign="top" class="tx_1">
              <strong>
                Item#: <a href="productdetails.aspx?&pid=$PRODUCT_ID$&fid=$FAMILY_ID$&byp=$BYP$">$ATTRIBUTE_ID_1$</a>       
              </strong>
            </td>
          </tr>
          <tr>
            <td height="37" valign="top" class="tx_1">
              <strong>
                Price: &#36; $ATTRIBUTE_ID_5$<br>         
              </strong>
            </td>
          </tr>
          <tr>
            <td>
              <a href="OrderDetails.aspx?&bulkorder=1&Pid=$PRODUCT_ID$&Qty=$MIN_ORD_QTY$&byp=$BYP$&ORDER_ID=$TBT_ORDER_ID$" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image$PRODUCT_ID$_$FAMILY_ID$','','images/but_buyitem2.gif',1)">
                <img src="images/but_buyitem1.gif" name="Image$PRODUCT_ID$_$FAMILY_ID$" width="145" height="25" border="0"/>
              </a>
            </td>
          </tr>
        </table>
      </td>

    </tr>
  </table>
</TD>