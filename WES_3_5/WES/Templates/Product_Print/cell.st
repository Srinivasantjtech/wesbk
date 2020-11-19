<td>
  <div class="product_top" style="padding: 0 0 25px;">
    <div class="family_img" style="float: left;padding: 10px;width: 260px;">
      <div  id="product-image"  style="width:260px; height:260px; float:left;">
        <img style="display: block;max-height:256px;max-width:256px;" max-height="256px" max-width="256px" alt="" src="prodimages$TBT_TWEB_IMAGE1$"/>
      </div>

<div class="gallery-thumbnails"  style="float:left;">
<div id="gallery" style="float: left;padding: 10px;width: 290px;padding-left:1px;">
<ul style=" list-style-type: none;padding: 0;margin: 0px;width: auto;">
  $TBT_MULTIIMAGES:{
  <li width="50px" height="50px" style="background-attachment: scroll; background-clip: border-box;background-color: transparent; background-image: none; background-origin: padding-box; background-position: 0 0; background-repeat: repeat; background-size: auto auto; float: left; height: 60px; width: 60px;">
<img src="prodimages$it.Thumpnail$" alt="" style="max-width:50px;max-height:50px;border: 2px solid #ccc;"/>
</li>}$
</ul>
</div>
</div>

    </div>
    <div style=" float: left;padding: 10px;width: 230px;font-family: Arial;">
      <h1 class="pad10" style=" color: #666666;font-size: 14px;text-align:left;font-family: Arial;">$TBT_FAMILY_NAME$ </h1>
      <p align="left" style="font-size:11px;font-family: Arial;color: #666666;">
        $TBT_SHORT_DESCRIPTION$
      </p>
      <!--<div id="div1" style="color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;">
        <p>
          $TBT_MORE$
        </p>
      </div>-->
      <div id="div2" style="display:block;color: #666666;font-size: 11px;line-height: 15px;margin: 5px 0 0 0px;overflow:hidden;text-align:left;min-height:160px;font-size:13px;font-family: Arial;">
        <p>
          $TBT_DESCALL$
        </p>
      </div>
   
     
    </div>

    <div style=" border-left: 1px solid #C8C8C8;float: left;padding: 20px;width: 235px;">
      <div style="background-color: #FFFFFF;border: 2px solid #7AC943;padding: 7px;">
        <div style="border-top:none;" class="box5">
          <table  border-color="green" style="border: 1px solid #C8C8C8; border-left:1px solid #c8c8c8; border-bottom:1px solid #c8c8c8; border-collapse: separate; border-collapse: collapse;border-left: 0;">
            <tbody>
              $if(TBT_ETA_PRINT)$
              <tr style="background-color: #dff0d8;border-bottom:1px solid #c8c8c8;">
                <td width="28%" style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >ORDER CODE</td>
                <td style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" colspan="2">STOCK STATUS</td>
                <!--<td style="border-left: 1px solid #c8c8c8;" align="center" >ETA</td>-->
              </tr>
              <tr style="border-bottom:1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;">
                <td style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >$TBT_CODE$</td>
                <td style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" colspan="2" >
                  $TBT_STOCK_STATUS$
                  $TBT_STOCK_STATUS_3$
                  $TBT_STOCK_STATUS_1$
                </td>
                <!--<td  style="border-left: 1px solid #c8c8c8;" align="center" >
                  $TBT_STOCK_STATUS_ETA$
                </td>-->
              </tr>
              <tr style="background-color: #f4f4f4;">
                <td  style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;font-weight:bold;" align="center">ETA</td>
                <td  style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;font-weight:bold;" align="center" colspan="2">
                  $TBT_STOCK_STATUS_ETA$
                </td>
              </tr>
              $else$
              <tr style="background-color: #dff0d8;border-bottom:1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;">
                <td width="28%" style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >ORDER CODE</td>
                <td  style="border-left: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" colspan="2">STOCK STATUS</td>
              </tr>
              <tr>
                <td style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;">$TBT_CODE$</td>
                <td  style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" colspan="2" >
                  $TBT_STOCK_STATUS$
                  $TBT_STOCK_STATUS_3$
                  $TBT_STOCK_STATUS_1$
                </td>
              </tr>
              $endif$
              <tr style="background-color: #dff0d8;">

                $if(TBT_WITHPRICE)$
                <td style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >QTY</td>
                <td width="38%" style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >Cost INC GST</td>
                <td width="34%" style="border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;line-height: 14px;font-size: 13px;padding:4px;font-family: Arial;color: #666666;" align="center" >Cost EX GST</td>
                $endif$
              </tr>
              <tr style="border-left: 1px solid #c8c8c8;font-family: Arial;color: #666666;">
                $if(TBT_WITHPRICE)$
                $TBT_PRICE_TABLE$
                $endif$
              </tr>

            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div style="clear: both;"></div>

  </div>
  <div style="clear: both;"></div>
  <br/>
 
  <!--<div style="color: black; text-decoration: none;background-color: #F0F0F0; padding-right: 15px; padding-left: 15px; height: 35px; margin-right: 1px; border: 1px solid #C8C8C8;">
    Product Specs
    <div class="clear"></div>
  </div >-->
  <div style='width:98%;margin:0 auto; border:1px solid #c8c8c8; border-left:none; height:40px; background:#f3f3f4;'>
    <div style='font-family:arial;font-size:13px;width:110px; height:22px; background:#fff; margin-top:-5px; border-right:1px solid #c8c8c8; border-left:1px solid #c8c8c8; border-top:1px solid #c8c8c8; position:relative; -webkit-border-top-left-radius: 5px; -webkit-border-top-right-radius: 5px; -moz-border-radius-topleft: 5px; -moz-border-radius-topright: 5px; border-top-left-radius: 5px; border-top-right-radius: 5px; color:#2678bd; text-align:center; padding:10px; font-weight:bold; float:left; padding-top:15px;color:#007BDB;'>
      Product Specs
    </div>
  </div>
  <div class="tab-content" style="font-size: 11px;color:#333; padding-left: 0;padding-top: 10px;">
    <div id="tab1"  style="padding: 10px;">
      <table border="0" class="table table-bordered" style=" border-left: 1px solid #C8C8C8; background-color: transparent;margin-bottom: 20px; border-collapse: collapse; border-spacing: 0; border: 1px solid #C8C8C8; border-collapse: separate; border-collapse: collapse;border-left: 0;" width="100%">
        <colgroup>
          <col style="background-color:#C8E6FF;"/>
        </colgroup>
        <tbody>
          $TBT_PRODDETAILS:{
          <tr style="border-bottom: 1px solid #c8c8c8;line-height: 14px;padding:8px;text-align:center;">
            <td width="26%" style="border-left: 1px solid #c8c8c8; line-height: 14px;padding:8px;font-size:13px;font-family: Arial;color: #666666;">$it.AttributeName$</td>
            <td width="74%" style="border-left: 1px solid #c8c8c8; line-height: 14px;padding:8px;font-size:13px;font-family: Arial;color: #666666;">$it.SpecValue$</td>
          </tr>
          }$
        </tbody>
      </table>
    </div>
   
  </div>
  
</td>
