<tr>
  <TD align="center">
    <TABLE cellSpacing="0" cellPadding="0">
      <TBODY>
        <TR>
          <TD>

            <TABLE>
              <TBODY>
                <TR>
                  <TD>
                    <SPAN
                style="FONT-WEIGHT: bold; WHITE-SPACE: normal">$TBW_ATTRIBUTE_NAME$</SPAN>
                  </TD>
                </TR>
                <TR>
                  <TD>
                    <SELECT multiple="multiple" id="$TBW_ATTRIBUTE_ID$" style="width: 160px; overflow:scroll;"  size="7" name="lst1">
                      $TBWDataList:{
                      $it.TBWDataListItem$}$
                    </SELECT>
                  </TD>
                </TR>
                <TR>
                  <TD>
                    <A href="#" onclick="Reset();">Reset</A>
                  </TD>
                </TR>
                <TR>
                  <TD height="5">
                    
                  </TD>
                </TR>
              </TBODY>
            </TABLE>

          </TD>
          <TD style="WIDTH: 15px" vAlign="bottom">
            &nbsp;
          </TD>
        </TR>
      </TBODY>
    </TABLE>
  </TD>
</tr>