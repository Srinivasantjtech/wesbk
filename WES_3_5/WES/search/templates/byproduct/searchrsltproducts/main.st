<br />
<!--<%--Featured Product--%>
<table width="558" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="30" background="images/23.gif">
      <table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
          <td height="30" class="tx_1">
            <strong>FEATURED PRODUCTS</strong>
          </td>
          <td align="right">&nbsp;</td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="back_9">
      <table width="96%" border="0" cellspacing="0" cellpadding="6">
        <tr>
          <td width="21%" valign="top">
            <span class="tx_1">
              <a href="javascript:Zoom('prodimages$ATTRIBUTE_ID_5333$')" alt="img">
                <img src="prodimages$ATTRIBUTE_ID_5332$" width="100" height="100" alt="img">
          </a>
            </span>
          </td>
          <td width="49%" valign="top" class="tx_1">
            $ATTRIBUTE_ID_41$
          </td>
          <td width="30%" valign="top">
            <table width="96%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td height="37" valign="top" class="tx_1">
                  <strong>
                    Price: &#36;$ATTRIBUTE_ID_4$<br>                 
                  </strong>
                </td>
              </tr>
              <tr>
                <td>
                  <a href="OrderDetails.aspx?Pid=$PRODUCT_ID$&Qty=1" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('featuredproduct','','images/but_buyitem2.gif',1)">
                    <img src="images/but_buyitem1.gif" name="featuredproduct" width="145" height="28" border="0">
                  </a>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
<%--/Featured Product--%>-->
<table align="center" width="558" border="0" cellpadding="0" cellspacing="0" class="border_6">
  <tr>
    <td valign="bottom">
      <table width="558" border="0" cellpadding="0" cellspacing="0" class="tx_1">
        <TR>
          <td colspan="4" valign="center" bgcolor="#a6a3a3" align="left" class="tx_6A">
            &nbsp;&nbsp;&nbsp;$TBW_CATEGORY_NAME$
          </td>
        </TR>
        <tr height="20">
          <td  width="84" valign="bottom" ALIGN="LEFT" bgcolor="#a6a3a3">
            <a href="byproduct.aspx?&pgno=$TBW_CURRENT_PAGE_NO$&cid=$TBW_CATEGORY_ID$&sl2=$TBT_SUBCATID_L2$&fid=$TBT_FAMILY_ID$&parameter=BS&byp=1"   class="tabs">Best Seller</a>
          </td>
          <td  width="96" valign="bottom" ALIGN="LEFT" bgcolor="#a6a3a3">
            <a href="byproduct.aspx?&pgno=$TBW_CURRENT_PAGE_NO$&cid=$TBW_CATEGORY_ID$&sl2=$TBT_SUBCATID_L2$&fid=$TBT_FAMILY_ID$&parameter=AZ&byp=1"   class="tabs">Brand (A - Z)</a>
          </td>
          <td  width="105" valign="bottom" ALIGN="LEFT" bgcolor="#a6a3a3">
            <a href="byproduct.aspx?&pgno=$TBW_CURRENT_PAGE_NO$&cid=$TBW_CATEGORY_ID$&sl2=$TBT_SUBCATID_L2$&fid=$TBT_FAMILY_ID$&parameter=LP&byp=1"   class="tabs">Lowest Price</a>
          </td>
          <td width="270" align="right" bgcolor="#a6a3a3" valign="bottom">
            <strong>Products per page</strong><SELECT id="pppopt" name="pppopt" onchange="GetSelectedIts();" >
              <option value="10">10</option>
              <option value="20">20</option>
              <option value="30">30</option>
              <option value="40">40</option>
              <option value="50">50</option>
              <option value="100">100</option>
              <option value="32767">View all</option>
            </SELECT>  &nbsp;          
          </td>
        </tr>
        <tr>
          <td height="30px" valign="middle" colspan="4" bgcolor="#e8e8e8">
            <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" class="tx_1">
              <!--<tr>
                <td>
                  <strong>Showing</strong> $TBW_START_PAGE_NO$ &ndash; $TBW_END_PAGE_NO$ of $TBW_PRODUCT_COUNT$ products
                </td>
                <TD>
                  <strong>Pages:</strong> $TBW_TOTAL_PAGES$
                </TD>
                <TD>
                  <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$
                </TD>
                <td align="right" class="tx_1">
                  <strong>Page</strong>: $TBW_DISPLAY_PAGE_NO$
</td>
              </tr>-->
              <tr>
                <TD align="left">
                  &nbsp;  <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$ of $TBW_TOTAL_PAGES$
                </TD>
                <td align="left">
                  <strong>Showing</strong> $TBW_START_PAGE_NO$ &ndash; $TBW_END_PAGE_NO$ of $TBW_PRODUCT_COUNT$ Products
                </td>
                <!--<TD align="left">
            <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$
          </TD>-->
                <td align="right" class="tx_1">
                  <table valign="top">
                    <tr>
                      <td class="tx_1">
                        <strong>Page</strong>:&nbsp;
                        $TBW_DISPLAY_PAGE_NO$$TBW_NEXT_PAGE$
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="border_5">
      <TABLE>
        <tr>
          <td>
            $TBWDataList:{
            $it.TBWDataListItem$}$
          </td>
        </tr>
      </TABLE>
    </td>
  </tr>
  <tr>
    <td align="center" valign="middle" height="30px" bgcolor="#e8e8e8">
      <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" class="tx_1">
        <!--<tr>
          <td>
            <strong>Showing</strong> $TBW_START_PAGE_NO$ &ndash; $TBW_END_PAGE_NO$ of $TBW_PRODUCT_COUNT$ products
          </td>
          <TD>
            <strong>Pages:</strong> $TBW_TOTAL_PAGES$
          </TD>
          <TD>
            <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$
          </TD>
          <td align="right" class="tx_1">
            <strong>Page</strong>: $TBW_DISPLAY_PAGE_NO$
          </td>
        </tr>-->
        <tr>
          <TD align="left">
            &nbsp;  <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$ of $TBW_TOTAL_PAGES$
          </TD>
          <td align="left">
            <strong>Showing</strong> $TBW_START_PAGE_NO$ &ndash; $TBW_END_PAGE_NO$ of $TBW_PRODUCT_COUNT$ Products
          </td>
          <!--<TD align="left">
            <strong>Page Number:</strong> $TBW_CURRENT_PAGE_NO$
          </TD>-->
          <td align="right" class="tx_1">
            <table valign="top">
              <tr>
                <td class="tx_1">
                  <strong>Page</strong>:&nbsp;
                  $TBW_DISPLAY_PAGE_NO$$TBW_NEXT_PAGE$
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
<table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" >
  <tr>
    <td width="70%">&nbsp;</td>
    <td width="28%" align="right">
      <a href="#TOP">Top</a>
    </td>
    <td width="2%">&nbsp;</td>
  </tr>
</table>