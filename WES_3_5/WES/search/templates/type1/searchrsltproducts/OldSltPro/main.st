<TABLE id="ctl00_ContentMain_tblSearchResults" width="588" align="center">
  <TBODY>
    <TR>
      <TD>
        <TABLE class="SearchResultsPaging">
          <TBODY>
            <TR>
              <TD style="WIDTH: 25px">
                <INPUT id="ctl00_ContentMain_btnBuyTop"
title="Buy Selected" style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
type="image" src="images/but_buy_selected.gif"
name="ctl00$ContentMain$btnBuyTop" onclick="return GetAddItems(document.forms[0].AddItem);"/>
              </TD>
              <TD>Search result found: </TD>
              <TD>
                <b>$TBW_PRODUCT_COUNT$</b>
              </TD>
              <TD> Products </TD>
              <TD>No. of Pages: </TD>
              <TD align="left">$TBW_TOTAL_PAGES$</TD>
              <TD>Page Number: </TD>
              <TD align="left">$TBW_CURRENT_PAGE_NO$</TD>
              <TD align="right">
                <a href="powersearch.aspx?pgno=$TBW_PREVIOUS_PG_NO$">Previous</a>
              </TD>
              <TD align="right">
                <a href="powersearch.aspx?pgno=$TBW_NEXT_PG_NO$">Next</a>
              </TD>
            </TR>
          </TBODY>
        </TABLE>
        <br />
      </TD>
    </TR>
    <TR>
      <TD>
        <DIV>
          <TABLE class="SearchResultsTable"
          id="ctl00_ContentMain_SearchResultsGrid_grid" style="WIDTH: 100%"
          cellSpacing="1" cellPadding="2" rules="all" border="1">
            <TBODY>
              <TR class="SearchResultColumnHeading"
              style="FONT-WEIGHT: bold; HEIGHT: 100%" vAlign="top">
                <TH style="WIDTH: 35px" scope="col">Select </TH>
                <TH scope="col"></TH>
                <TH scope="col">Product Name</TH>
                <TH scope="col">Item#</TH>
                <TH scope="col">Price</TH>
                <TH class="SearchResultsBuyColumnHeading" scope="col">Buy Now</TH>
                <TH class="SearchResultParametricColumnHeading" scope="col">Manufacturer</TH>
              </TR>
              <!--
                  <TR class="SearchResultsSortCell">
                    <TD align="middle"></TD>
                    <TD>
                      <A href="/powersearch.aspx?sortcol=$TBW_SORT_COL_ID$">
                        <IMG
                title="Sort Ascending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort Ascending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Up.gif">
                        </A>
                      <A
             
      href="">
                        <IMG
                title="Sort by Descending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort by Descending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Down.gif">
                        </A>
                    </TD>
                    <TD>
                      &nbsp;</TD>
                    <TD>
                      &nbsp;
                      <A
                    href="">
                        <IMG
                title="Sort by Ascending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort by Ascending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Up.gif">
                          <IMG
      title="Sort by Descending"
      style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
      alt="Sort by Descending"
      src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Down.gif">
                          </A>
                    </TD>
                    <TD>
                      &nbsp;
                      <A
                    href="">
                        <IMG
                title="Sort by Ascending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort by Ascending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Up.gif">
                          <IMG
      title="Sort by Descending"
      style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
      alt="Sort by Descending"
      src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Down.gif">
                          </A>
                    </TD>
                    <TD>
                      <A
                    href="">
                        <IMG
                title="Sort by Ascending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort by Ascending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Up.gif">
                          <IMG
      title="Sort by Descending"
      style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
      alt="Sort by Descending"
      src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Down.gif">
                          </A>
                    </TD>
                    <TD>
                      <A
                    href="/powersearch.aspx?sortby=3&">
                        <IMG
                title="Sort by Ascending"
                style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
                alt="Sort by Ascending"
                src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Up.gif">
                          <IMG
      title="Sort by Descending"
      style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
      alt="Sort by Descending"
      src="/search/templates/type1/searchrsltproducts/searchrsltproducts_files/btn_Sort_Down.gif">
                          </A>
                    </TD>
                  </TR>
-->
              $TBWDataList:{
              $it.TBWDataListItem$}$
            </TBODY>
          </TABLE>
        </DIV>
      </TD>
    </TR>
    <TR>
      <TD>
        <br />
        <TABLE class="SearchResultsPaging" border="0">
          <TBODY>
            <TR>
              <TD style="WIDTH: 25px">
                <INPUT id="ctl00_ContentMain_btnBuyBottom" title="Buy Selected"
style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px"
type="image" src="images/but_buy_selected.gif" name="ctl00$ContentMain$btnBuyBottom" onclick="return GetAddItems(document.forms[0].AddItem);"/>
              </TD>
              <TD>Search result found: </TD>
              <TD>
                <b>$TBW_PRODUCT_COUNT$</b>
              </TD>
              <TD> Products</TD>
              <TD>No. of Pages: </TD>
              <TD align="left">$TBW_TOTAL_PAGES$</TD>
              <TD>Page Number: </TD>
              <TD align="left">$TBW_CURRENT_PAGE_NO$</TD>
              <TD align="right">
                <a href="powersearch.aspx?pgno=$TBW_PREVIOUS_PG_NO$">Previous</a>
              </TD>
              <TD align="right">
                <a href="powersearch.aspx?pgno=$TBW_NEXT_PG_NO$">Next</a>
              </TD>

            </TR>
          </TBODY>
        </TABLE>
      </TD>
    </TR>
  </TBODY>
</TABLE>