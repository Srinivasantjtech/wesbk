<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td>
      <table border="0" width="300" class="border_7" align="Center" >       
        <tr>
          $if(TBT_PDF_CATALOGUE)$
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:Blue">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:white">WES NEWS</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:white">FORMS / OTHERS</a>
          </td>
          $endif$
          $if(TBT_PDF_NEWS)$
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:white">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:Blue">WES NEWS</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:white">FORMS / OTHERS</a>
          </td>
          $endif$
          $if(TBT_PDF_FORMS)$
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:white">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:white">WES NEWS</a>
          </td>
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:Blue">FORMS / OTHERS</a>
          </td>
          $endif$
        </tr>
      </table>
    </td>
  </tr> 
  <tr>
    <td align="center">
      <table>
        <tr>
          <td valign="top" align="center" colspan="2"  align="left">
            <!--width="580"-->
            <table border="0" width="680" class="border_7">
              $if(TBT_PDF_CATALOGUE)$
              <tr>
                <td align="Left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  File Description
                </td>
                <td align="right" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  PDF Modified
                </td>
                <td align="right" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  PDF Size
                </td>
                <td bgcolor="#0077cc" width="10px"></td>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Download PDF
                </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Online eBook
                </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Browse Online
                </td>
              </tr>
              $endif$
              $if(TBT_PDF_NEWS)$
              <tr>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Published Date
                </td>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  File Description
                </td>
              
                <td align="right" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  PDF Size
                </td>
                <td bgcolor="#0077cc" width="10px"></td>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Download PDF
                </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Online eBook
                </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Browse Online
                </td>
              </tr>
              $endif$
              $if(TBT_PDF_FORMS)$
              <tr>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  File Description
                </td>
                <td align="right" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  PDF Modified
                </td>
                <td align="right" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  PDF Size
                </td>
                <td bgcolor="#0077cc" width="10px"></td>
                <td align="left" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">
                  Download PDF
                </td>
              </tr>
              $endif$
              $TBWDataList:{
              $it.TBWDataListItem$}$
              
            </table>
          
          </td>
        </tr>
       
        <tr>
          <td height="5px" colspan="2"></td>
        </tr>     
      </table>
    </td>
  </tr>
  <tr>
    <td>
      <br/>
      <br/>
    </td>
  </tr>


   

             



    

</table>
      
  $if(TBT_PDF_CATALOGUE)$
<table>
  <tr>

    <div class="fcatalogue_wrap">
                 <td>
	<div class="download_box">
    	<div class="download_title">WES CATALOGUE 2017</div>
        <div class="cat_17"><img src="images/WESCAT-2017.jpg"></div>
        <div class="dbtn_block">
        	<p>Download as a single pdf file:</p>
            <a href="https://www.wes.com.au/media/pdf/wescat2017/WESCAT.2017.ExGST.pdf" target="blank" class="dbutton"><span class="icon_pdf"></span><span class="btn_txt">Download PDF</span></a>
            <span>Note file size is 160Mb</span>
           $Modifieddate_exgst$
            <div style="clear:both"></div>
        </div>
        <div class="vbtn_block">
        	<p>View ebook online:</p>
            <a href="https://www.wes.com.au/media/ebook/wescat2017/"  target="blank" class="vbutton"><span class="icon_view"></span><span class="btn_txt">View eBook Online</span></a>
            <div style="clear:both"></div>
        </div>
    </div>
    </td>
    <td>
    <div class="download_box">
    	<div class="download_title">PRICE LIST SUPPLEMENT</div>
        <div class="cat_17"><img src="images/WESCAT-2017pricelist.jpg"></div>
        <div class="dbtn_block mgntop65">
        	<p>Download as a single pdf file:</p>
            <a href="https://www.wes.com.au/media/pdf/wescat2017/PriceList_2017.pdf"  target="blank" class="dbutton"><span class="icon_pdf"></span><span class="btn_txt">Download PDF</span></a>
            <span>Note file size is < 1Mb</span>
              $Modifieddate_pricelist$
            <div style="clear:both"></div>
        </div>
    </div>
              </td>
</div>
  </tr>

</table>
        $endif$




