<table width="96%" border="2" cellpadding="0" cellspacing="0">
  <tr>
    <td height="2"></td>
  </tr>
  <tr>
    <td align="center" valign="middle">
      <table width="579" border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td>
            <table>
              <tr>
                <td width="100">
                  <a href="javascript:Zoom('prodimages$TBT_FWEB_IMAGE$')">
                    <img src="prodimages$TBT_FWEB_IMAGE$" width="100px" height="100px"/>
                </a>
                </td>


                <td valign="middle" align="center" width="346">
                  <p class ="tx_15">
                    $TBT_FAMILY_NAME$
                  </p>
                </td>
                <td valign="middle">
                  $TBT_DESCRIPTIONS$
                </td>
              </tr>
            </table>
          </td>
        </tr>

        <%--~Family$TBT_FAMILY_ID$Product--%>
          <tr>
            <td>
              <table width="100%" cellspacing="2" cellpadding="2">
                <tr class="TableFHeadRow">
                  <td>
                    <strong>Item#</strong>
                  </td>
                  <td>
                    <strong>Description</strong>
                  </td>
                  <td  >
                    <strong>Cost</strong>
                  </td>
                  <td >
                    <strong>More Info</strong>
                  </td>
                  <td >
                    <strong>QTY</strong>
                  </td>
                </tr>
                $TBWDataList:{
                $it.TBWDataListItem$}$
              </table>
            </td>
          </tr>
          <%--Family$TBT_FAMILY_ID$Product--%>         
         
          </table>
    </td>
  </tr>
</table>
<br/>

