
'use strict';

//const applicationServerPublicKey = 'BNCjnFqqm-uAQD-B7DtiJrjED5HvJOhf6ByGVQ-KEQMUlf-1BmOD2kqH2wIcH5MfK9ccg2Otv5KvcipeRliFiCQ';
const applicationServerPublicKey=window.base64UrlToUint8Array(
        'BDd3_hVL9fZi9Ybo2UUzA284WG5FZR30_95YeZJsiA' +
        'pwXKpNcF1rRPF3foIiBHXRdJI2Qhumhf6_LFTeZaNndIo');
//const pushButton = document.querySelector('.btn-primary');
const subscriptionDetails =  document.querySelector('.notify_box');
let isSubscribed = false;
let swRegistration = null;

function base64UrlToUint8Array(base64UrlData) {
      const padding = '='.repeat((4 - base64UrlData.length % 4) % 4);
      const base64 = (base64UrlData + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');
      const rawData = atob(base64);
      const buffer = new Uint8Array(rawData.length);
    for (let i = 0; i < rawData.length; ++i) {
        buffer[i] = rawData.charCodeAt(i);
    }
    return buffer;
}
function urlB64ToUint8Array(base64String) {
  const padding = '='.repeat((4 - base64String.length % 4) % 4);
  const base64 = (base64String + padding)
    .replace(/\-/g, '+')
    .replace(/_/g, '/');

  const rawData = window.atob(base64);
  const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
      outputArray[i] = rawData.charCodeAt(i);
    }
return outputArray;
}

//function updateBtn() {
//    //if (Notification.permission === 'denied') {
//    //    pushButton.textContent = 'Push Messaging Blocked.';
//    //    pushButton.disabled = true;
//    //    updateSubscriptionOnServer(null);
//    //    return;
//    //}

//    //if (isSubscribed) {
//    //    pushButton.textContent = 'Disable Push Messaging';
//    //} else {
//    //    pushButton.textContent = 'Enable Push Messaging';
//    //}

//    //pushButton.disabled = false;
//}

function updateSubscriptionOnServer(subscription) {
    // TODO: Send subscription to application server

  //const subscriptionJson = document.querySelector('.js-subscription-json');
  //const subscriptionDetails =
  //  document.querySelector('.js-subscription-details');

    if (subscription) {
        subscriptionDetails.classList.add('is-invisible');
        //var epointval=subscription.endpoint;
        //$.ajax({
        //    type: "POST",
        //    url: "/GblWebMethods.aspx/endpoint",
        //    data: '{"endpoint":"' + epointval + '"}',
        //    contentType: "application/json;charset=utf-8",
        //    dataType: "json",
        //    success: function (data) {
        //        if (data.d != "") {
        //            window.location.href = "/" + data.d + "/ps/";
        //        }
        //    },
        //    error: function (xhr, status, error) {
        //        var err = eval("(" + xhr.responseText + ")");
        //        alert(err.Message);
        //    }



            //});
            //subscriptionJson.textContent = JSON.stringify(subscription);
            //subscriptionDetails.classList.remove('is-invisible');
        //});
        }
    else {
      
    }
}

function subscribeUser() {
   
  const applicationServerKey = applicationServerPublicKey;
  
    
    swRegistration.pushManager.subscribe({
        userVisibleOnly: true,
        
        applicationServerKey: applicationServerKey
    })
    .then(function(subscription) {
   
     

        var markers =subscription;

        $.ajax({
            type: "POST",
            url: "/GblWebMethods.aspx/endpoint",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ Markers: markers }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data){
                console.log("Inserted Succesfully");
            },
            failure: function(errMsg) {
                console.log(errMsg);
            }
        });

        console.log('User is subscribed:', subscription);

        updateSubscriptionOnServer(subscription);

        isSubscribed = true;

        //updateBtn();
    })
    .catch(function(err) {
       
        console.log('Failed to subscribe the user: ', err);
        //updateBtn();
    });
}

function initialiseUI() {
    //pushButton.addEventListener('click', function() {
    //    pushButton.disabled = true;
    //    if (isSubscribed) {
    //        // TODO: Unsubscribe user
    //    } else {
    //        subscribeUser();
    //    }
    //});

    // Set the initial subscription value
   
}
function checksubscription()
    {
if ('serviceWorker' in navigator && 'PushManager' in window) {
    console.log('Service Worker and Push is supported');

    navigator.serviceWorker.register('service-worker-wes.js')
    .then(function(swReg) {
        console.log('Service Worker is registered', swReg);

        swRegistration = swReg;
        swRegistration.pushManager.getSubscription()
  .then(function(subscription) {
      isSubscribed = !(subscription === null);
 //HideSubscribe();
    //  subscriptionDetails.classList.remove('is-invisible');
      //  updateSubscriptionOnServer(subscription);

     // alert(isSubscribed);
      if (isSubscribed) {

         console.log('User IS subscribed.');
         var x = document.getElementById('notification_wagner');
        x.style.display = 'none';
        
      } else {  
          console.log('User is NOT subscribed.');
          var x = document.getElementById('notification_wagner');
          x.style.display = 'block';
        
      }

      // updateBtn();
  });
       
       // initialiseUI();
    })
    .catch(function(error) {
        console.error('Service Worker Error', error);
    });

   

} else {
    console.warn('Push messaging is not supported');
    pushButton.textContent = 'Push Not Supported';
}
}
//$(window).load(function () {
//   // navigator.serviceWorker.register('service-worker.js')
//   //.then(function(swReg) {
//   //    console.log('Service Worker is registered', swReg);

//   //    swRegistration = swReg;
      
//   //})


  
//    alert("inside load");
//    if (isSubscribed) {
//        // TODO: Unsubscribe user
//    } else {
//        subscribeUser();
//    }

   

//       updateBtn();
//   });
    // Run code
function CallSubscribe() {
    subscribeUser();
    HideSubscribe();
}

function HideSubscribe() {
   // subscriptionDetails= document.querySelector('.notify_box');
  //  subscriptionDetails.classList.add('is-invisible');
   // alert(  $('#notification_wagner').attr('id'));
    var x = document.getElementById('notification_wagner');
    if (x.style.display === 'none') {
        x.style.display = 'block';
    } else {
        x.style.display = 'none';
    }
}