<div id="tab4" class="tab_content" style="text-align: left;margin-left: 6px;">
  <br/><br/>
  <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Contact us for Bulk Buy and Project Based Pricing</span>
  <br/><br/>
  <div id="BulkBuyPP" style="display:block;">
    <table>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Product Code*</span>

            <!--<input type="text" id="txtproductcode"  class="cardinputAQ" style="width:248px;"   maxlength="30"   />-->
            <select  id="ddlprodcode" name="ddlprodcode" style="font-size: 12px;height: 30px;padding: 4px;width: 257px;border: 1px solid #b2b2b2;"  onchange="getprodcodevalue();" onblur="Controlvalidate_BulkBuyPP('procode')" tabindex="1">
              $if(TBT_CHK_PRODCOUNT)$
              <option  SELECTED="SELECTED" value="Please Select Product" title="Please Select Product">Please Select Product</option>
              $endif$
              $TBW_DDL_VALUE$
            </select>
            
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errprocode_BBPP" style="display:none;color: Red;"> Select Product code </span>
          </div>
        </td>
        <td width="30px">
          
        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Full Name*</span>

            <input type="text" id="txtFullname_BBPP"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" tabindex="2"/>
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errfullname_BBPP" style="display:none;color: Red;"> Enter Full Name </span>

          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >QTY Required *</span>

            <input type="text" id="txtQTY"  class="cardinputAQ" style="width:248px;" tabindex="3"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('qty')"   onkeypress="return validateNumber(event);"/>
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="ErrQTY" style="display:none;color: Red;"> Enter QTY </span>

          </div>
        </td>
        <td width="30px">

        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Email*</span>
            <input type="text" id="txtEmail"  class="cardinputAQ" style="width:248px;" tabindex="4"  maxlength="50"  onblur="Controlvalidate_BulkBuyPP('email')" />
          </div>
          <div class="form-col-2-81">
            <span class="error-text" id="erremailadd_BBPP" style="display:none;color: Red;"> Enter Email Address </span>
            <span class="error-text" id="errvalidmail_BBPP" style="display:none;color: Red;">Enter Valid Email </span>

          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Delivery Time *</span>

            <input type="text" id="txtdeliverytime"  class="cardinputAQ" style="width:248px;" tabindex="5"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('deltime')" />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errdeliverytime" style="display:none;color: Red;">Enter Delivery Time </span>
          </div>
        </td>
        <td width="30px">

        </td>
        <td>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Phone*</span>

            <input type="text" id="txtPhone_BBPP"  class="cardinputAQ" style="width:248px;height:12px;" tabindex="6"  maxlength="30"  onblur="Controlvalidate_BulkBuyPP('phone');" onkeypress="return validateNumber(event);"  />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errphone_BBPP" style="display:none;color: Red;">Enter Phone Number </span>
          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Target Price</span>

            <input type="text" id="txttargetprice"  class="cardinputAQ" style="width:248px;"  tabindex="7" maxlength="30"  onblur="Controlvalidate_BulkBuyPP('fullname')" />
          </div>
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Notes / Addtional Info.</span>

            <textarea id="txtnotesadditionalinfo" cols="34" tabindex="8"  class="textarea2"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;height:100px;width:435px;"
                                                onblur="Controlvalidate_BulkBuyPP('notes')" onkeypress="textCounter_BulkBuyPP(this,this.form.counter_BBPP,600);" ></textarea>
            <input class="cardinputAQ" type="text" onblur="textCounter_BulkBuyPP(this.form.counter_BBPP,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_BBPP" style="width:35px; color:#B2B2B2;height:12px;" />
              <span style=" color: #7f7f7f;font-size: 11px;font-weight: bold;">Chars Remaining </span>
          </div>
          <div class="form-col-2-81">
            <span class="error-text" id="errnotes" style="display:none;color: Red;"></span>


          </div>
          <div class="cl"></div>
          <!--<div class="form-col-2-81" style="width:248px;">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Code*</span>

            <div style="display: flex;height: 29px;">

              <input type="text" id="txtCaptchCode_BBPP"  class="cardinputAQ" style="width:50px;" tabindex="9"  maxlength="4"  onblur="Controlvalidate_BulkBuyPP('capcode');" onkeypress="return validateNumber(event);"   />
              <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />

            </div>
            <div class="cl" style="height:12px;"></div>
            <div class="form-col-2-81" style="width:248px;">
              <span class="error-text" id="errCaptchCode_BBPP" style="display:none;color: Red;"> Enter the code </span>
              <span class="error-text" id="errCaptchInvalid_BBPP" style="display:none;color: Red;"> Invalid code </span>
              <span class="error-text" id="errCaptchCode1_BBPP" style="display:none;">$CC_CODE$</span>
            </div>
            <div class="cl"></div>
          </div>-->
        </td>
      </tr>
      <tr>
        <td valign="top" width="275px" colspan="3">
          <a onclick="MailSend_BulkBuyPP()" Class="Familysumit" style="width:87px;height:27px; " tabindex="10" >Submit</a>

          <a onclick="MailReset_BulkBuyPP()" Class="Familysumit" style="width:68px;height:27px; " tabindex="11" >Reset</a>
        </td>
      </tr>
    </table>
  </div>
  <div id="BulkBuyPPSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
    <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 663px;">
      <img src="/images/tick2.png"  style="margin-left: -20px;margin-right: 10px;vertical-align: middle;" />
      <span  >
        Thanks for your Bulk Buy enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
      </div>

  </div>

</div>
