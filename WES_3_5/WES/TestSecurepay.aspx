<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSecurepay.aspx.cs" Inherits="WES.TestSecurepay" %>

<!DOCTYPE html>

<html>
  <body>
    <form onsubmit="return false;">
      <div id="securepay-ui-container"></div>
      <button onclick="mySecurePayUI.tokenise();">Submit</button>
      <button onclick="mySecurePayUI.reset();">Reset</button>
    </form>
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script id="securepay-ui-js" src="https://payments-stest.npe.auspost.zone/v3/ui/client/securepay-ui.min.js"></script>
    <script type="text/javascript">
        var mySecurePayUI = new securePayUI.init({
            containerId: 'securepay-ui-container',
            scriptId: 'securepay-ui-js',
            clientId: 'ABC0001',
            merchantCode: 'ABC0001',
            card: {
                allowedCardTypes: ['visa', 'mastercard'],
                showCardIcons: true,
                onCardTypeChange: function (cardType) {
                    // card type has changed
                    console.log(cardType);
                },
                onBINChange: function (cardBIN) {
                    console.log(cardBIN);
                    // card BIN has changed
                },
                
                onFormValidityChange: function (valid) {
                    // form validity has changed
                    console.log(this.valueOf('#number'))
                    alert(valid);
                    alert(valid.cardNumber);
                },
                onTokeniseSuccess: function (tokenisedCard) {
                    console.log(tokenisedCard);
                    // card was successfully tokenised or saved card was successfully retrieved 
                },
                onTokeniseError: function (tokenisedCard) {
                    console.log(tokenisedCard)
                  //  var cardNumber = $('#number').val();
                    console.log(this.valueOf('#number'))
                    var cardNumber = $('#number').val();
                    alert(cardNumber);
                    //console.log(errors);
                    // tokenization failed
                },
                onError: function (error) {
                    console.log("load error");
                    console.log(error);
                    // the UI Component has successfully loaded and is ready to be interacted with
                }
            },
          
            onLoadComplete: function () {
                console.log("load completed");
                // the UI Component has successfully loaded and is ready to be interacted with
            },
            onError: function (error) {
                console.log("load error");
                console.log(error);
                // the UI Component has successfully loaded and is ready to be interacted with
            }
        });
    </script>
  </body>
</html>