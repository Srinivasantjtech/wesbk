<!--<div id="tab2" class="tab_content" style="text-align: left;margin-left: 6px;">-->
  $if(IS_NO_DOWNLOAD)$
  <br/><br/>
  <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >No downloads currently available for this product.</span>
  $endif$
  <br/><br/>
   <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Please contact us with your download requirements,e.g. User Manual, Data Sheet, etc and will get back to you.</span>
  <br/>
  <br/>
  <div id="DownloadUpdate" style="display:block;">
    <table>
      <tr>
        <td valign="top" width="275px" >
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Full Name*</span>

            <input type="text" id="txtFullname_DU"  class="cardinputAQ" style="width:248px;" tabindex="1"   maxlength="30"  onblur="Controlvalidate_DU('fullname_du')" />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errfullname_DU" style="display:none;color: Red;"> Enter Full Name </span>

          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Email*</span>
            <input type="text" id="txtEmail_DU"  class="cardinputAQ" style="width:248px;"  tabindex="2"  maxlength="50"  onblur="Controlvalidate_DU('email_du')" />
          </div>
          <div class="form-col-2-81">
            <span class="error-text" id="erremailadd_DU" style="display:none;color: Red;"> Enter Email Address </span>
            <span class="error-text" id="errvalidmail_DU" style="display:none;color: Red;">Enter Valid Email </span>

          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Phone*</span>

            <input type="text" id="txtPhone_DU"  class="cardinputAQ" style="width:248px;height:12px;" tabindex="3"   maxlength="30"  onblur="Controlvalidate_DU('phone_du');" onkeypress="return validateNumber(event);"  />
          </div>
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span class="error-text" id="Errphone_DU" style="display:none;color: Red;">Enter Phone Number </span>
          </div>
          <div class="cl"></div>
          <!--<div class="form-col-2-81" style="width:248px;">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Code*</span>

            <div style="display: flex;height: 29px;">

              <input type="text" id="txtCaptchCode_DU"  class="cardinputAQ" style="width:50px;"   maxlength="4" tabindex="4"  onblur="Controlvalidate_DU('capcode_du');" onkeypress="return validateNumber(event);"   />
              <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />

            </div>

          </div>
          <div class="cl" style="height:12px;"></div>
          <div class="form-col-2-81" style="width:248px;">
            <span class="error-text" id="errCaptchCode_DU" style="display:none;color: Red;"> Enter the code </span>
            <span class="error-text" id="errCaptchInvalid_DU" style="display:none;color: Red;"> Invalid code </span>
            <span class="error-text" id="errCaptchCode1_DU" style="display:none;">$CC_CODE$</span>
          </div>
          <div class="cl" style="height: 26px;"></div>-->
        </td>
        <td valign="top">
          <div class="cl"></div>
          <div class="form-col-2-81">
            <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Download Required / Comments*</span>

            <textarea id="txtdownloadre" cols="34"   class="textarea2" tabindex="5"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;height:144px;width:390px;"
                                                onblur="Controlvalidate_DU('notes_du')" onkeypress="textCounter_DU(this,this.form.counter_DU,600);" ></textarea>
            <input class="cardinputAQ" type="text" onblur="textCounter_DU(this.form.counter_DU,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter_DU" style="width:35px; color:#B2B2B2;height:12px;" />
            <span style=" color: #7f7f7f;font-size: 11px;font-weight: bold;">Chars Remaining </span>
          </div>
          <div class="form-col-2-81">
            <span class="error-text" id="errdownloadre" style="display:none;color: Red;">Download Required / Comments.</span>


          </div>
        </td>     
      </tr>   
      <tr>
        <td valign="top" width="275px" height="49px" colspan="2">
          <a onclick="MailSend_DU()" Class="Familysumit" style="width:87px;height:27px; margin-top:0px;" tabindex="6" >Submit</a>

          <a onclick="MailReset_DU()" Class="Familysumit" style="width:68px;height:27px; margin-top:0px;" tabindex="7" >Reset</a>
        </td>
      </tr>
    </table>
  </div>
  <div id="DUSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
    <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 664px;">
      <img src="/images/tick2.png"  style="margin-left: -20px;margin-right: 10px;vertical-align: middle;" />
      <span>
        Thanks for your download enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
      </div>

  </div>
 
<!--</div>-->
