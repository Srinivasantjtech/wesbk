<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_family" Codebehind="family.ascx.cs" %>
<%@ Register Src="searchby.ascx" TagName="searchby" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">
    $('body').click(function () {
        try {
            document.getElementById('testdiv').style.visibility = 'hidden';
        }
        catch (Error)
        { }
        
 
      

    });
    $(function () {
        $('.tab_click').click(function () {
            try {
                document.getElementById('testdiv').style.visibility = 'hidden';
            }
            catch (Error)
            { }
            // or alert($(this).hash();
        });
    });
    </script>
<script type="text/javascript" >
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
              err1.style.display="block";
            }
            else {
                dd.style.border = "";
                err1.style.display="none";
               
            }
        }
        if (ctype == "ea") {
            var cno = document.getElementById("txtEmailAdd");
            var err1 = document.getElementById("erremailadd");
            var err2 = document.getElementById("errvalidmail");
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
                 err1.style.display="block";
            }
            else {

                cno.style.border = "";
                err1.style.display="none";

                var vaildemail = checkEmail(cno.value.trim());
              if (vaildemail==false) {
                 err2.style.display="block";
              }else
                 {
                 err2.style.display="none";
                }
            }
              
        }
        if (ctype == "p") {
            var cn = document.getElementById("txtPhone");
            var err1 = document.getElementById("Errphone");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                 err1.style.display="block";
            }
            else {
                    cn.style.border = "";
                 err1.style.display="none";                
            }
        }
        if (ctype == "q") {
            var cn = document.getElementById("txtQuestionx");
                var err1 = document.getElementById("errquestion");
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
                 err1.style.display="block";
            }
            else {
                cn.style.border = "";
                err1.style.display="none";                
            }
        }
    }
   
    function MailReset() {
         document.getElementById("txtEmailAdd").value="";
        document.getElementById("txtFullname").value="";
         document.getElementById("txtPhone").value="";
         document.getElementById("txtQuestionx").value="";
    }
    function strtrim(s) {
    s = s.replace(/(^\s*)|(\s*$)/gi,"");
    s = s.replace(/[ ]{2,}/gi," ");
    s = s.replace(/\n /,"\n");
    return s;
}
    function MailSend() {


        var ma = document.getElementById("txtEmailAdd");
        var fn = document.getElementById("txtFullname");
        var p = document.getElementById("txtPhone");
        var q = document.getElementById("txtQuestionx");
        var fname = document.getElementById("familyName");
       
        var valid = true;
        Controlvalidate("fn");
        Controlvalidate("ea");
        Controlvalidate("p");
        Controlvalidate("q");

        if (fn == null || fn.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate("fn");
            fn.focus();
            return;
        }

        if (ma == null || ma.value.trim() == ""  ) {
            valid = false;
          //  alert("enter Email id")
            ma.focus();
            return;
        }

        var vaildemail = checkEmail(ma.value.trim());
      if (vaildemail==false) {
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
                url: "family.aspx/SendAskQuestionMail",
                data: "{'fromid':'" + ma.value.trim() + "','fname':'" + fn.value.trim() + "','phone':'" + p.value.trim() + "','qustion':'" + q.value.trim() + "','familyName':'" + fname.innerHTML + "'}",
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
            q.style.display="none";
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
  <script type="text/javascript">
      window.onbeforeunload = function (e) {
          $("#" + '<%= hfcheckload.ClientID %>').val("1");
        $("#" + '<%= HFcnt.ClientID %>').val("0");
    }
    </script>
<script type="text/javascript">


    $(document).ready(function () {






        function lastPostFunc() {

            try {
            
              

                var irecords = '11';
                var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
                var hno = hfpageno;
                $(".lodder" + hno).append("<img src='images/bigLoader.gif' width='6%' height ='6%'/>");
                var fid = $("#" + '<%= hffid.ClientID %>').val();

                hfpageno = parseInt(hfpageno) + 1;

                var eapath = $("#" + '<%= hfeapath.ClientID %>').val();


                $("#" + '<%= HFcnt.ClientID %>').val(hfpageno)
                var pagecount = $("#" + '<%= itotalrecords.ClientID %>').val();

                var Rawurl = $("#" + '<%= hfrawurl.ClientID %>').val();

                $.ajax({
                    type: "POST",
                    url: "/family.aspx/DynamicPag",
                    data: "{'ipageno':" + hfpageno + ",'_Fid':'" + fid + "','eapath':'" + eapath + "','Rawurl':'" + Rawurl + "','Pagecnt':'" + pagecount + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                      
                        if (data.d != "") {
                         
                            $('.divLoadData:last').before(data.d);

                            // $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_balrecords_cnt").val("0");
                            jQuery(document).ready(function () {
                                $("img.lazy").lazyload({

                                });
                            });

                            $("#" + '<%= hfcheckload.ClientID %>').val("1");
                         
                         
                            $(".lodder" + hno).empty();
                        }
                        else {

                         
                            $("#" + '<%= hfcheckload.ClientID %>').val("0");
                            $(".lodder" + hno).empty();
                        
                            $('#data').toggle();
                            $('#data').show();
                        }
                        $('#divPostsLoader').empty();
                    },
                    error: function (xhr, status, error) {
                        $('#tblload').hide();
                        $(".lodder").empty();
                        //                        var err = eval("(" + xhr.responseText + ")");
                        //                        alert(err);
                    }
                })
            }

            catch (err) {
                alert(err.Message);
            }
        };
        $(window).scroll(function () {

            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
         
            if (checkload == 1 && $(window).scrollTop() > 800) {

                $("#" + '<%= hfcheckload.ClientID %>').val("0");
                lastPostFunc();
            }
            else {
                //$('#tblload').hide();
            }


        });
    });
</script>
<script language="javascript" type="text/javascript">
    function MailSend_DU() {
        var fullname_du = document.getElementById("txtFullname_DU");
        var email_du = document.getElementById("txtEmail_DU");
        var phone_du = document.getElementById("txtPhone_DU");
        var notes_du = document.getElementById("txtdownloadre");
        var fname_du = document.getElementById("familyName");
      //  var capcode_du = document.getElementById("txtCaptchCode_DU");
      //  var caperr_du = document.getElementById("errCaptchCode1_DU");
        var valid = true;
        Controlvalidate_DU("fullname_du");
        Controlvalidate_DU("email_du");
        Controlvalidate_DU("phone_du");
        Controlvalidate_DU("notes_du");
       // Controlvalidate_DU("capcode_du");

        if (fullname_du == null || fullname_du.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate_DU("fullname_du");
            fullname_du.focus();
            return;
        }

        if (email_du == null || email_du.value.trim() == "") {
            valid = false;
            //  alert("enter Email id")
            email_du.focus();
            return;
        }

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
//        if (capcode_du.value != caperr_du.innerHTML) {
//            valid = false;
//            // err2.style.display = "block";
//            capcode_du.focus();
//            return;
//        }
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
                url: "/family.aspx/DownloadUpdate",
                data: "{'fullname':'" + fullname_du.value.trim() + "','fromid':'" + email_du.value.trim() + "','phone':'" + phone_du.value.trim() + "','downloadrequire':'" + notes_du.value.trim() + "','familyName':'" + fname_du.innerHTML + "'}",
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
        //document.getElementById("txtCaptchCode_DU").style.border = "";
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
            url: "/family.aspx/ValidateCaptcha_DU",
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

<script language="javascript" type="text/javascript">
    function MailSend_BulkBuyPP() {
        var email = document.getElementById("txtEmail");
        var fullname = document.getElementById("txtFullname_BBPP");
        var deltime = document.getElementById("txtdeliverytime");
        var phone = document.getElementById("txtPhone_BBPP");
        var notes = document.getElementById("txtnotesadditionalinfo");
        var fname_bbpp = document.getElementById("familyName");
        var tarprice = document.getElementById("txttargetprice");
        var qty = document.getElementById("txtQTY");
        // var procode = document.getElementById("txtproductcode");
       // var capcode = document.getElementById("txtCaptchCode_BBPP");
      //  var caperr = document.getElementById("errCaptchCode1_BBPP");
        var valid = true;
        Controlvalidate_BulkBuyPP("fullname");
        Controlvalidate_BulkBuyPP("email");
        Controlvalidate_BulkBuyPP("phone");
        // Controlvalidate_BulkBuyPP("notes");
        Controlvalidate_BulkBuyPP("deltime");
        Controlvalidate_BulkBuyPP("qty");
     //   Controlvalidate_BulkBuyPP("capcode");
        var e = document.getElementById("ddlprodcode");
        var procode = e.options[e.selectedIndex].text;
        Controlvalidate_BulkBuyPP("procode");
        if (fullname == null || fullname.value.trim() == "") {
            valid = false;
            // alert("enter Full Name")
            Controlvalidate_BulkBuyPP("fullname");
            fullname.focus();
            return;
        }

        if (procode == "Please Select Product") {
            valid = false;
            procode.focus();
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
//        if (capcode.value != caperr.innerHTML) {
//            valid = false;
//            // err2.style.display = "block";
//            capcode.focus();
//            return;
//        }
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

        //        if (notes == null || notes.value.trim() == "") {
        //            valid = false;
        //            notes.focus();
        //            return;
        //        }
        if (qty == null || qty.value.trim() == "") {
            valid = false;
            notes.focus();
            return;
        }






        var s = fullname.value;
        fullname.value = s.replace(/'/g, "`");
        s = notes.value;
        notes.value = s.replace(/'/g, "`");


        if (valid == true) {
            $.ajax({
                type: "POST",
                url: "/family.aspx/SendBulkBuyProjectPricing",
                data: "{'productcode':'" + procode + "','fullname':'" + fullname.value.trim() + "','qty':'" + qty.value.trim() + "','fromid':'" + email.value.trim() + "','deliverytime':'" + deltime.value.trim() + "','phone':'" + phone.value.trim() + "','targetprice':'" + tarprice.value.trim() + "','notesandaddtionalinfo':'" + notes.value.trim() + "','familyName':'" + fname_bbpp.innerHTML + "'}",
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

        if (ctype == "procode") {

            var dd = document.getElementById("ddlprodcode");
            var err1 = document.getElementById("Errprocode_BBPP");
            if (dd != null && dd.value == "Please Select Product") {

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
        if (ctype == "notes") {
            var cn = document.getElementById("txtnotesadditionalinfo");
            var err1 = document.getElementById("errnotes");
            if (cn != null && cn.value == "") {

                //cn.style.border = "1px solid #FF0000";
                // err1.style.display = "block";
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
        // document.getElementById("txtproductcode").value = "";
        document.getElementById("txtFullname_BBPP").value = "";
        document.getElementById("txtQTY").value = "";
        document.getElementById("txtEmail").value = "";
        document.getElementById("txtdeliverytime").value = "";
        document.getElementById("txtPhone_BBPP").value = "";
        document.getElementById("txttargetprice").value = "";
        document.getElementById("txtnotesadditionalinfo").value = "";
        document.getElementById("txtCaptchCode_BBPP").value = "";
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
        document.getElementById("Errprocode_BBPP").style.display = "none";
        document.getElementById("txtFullname_BBPP").style.border = "";
        document.getElementById("txtQTY").style.border = "";
        document.getElementById("txtEmail").style.border = "";
        document.getElementById("txtdeliverytime").style.border = "";
        document.getElementById("txtPhone_BBPP").style.border = "";
        document.getElementById("txtnotesadditionalinfo").style.border = "";
    //    document.getElementById("txtCaptchCode_BBPP").style.border = "";
        document.getElementById("ddlprodcode").style.border = "";
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

    function getprodcodevalue() {
        var e = document.getElementById("ddlprodcode");
        var strURL = e.options[e.selectedIndex].text;
        var strURL1 = e.options[e.selectedIndex].value;
        // alert(strURL);
    }

    function ValidateCaptcha_BBPP() {
        var rtn = false;
        var p = document.getElementById("txtCaptchCode_BBPP");
        $.ajax({
            type: "POST",
            url: "/family.aspx/ValidateCaptcha_BBPP",
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

</script>

<script type="text/javascript">
    function productbuy(buyvalue, pid) {
        try
        {
            document.getElementById('testdiv').style.visibility = 'hidden';
        }
        catch(Error)
        {}
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
           // window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval;
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
</script>
<script type="text/javascript">
    function test(id) {
        try
        {
            var testid = "popupouterdiv" + id;
            var objDiv = document.getElementById(testid);
            var pidr = objDiv.id;
            document.getElementById(pidr).style.visibility = 'hidden';
            var podv = $(objDiv).html();
            // var objDiva = document.getElementById(id);
            var d = document.getElementById(pidr);
            var menuDiv1 = document.getElementById('testdiv');
            var menuDiv = document.getElementById('testdiv');
            $(menuDiv).html('');
            $(menuDiv).css("width", "220px");
            $(menuDiv).css("visibility", "visible");
            $(menuDiv).css("padding", "5px");
            $(menuDiv).append(podv);
            var IE = document.all ? true : false;
            if (IE) {
                tempX = event.clientX + document.documentElement.scrollLeft;
                tempY = event.clientY + document.documentElement.scrollTop;
                MouseX = tempX;
                MouseY = tempY;
                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
            }
            );
            }
            else {
                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
            }
            );
            }
        }
        catch(Error)
        {
        }
    }
    function Mouseout(id) {
        var testid = "popupouterdiv" + id;
        var objDiv = document.getElementById(testid);
        var pidr = objDiv.id;
        document.getElementById(pidr).style.visibility = 'hidden';
        document.getElementById('testdiv').style.visibility = 'hidden';
    }
    function Moutstocktestdiv() {

        document.getElementById('testdiv').style.visibility = 'hidden';
    }
    function Moutstocktestdivimg() {

        document.getElementById('testdivimg').style.visibility = 'hidden';
    }
    function createMenuDiv() {
        var menuDiv = document.createElement("div");
        $(menuDiv).css("background-color", "yellow");
        $(menuDiv).css("z-index", "1");
        $(menuDiv).css("width", "220px");
        $(menuDiv).css("height", "70px");
        $(menuDiv).css("visibility", "visible");
        return menuDiv;
    }
    function Moverstockstatus(id) {
        try
        {
            var testid = "pid" + id;
            var objDiv = document.getElementById(testid);
            var pidr = objDiv.id;
            document.getElementById(pidr).style.visibility = 'hidden';
            var podv = $(objDiv).html();
            var objDiva = document.getElementById(id);
            // var pida = objDiva.id;
            var d = document.getElementById(pidr);
            var menuDiv1 = document.getElementById('testdiv');
            var menuDiv = document.getElementById('testdiv');
            $(menuDiv).html('');
            $(menuDiv).css("width", "220px");
            $(menuDiv).css("visibility", "visible");
            $(menuDiv).css("padding", "5px");
            $(menuDiv).append(podv);
            var IE = document.all ? true : false;
            if (IE) {
                tempX = event.clientX + document.documentElement.scrollLeft;
                tempY = event.clientY + document.documentElement.scrollTop;
                MouseX = tempX;
                MouseY = tempY;
                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
            }
            );
            }
            else {
                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
            }
            );
            }
        }
        catch(Error)
        {
        
        }
    }
    function Moutstockstatus(id) {
        try
        {
            var testid = "pid" + id;
            var objDiv = document.getElementById(testid);
            var pidr = objDiv.id;
            document.getElementById(pidr).style.visibility = 'hidden';
            document.getElementById('testdiv').style.visibility = 'hidden';
        }
        catch (Error)
        { }
    }
    //    new script
    function Moverimgtag(id) {
        try
        {
            var testid = "pro_img_popup" + id;
            var objDiv = document.getElementById(testid);
            var pidrnew = objDiv.id;
            document.getElementById(pidrnew).style.visibility = 'hidden';
            var podvimg = $(objDiv).html();
            //var objDiva = document.getElementById(id);

            // var pida = objDiva.id;
            var d = document.getElementById(pidrnew);
            var menuDiv1 = document.getElementById('testdivimg');
            var menuDiv = document.getElementById('testdivimg');
            $(menuDiv).html('');
            $(menuDiv).css("visibility", "visible");
            $(menuDiv).append(podvimg);
            var IE = document.all ? true : false;
            if (IE) {
                tempX = event.clientX + document.documentElement.scrollLeft;
                tempY = event.clientY + document.documentElement.scrollTop;
                MouseX = tempX;
                MouseY = tempY;
                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 200) + 'px',
                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
            }
            );
            }
            else {

                $(menuDiv).css(
            {
                position: "absolute",
                left: MouseX - (menuDiv.clientWidth - 200) + 'px',
                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
            }
            );
            }
        }
        catch (error)
        { }
    }
    function Moutimgtag(id) {
        try
        {
        var testid = "pro_img_popup" + id;
        var objDivnew = document.getElementById(testid);
        var pidrnew = objDivnew.id;
        document.getElementById(pidrnew).style.visibility = 'hidden';
        document.getElementById('testdivimg').style.visibility = 'hidden';
        }
        catch (error)
        { }
    }
</script>
<script type="text/javascript">
//    $(document).ready(function () {
//        $(".tab_content").hide(); //Hide all content
//        $(".tab_content_product").hide();
//        $(".title7_prod").hide();

//        $("ul.tabs li:first").addClass("active").show(); //Activate first tab
//        $(".tab_content:first").show(); //Show first tab content
//        //On Click Event
//        $("ul.tabs li").click(function () {
//            $("ul.tabs li").removeClass("active"); //Remove any "active" class
//            $(this).addClass("active"); //Add "active" class to selected tab
//            $(".tab_content").hide(); //Hide all tab content
//            $(".tab_content_product").hide();
//            $(".title7_prod").hide();

//            var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content

//            $(activeTab).fadeIn(); //Fade in the active content
//            if (activeTab == "#tab2") {
//                $(".tab_content_product").show();
//                $(".title7_prod").show();
//           
//            }
//            return false;
//        });
    //    });
    
</script>
<script type="text/javascript">
    $("#btnfamilyprint").live("click", function () {
        var print_data = getFamilyprintdata();
        // alert(print_data);

        var windowUrl = 'http://staging2.wesonline.com.au/productdetails';
        var windowName = 'Print Family Details';
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        var Family_PW = '';
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            var left = (screen.width / 2) - (600 / 2);
            var top = (screen.height / 2) - (600 / 2);
            Family_PW = window.open('', '', 'width=600,height=550,top=' + top + ',left=' + left + '');

        }
        else {

            var left = (screen.width / 2) - (600 / 2);
            var top = (screen.height / 2) - (600 / 2);
            Family_PW = window.open(windowUrl, windowName, 'width=600,height=570,resizable=1,directories=0,dialog=yes,minimizable=no,maximizable=no,close=no,top=' + top + ',left=' + left + '');

        }





        // var product_PW = window.open('', '', 'height=400,width=800,scroll=yes');


        //        Family_PW.document.write(print_data);
        //        Family_PW.document.close();
        //        Family_PW.print();
        //        Family_PW.close();



        if (navigator.userAgent.toLowerCase().indexOf("chrome") > -1) {

            alert("Leaving Print page will block the parent window!\Use Cancel button to close the Print Preview Window.\n");

            Family_PW.document.write(print_data);
            Family_PW.document.close();
            Family_PW.focus();
            Family_PW.print();
           // Family_PW.close();

        }
        else {
            Family_PW.document.write(print_data);
            Family_PW.document.close();
            Family_PW.focus();
            Family_PW.print();
            Family_PW.close();
        }




        // if (navigator.userAgent.match(/MSIE/) !== null) {
        //  Family_PW.location.reload();
        // }
        // return true;

    });
    function getFamilyprintdata() {

        try {


            var printContent = document.getElementById("ctl00_maincontent_ctl00_chkpriceprint");
            var chkprintprice = document.getElementById("ctl00_maincontent_ctl00_chkdetailprint");

            var hffid = document.getElementById("ctl00_maincontent_ctl00_hffid");

            //alert(hffid.value);

            var hfpath = document.getElementById("ctl00_maincontent_ctl00_hfpath");

            var printprice = false;
            var printdet = false;
            var cellx = "cell";
            if (printContent.checked) {
                printprice = true;
            }
            if (chkprintprice.checked) {
                printdet = true;
            }
            var result = "";
            $.ajax({
                type: "POST",
                url: "family.aspx/Strval",
                data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','fid':'" + hffid.value + "','path':'" + hfpath.value + "'}",
                contentType: "application/json; charset=utf-8",
                async: false, 
                dataType: "json",
                success: function (data) {
                    result = data.d;
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


    

     function webmethodprint() {
         try {


             var printContent = document.getElementById("ctl00_maincontent_ctl00_chkpriceprint");
             var chkprintprice = document.getElementById("ctl00_maincontent_ctl00_chkdetailprint");

             var hffid = document.getElementById("ctl00_maincontent_ctl00_hffid");

             //alert(hffid.value);

             var hfpath = document.getElementById("ctl00_maincontent_ctl00_hfpath");

             var printprice = false;
             var printdet = false;
             var cellx = "cell";
             if (printContent.checked) {
                 printprice = true;
             }
             if (chkprintprice.checked) {
                 printdet = true;
             }

             $.ajax({
                 type: "POST",
                 url: "family.aspx/Strval",
                 data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','fid':'" + hffid.value + "','path':'"+hfpath.value +"'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {

                     var windowUrl = 'http://staging2.wesonline.com.au/productdetails';

                     var windowName = 'Print Product Details';
                     var printWindow = ''
                     // window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

                     var ua = window.navigator.userAgent;
                     var msie = ua.indexOf("MSIE ");

                     if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {    // If Internet Explorer, return version number

                         //printWindow = window.open('', '', 'left=50000,top=50000,width=0,height=0');
                         var left = (screen.width / 2) - (600 / 2);
                         var top = (screen.height / 2) - (600 / 2);
                         printWindow = window.open('', '', 'width=600,height=550,top=' + top + ',left=' + left + '');
                     }
                     else {              // If another browser, return 0
                         //alert('otherbrowser');
                         // printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
                         var left = (screen.width / 2) - (600 / 2);
                         var top = (screen.height / 2) - (600 / 2);
                         printWindow = window.open(windowUrl, windowName, 'width=600,height=550,top=' + top + ',left=' + left + '');
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


    
    </script> 
    <script language="javascript" type = "text/javascript"> 
       function showpopup_print() {

      try
      {
          var objDiv = document.getElementById("divprintpop");
       objDiv.style.visibility = 'visible';
       var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';
       var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
       var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkpriceprint");
       chkprice.checked = true;
       var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkdetailprint");
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
      //  var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
      //  var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
        var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkpricepdf");
        chkprice.checked = true;
        var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkdetailpdf");
        chkDetialsemail.checked = true;
        $(".popupouterdiv4").css({ 'left': '1px' });
    }
    function showpopup_email() {
        var objDiv = document.getElementById("divemailpop");
        objDiv.style.visibility = 'visible';
        var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkPriceemail");
        chkprice.checked = true;
        var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkdetailemail");
        chkDetialsemail.checked = true;
     
        var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
        var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';
        //document.getElementById("ctl00_maincontent_ctl00_txtemail").focus();
        //document.getElementById("ctl00$maincontent$ctl00$txtnotes").focus();

        //document.getElementById('ctl00_maincontent_ctl00_txtnotes').focus();

        document.getElementById('ctl00_maincontent_ctl00_txtemail').value.trim();
        document.getElementById('ctl00_maincontent_ctl00_txtemail').focus();
        
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
        var txtemail = document.getElementById("ctl00_maincontent_ctl00_txtemail");
        txtemail.value = "";
        var txtnotes = document.getElementById("ctl00_maincontent_ctl00_txtnotes");
        txtnotes.value = "";

    }
     function buttonmouseout() {
        
        
            // printmethodclose();
             pdfmethodclose();
           //  emailmethodclose();
     }
     function buttonmouseout_clear() {


        // printmethodclose();
         pdfmethodclose();
       //  emailmethodclear();
     }
</script>
<script type="text/javascript">
    function mailproshow() {
        // alert("yes");

        var txtemailvalue = document.getElementById("ctl00_maincontent_ctl00_txtemail").value;
        //   alert(txtemailvalue);
        var vaildemail = checkEmail(txtemailvalue.trim());
        // alert(vaildemail);
        if (vaildemail == false) {
            alert('Please Enter Valid Email Address');
            return false;
        }

        if (txtemailvalue == "" || txtemailvalue.trim() == null) {
            alert('Please Enter Email Address');
            return false;
        }
        else {
            //document.getElementById("mailpro").style.visibility = 'visible';  
            document.getElementById("TB_overlay1").style.visibility = 'visible';
            document.getElementById("TB_window1").style.visibility = 'visible';
            
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

</script>


<script type="text/javascript" language="javascript">
    $('html').click(function (e) {
        if (e.target.id == 'jquery-lightbox') {
            $("#jquery-lightbox").remove();
            // $("#jquery-overlay").remove();
            $("#jquery-overlay").fadeOut(function () { $("#jquery-overlay").remove(); }); $("embed, object, select").css({ visibility: "visible" })
            $(document).unbind();
        }
    });

    function btn_popupclose() {
        $("#jquery-lightbox").remove();
        // $("#jquery-overlay").remove();
        $("#jquery-overlay").fadeOut(function () { $("#jquery-overlay").remove(); }); $("embed, object, select").css({ visibility: "visible" })
        $(document).unbind();
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
<%--<script type="text/javascript">
    $(document).ready(function () {
   
            //document.getElementById('ctl00_maincontent_ctl00_txtemail').value.trim();
           // document.getElementById('ctl00_maincontent_ctl00_txtnotes').value.trim();
    }); 

</script>--%>


<div class="br" style="clear:both;">
 <%
                Response.Write(Bread_Crumbs());
  %>
</div> 
<div class="clear"></div>




<div class="box1" style="width:764px;margin:0 0 0 10px;"  id="familyprint_div">
<div onmouseover="buttonmouseout()" >
<%=ST_FamilypageALLData()%>
<%=Generateparentfamilyhtml()%>

<%
    if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["qf"] != null && HttpContext.Current.Request.QueryString["qf"].ToString() == "1" && HttpContext.Current.Request.QueryString["byp"].ToString() == "2" && HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
    {
%>
<uc5:searchby ID="Searchby2" runat="server" />
<%
    }
%>
</div>
   <div style="visibility:visible;" id="divpdf">
    <table  width="761px"  align="right" style="border-color:grey">
      <tr>
        <td>
          <%--<div class="costable">

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
          <a id="print" class="aprint costable" href="#" onmouseover="showpopup_print();" style="visibility:hidden;"></a>--%>
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
       
         <%-- <div class="costable">
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
        
          <a class="aemail" href="#" onmouseover="showpopup_email(); " style="visibility:hidden;"> </a>--%>
     <%--     <a class="apdf" href="#" onmouseover="showpopup_pdf();"> </a>
          <a href="<%=ST_VPC() %>" class="avpc" onmouseover="buttonmouseout();"></a>--%>
                  <div class="button-group">
	        <a onmouseover="showpopup_pdf();" class="printpdf-btnlg" style="width:460px;">Print PDF Product Page<br/>
               </a>

        <div class="clearfix"></div>
        <a href="<%=ST_VPC() %>" class="all-ctgry-btn" onmouseover="buttonmouseout();" style="width:460px;">
        View all Products from Category <%=ctname%> <br/>
        </a>
        </div>
        </td>
       

      </tr>

    </table>


  </div>
  <div style="height: 6px;" class="clear"></div>
<div class="tabbable" onmouseover="buttonmouseout_clear()">
  <ul class="tabs">
    <li>
      <a href="#tab1" >Products</a>
    </li>
     <li>
      <a href="#tab3" class="tab_click"  >Ask a Question</a>
    </li>
      <li>
        <a href="#tab4" class="tab_click" >Bulk Buy / Project Pricing</a>
      </li>
    <li style=display:<%= ST_Family_Download() %>; >
      <a href="#tab2" class="tab_click"  >Downloads</a>
    </li>
   
  </ul>
  <div class="clear"></div>
  </div>
  <div class="tab-content">
    <div id="tab1" class="tab_content" >
      <%=ST_Familypage("","")%>
    </div>
      <div id="tab3" class="tab_content" >
      <div id="divaskquestion" style="display:block;">
      <table > <tr><td valign="top" width="275px;" height="270px">
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
                 <div >
                      <a onclick="MailSend()" Class="Familysumit" style="width:87px;height:27px; ">Submit</a>
           
                               <a onclick="MailReset()" Class="Familysumit" style="width:68px;height:27px; ">Reset</a>
                              
           </div>
           </td><td valign="top">
            <div class="cl"></div>
                <div class="form-col-2-81">
                  <span style=" color: #7f7f7f;font-size: 12px;font-wseight: bold;" >Question*</span>
                  
          

       <textarea id="txtQuestionx" cols="34"   class="textarea2"   rows="10" maxlength="600"
                                             onblur="Controlvalidate('q')" onkeypress="textCounter(this,this.form.counter,600);" ></textarea>
                                             <input class="cardinputAQ" type="text" onblur="textCounter(this.form.counter,this,600);" value="600" size="3" readonly="readonly" maxlength="3" name="counter" style="width:35px; color:#B2B2B2;height:10px;">
<span style=" color: #7f7f7f;">Chars Remaining </span>
                                            </div>
                                                       <div class="form-col-2-81">
                                                       <span class="error-text" id="errquestion" style="display:none;color: Red;">Enter The Question </span>

                                       
                    </div>
           </td></tr></table>
      </div>           
               <div id="divAskQuestionSubmit" style="text-align:center;display:none; height: 160px; padding-bottom: 135px;padding-top: 33px;" >
                    <div style="background-color: #e6f7ee;border: thin solid #85d6ad;border-radius: 6px;color: #339966;font-size: 12px;font-weight: bold;padding: 14px;width: 558px;">
                    <img src="/images/tick2.png"  style="margin-left: -58px;margin-right: 10px;vertical-align: middle;" />
                    <span  >Thanks for your enquiry. Our Customer Sales Team will be in contact with you Shortly.</apan>
                    </div>
                    
               </div>
    </div>
    <%=ST_BulkBuyPP()%>
     <%=DownloadST %>
  </div>
</div >
<style>
    .ppdivtest:hover, #testdiv:hover
    {
        visibility:visible !important;
    }
     .testdivimgcss:hover, #testdivimg:hover
    {
        visibility:visible !important;
    }
</style>

<div id="testdiv" class="ppdivtest"  onmouseout="Moutstocktestdiv();"></div>
<div id="testdivimg" class="testdivimgcss"  onmouseout="Moutstocktestdivimg();" ></div>
 <asp:HiddenField ID="hffid" runat ="server" ></asp:HiddenField>
  <asp:HiddenField ID="hfpath" runat ="server" ></asp:HiddenField>
<asp:HiddenField ID="hfFN" runat="server"  > </asp:HiddenField>
    <asp:HiddenField ID="HFcnt" runat="server" />
            <asp:HiddenField ID="hfcheckload" runat="server"  />
              <asp:HiddenField ID="itotalrecords" runat="server" />
               <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="hfeapath" runat="server" />
         <asp:HiddenField ID="hfrawurl" runat="server" />
<%--<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">
<div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
</div>
<div id="ModalPanel_mailpro" class="PopUpDisplayStyleemailpro"  align="center" style="position:absolute; z-index: 10001;top:48%;left:40%;">  
<img alt="" src="images/sending-gif.gif"  />
</div>

</div>--%>
<%--<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">
<table  id="ModalPanel_mailpro" width="100%">
<tr>

<td  align="right"  >
<div style="top:48%;z-index:10001;width:320px;position:fixed;">
<img alt="" src="images/sending-gif.gif"  />
</div>
</td>

</tr>
</table>
<div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
</div>
</div>--%>


<%--<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">--%>


<div id="TB_window1" style="display: block;height:50px;width:320px; margin-left: -161px;visibility:hidden;">
<div id="TB_ajaxContent1" class="TB_modal" style="width:320px;height:50px;padding:0px;">
<img alt="" src="images/sending-gif.gif"  />
</div>
</div>
<div id="TB_overlay1" class="TB_overlayBG" style="visibility:hidden;"></div>




<%--</div>--%>








<%--<div id="mailpro"  style="position:fixed;left:50%;visibility:hidden;z-index:-1;">
  <div  style="position:relative;left:-50%;" >
   <img alt="" src="images/sending-gif.gif"  />             
    </div>
    <div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
</div>
</div>--%>



<%--<div id="mailpro" style="visibility:hidden;background-color: White;z-index:-1;">
<div id="backgroundElement_mailpro" class="modalBackground" style="position: fixed; left: 0px; top: 0px; z-index: 10000; width: 100%; height: 100%;">
<div id="ModalPanel_mailpro" class="PopUpDisplayStyleemailpro"  align="center" style="position: fixed; z-index: 10001;top:45%;left:35%;">  
<img alt="" src="images/sending-gif.gif"  />
</div>
</div>
</div>--%>