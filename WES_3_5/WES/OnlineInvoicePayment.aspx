<%@ Page Title="" Language="C#" MasterPageFile="~/FamilyMaster.master" AutoEventWireup="true"  CodeBehind="OnlineInvoicePayment.aspx.cs" Inherits="WES.OnlineInvoicePayment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
        <style>

          .paid_msg {
    font-size: 20px;
    color: #008f51;
    font-weight: bold;
}
          .paid_msg span {
	display:inline-block;
	margin-right:20px;
	vertical-align:middle;
}
.paid_msg span img {
	width:48px;
	height:48px;
}


.onlineInv_payment-wrap {
	width:970px;
	display:block;
	position:relating;
	padding:25px;
	font-family:Arial, Helvetica, sans-serif;
}
.onlineInv_payment-wrap .payment_title {
	font-family:arial;
	text-align:left;
	font-size:22px;
	color:#707070;
	margin-top:25px;
	margin-bottom:15px;
}
.onlineInv_payment-wrap .payment_caption {
	font-family:arial;
	text-align:left;
	font-size:16px;
	color:#707070;
	margin-bottom:20px;
	margin-top:15px;
}
.title_wrap {
	display:block;
	margin-bottom:20px;
}
.onlineInv_lft {
	width:265px;
	padding:20px;
	height:375px;
	float:left;
	color:#707070;
	font-size:16px;
	text-align:left;
	border-radius:6px;
	margin-right:20px;
	border:1px solid #c7c7c7;
	font-family:Arial, Helvetica, sans-serif;
}
.onlineInv_lft b {
	margin:10px 0 20px;
	display:block;
}
.onlineInv_detail {
	display:block;
	height:100px;
	font-size:16px;
	color:#707070;
	
}
.onlineInv_detail li {
	list-style:none;
	margin-bottom:12px;
}
.onlineInv_detail li span:first-child {
	width:86px;
	display:inline-block;
}
.onlineInv_detail li span:first-child {
	margin-right:15px;
}
.onlineInv_lft .viewInvoice {
	display:block;
	/*border-bottom:1px solid #c7c7c7;*/
	margin-left:-20px;
	margin-right:-20px;
}
.onlineInv_lft a.viewInv_btn {
	text-decoration:none;
	font-size:16px;
	margin-bottom:20px;
	width:100%;
	display:block;
	padding: 0 15px;
	color:#707070;
}
.onlineInv_lft a.viewInv_btn span {
	display: inline-block;
    vertical-align: top;
    line-height: 34px;
}
.onlineInv_lft a.viewInvoice img {
	margin-right:15px;
	display:inline-block;
	width:34px;
	height:33px;
}
.invTotal {
	font-size:20px;
	color:#1d9905;
	margin:5px 0;
    text-align:right;
    width:568px;
    display:inline-block;
}
.onlineInv_rgt {
	width:580px;
	height:420px;
	padding:0px;
	float:left;
	font-size:16px;
	text-align:left;
	margin-left:20px;
	/*border-radius:6px;*/
	/*border:1px solid #c7c7c7;*/
	font-family:Arial, Helvetica, sans-serif;
    position: relative;
}

.clear { clear:both; }
.braintree-sheet__content--form {
    padding: 10px 15px 10px 10px;
    width: 60% !important;
    height:365px;
}
.braintree-placeholder {
    margin-bottom: 13px;
    display: none !important;
}
.braintree-show-card .braintree-card {
position:relative !important;
}
            .braintree-form braintree-sheet {
                height:420px !important;
            }
input.paynow_br {
    width: 150px;
    height: 48px;
    text-align: right;
    display: inline-block;
    /*background: #48B335;*/
    background: rgb(55,162,14);
    background: linear-gradient(180deg, rgba(111,183,34,1) 0%, rgba(26,152,4,1) 100%);
    text-transform: initial;
    color: #fff;
    font-size: 18px;
    font-weight: bold;
    text-align: center;
    border-radius: 6px;
    cursor: pointer;
    padding-bottom: 2px;
    position: absolute;
 right: 30px;
bottom: 27px;
    z-index: 99;
}

</style>
<div class="onlineInv_payment-wrap">
	<div class="title_wrap">
		<h2 class="payment_title">Online Invoice Payment</h2>
        <div id="divcaption" runat="server">
		<span class="payment_caption"> Your order is awaiting payment. Please pay below </span>
            <b class="invTotal">Invoice Total: $<asp:Label ID="lblinvoicetotal" runat="server" 

Text=""></asp:Label></b>
            </div>
        	
    
       
	</div>
	<div class="onlineInv_lft" runat="server" id="divleft"  >
		<b>Invoice Details</b>
		<div class="onlineInv_detail">
			<li><span>Amount : </span>$  <span> <asp:Label ID="lblamount" 

runat="server" Text=""></asp:Label>  </span> </li>
			<li><span>Invoice No :</span> <span><asp:Label ID="lblinvoice" 

runat="server" Text=""></asp:Label></span></li>
			<li><span>Order No : </span>  <span><asp:Label ID="lblorder" runat="server" 

Text=""></asp:Label></span> </li>
		</div>
		<div class="viewInvoice">
			<a  onClick="openTab(this)" href="#" class="viewInv_btn"  name="/Invoices/In123763.pdf"> <img src="../images/pdf_icon.jpg"> 

</img><span> View Order <span></a>
		</div>
	
	</div>
	
	<div class="onlineInv_rgt" >
      <div class="paid_msg" runat="server"  id="divpaidmsg">
			<span><img id="imgerr" visible="false" runat="server" src="../images/check-mark-green-tick-mark.png"></span>
		 <asp:Label ID="lblerror" runat="server" Text="Label" style=""> 


		 </asp:Label>
		</div>
     <div id="dropin-container" >

        
                 
      </div> 
               <div id="divsucess" style="display:none;text-align:center" class="alert greenbox icon_2" >
        Transaction Approved! <br/> Your Order will now be processed, Thanks for shopping at Wes Online! 

<br />
         Payment Method: Credit Card
                               <div id="divrefno" class="accordion_head_green clear"></div>
     </div>
     <div id="divFailed" style="display:none;text-align:center;background: #FFF200;font-size:12px" 

class="alert yellowbox icon_2">
       Transaction failed!
           <div id="diverrorno"> Please Try again or use a different card.</div>
     </div>

         <div  runat="server" id="loading"  align="center" >
         
        <input disabled id="pay-btn" class="paynow_br"  onclick="return false;" type="submit" 

value="Loading...">
           
      </div>
           
   
   <%--   <div class="input-group nonce-group hidden">
        <span class="input-group-addon">nonce</span>
        <input readonly name="nonce" class="form-control">
      </div>--%>
     
    </div>
  




	
	<div class="clear"></div>
	
</div>
    <script>
         function openTab(th)
            {
                window.open(th.name,'_blank');
            }

        function hide() {
        
            var divOk = document.getElementById('divFailed');
            divOk.style.display = "none";
            document.getElementById("pay-btn").style.display = "inline-block";
            enablePayNow();
            dropin.clearSelectedPaymentMethod();       
           
            var x = document.getElementsByClassName("braintree-large-button");
           
            x[0].outerHTML = '<div  data-braintree-id="toggle" class="braintree-large-button braintree-toggle braintree-hidden" tabindex="0" style="background:white;border:none" onclick="hide()"><span onclick="hide()">Choose another way to pay</span></div>';
                   
        }
    </script>





	  <script src="https://js.braintreegateway.com/web/dropin/1.22.0/js/dropin.min.js"></script>
       <script src="https://js.braintreegateway.com/web/3.47.0/js/three-d-secure.min.js"></script>
      <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" 

type="text/javascript"></script>

    

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.js"></script>


<script src="https://js.braintreegateway.com/web/3.59.0/js/client.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.59.0/js/data-collector.min.js"></script>


     <script>

         var totamt = document.getElementById('ctl00_maincontent_lblinvoicetotal');
       
                 //   alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '');
       //  alert(totamt.innerHTML.replace('<b>','').replace('</b>','').replace('$',''));
         var amt = parseFloat(amt1);
        // alert(amt);
        
         var payBtn = document.getElementById('pay-btn');
         var nonceGroup = document.querySelector('.nonce-group');
         var nonceInput = document.querySelector('.nonce-group input');
         var payGroup = document.querySelector('.pay-group');

         var dropin;
         var clienttoken = "<%= this.ClientToken %>";
         
         braintree.dropin.create({
             authorization: clienttoken,
             container: '#dropin-container',

             card: {
                 cardholderName: {
                      selector: '#cc-name',
                     required: false
                     

                     // to make cardholder name required
                     // required: true
                 }
             },
             fields: {
                 number: {
                     selector: '#card-number'
                 },
                 cvv: {
                     selector: '#cvv',
                  
                       value: '111'
                 },
                 expirationDate: {
                     selector: '#expiration-date'
                    
                 }
             },


             threeDSecure: {
                 amount: amt
             }
         }, function (err, instance) {
             if (err) {


                 console.log('component error:', err);
                 return;
             }
            
             dropin = instance;

             setupForm();

             

         });
         //braintree.dataCollector.create({
         //    client: clientInstance,
         //    kount: true
         //}, function (err, dataCollectorInstance) {
         //    if (err) {
         //      console.log(err);
         //        // Handle error in data collector creation
         //        return;
         //    }
         //    //var form = document.getElementById('form1');
         //    //var deviceDataInput = form['device_data'];

         //    //if (deviceDataInput == null) {
         //    //    deviceDataInput = document.createElement('input');
         //    //    deviceDataInput.name = 'device_data';
         //    //    deviceDataInput.type = 'hidden';
         //    //    form.appendChild(deviceDataInput);
         //    //}
                

         //  //  alert("datacollector");
         //  //  alert(dataCollectorInstance.deviceData);
         //    devicedata= dataCollectorInstance.deviceData;
         //   // document.getElementById(' device_data').value = dataCollectorInstance.deviceData;
                
         //})

         // });

    
         function setupForm() {
             //alert("inside setip form");
           
             document.getElementById('divFailed').style.display = "none";
             document.getElementById('diverrorno').style.display = "none";
             enablePayNow();

         }
       

         function enablePayNow() {
             payBtn.value = 'Pay Now';
             payBtn.removeAttribute('disabled');
             console.log("enable paynow");
             var name = "<%= this.NAME_ONCARD %>";
           //  var dr = "11/20";
             $('#braintree__card-view-input__cardholder-name').val(name);
           <%--  alert(dr);

             var x = $('#braintree__card-view-input__cardholder-name').val()
             alert(x);

//             var iframe = document.getElementById("braintree-hosted-field-number");
//var elmnt = iframe.contentWindow.document.getElementsByTagName("credit-card-number")[0];
//elmnt.style.display = "none";      
             <%--var cardnumber = "<%= this.CARD_NO %>";--%>
             //$('#braintree-form__hosted-field braintree-form-expiration').innerHTML = dr;
              // $('#bt-expirationDate-before').html(dr);
             //$('#braintree-hosted-field-expirationDate').val(dr);
             //alert($('#bt-expirationDate-before').val());
            // $('#bt-expirationDate-after').html(dr);
             //alert($('#bt-expirationDate-after').val());
             //alert($('#braintree-hosted-field-expirationDate').val());
            // $('#expiration').val("11/20");
            
             
         }

         function showNonce(payload) {
             nonceInput.value = payload.nonce;
             
             //document.getElementById("HiddenField1").value = payload.nonce;
             payGroup.classList.add('hidden');
             payGroup.style.display = 'none';
             nonceGroup.classList.remove('hidden');



         }
         function disableForm(){
             var form = document.getElementById("aspnetForm");
             var elements = form.elements;
             for (var i = 0, len = elements.length; i < len; ++i) {
                 elements[i].disabled = true;
             }
         }
         function enableFrm(){
             var form = document.getElementById("aspnetForm");
             var elements = form.elements;
             for (var i = 0, len = elements.length; i < len; ++i) {
                 elements[i].disabled = false;
             }
         }
         payBtn.addEventListener('click', function (event) {
             disableForm();
            
             payBtn.setAttribute('disabled', 'disabled');
             payBtn.value = 'Processing...';
             // var i = $("#braintree-hosted-field-number").contents().find("#credit-card-number");
             //alert(i);
             
            
            
             try{
                 var y = document.getElementsByClassName("braintree-heading");
                 y[0].innerHTML='';
                 y[0].outerHTML='';
             }
             catch(err)
             {}
             dropin.requestPaymentMethod(function (err, payload) {
                     
                 if (err) {
                     
                     console.log('tokenization error:', err);

                     enableFrm();
                     if(err=="DropinError: No payment method is available.")
                     {
                       
                         payBtn.removeAttribute("disabled");
                         payBtn.value = 'PayNow';
                        
                     }
                     else
                     {
                         //dropin.clearSelectedPaymentMethod();
                         //  enablePayNow();
                        SaleTrans('no');
                     document.getElementById('pay-btn').style.display = "none";  
                         //var x = document.getElementsByClassName("braintree-large-button");
                    
                         //x[0].innerHTML = '<span  onclick=hide()>Choose another way to pay</span>';
                       
                   
                 
                     }
                     //enablePayNow();
                     return;
                 } else {
                     console.log('initial tokenization success:', payload);
                 }

                 if (payload.type !== 'CreditCard') {
                    
                     // if not a credit card, skip 3ds and send nonce to server
                     return;
                 }
                 // console.log(payload.status.threeDSecure);
                   
                 if (!payload.liabilityShifted) {

                     enableFrm();
                     // dropin.clearSelectedPaymentMethod();
                     console.log('Liability did not shift', payload);

                        
                     //enablePayNow();
                       
                    
            
                  SaleTrans(payload.nonce);
                   
                     enableFrm();
                 }
                 else {
                     
             SaleTrans(payload.nonce);
                     //var x =    SaleTrans(payload.nonce);
                     //if (x == 1) {
                       
                     //    payBtn.value = 'Card Verification Successful..Please Wait..';

                     //    //  $("#ctl00_maincontent_ImagePay").attr("disabled", "disabled");

                        

                         var x = document.getElementsByClassName("braintree-large-button");
                         x[0].innerHTML = '';
                         x[0].outerHTML = '';
                     //    enableFrm();
                     //}
                 }

                 console.log('verification success:', payload);


                   
                  
                 // send nonce and verification data to your server
             });
         });

       



         </script>



    <script type="text/javascript" language="javascript">


            function SaleTrans(a) {
                try { 
                    var totamt = document.getElementById('ctl00_maincontent_lblinvoicetotal');
                   // alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '');
       //  alert(totamt.innerHTML.replace('<b>','').replace('</b>','').replace('$',''));
         var amt = parseFloat(amt1);

                $.ajax({
                    type: "POST",
                    url: "OnlineInvoicePayment.aspx/SaleTrans",
                    data: "{'nounce':'" + a + "','Amount':'"+ amt +"'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        
                       if (data.d.includes("Sucess") == true) {
                         
                     var divOk = document.getElementById('divsucess');
                           divOk.style.display = "block";
                            document.getElementById('ctl00_maincontent_divcaption').style.display = "none";
                 
                     document.getElementById('divFailed').style.display = "none";
                           document.getElementById('diverrorno').style.display = "none";
                 document.getElementById('pay-btn').style.display = "none";  
                 }
                 

                 else {

                     // enablePayNow();
                   
                     var divOk = document.getElementById('divFailed');
                     document.getElementById('diverrorno').style.display = "block";
                     // document.getElementById('diverrorno').innerHTML = data.d
                     divOk.style.display = "block";
                     document.getElementById('divsucess').style.display = "none";

                   
                     // dropin.clearSelectedPaymentMethod();
                     document.getElementById('dropin-container').style.display = "block";
                        
                          document.getElementById('pay-btn').style.display = "none";  
                     var x = document.getElementsByClassName("braintree-large-button");
                     x[0].outerHTML = '<div data-braintree-id="toggle" class="braintree-large-button braintree-toggle" tabindex="0" style="background:white;border:none" onclick="hide()"><span onclick="hide()">Choose another way to pay</span></div>';
                   
                     
                 
                   
                   
                 }
                
             },
             error: function (xhr, status, error) {

                 var err = eval("(" + xhr.responseText + ")");
               alert(err);
             }
         })
            }
            catch (err) {
//alert(err)
            }
      }
         




</script>     
</asp:Content>