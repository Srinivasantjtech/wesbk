<td>
  
  <div class="product_top">
    <div class="family_img">
      <div  id="product-image"  style="width:260px; height:260px; float:left;">
        <img style="display: block;max-height:256px;max-width:256px;" alt="" src="prodimages$TBT_TWEB_IMAGE1$"/>
      </div>

      <div class="gallery-thumbnails"  style="float:left;">
        <div id="gallery">
          <ul>
            $TBT_MULTIIMAGES:{
            <li width="50" height="50">
              <a href="prodimages$it.LargeImage$">
                <img src="prodimages$it.Thumpnail$" alt="" style="max-width:50px;max-height:50px"/>
              </a>
            </li>}$
          </ul>
        </div>
      </div>

    </div>
    <div style="float: left;padding: 10px;width: 230px;">
      <h1 class="pad10" style=" color: #666666;font-size: 16px;text-align:left;">$TBT_FAMILY_NAME$ </h1>
      <p align="left">
        $TBT_SHORT_DESCRIPTION$
      </p>
      <div id="div1" style="color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;">
        <p>
          $TBT_MORE$
        </p>
      </div>
      <div id="div2" style="display:none;color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;">
        <p>
          $TBT_DESCALL$
        </p>
      </div>
   
     
    </div>

    <div style=" border-left: 1px solid #C8C8C8;float: left;padding: 20px;width: 180px;">
     
      <div style="border-top:none;" class="box5">
        <table border="1" border-color="green" style="border: 1px solid #C8C8C8; border-collapse: separate; border-collapse: collapse;border-left: 0;">
          <tbody>
            <tr style="background-color: #dff0d8;">
              <td width="28%">ORDER CODE</td>
              <td colspan="2">STOCK STATUS</td>
            </tr>
            <tr>
              <td>$TBT_CODE$</td>
              <td colspan="2">
                $TBT_STOCK_STATUS$
                $TBT_STOCK_STATUS_3$
                $TBT_STOCK_STATUS_1$
              </td>
            </tr>
            <tr style="background-color: #dff0d8;">

              $if(TBT_WITHPRICE)$
              <td>QTY</td>
              <td width="38%">Cost INC GST</td>
              <td width="34%">Cost EX GST</td>
              $endif$
            </tr>
            <tr>
              $if(TBT_WITHPRICE)$
              $TBT_PRICE_TABLE$
              $endif$
            </tr>

          </tbody>
        </table>
      </div>
    </div>
    <div class="clear"></div>

  </div>
  <div class="clear"></div>

  <div class="tabbable">
    <ul  style="color: #FFF;
    text-decoration: none;
  background-color: #F0F0F0;
    padding-right: 15px;
    padding-left: 15px;
    height: 35px;
    margin-right: 1px;
    border: 1px solid #C8C8C8;" >
      <li>
        <a href="#tab1">Product Specs</a>
      </li>
     
    </ul>
    <div class="clear"></div>

  </div >
  <div class="tab-content">
    <div id="tab1"  style="padding: 10px;">
      <table border="1" class="table table-bordered" width="100%">
        <colgroup>
          <col style="background-color:#C8E6FF;"/>
        </colgroup>
        <tbody>
          $TBT_PRODDETAILS:{
          <tr>
            <td width="26%">$it.AttributeName$</td>
            <td width="74%">$it.SpecValue$</td>
          </tr>
          }$
        </tbody>
      </table>
    </div>
   
  </div>
</td>
