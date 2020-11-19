<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Products" Codebehind="Product.ascx.cs" %>


<script language="javascript" >
    function validateNumber(event) {
        var key = window.event ? event.keyCode : event.which;

        if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 9
     || event.keyCode == 37 || event.keyCode == 39) {
            return true;
        }
        else if (key < 48 || key > 57) {
            return false;
        }
        else return true;
    }
    function Controlvalidate(ctype) {
        if (ctype == "fn") {
            var dd = document.getElementById("txtFullname");
            var err1 = document.getElementById("Errfullname");
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                dd.style.border = "";
                err1.style.display = "none";

            }
        }
        if (ctype == "ea") {
            var cno = document.getElementById("txtEmailAdd");
            var err1 = document.getElementById("erremailadd");
            var err2 = document.getElementById("errvalidmail");
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {

                cno.style.border = "";
                err1.style.display = "none";

                var vaildemail = checkEmail(cno.value.trim());
                if (vaildemail == false) {
                    err2.style.display = "block";
                } else {
                    err2.style.display = "none";
                }
            }

        }
        if (ctype == "p") {
            var cn = document.getElementById("txtPhone");
            var err1 = document.getElementById("Errphone");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }
        if (ctype == "q") {
            var cn = document.getElementById("txtQuestionx");
            var err1 = document.getElementById("errquestion");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                err1.style.display = "block";
            }
            else {
                cn.style.border = "";
                err1.style.display = "none";
            }
        }
    }
    function checkEmail(inputvalue) {
        var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
        if (pattern.test(inputvalue)) {
            return true;
        } else {
            return false;
        }
    }
    function MailReset() {
        document.getElementById("txtEmailAdd").value = "";
        document.getElementById("txtFullname").value = "";
        document.getElementById("txtPhone").value = "";
        document.getElementById("txtQuestionx").value = "";
    }
    function strtrim(s) {
        s = s.replace(/(^\s*)|(\s*$)/gi, "");
        s = s.replace(/[ ]{2,}/gi, " ");
        s = s.replace(/\n /, "\n");
        return s;
    }
    function MailSend() {


        var ma = document.getElementById("txtEmailAdd");
        var fn = document.getElementById("txtFullname");
        var p = document.getElementById("txtPhone");
        var q = document.getElementById("txtQuestionx");
        var pname = document.getElementById("productcode");

        var valid = true;
        Controlvalidate("fn");
        Controlvalidate("ea");
        Controlvalidate("p");
        Controlvalidate("q");

        if (fn == null || fn.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            
            fn.focus();
            return;
        }

        if (ma == null || ma.value.trim() == "") {
            valid = false;
            //  alert("enter Email id")
            ma.focus();
            return;
        }

        var vaildemail = checkEmail(ma.value.trim());
        if (vaildemail == false) {
            valid = false;
            //  alert("enter valid Email id")
            ma.focus();
            return;
        }
        if (p == null || p.value.trim() == "") {
            valid = false;
            //  alert("enter Phone Numbar")
            p.focus();
            return;
        }
        if (q == null || q.value.trim() == "") {
            valid = false;
            //   alert("enter Question")
            q.focus();
            return;
        }
        var s = fn.value;
        fn.value = s.replace(/'/g, "`");
        s = q.value;
        q.value = s.replace(/'/g, "`");

        if (valid == true) {
            $.ajax({
                type: "POST",
                url: "ProductDetails.aspx/SendAskQuestionMail",
                data: "{'fromid':'" + ma.value.trim() + "','fname':'" + fn.value.trim() + "','phone':'" + p.value.trim() + "','qustion':'" + q.value.trim().replace("'", "`") + "','productcode':'" + pname.innerHTML + "'}",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: OnmailSuccess,
                error: OnmailFailure


            });
        }

    }
    function OnmailSuccess(result) {

        var dt;
        if (result.d != null && result.d != "-1") {

            var q = document.getElementById("divaskquestion");
            var qs = document.getElementById("divAskQuestionSubmit");
            q.style.display = "none";
            qs.style.display = "block";
            //MailReset();
            //alert("Thank you for contacting The Wes, Inc. We will be contact you as soon as possible.");
        }
        else {
            alert("Unable to send your message. Please contact The Wes at Info@wes.com");
        }

    }
    function OnmailFailure(result) {
        alert("Unable to send your message. Please contact The Wes at Info@wes.com");
    }
    function textCounter(field, countfield, maxlimit) {
        if (field.value.length > maxlimit) {
            field.value = field.value.substring(0, maxlimit);
            alert('Enquiry/Comments maximum allowed 600 characters.');
            return false;
        }
        else {
            countfield.value = maxlimit - field.value.length;
        }
    }
    
</script>


<script language="javascript">


       function MailSend_BulkBuyPP() {
           var email = document.getElementById("txtEmail");
           var fullname = document.getElementById("txtFullname_BBPP");
           var deltime = document.getElementById("txtdeliverytime");
           var phone = document.getElementById("txtPhone_BBPP");
           var notes = document.getElementById("txtnotesadditionalinfo");
           var fname_bbpp = document.getElementById("familyName");
           var tarprice = document.getElementById("txttargetprice");
           var qty = document.getElementById("txtQTY");
           var procode = document.getElementById("productcode");
           //var capcode = document.getElementById("txtCaptchCode_BBPP"); 
           //var caperr = document.getElementById("errCaptchCode1_BBPP");
           var valid = true;
           Controlvalidate_BulkBuyPP("fullname"); 
           Controlvalidate_BulkBuyPP("email");
           Controlvalidate_BulkBuyPP("phone");
          // Controlvalidate_BulkBuyPP("notes");
           Controlvalidate_BulkBuyPP("deltime");
           Controlvalidate_BulkBuyPP("qty");
          // Controlvalidate_BulkBuyPP("capcode");
           if (fullname == null || fullname.value.trim() == "") {
               valid = false;
               // alert("enter Full Name")
               // Controlvalidate_BulkBuyPP("fullname");
               fullname.focus();
               return;
           }

           if (email == null || email.value.trim() == "") {
               valid = false;
               //  alert("enter Email id")
               email.focus();
               return;
           }

           var vaildemail = checkEmail(email.value.trim());
           if (vaildemail == false) {
               valid = false;
               email.focus();
               return;
           }
           if (phone == null || phone.value.trim() == "") {
               valid = false;
               phone.focus();
               return;
           }
           if (deltime == null || deltime.value.trim() == "") {
               valid = false;
               deltime.focus();
               return;
           }

//           if (notes == null || notes.value.trim() == "") {
//               valid = false;
//               notes.focus();
//               return;
//           }
           if (qty == null || qty.value.trim() == "") {
               valid = false;
               notes.focus();
               return;
           }

//        if (capcode.value != caperr.innerHTML) {
//               valid = false;
//               // err2.style.display = "block";
//               capcode.focus();
//               return;
//           }




           var s = fullname.value;
           fullname.value = s.replace(/'/g, "`");
           s = notes.value;
           notes.value = s.replace(/'/g, "`");


           if (valid == true) {
               $.ajax({
                   type: "POST",
                   url: "/productdetails.aspx/SendBulkBuyProjectPricing",
                   data: "{'productcode':'" + procode.innerHTML + "','fullname':'" + fullname.value.trim() + "','qty':'" + qty.value.trim() + "','fromid':'" + email.value.trim() + "','deliverytime':'" + deltime.value.trim() + "','phone':'" + phone.value.trim() + "','targetprice':'" + tarprice.value.trim() + "','notesandaddtionalinfo':'" + notes.value.trim() + "'}",
                   contentType: "application/json;charset=utf-8",
                   dataType: "json",
                   success: OnmailSuccess_BBPP,
                   error: OnmailFailure_BBPP


               });
           }



       }

       function Controlvalidate_BulkBuyPP(ctype) {
           if (ctype == "fullname") {
               var dd = document.getElementById("txtFullname_BBPP");
               var err1 = document.getElementById("Errfullname_BBPP");
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   dd.style.border = "";
                   err1.style.display = "none";

               }
           }
           if (ctype == "deltime") {
               var dd = document.getElementById("txtdeliverytime");
               var err1 = document.getElementById("Errdeliverytime");
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   dd.style.border = "";
                   err1.style.display = "none";

               }
           }

           if (ctype == "qty") {
               var dd = document.getElementById("txtQTY");
               var err1 = document.getElementById("ErrQTY");
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   dd.style.border = "";
                   err1.style.display = "none";

               }
           }
           if (ctype == "capcode") {
               var cn = document.getElementById("txtCaptchCode_BBPP");
               var err1 = document.getElementById("errCaptchCode_BBPP");
               var err2 = document.getElementById("errCaptchInvalid_BBPP");
               var err3 = document.getElementById("errCaptchCode1_BBPP");
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";


                   if (cn.value != err3.innerHTML) {
                       err2.style.display = "block";
                   } else {
                       err2.style.display = "none";
                   }

               }

           }
           if (ctype == "email") {
               var cno = document.getElementById("txtEmail");
               var err1 = document.getElementById("erremailadd_BBPP");
               var err2 = document.getElementById("errvalidmail_BBPP");
               if (cno != null && cno.value == "") {
                   cno.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {

                   cno.style.border = "";
                   err1.style.display = "none";

                   var vaildemail = checkEmail(cno.value.trim());
                   if (vaildemail == false) {
                       err2.style.display = "block";
                   } else {
                       err2.style.display = "none";
                   }
               }

           }
           if (ctype == "phone") {
               var cn = document.getElementById("txtPhone_BBPP");
               var err1 = document.getElementById("Errphone_BBPP");
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";
               }
           }
           if (ctype == "notes") {
               var cn = document.getElementById("txtnotesadditionalinfo");
               var err1 = document.getElementById("errnotes");
               if (cn != null && cn.value == "") {

                  // cn.style.border = "1px solid #FF0000";
                   //err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";
               }
           }

       }
       function textCounter_BulkBuyPP(field, countfield, maxlimit) {
           if (field.value.length > maxlimit) {
               field.value = field.value.substring(0, maxlimit);
               alert('Notes / Addtional Info maximum allowed 600 characters.');
               return false;
           }
           else {
               countfield.value = maxlimit - field.value.length;
           }
       }

       function MailReset_BulkBuyPP() {
           //document.getElementById("txtproductcode").value = "";
           document.getElementById("txtFullname_BBPP").value = "";
           document.getElementById("txtQTY").value = "";
           document.getElementById("txtEmail").value = "";
           document.getElementById("txtdeliverytime").value = "";
           document.getElementById("txtPhone_BBPP").value = "";
           document.getElementById("txttargetprice").value = "";
           document.getElementById("txtCaptchCode_BBPP").value = "";
           document.getElementById("txtnotesadditionalinfo").value = "";
           document.getElementById("Errfullname_BBPP").style.display = "none";
           document.getElementById("ErrQTY").style.display = "none";
           document.getElementById("erremailadd_BBPP").style.display = "none";
           document.getElementById("Errdeliverytime").style.display = "none";
           document.getElementById("Errphone_BBPP").style.display = "none";
           document.getElementById("errnotes").style.display = "none";
           document.getElementById("errvalidmail_BBPP").style.display = "none";
           document.getElementById("errCaptchInvalid_BBPP").style.display = "none";
           document.getElementById("errCaptchCode_BBPP").style.display = "none";
           document.getElementById("errCaptchCode1_BBPP").style.display = "none";
           document.getElementById("txtFullname_BBPP").style.border = "";
           document.getElementById("txtQTY").style.border = "";
           document.getElementById("txtEmail").style.border = "";
           document.getElementById("txtdeliverytime").style.border = "";
           document.getElementById("txtPhone_BBPP").style.border = "";
           document.getElementById("txtnotesadditionalinfo").style.border = "";
        //   document.getElementById("txtCaptchCode_BBPP").style.border = "";

       }

       function OnmailSuccess_BBPP(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("BulkBuyPP");
               var bbppsub = document.getElementById("BulkBuyPPSubmit");
               bbpp.style.display = "none";
               bbppsub.style.display = "block";
           }
           else {
               alert("Unable to send your message. Please contact The Wes at Info@wes.com");
           }

       }
       function OnmailFailure_BBPP(result) {
           alert("Unable to send your message. Please contact The Wes at Info@wes.com");
       }

       function ValidateCaptcha_BBPP() {
           var rtn = false;
           var p = document.getElementById("txtCaptchCode_BBPP");
           $.ajax({
               type: "POST",
               url: "/productdetails.aspx/ValidateCaptcha_BBPP",
               data: "{'secCode':'" + p.value.trim() + "'}",
               contentType: "application/json;charset=utf-8",
               dataType: "json",
               success: function (result) {
                   if (result.d == -2) {
                       rtn = false;

                   } else if (result.d == -1) {
                       rtn = false;
                   }
               },
               error: function (result) {
                   rtn = false;
               }


           });
           return rtn;
       }
       function OnCaptchaSuccess_BBPP(result) {
           if (result.d == -2) {
               captchavalid = false;
           } else if (result.d == -1) {
               captchavalid = false;
           }

       }
       function OnCaptchaFailure_BBPP(result) {

           captchavalid = false;

       }
       function MailSend_DU() {
           var fullname_du = document.getElementById("txtFullname_DU");
           var email_du = document.getElementById("txtEmail_DU");
           var phone_du = document.getElementById("txtPhone_DU");
           var notes_du = document.getElementById("txtdownloadre");
           var procode_du = document.getElementById("productcode");
           //var capcode_du = document.getElementById("txtCaptchCode_DU");
           //var caperr_du = document.getElementById("errCaptchCode1_DU");
           var valid = true;
           Controlvalidate_DU("fullname_du");
           Controlvalidate_DU("email_du");
           Controlvalidate_DU("phone_du");
           Controlvalidate_DU("notes_du");
          // Controlvalidate_DU("capcode_du");
           if (fullname_du == null || fullname_du.value.trim() == "") {
               valid = false;
               // alert("enter Full Name")
               // Controlvalidate_BulkBuyPP("fullname");
               fullname_du.focus();
               return;
           }

           if (email_du == null || email_du.value.trim() == "") {
               valid = false;
               //  alert("enter Email id")
               email_du.focus();
               return;
           }
//           if (capcode_du.value != caperr_du.innerHTML) {
//               valid = false;
//               // err2.style.display = "block";
//               capcode_du.focus();
//               return;
//           }
           var vaildemail = checkEmail(email_du.value.trim());
           if (vaildemail == false) {
               valid = false;
               email_du.focus();
               return;
           }
           if (phone_du == null || phone_du.value.trim() == "") {
               valid = false;
               phone_du.focus();
               return;
           }
           if (notes_du == null || notes_du.value.trim() == "") {
               valid = false;
               notes_du.focus();
               return;
           }

           var s = fullname_du.value;
           fullname_du.value = s.replace(/'/g, "`");
           s = notes_du.value;
           notes_du.value = s.replace(/'/g, "`");


           if (valid == true) {
               $.ajax({
                   type: "POST",
                   url: "/productdetails.aspx/DownloadUpdate",
                   data: "{'fullname':'" + fullname_du.value.trim() + "','fromid':'" + email_du.value.trim() + "','phone':'" + phone_du.value.trim() + "','downloadrequire':'" + notes_du.value.trim() + "','productcode':'" + procode_du.innerHTML + "'}",
                   contentType: "application/json;charset=utf-8",
                   dataType: "json",
                   success: OnmailSuccess_DU,
                   error: OnmailFailure_DU


               });
           }



       }

       function Controlvalidate_DU(ctype) {
           if (ctype == "fullname_du") {
               var dd = document.getElementById("txtFullname_DU");
               var err1 = document.getElementById("Errfullname_DU");
               if (dd != null && dd.value == 0) {

                   dd.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   dd.style.border = "";
                   err1.style.display = "none";

               }
           }

           if (ctype == "email_du") {
               var cno = document.getElementById("txtEmail_DU");
               var err1 = document.getElementById("erremailadd_DU");
               var err2 = document.getElementById("errvalidmail_DU");
               if (cno != null && cno.value == "") {
                   cno.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {

                   cno.style.border = "";
                   err1.style.display = "none";

                   var vaildemail = checkEmail(cno.value.trim());
                   if (vaildemail == false) {
                       err2.style.display = "block";
                   } else {
                       err2.style.display = "none";
                   }
               }

           }
           if (ctype == "capcode_du") {
               var cn = document.getElementById("txtCaptchCode_DU");
               var err1 = document.getElementById("errCaptchCode_DU");
               var err2 = document.getElementById("errCaptchInvalid_DU");
               var err3 = document.getElementById("errCaptchCode1_DU");
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";


                   if (cn.value != err3.innerHTML) {
                       err2.style.display = "block";
                   } else {
                       err2.style.display = "none";
                   }

               }

           }
           if (ctype == "phone_du") {
               var cn = document.getElementById("txtPhone_DU");
               var err1 = document.getElementById("Errphone_DU");
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";
               }
           }
           if (ctype == "notes_du") {
               var cn = document.getElementById("txtdownloadre");
               var err1 = document.getElementById("errdownloadre");
               if (cn != null && cn.value == "") {

                   cn.style.border = "1px solid #FF0000";
                   err1.style.display = "block";
               }
               else {
                   cn.style.border = "";
                   err1.style.display = "none";
               }
           }

       }
       function textCounter_DU(field, countfield, maxlimit) {
           if (field.value.length > maxlimit) {
               field.value = field.value.substring(0, maxlimit);
               alert('Notes / Addtional Info maximum allowed 600 characters.');
               return false;
           }
           else {
               countfield.value = maxlimit - field.value.length;
           }
       }

       function MailReset_DU() {
           document.getElementById("txtFullname_DU").value = "";
           document.getElementById("txtEmail_DU").value = "";
           document.getElementById("txtPhone_DU").value = "";
           document.getElementById("txtdownloadre").value = "";
           document.getElementById("txtCaptchCode_DU").value = "";
           document.getElementById("Errfullname_DU").style.display = "none";
           document.getElementById("erremailadd_DU").style.display = "none";
           document.getElementById("Errphone_DU").style.display = "none";
           document.getElementById("errdownloadre").style.display = "none";
           document.getElementById("errvalidmail_DU").style.display = "none";
           //document.getElementById("errCaptchInvalid_DU").style.display = "none";
           //document.getElementById("errCaptchCode_DU").style.display = "none";
           //document.getElementById("errCaptchCode1_DU").style.display = "none";
           document.getElementById("txtFullname_DU").style.border = "";
           document.getElementById("txtEmail_DU").style.border = "";
           document.getElementById("txtPhone_DU").style.border = "";
           document.getElementById("txtdownloadre").style.border = "";
         //  document.getElementById("txtCaptchCode_DU").style.border = "";
       }

       function OnmailSuccess_DU(result) {

           var dt;
           if (result.d != null && result.d != "-1") {

               var bbpp = document.getElementById("DownloadUpdate");
               var bbppsub = document.getElementById("DUSubmit");
               bbpp.style.display = "none";
               bbppsub.style.display = "block";
           }
           else {
               alert("Unable to send your message. Please contact The Wes at Info@wes.com");
           }

       }
       function OnmailFailure_DU(result) {
           alert("Unable to send your message. Please contact The Wes at Info@wes.com");
       }
       function ValidateCaptcha_DU() {
           var rtn = false;
           var p = document.getElementById("txtCaptchCode_DU");
           $.ajax({
               type: "POST",
               url: "/productdetails.aspx/ValidateCaptcha_DU",
               data: "{'secCode':'" + p.value.trim() + "'}",
               contentType: "application/json;charset=utf-8",
               dataType: "json",
               success: function (result) {
                   if (result.d == -2) {
                       rtn = false;

                   } else if (result.d == -1) {
                       rtn = false;
                   }
               },
               error: function (result) {
                   rtn = false;
               }


           });
           return rtn;
       }
       function OnCaptchaSuccess_DU(result) {
           if (result.d == -2) {
               captchavalid = false;
           } else if (result.d == -1) {
               captchavalid = false;
           }

       }
       function OnCaptchaFailure_DU(result) {

           captchavalid = false;

       }
       </script>
<script language="javascript">

    function productbuy(buyvalue, pid) {
        var qtyval = document.forms[0].elements[buyvalue].value;
        var qtyavail = document.forms[0].elements[buyvalue].name;
        qtyavail = qtyavail.toString().split('_')[1];
        var minordqty = document.forms[0].elements[buyvalue].name;
        minordqty = minordqty.toString().split('_')[2];

        var fid = document.forms[0].elements[buyvalue].name;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";


        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
            alert('Invalid Quantity!');
            window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            document.forms[0].elements[buyvalue].focus();
            return false;
        }
        else {
            //window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval;
            CallProductPopup(orgurl,buyvalue, pid, qtyval, 0, fid);
        }
    }
    
    function keyct(e) {
        var keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

        }
        else {
            e.preventDefault();
        }
    }
   
    function showpopup_print() {

      try
      {
        var objDiv = document.getElementById("divprintpop");
       objDiv.style.visibility = 'visible';
       var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';
       var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
       var chkprice = document.getElementById("chkpriceprint");
       chkprice.checked = true;
       var chkDetialsemail = document.getElementById("chkDetailprint");
       chkDetialsemail.checked = true;
       chkDetialsemail.focus();
      
  }
  catch (Error)
  {
  alert(Error);
  }
  }
  

    function showpopup_pdf() {
        var objDiv = document.getElementById("divpdfpop");
        objDiv.style.visibility = 'visible';
        var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
        var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
        var chkprice = document.getElementById("chkpricepdf");
        chkprice.checked = true;
       var chkDetialsemail = document.getElementById("chkDetailpdf");
       chkDetialsemail.checked = true;

       var dpy = $("#avrp").css("display");
       if (dpy == "none") {
           $(".popupouterdiv4").css({ 'left': '0px' });
       }
       else {
           $(".popupouterdiv4").css({ 'left': '171px' });
       }
       
    }
    function showpopup_email() {
        var objDiv = document.getElementById("divemailpop");
        objDiv.style.visibility = 'visible';
        var chkprice = document.getElementById("chkPriceemail");
        chkprice.checked = true;
        var chkDetialsemail = document.getElementById("chkDetialsemail");
        chkDetialsemail.checked = true;
     
        var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
        var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';

        document.getElementById('txtemail').value.trim();
        document.getElementById('txtemail').focus();
    }

    function printmethodclose() {
      
        var objDiv = document.getElementById('divprintpop');
        objDiv.style.visibility = 'hidden';
      
    }
    function pdfmethodclose() {

        var objDiv = document.getElementById('divpdfpop');
        objDiv.style.visibility = 'hidden';
      
    }
    function emailmethodclose() {

        var objDiv = document.getElementById('divemailpop');
        objDiv.style.visibility = 'hidden';
     

    }
    function emailmethodclear() {

        var objDiv = document.getElementById('divemailpop');
        objDiv.style.visibility = 'hidden';
        var txtemail = document.getElementById("txtemail");
        txtemail.value = "";
        var txtnotes = document.getElementById("txtnotes");
        txtnotes.value = "";

    }
     function buttonmouseout() {
        
        
             printmethodclose();
             pdfmethodclose();
             emailmethodclose();
     }
     function buttonmouseout_clear() {


         printmethodclose();
         pdfmethodclose();
         emailmethodclear();
     }
</script>
 <script language="javascript" type="text/javascript" src="Scripts/jquery172.js"></script>
  <script type="text/javascript">
      $(document).ready(function () {
          var dpy = $("#avrp").css("display");
          if (dpy == "none") {
              $("#apdf").removeClass("printpdf-btn");
              $("#apdf").addClass("printpdf-btnlg");
          }
          else {
              $("#apdf").removeClass("printpdf-btnlg");
              $("#apdf").addClass("printpdf-btn");
          }
      });
  </script>
 <script type="text/javascript">
     $(document).ready(function () {
         $("#lmo").hide();
         $("#preview").toggle(function () {
             $("#div1").hide();
             $("#div2").show();
             $("#lmo").show();
             $("#smo").hide();
         }, function () {
             $("#div1").show();
             $("#div2").hide();
             $("#smo").show();
             $("#lmo").hide();
         });
      
     });

</script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("#btnPrint").live("click", function () {
            var print_data = getdata();
            // alert(dd);

            var windowUrl = 'http://staging2.wesonline.com.au/productdetails';
            var windowName = 'Print Product Details';
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            var product_PW = '';
            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                var left = (screen.width / 2) - (600 / 2);
                var top = (screen.height / 2) - (600 / 2);
                product_PW = window.open('', '', 'width=600,height=550,top=' + top + ',left=' + left + '');

            }
            else {

                var left = (screen.width / 2) - (600 / 2);
                var top = (screen.height / 2) - (600 / 2);
                product_PW = window.open(windowUrl, windowName, 'width=600,height=550,resizable=1,top=' + top + ',left=' + left + '');

            }




            if (navigator.userAgent.toLowerCase().indexOf("chrome") > -1) {

                alert("Leaving Print page will block the parent window!\Use Cancel button to close the Print Preview Window.\n");

                product_PW.document.write(print_data);
                product_PW.document.close();
                product_PW.focus();
                product_PW.print();
                //product_PW.close();
                //            var url = window.location.toString();
                //            alert(url);
                //            window.location.href(url);
                //            Family_PW.close();
            }
            else {
                product_PW.document.write(print_data);
                product_PW.document.close();
                product_PW.focus();
                product_PW.print();
                product_PW.close();
            }
//            product_PW.document.write(print_data);
//            product_PW.document.close();
//            product_PW.print();
//            product_PW.close();

            // var divContents = $("#result").html();
            // var printWindow = window.open('', '', 'height=400,width=800,scroll=yes');
            // printWindow.document.write(divContents);
            // printWindow.document.close();
            // printWindow.print();



            //divContents = $("#result").html();
       

            // var divContents = $("#result").html();
            // var printWindow = window.open('', '', 'height=400,width=800');
            // printWindow.document.write('<html><head><title>DIV Contents</title>');
            //  printWindow.document.write('</head><body >');
            //  printWindow.document.write(divContents);
            //  printWindow.document.write('</body></html>');
            // printWindow.document.close();
            // printWindow.print();
        });


           
    </script>

<script type="text/javascript">
    function getdata() {
        try {

            var printContent = document.getElementById("chkpriceprint");
            var chkprintprice = document.getElementById("chkDetailprint");
            var hfpid = document.getElementById("ctl00_maincontent_Product1_hfpid");
            var hffid = document.getElementById("ctl00_maincontent_Product1_hffid");
            var hfcid = document.getElementById("ctl00_maincontent_Product1_hfcid");
            var hfpath = document.getElementById("ctl00_maincontent_Product1_hfpath");

            var printdet = false;
            var printprice = false;

            var cellx = "cell";
            if (printContent.checked) {
                printprice = true;
            }
            if (chkprintprice.checked) {
                printdet = true;
            }
            // authWindow = window.open('about:blank', '', 'left=20,top=20,width=400,height=300,toolbar=0,resizable=1');
            var result = "";
            $.ajax({
                type: "POST",
                url: "ProductDetails.aspx/Strval",
                data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','pid':'" + hfpid.value + "','fid':'" + hffid.value + "','cid':'" + hfcid.value + "','path':'" + hfpath.value + "'  }",
                contentType: "application/json; charset=utf-8",
                async: false, 
                dataType: "json",
                success: function (data) {

                    result = data.d; 

//                    setTimeout(function () {
//                        alert(data.d);
//                        $("#result").append(data.d);
//                          }, 6000);


                    // $("#result").html(data.d);

                   // setTimeout(function () { $("#result").html(data.d); }, 3000);

                   // authWindow.location(data.d);
                   // window.open(data.d, '_blank');
                     // var divContents = data.d;
//                      var printWindow = window.open('', '', 'height=400,width=800,scroll=yes');
//                     printWindow.document.write(divContents);
//                     printWindow.document.close();
//                     printWindow.print();

                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            })

            return result;

            printmethodclose();
            
        }
        catch (Error) {
            alert(Error);
        }
    }
</script>

 <script language="javascript" type = "text/javascript">
     function test() {
         alert("aaa");
     }
 
     function printPage(str) {
       
         var windowUrl = 'about:blank';
         var uniqueName = new Date();
         var windowName = 'Print' + uniqueName.getTime();
         var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

         printWindow.document.write(str);

         printWindow.document.close();
         printWindow.focus();
         printWindow.print();
         printWindow.close();
        
     }

     function webmethodprint() {
         try {
           
             var printContent = document.getElementById("chkpriceprint");
             var chkprintprice = document.getElementById("chkDetailprint");
             var hfpid = document.getElementById("ctl00_maincontent_Product1_hfpid");
             var hffid = document.getElementById("ctl00_maincontent_Product1_hffid");
             var hfcid = document.getElementById("ctl00_maincontent_Product1_hfcid");
             var hfpath = document.getElementById("ctl00_maincontent_Product1_hfpath");
           
             var printdet = false;
             var printprice = false;
           
             var cellx = "cell";
             if (printContent.checked) {
                 printprice = true;
             }
             if (chkprintprice.checked) {
                 printprice = true;
             }

             $.ajax({
                 type: "POST",
                 url: "ProductDetails.aspx/Strval",
                 data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','pid':'" + hfpid.value + "','fid':'" + hffid.value + "','cid':'" + hfcid.value + "','path':'" + hfpath.value + "'  }",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {

                     var windowUrl = 'http://staging2.wesonline.com.au/productdetails';

                     var windowName = 'Print Product Details';
                     var printWindow = '';
                     // window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

                     var ua = window.navigator.userAgent;
                     var msie = ua.indexOf("MSIE ");


                     if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {    // If Internet Explorer, return version number
                         var left = (screen.width / 2) - (600 / 2);
                         var top = (screen.height / 2) - (600 / 2);
                         printWindow = window.open('', '', 'width=600,height=550,top=' + top + ',left=' + left + '');
                         // printWindow = window.open('', '', 'left=50000,top=50000,width=0,height=0');
                     }
                     else {              // If another browser, return 0

                         var left = (screen.width / 2) - (600 / 2);
                         var top = (screen.height / 2) - (600 / 2);
                         // alert(left);
                         // alert(top);
                         //printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
                         printWindow = window.open(windowUrl, windowName, 'width=600,height=550,menubar=1,resizable=1,top=' + top + ',left=' + left + '');
                        // window.open("contactus.aspx", "fdhjdf", 'width=600,height=550,top=' + top + ',left=' + left + '');
                         // if (printWindow == null || typeof (printWindow) == 'undefined') {
                         //    alert('Please disable your pop-up blocker and click the "Open" link again.');
                         // }

                     }


                     printWindow.document.write(data.d);

                     printWindow.document.close();

                     if (navigator.userAgent.toLowerCase().indexOf('chrome') >= 64) {
                         // Chrome Browser Detected?

                         printWindow.PPClose = false;

                         printWindow.onbeforeunload = function () {

                             // Before Window Close Event
                             if (printWindow.PPClose == false) {
                                 // Close not OK?

                                 return 'Leaving this page will block the parent window!\nPlease select "Stay on this Page option" and use the\nCancel button instead to close the Print Preview Window.\n';

                             }
                         }
                         printWindow.focus();

                         printWindow.print();

                         alert("Leaving Print page will block the parent window!\Use Cancel button to close the Print Preview Window.\n");                              // Clear Close Flag                                          // Print preview
                         printWindow.PPClose = true;                                      // Set Close Flag to OK.
                     }
                     else {

                         printWindow.print();
                         printWindow.focus();
                         printWindow.close();
                     }

                 },
                 error: function (xhr, status, error) {
                     var err = eval("(" + xhr.responseText + ")");
                     alert(err.Message);
                 }
             })
             printmethodclose();
         }
         catch (Error) {
             alert(Error);
          }
        }


        function webmethodemail() {
            try {

                var emailContent = document.getElementById("chkPriceemail");
                var chkemailprice = document.getElementById("chkDetialsemail");
                var txtemailid = document.getElementById("txtemail");
                
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

                if (!filter.test(txtemailid.value.trim())) {
                    alert('Please provide a valid email address');
                    txtemailid.focus;
                    return false;
                }
                // document.getElementById("mailpro").style.visibility = 'visible';

                document.getElementById("TB_overlay1").style.visibility = 'visible';
                document.getElementById("TB_window1").style.visibility = 'visible';

                var notes = document.getElementById("txtnotes");
            
                var hfpid = document.getElementById("ctl00_maincontent_Product1_hfpid");
                var hffid = document.getElementById("ctl00_maincontent_Product1_hffid");
                var hfcid = document.getElementById("ctl00_maincontent_Product1_hfcid");
                var hfpath = document.getElementById("ctl00_maincontent_Product1_hfpath");
              
                var printdet = false;
                var printprice = false;            
                var cellx = "cell";
                var emailid = txtemailid.value;
                if (emailContent.checked) {
                    printprice = true;
                  
                }
                if (chkemailprice.checked) {
                    printdet = true;
                }

                var hurl = "productdetails.aspx?emailprice=" + printprice + "&emaildet=" + printdet + "&pid=" + hfpid.value + "&fid=" + hffid.value + "&cid=" + hfcid.value + "&path=" + hfpath.value;


                $.ajax({
                    type: "POST",
                    url: "ProductDetails.aspx/StrEmail",
                    data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','emailid':'" + emailid + "','notes':'" + notes.value + "', 'pid':'" + hfpid.value + "','fid':'" + hffid.value + "','cid':'" + hfcid.value + "','path':'"+ hfpath.value +"' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var d = data.d;
                        emailmethodclose();
                        window.location.href = hurl;
                    },
                    error: function (xhr, status, error) {
                        var err = eval("(" + xhr.responseText + ")");
                        alert(err.Message);
                    }
                })
               
            }
            catch (Error) {
                alert(Error);
            }
        }
        function webmethodpdf() {
            try {
                
                var chkpdfContent = document.getElementById("chkpricepdf");
                var chkpdfprice = document.getElementById("chkDetailpdf");
               
                var printprice = "false";
                var printdet = "false";
                var cellx = "cell_pdf";
                var hfpid = document.getElementById("ctl00_maincontent_Product1_hfpid");
                var hffid = document.getElementById("ctl00_maincontent_Product1_hffid");
                var hfpath = document.getElementById("ctl00_maincontent_Product1_hfpath");
                var hfcid = document.getElementById("ctl00_maincontent_Product1_hfcid");
              
                if (chkpdfContent.checked) {
                    printprice = "true";
                    document.getElementById("ctl00_maincontent_Product1_hfpricepdf").value=printprice;
                  
                }
                if (chkpdfprice.checked) {
                    printdet = "true";
                    document.getElementById("ctl00_maincontent_Product1_hfdetailspdf").value=printdet;
                }
                var hurl = "productdetails.aspx?printprice=" + printprice + "&printdet=" + printdet + "&pid=" + hfpid.value + "&fid=" + hffid.value + "&cid=" + hfcid.value + "&path=" + hfpath.value;
                pdfmethodclose();
                window.location.href = hurl;
                


            }
            catch (Error) {
                alert(Error);
            }
        }
  
    
    </script> 
 <script type="text/javascript">
     $(document).ready(function () {
         $(".tab_content").hide(); //Hide all content
         $("ul.tabs li:first").addClass("active").show(); //Activate first tab
         $(".tab_content:first").show(); //Show first tab content
         //On Click Event
         $("ul.tabs li").click(function () {
             $("ul.tabs li").removeClass("active"); //Remove any "active" class
             $(this).addClass("active"); //Add "active" class to selected tab
             $(".tab_content").hide(); //Hide all tab content
             var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content
             $(activeTab).fadeIn(); //Fade in the active content
             return false;
         });
     });
</script>
  <script type="text/javascript">
      function SetCursorPosition(pos) {
           // HERE txt is the text field name
          var obj = document.getElementById('txtemail');
          //alert(obj);
          //FOR IE
          if (obj.setSelectionRange) {
              obj.focus();
              obj.setSelectionRange(pos, pos);
          }

          // For Firefox
          else if (obj.createTextRange) {
              var range = obj.createTextRange();
              range.collapse(true);
              range.moveEnd('character', pos);
              range.moveStart('character', pos);
              range.select();
          }
      }

      function SetCursorPosition_notes(pos) {
          // HERE txt is the text field name
          var obj = document.getElementById('txtnotes');

          //FOR IE
          if (obj.setSelectionRange) {
              obj.focus();
              obj.setSelectionRange(pos, pos);
          }

          // For Firefox
          else if (obj.createTextRange) {
              var range = obj.createTextRange();
              range.collapse(true);
              range.moveEnd('character', pos);
              range.moveStart('character', pos);
              range.select();
          }
      }
</script>

<script type="text/javascript">
    $(document).ready(function () {

        document.getElementById('txtemail').value.trim();
        document.getElementById('txtnotes').value.trim();
    }); 

</script>
<script type="text/javascript" language="javascript">
    $('html').click(function (e) {
        if (e.target.id == 'jquery-lightbox') {
            $("#jquery-lightbox").remove();
            $("#jquery-overlay").remove();
        }
    });

    function btn_popupclose() {
        $("#jquery-lightbox").remove();
        $("#jquery-overlay").remove();
    }
</script>

<script type="text/javascript" language="javascript">
    function scrolldown() {

        // $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 140 }, 500);
        $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() - 400 }, 500);
        var stop = $("#lightbox-image-details").scrollLeft();
        if (stop == '0') {

            $("#lightbox-image-up").css({ 'visibility': 'hidden' });
            $("#lightbox-image-up").css({ 'cursor': 'default' });
        }
        else {
            $("#lightbox-image-down").css({ 'visibility': 'visible' });
            $("#lightbox-image-down").css({ 'cursor': 'pointer' });
        }
    }
    var j = 0;
    function scrollup() {

        //$("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 140 }, 500);
        $("#lightbox-image-details").animate({ "scrollLeft": $("#lightbox-image-details").scrollLeft() + 400 }, 500);
        $("#lightbox-image-up").css({ 'visibility': 'visible' });
        $("#lightbox-image-up").css({ 'cursor': 'pointer' });
        var sbot = $(window).scrollLeft() + $(window).width();
        // if ($("#lightbox-image-details").scrollLeft() + 100 >= $("#lightbox-image-details-caption").innerWidth()) {

        // alert($("#lightbox-image-details").scrollLeft());
        // alert($("#lightbox-image-details-caption").innerWidth());
        var ua = window.navigator.userAgent;
        var msiechk = ua.indexOf("MSIE ");
        var lidcwidth = 0;
        if (navigator.userAgent.toLowerCase().indexOf('chrome') >= 64 || msiechk > 0) {
            lidcwidth = $("#lightbox-image-details-caption").innerWidth() + 1370;
        }
        else {
            lidcwidth = $("#lightbox-image-details-caption").innerWidth()
        }
        if ($("#lightbox-image-details").scrollLeft() + 400 >= lidcwidth) {
            $("#lightbox-image-details").animate({ "scrollLeft": 0 }, 500);
            $("#lightbox-image-up").css({ 'visibility': 'hidden' });
            $("#lightbox-image-up").css({ 'cursor': 'default' });
        }

        //alert($("#lightbox-image-details-caption").scrollWidth)
        //alert(document.getElementById('lightbox-image-details-caption').scrollWidth);

    }
</script>


<table align="center" width="100%" border="0" cellspacing="0" cellpadding="0" style="padding-left:5px;">
     <tr>
    <td align="center">        
      <table align="center" width="100%" border="0" cellspacing="0" cellpadding="0" >
                 <tr>
                 <td>
                         <%Response.Write(Bread_Crumbs());  %>
                 </td>
                    </tr>
                    <tr>
                    <td height="4px"></td>
                    </tr>
                    
                    </table>                 
      <table width="100%"  border="0" cellspacing="0" cellpadding="0">

        <tr>


       
            <td >   
                   <div class="box1" style="width:762px;margin:0 0 0 10px;">
                  <% Response.Write(ST_Product()); %>
                  </div>   
                               
            </td>
            
          </tr>    
         
        </table>
        
         <asp:HiddenField ID="hfpid" runat ="server" >
        </asp:HiddenField>
        <asp:HiddenField ID="hffid" runat ="server" >
        </asp:HiddenField>
       <asp:HiddenField ID="hfcid" runat ="server" >
        </asp:HiddenField>
        <asp:HiddenField ID="hfdetailspdf" runat ="server" >
        </asp:HiddenField>
         <asp:HiddenField ID="hfpricepdf" runat ="server" >
        </asp:HiddenField>
        <asp:HiddenField ID="hfpath" runat ="server" >
        </asp:HiddenField>
    </td>
      </tr>
   
</table>
<%--<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">
<div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
</div>
<div id="ModalPanel_mailpro" class="PopUpDisplayStyleemailpro"  align="center" style="position: fixed; z-index: 10001; top:48%;left:40%;">
<img alt="" src="images/sending-gif.gif"  />
</div>

</div>--%>

<%--
<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">
<div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
</div>
<div id="ModalPanel_mailpro" class="PopUpDisplayStyleemailpro"  align="center" style="background-image: url(../images/sending-gif.gif);width:320px;height:50px;position:fixed;z-index: 10001; top:48%;left:38.3%;">
</div>

</div>--%>

<div id="TB_window1" style="display: block;height:50px;width:320px; margin-left: -161px;visibility:hidden;">
<div id="TB_ajaxContent1" class="TB_modal" style="width:320px;height:50px;padding:0px;">
<img alt="" src="images/sending-gif.gif"  />
</div>
</div>
<div id="TB_overlay1" class="TB_overlayBG" style="visibility:hidden;"></div>

<asp:HiddenField ID="hfPN" runat="server"> </asp:HiddenField>
<asp:HiddenField ID="hfFN" runat="server"> </asp:HiddenField>


 
 <script type="text/javascript" language="javascript">
     jQuery(document).ready(function () {
         $("img.lazy").lazyload({
             failure_limit: 10
         });
     });
     (function ($, window, document, undefined) { var $window = $(window); $.fn.lazyload = function (options) { var elements = this; var $container; var settings = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: window, data_attribute: "original", skip_invisible: true, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; function update() { var counter = 0; elements.each(function () { var $this = $(this); if (settings.skip_invisible && !$this.is(":visible")) { return } if ($.abovethetop(this, settings) || $.leftofbegin(this, settings)) { } else { if (!$.belowthefold(this, settings) && !$.rightoffold(this, settings)) { $this.trigger("appear"); counter = 0 } else { if (++counter > settings.failure_limit) { return false } } } }) } if (options) { if (undefined !== options.failurelimit) { options.failure_limit = options.failurelimit; delete options.failurelimit } if (undefined !== options.effectspeed) { options.effect_speed = options.effectspeed; delete options.effectspeed } $.extend(settings, options) } $container = (settings.container === undefined || settings.container === window) ? $window : $(settings.container); if (0 === settings.event.indexOf("scroll")) { $container.bind(settings.event, function () { return update() }) } this.each(function () { var self = this; var $self = $(self); self.loaded = false; if ($self.attr("src") === undefined || $self.attr("src") === false) { if ($self.is("img")) { $self.attr("src", settings.placeholder) } } $self.one("appear", function () { if (!this.loaded) { if (settings.appear) { var elements_left = elements.length; settings.appear.call(self, elements_left, settings) } $("<img />").bind("load", function () { var original = $self.attr("data-" + settings.data_attribute); $self.hide(); if ($self.is("img")) { $self.attr("src", original) } else { $self.css("background-image", "url('" + original + "')") } $self[settings.effect](settings.effect_speed); self.loaded = true; var temp = $.grep(elements, function (element) { return !element.loaded }); elements = $(temp); if (settings.load) { var elements_left = elements.length; settings.load.call(self, elements_left, settings) } }).attr("src", $self.attr("data-" + settings.data_attribute)) } }); if (0 !== settings.event.indexOf("scroll")) { $self.bind(settings.event, function () { if (!self.loaded) { $self.trigger("appear") } }) } }); $window.bind("resize", function () { update() }); if ((/(?:iphone|ipod|ipad).*os 5/gi).test(navigator.appVersion)) { $window.bind("pageshow", function (event) { if (event.originalEvent && event.originalEvent.persisted) { elements.each(function () { $(this).trigger("appear") }) } }) } $(document).ready(function () { update() }); return this }; $.belowthefold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = (window.innerHeight ? window.innerHeight : $window.height()) + $window.scrollTop() } else { fold = $(settings.container).offset().top + $(settings.container).height() } return fold <= $(element).offset().top - settings.threshold }; $.rightoffold = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.width() + $window.scrollLeft() } else { fold = $(settings.container).offset().left + $(settings.container).width() } return fold <= $(element).offset().left - settings.threshold }; $.abovethetop = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollTop() } else { fold = $(settings.container).offset().top } return fold >= $(element).offset().top + settings.threshold + $(element).height() }; $.leftofbegin = function (element, settings) { var fold; if (settings.container === undefined || settings.container === window) { fold = $window.scrollLeft() } else { fold = $(settings.container).offset().left } return fold >= $(element).offset().left + settings.threshold + $(element).width() }; $.inviewport = function (element, settings) { return !$.rightoffold(element, settings) && !$.leftofbegin(element, settings) && !$.belowthefold(element, settings) && !$.abovethetop(element, settings) }; $.extend($.expr[":"], { "below-the-fold": function (a) { return $.belowthefold(a, { threshold: 0 }) }, "above-the-top": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-screen": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-screen": function (a) { return !$.rightoffold(a, { threshold: 0 }) }, "in-viewport": function (a) { return $.inviewport(a, { threshold: 0 }) }, "above-the-fold": function (a) { return !$.belowthefold(a, { threshold: 0 }) }, "right-of-fold": function (a) { return $.rightoffold(a, { threshold: 0 }) }, "left-of-fold": function (a) { return !$.rightoffold(a, { threshold: 0 }) } }) })(jQuery, window, document);
</script>
  