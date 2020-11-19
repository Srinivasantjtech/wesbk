<div class="box1" style="width:764px;margin:0 0 0 10px;"  id="familyprint_div">
  <div onmouseover="buttonmouseout()" >
  
  $Generateparentfamilyhtml$
</div>

  <div style="visibility:hidden;">
    <table  width="320px" height="20px" align="right" style="border-color:grey">
      <tr>
        <td>
          <div class="costable">

            <div class="pricepopup">
              <div class="popupouterdiv3" id="divprintpop" >
                <div class="popupaero15_1"></div>
                <table>
                  <tr>
                    <td>
                      <div class="row-mid">
                        <div class="poptitle">
                          <span class="titleimg-pop">
                            <img src="/images/print.png" />
                          </span>
                          <span class="title-pop">Print Product Details Page Options</span>
                        </div>
                      </div>
                      <div class="row-mid">
                        <p>Available options when creating Page</p>
                      </div>
                      <div class="row-mid">
                        <div class="opochoice">
                          <table align="center">
                            <tr>
                              <td>
                                <input type="checkbox" id="chkpriceprint" runat="server"  value="false" />
                              </td>
                              <td align="left">

                                <label>Show price</label>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                <input type="checkbox" id="chkdetailprint" runat="server"  value="false" />
                              </td>
                              <td>
                                <label>Display WES Details</label>
                              </td>
                            </tr>

                          </table>




                        </div>
                      </div>

                      <div class="row-center">
                        <table align="center">
                          <tr>
                            <td>
                              <a class="btnbuy2 button psmallsiz btngreen" href="#"   id="btnfamilyprint">Submit</a>


                            </td>
                          </tr>
                        </table >
                      </div>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
          <a id="print" class="aprint costable" href="#" onmouseover="showpopup_print();" style="visibility:hidden;"></a>
          <div class="costable">

            <div class="pricepopup">
              <div class="popupouterdiv4" id="divpdfpop">
                <div class="popupaero15_2"></div>
                <table>
                  <tr>
                    <td>
                      <div class="row-mid">
                        <div class="poptitle">
                          <span class="titleimg-pop">
                            <img src="/images/pdf.png" />
                          </span>
                          <span class="title-pop">PDF Product Details Page Options</span>
                        </div>
                      </div>
                      <div class="row-mid">
                        <p>Available options when creating Page</p>
                      </div>
                      <div class="row-mid">
                        <div class="opochoice">
                          <table align="center">
                            <tr>
                              <td>
                                <input type="checkbox" runat="server" id="chkpricepdf" value="false"/>
                              </td>
                              <td align="left">
                                <label>Show price</label>
                              </td>
                            </tr>
                            <tr>
                              <td>
                                <input type="checkbox" id="chkdetailpdf" runat="server"  value="false" />
                              </td>
                              <td>
                                <label>Display WES Details</label>
                              </td>
                            </tr>
                          </table>
                        </div>
                      </div>
                      <div class="row-center">
                        <table align="center">
                          <tr>
                            <td>

                              <asp:Button ID="pdfbtn" runat="server" class="btnbuy2 button psmallsiz_fam btngreen"
                                     Text="Submit" onclick="pdfbtn_Click"></asp:Button>
                            </td>
                          </tr>
                        </table >
                      </div>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
          <a class="apdf" href="#" onmouseover="showpopup_pdf();"></a>
          <div class="costable">
            <div class="pricepopup">
              <div class="popupouterdiv5" id="divemailpop">
                <div class="popupaero15_3"></div>
                <table>
                  <tr>
                    <td>
                      <div class="row-mid">
                        <div class="poptitle">
                          <span class="titleimg-pop">
                            <img src="/images/email.png" />
                          </span>
                          <span class="title-pop">Email Product Details Page Options</span>

                        </div>
                      </div>
                      <div class="row-mid">
                        <table align="center">
                          <tr>
                            <td style="width:20px">

                              <label>Send to Email Address</label>

                             
                              <textarea runat="server"   id="txtemail" class="input_test" value="" autofocus="true"  rows="1" cols="50" style="width:200px;height:20px"/>

                             
                            </td>
                          </tr>
                          <tr>
                            <td style="width:20px">
                              <label>Notes</label>
                           
                              <textarea runat="server"    id="txtnotes"  value="" autofocus="true"  name="txtnotes" rows="5" cols="50" style="width:200px;height:40px"/>

                            </td>
                          </tr>

                        </table>
                      </div>
                      <div class="row-mid">
                        <p>Available options when creating Page</p>
                      </div>
                      <div class="row-mid">
                        <div class="opochoice">
                          <table align="center">
                            <tr>
                              <td>
                                <input type="checkbox"  runat="server" id="chkPriceemail"  value="false"/>
                              </td>
                              <td align="left">
                                <label>Show price</label>

                              </td>
                            </tr>
                            <tr>
                              <td>
                                <input type="checkbox" id="chkdetailemail" runat="server"  value="false" />
                              </td>
                              <td>
                                <label>Display WES Details</label>
                              </td>
                            </tr>
                          </table>
                        </div>
                      </div>
                      <div class="row-center">
                        <table align="center">
                          <tr>
                            <td>
                            
                              <asp:Button id="emailbtn" runat="server" class="btnbuy2 button psmallsiz_fam btngreen"
                                           Text="Send" onclick="emailbtn_Click" OnClientClick="mailproshow();">
                              </asp:Button>
                            </td>
                          </tr>
                        </table>

                      </div>
                      <label ID="lblmessage" runat ="server" text="">
                      </label>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>

          <a class="aemail" href="#" onmouseover="showpopup_email(); "> </a>

        </td>


      </tr>

    </table>


  </div>


  <div class="tabbable">
    <ul class="tabs">
      <li>
        <a href="#tab1" >Products</a>
        
      </li>
      <li style="display:$ST_Family_Download$" >
        <a href="#tab2" >Downloads</a>
      </li>
      <li  >
        <a href="#tab3" >Ask a Question</a>
      </li>
    </ul>
    <div class="clear"></div>
    <div class="tab-content">
      <div id="tab1" class="tab_content" >
        $ST_Familypage$


      
              
           

      </div>
      
      $DownloadST$
      <div id="tab3" class="tab_content" >
      <div id="divaskquestion" style="display:block;">
        <table >
          <tr>
            <td valign="top" width="275px;" height="270px">
              <div class="cl"></div>

              <div class="form-col-2-81">
                <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Full Name*</span>

                <input type="text" id="txtFullname"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate('fn')" />

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="error-text" id="Errfullname" style="display:none;color: Red;"> Enter Full Name </span>

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Email*</span>


                <input type="text" id="txtEmailAdd"  class="cardinputAQ" style="width:248px;"   maxlength="50"  onblur="Controlvalidate('ea')" />
              </div>
              <div class="form-col-2-81">
                <span class="error-text" id="erremailadd" style="display:none;color: Red;"> Enter Email Address </span>
                <span class="error-text" id="errvalidmail" style="display:none;color: Red;">Enter Valid Email </span>

              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Phone*</span>

                <input type="text" id="txtPhone"  class="cardinputAQ" style="width:248px;"   maxlength="30"  onblur="Controlvalidate('p');" onkeypress="return validateNumber(event);"  />
              </div>
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span class="error-text" id="Errphone" style="display:none;color: Red;">Enter Phone Number </span>
              </div>
              <div class="cl"></div>              
              <div class="form-col-2-81" style="width:248px;">
                <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Code*</span>
                <div>
                <input type="text" id="txtCaptchCode"  class="cardinputAQ" style="width:50px;"   maxlength="4"  onblur="Controlvalidate('cc');" onkeypress="return validateNumber(event);"   />
                <img src="$CAPTCHA_IMAGE$"  width="120" height="40"  />
                </div>
               
              </div>
              <div class="form-col-2-81" style="width:248px;">
                <span class="error-text" id="errCaptchCode" style="display:none;color: Red;"> Enter the code </span>
                <span class="error-text" id="errCaptchInvalid" style="display:none;color: Red;"> Invalid code </span>
                <span class="error-text" id="errCaptchCode1" style="display:none;">$CC_CODE$</span>
              </div>
              <div class="cl"></div>
              <div >
                <a onclick="MailSend()" Class="Familysumit" style="width:87px;height:27px; ">Submit</a>

                <a onclick="MailReset()" Class="Familysumit" style="width:68px;height:27px; ">Reset</a>

              </div>
            </td>
            <td valign="top">
              <div class="cl"></div>
              <div class="form-col-2-81">
                <span style=" color: #7f7f7f;font-size: 12px;font-weight: bold;" >Question*</span>


    
                <textarea id="txtQuestionx" cols="34"   class="textarea2"   rows="10" maxlength="600" style="box-shadow: 0 0 0px #CCCCCC; border:1px solid #b2b2b2;"
                                                      onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" ></textarea>
                <input class="cardinputAQ" type="text" onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" style="width:35px; color:#B2B2B2;height:10px;">
                  <span style=" color: #7f7f7f;">Chars Remaining </span>
                </div>
              <div class="form-col-2-81">
                <span class="error-text" id="errquestion" style="display:none;color: Red;">Enter The Question </span>


              </div>
            </td>
          </tr>
        </table>
      </div>
      <div id="divAskQuestionSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
        <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 558px;">
          <img src="/images/tick2.png"  style="margin-left: -58px;margin-right: 10px;vertical-align: middle;" />
          <span  >
            Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.</span>
          </div>

      </div>
    </div>

  
    </div>
  </div >
</div>
