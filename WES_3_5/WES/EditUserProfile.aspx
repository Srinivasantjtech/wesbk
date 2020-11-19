<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="EditUserProfile" Title="Untitled Page"
    Culture="en-US" UICulture="en-US" CodeBehind="EditUserProfile.aspx.cs" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">

    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&signed_in=true&libraries=places"></script>

    <style>
        .p20 {
            padding: 20px;
        }

        .mt10 {
            margin-top: 10px!important;
        }

        .mt20 {
            margin-top: 20px!important;
        }

        .mt30 {
            margin-top: 30px!important;
        }

        .mb10 {
            margin-bottom: 10px!important;
        }

        .mb20 {
            margin-bottom: 20px!important;
        }

        .mb30 {
            margin-bottom: 30px!important;
        }


        .form-group {
            margin-bottom: 15px;
        }

        #editaddresspopup .modal-box {
            border: 6px solid #e6e6e5;
        }

        .form-control {
            display: block;
            width: 100%;
            /*height: 34px;*/
            /*width: 96%;*/
            height: 20px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555555;
            background-color: #ffffff;
            background-image: none;
            border: 1px solid #cccccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        select.form-control {
            height: 32px;
        }

        #editaddresspopup .pophead {
            font-size: 13px;
            font-weight: normal;
        }

        #editaddresspopup label {
            font-size: 13px;
            font-weight: bold;
            text-align: left;
            display: block;
            margin-bottom: 3px;
        }

        #editaddresspopup .form-control {
            padding: 6px 2px;
            margin-left: 0;
        }

        .pop_btngreen {
            font-family: Arial, Helvetica, sans-serif;
            background: #39b54a;
            color: #fff;
            width: 25%;
            display: inline-block;
            text-align: center;
            padding: 10px 0 10px;
            font-size: 15px;
            margin: 8px 0 10px;
            text-decoration: none;
            opacity: 1;
            font-weight: 600;
        }

        .pop_btnwhite {
            font-family: Arial, Helvetica, sans-serif;
            background: #e6e6e5;
            color: #333;
            width: 25%;
            display: inline-block;
            text-align: center;
            padding: 10px 0 10px;
            font-size: 15px;
            margin: 8px 0 10px;
            text-decoration: none;
            opacity: 1;
            font-weight: 600;
        }
    </style>

    <script type="text/javascript">
        function ShowModal() {
            $("#editaddresspopup").show();

            var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
            //  e.preventDefault();
            $("body").append(appendthis);
            $(".modal-overlay").fadeTo(500, 0.7);
            // $(".modal-box-change").css({ "display": "block" });

            //$(".js-modalbox").fadeIn(500);
            var modalBox = $(this).attr('data-modal-id');
            //$('#' + modalBox).fadeIn($(this).data());
            $("[id=editaddresspopup]").show();
          //  document.getElementById("ctl00_maincontent_drpupdatecountry").value = "AU";
            //console.log(document.getElementById("ctl00_maincontent_drpupdatecountry").value);
            return true;
        }

        function AdminUser() {
             alert("Please contact the Admin user for your account to update details");
            //location.href = '/ConfirmMessage.aspx?Result=ADMINADDRUPDATEREQUESTSENT';
             //ShowModal();
        }

        function HideModel() {
            var modalBox = $(this).attr('data-modal-id');
            console.log(modalBox);
            // $('#' + modalBox).fadeIn($(this).data());
            $("[id=editaddresspopup]").hide();

            $(".modal-box, .modal-overlay").fadeOut(500, function () {
                $(".modal-overlay").remove();
            });

        }

        function drpcountry_change() {
            document.getElementById('ctl00_maincontent_txtupdateadd').value = '';
            document.getElementById('ctl00_maincontent_txtupdateadd2').value = '';
            document.getElementById('ctl00_maincontent_txtupdatetown').value = '';
            document.getElementById('ctl00_maincontent_txtupdatestate').value = '';
            document.getElementById('ctl00_maincontent_drpupdatestate').value = '-1';
            document.getElementById('ctl00_maincontent_txtupdatezip').value = '';

            console.log(document.getElementById("ctl00_maincontent_drpupdatecountry").value);

            shippingautocomplete.setComponentRestrictions({ 'country': document.getElementById("ctl00_maincontent_drpupdatecountry").value });
            if (document.getElementById("ctl00_maincontent_drpupdatecountry").value == "AU") {
                document.getElementById('ctl00_maincontent_drpupdatestate').style.display = 'block';
                document.getElementById('ctl00_maincontent_txtupdatestate').style.display = 'none';
                // document.getElementById('ctl00_maincontent_rfvstate').style.display = 'none';
                // ValidatorEnable(document.getElementById('<%= rfvstate.ClientID %>'), false);
            } else {
                document.getElementById('ctl00_maincontent_drpupdatestate').style.display = 'none';
                document.getElementById('ctl00_maincontent_txtupdatestate').style.display = 'block';
                //document.getElementById('ctl00_maincontent_rfvstate').style.display = 'block';
                //ValidatorEnable(document.getElementById('<%= rfvstate.ClientID %>'), true);
            }
        }

    </script>



    <script type="text/javascript">

    $(document).ready(function() {
      $(window).keydown(function(event){
        if(event.keyCode == 13) {
          event.preventDefault();
          return false;
        }
      });
    });

        google.maps.event.addDomListener(window, 'load', initAutocomplete);

        var base64map = "eyLDgSI6IkEiLCLEgiI6IkEiLCLhuq4iOiJBIiwi4bq2IjoiQSIsIuG6sCI6IkEiLCLhurIiOiJBIiwi4bq0IjoiQSIsIseNIjoiQSIsIsOCIjoiQSIsIuG6pCI6IkEiLCLhuqwiOiJBIiwi4bqmIjoiQSIsIuG6qCI6IkEiLCLhuqoiOiJBIiwiw4QiOiJBIiwix54iOiJBIiwiyKYiOiJBIiwix6AiOiJBIiwi4bqgIjoiQSIsIsiAIjoiQSIsIsOAIjoiQSIsIuG6oiI6IkEiLCLIgiI6IkEiLCLEgCI6IkEiLCLEhCI6IkEiLCLDhSI6IkEiLCLHuiI6IkEiLCLhuIAiOiJBIiwiyLoiOiJBIiwiw4MiOiJBIiwi6pyyIjoiQUEiLCLDhiI6IkFFIiwix7wiOiJBRSIsIseiIjoiQUUiLCLqnLQiOiJBTyIsIuqctiI6IkFVIiwi6py4IjoiQVYiLCLqnLoiOiJBViIsIuqcvCI6IkFZIiwi4biCIjoiQiIsIuG4hCI6IkIiLCLGgSI6IkIiLCLhuIYiOiJCIiwiyYMiOiJCIiwixoIiOiJCIiwixIYiOiJDIiwixIwiOiJDIiwiw4ciOiJDIiwi4biIIjoiQyIsIsSIIjoiQyIsIsSKIjoiQyIsIsaHIjoiQyIsIsi7IjoiQyIsIsSOIjoiRCIsIuG4kCI6IkQiLCLhuJIiOiJEIiwi4biKIjoiRCIsIuG4jCI6IkQiLCLGiiI6IkQiLCLhuI4iOiJEIiwix7IiOiJEIiwix4UiOiJEIiwixJAiOiJEIiwixosiOiJEIiwix7EiOiJEWiIsIseEIjoiRFoiLCLDiSI6IkUiLCLElCI6IkUiLCLEmiI6IkUiLCLIqCI6IkUiLCLhuJwiOiJFIiwiw4oiOiJFIiwi4bq+IjoiRSIsIuG7hiI6IkUiLCLhu4AiOiJFIiwi4buCIjoiRSIsIuG7hCI6IkUiLCLhuJgiOiJFIiwiw4siOiJFIiwixJYiOiJFIiwi4bq4IjoiRSIsIsiEIjoiRSIsIsOIIjoiRSIsIuG6uiI6IkUiLCLIhiI6IkUiLCLEkiI6IkUiLCLhuJYiOiJFIiwi4biUIjoiRSIsIsSYIjoiRSIsIsmGIjoiRSIsIuG6vCI6IkUiLCLhuJoiOiJFIiwi6p2qIjoiRVQiLCLhuJ4iOiJGIiwixpEiOiJGIiwix7QiOiJHIiwixJ4iOiJHIiwix6YiOiJHIiwixKIiOiJHIiwixJwiOiJHIiwixKAiOiJHIiwixpMiOiJHIiwi4bigIjoiRyIsIsekIjoiRyIsIuG4qiI6IkgiLCLIniI6IkgiLCLhuKgiOiJIIiwixKQiOiJIIiwi4rGnIjoiSCIsIuG4piI6IkgiLCLhuKIiOiJIIiwi4bikIjoiSCIsIsSmIjoiSCIsIsONIjoiSSIsIsSsIjoiSSIsIsePIjoiSSIsIsOOIjoiSSIsIsOPIjoiSSIsIuG4riI6IkkiLCLEsCI6IkkiLCLhu4oiOiJJIiwiyIgiOiJJIiwiw4wiOiJJIiwi4buIIjoiSSIsIsiKIjoiSSIsIsSqIjoiSSIsIsSuIjoiSSIsIsaXIjoiSSIsIsSoIjoiSSIsIuG4rCI6IkkiLCLqnbkiOiJEIiwi6p27IjoiRiIsIuqdvSI6IkciLCLqnoIiOiJSIiwi6p6EIjoiUyIsIuqehiI6IlQiLCLqnawiOiJJUyIsIsS0IjoiSiIsIsmIIjoiSiIsIuG4sCI6IksiLCLHqCI6IksiLCLEtiI6IksiLCLisakiOiJLIiwi6p2CIjoiSyIsIuG4siI6IksiLCLGmCI6IksiLCLhuLQiOiJLIiwi6p2AIjoiSyIsIuqdhCI6IksiLCLEuSI6IkwiLCLIvSI6IkwiLCLEvSI6IkwiLCLEuyI6IkwiLCLhuLwiOiJMIiwi4bi2IjoiTCIsIuG4uCI6IkwiLCLisaAiOiJMIiwi6p2IIjoiTCIsIuG4uiI6IkwiLCLEvyI6IkwiLCLisaIiOiJMIiwix4giOiJMIiwixYEiOiJMIiwix4ciOiJMSiIsIuG4viI6Ik0iLCLhuYAiOiJNIiwi4bmCIjoiTSIsIuKxriI6Ik0iLCLFgyI6Ik4iLCLFhyI6Ik4iLCLFhSI6Ik4iLCLhuYoiOiJOIiwi4bmEIjoiTiIsIuG5hiI6Ik4iLCLHuCI6Ik4iLCLGnSI6Ik4iLCLhuYgiOiJOIiwiyKAiOiJOIiwix4siOiJOIiwiw5EiOiJOIiwix4oiOiJOSiIsIsOTIjoiTyIsIsWOIjoiTyIsIseRIjoiTyIsIsOUIjoiTyIsIuG7kCI6Ik8iLCLhu5giOiJPIiwi4buSIjoiTyIsIuG7lCI6Ik8iLCLhu5YiOiJPIiwiw5YiOiJPIiwiyKoiOiJPIiwiyK4iOiJPIiwiyLAiOiJPIiwi4buMIjoiTyIsIsWQIjoiTyIsIsiMIjoiTyIsIsOSIjoiTyIsIuG7jiI6Ik8iLCLGoCI6Ik8iLCLhu5oiOiJPIiwi4buiIjoiTyIsIuG7nCI6Ik8iLCLhu54iOiJPIiwi4bugIjoiTyIsIsiOIjoiTyIsIuqdiiI6Ik8iLCLqnYwiOiJPIiwixYwiOiJPIiwi4bmSIjoiTyIsIuG5kCI6Ik8iLCLGnyI6Ik8iLCLHqiI6Ik8iLCLHrCI6Ik8iLCLDmCI6Ik8iLCLHviI6Ik8iLCLDlSI6Ik8iLCLhuYwiOiJPIiwi4bmOIjoiTyIsIsisIjoiTyIsIsaiIjoiT0kiLCLqnY4iOiJPTyIsIsaQIjoiRSIsIsaGIjoiTyIsIsiiIjoiT1UiLCLhuZQiOiJQIiwi4bmWIjoiUCIsIuqdkiI6IlAiLCLGpCI6IlAiLCLqnZQiOiJQIiwi4rGjIjoiUCIsIuqdkCI6IlAiLCLqnZgiOiJRIiwi6p2WIjoiUSIsIsWUIjoiUiIsIsWYIjoiUiIsIsWWIjoiUiIsIuG5mCI6IlIiLCLhuZoiOiJSIiwi4bmcIjoiUiIsIsiQIjoiUiIsIsiSIjoiUiIsIuG5niI6IlIiLCLJjCI6IlIiLCLisaQiOiJSIiwi6py+IjoiQyIsIsaOIjoiRSIsIsWaIjoiUyIsIuG5pCI6IlMiLCLFoCI6IlMiLCLhuaYiOiJTIiwixZ4iOiJTIiwixZwiOiJTIiwiyJgiOiJTIiwi4bmgIjoiUyIsIuG5oiI6IlMiLCLhuagiOiJTIiwixaQiOiJUIiwixaIiOiJUIiwi4bmwIjoiVCIsIsiaIjoiVCIsIsi+IjoiVCIsIuG5qiI6IlQiLCLhuawiOiJUIiwixqwiOiJUIiwi4bmuIjoiVCIsIsauIjoiVCIsIsWmIjoiVCIsIuKxryI6IkEiLCLqnoAiOiJMIiwixpwiOiJNIiwiyYUiOiJWIiwi6pyoIjoiVFoiLCLDmiI6IlUiLCLFrCI6IlUiLCLHkyI6IlUiLCLDmyI6IlUiLCLhubYiOiJVIiwiw5wiOiJVIiwix5ciOiJVIiwix5kiOiJVIiwix5siOiJVIiwix5UiOiJVIiwi4bmyIjoiVSIsIuG7pCI6IlUiLCLFsCI6IlUiLCLIlCI6IlUiLCLDmSI6IlUiLCLhu6YiOiJVIiwixq8iOiJVIiwi4buoIjoiVSIsIuG7sCI6IlUiLCLhu6oiOiJVIiwi4busIjoiVSIsIuG7riI6IlUiLCLIliI6IlUiLCLFqiI6IlUiLCLhuboiOiJVIiwixbIiOiJVIiwixa4iOiJVIiwixagiOiJVIiwi4bm4IjoiVSIsIuG5tCI6IlUiLCLqnZ4iOiJWIiwi4bm+IjoiViIsIsayIjoiViIsIuG5vCI6IlYiLCLqnaAiOiJWWSIsIuG6giI6IlciLCLFtCI6IlciLCLhuoQiOiJXIiwi4bqGIjoiVyIsIuG6iCI6IlciLCLhuoAiOiJXIiwi4rGyIjoiVyIsIuG6jCI6IlgiLCLhuooiOiJYIiwiw50iOiJZIiwixbYiOiJZIiwixbgiOiJZIiwi4bqOIjoiWSIsIuG7tCI6IlkiLCLhu7IiOiJZIiwixrMiOiJZIiwi4bu2IjoiWSIsIuG7viI6IlkiLCLIsiI6IlkiLCLJjiI6IlkiLCLhu7giOiJZIiwixbkiOiJaIiwixb0iOiJaIiwi4bqQIjoiWiIsIuKxqyI6IloiLCLFuyI6IloiLCLhupIiOiJaIiwiyKQiOiJaIiwi4bqUIjoiWiIsIsa1IjoiWiIsIsSyIjoiSUoiLCLFkiI6Ik9FIiwi4bSAIjoiQSIsIuG0gSI6IkFFIiwiypkiOiJCIiwi4bSDIjoiQiIsIuG0hCI6IkMiLCLhtIUiOiJEIiwi4bSHIjoiRSIsIuqcsCI6IkYiLCLJoiI6IkciLCLKmyI6IkciLCLKnCI6IkgiLCLJqiI6IkkiLCLKgSI6IlIiLCLhtIoiOiJKIiwi4bSLIjoiSyIsIsqfIjoiTCIsIuG0jCI6IkwiLCLhtI0iOiJNIiwiybQiOiJOIiwi4bSPIjoiTyIsIsm2IjoiT0UiLCLhtJAiOiJPIiwi4bSVIjoiT1UiLCLhtJgiOiJQIiwiyoAiOiJSIiwi4bSOIjoiTiIsIuG0mSI6IlIiLCLqnLEiOiJTIiwi4bSbIjoiVCIsIuKxuyI6IkUiLCLhtJoiOiJSIiwi4bScIjoiVSIsIuG0oCI6IlYiLCLhtKEiOiJXIiwiyo8iOiJZIiwi4bSiIjoiWiIsIsOhIjoiYSIsIsSDIjoiYSIsIuG6ryI6ImEiLCLhurciOiJhIiwi4bqxIjoiYSIsIuG6syI6ImEiLCLhurUiOiJhIiwix44iOiJhIiwiw6IiOiJhIiwi4bqlIjoiYSIsIuG6rSI6ImEiLCLhuqciOiJhIiwi4bqpIjoiYSIsIuG6qyI6ImEiLCLDpCI6ImEiLCLHnyI6ImEiLCLIpyI6ImEiLCLHoSI6ImEiLCLhuqEiOiJhIiwiyIEiOiJhIiwiw6AiOiJhIiwi4bqjIjoiYSIsIsiDIjoiYSIsIsSBIjoiYSIsIsSFIjoiYSIsIuG2jyI6ImEiLCLhupoiOiJhIiwiw6UiOiJhIiwix7siOiJhIiwi4biBIjoiYSIsIuKxpSI6ImEiLCLDoyI6ImEiLCLqnLMiOiJhYSIsIsOmIjoiYWUiLCLHvSI6ImFlIiwix6MiOiJhZSIsIuqctSI6ImFvIiwi6py3IjoiYXUiLCLqnLkiOiJhdiIsIuqcuyI6ImF2Iiwi6py9IjoiYXkiLCLhuIMiOiJiIiwi4biFIjoiYiIsIsmTIjoiYiIsIuG4hyI6ImIiLCLhtawiOiJiIiwi4baAIjoiYiIsIsaAIjoiYiIsIsaDIjoiYiIsIsm1IjoibyIsIsSHIjoiYyIsIsSNIjoiYyIsIsOnIjoiYyIsIuG4iSI6ImMiLCLEiSI6ImMiLCLJlSI6ImMiLCLEiyI6ImMiLCLGiCI6ImMiLCLIvCI6ImMiLCLEjyI6ImQiLCLhuJEiOiJkIiwi4biTIjoiZCIsIsihIjoiZCIsIuG4iyI6ImQiLCLhuI0iOiJkIiwiyZciOiJkIiwi4baRIjoiZCIsIuG4jyI6ImQiLCLhta0iOiJkIiwi4baBIjoiZCIsIsSRIjoiZCIsIsmWIjoiZCIsIsaMIjoiZCIsIsSxIjoiaSIsIsi3IjoiaiIsIsmfIjoiaiIsIsqEIjoiaiIsIsezIjoiZHoiLCLHhiI6ImR6Iiwiw6kiOiJlIiwixJUiOiJlIiwixJsiOiJlIiwiyKkiOiJlIiwi4bidIjoiZSIsIsOqIjoiZSIsIuG6vyI6ImUiLCLhu4ciOiJlIiwi4buBIjoiZSIsIuG7gyI6ImUiLCLhu4UiOiJlIiwi4biZIjoiZSIsIsOrIjoiZSIsIsSXIjoiZSIsIuG6uSI6ImUiLCLIhSI6ImUiLCLDqCI6ImUiLCLhursiOiJlIiwiyIciOiJlIiwixJMiOiJlIiwi4biXIjoiZSIsIuG4lSI6ImUiLCLisbgiOiJlIiwixJkiOiJlIiwi4baSIjoiZSIsIsmHIjoiZSIsIuG6vSI6ImUiLCLhuJsiOiJlIiwi6p2rIjoiZXQiLCLhuJ8iOiJmIiwixpIiOiJmIiwi4bWuIjoiZiIsIuG2giI6ImYiLCLHtSI6ImciLCLEnyI6ImciLCLHpyI6ImciLCLEoyI6ImciLCLEnSI6ImciLCLEoSI6ImciLCLJoCI6ImciLCLhuKEiOiJnIiwi4baDIjoiZyIsIselIjoiZyIsIuG4qyI6ImgiLCLInyI6ImgiLCLhuKkiOiJoIiwixKUiOiJoIiwi4rGoIjoiaCIsIuG4pyI6ImgiLCLhuKMiOiJoIiwi4bilIjoiaCIsIsmmIjoiaCIsIuG6liI6ImgiLCLEpyI6ImgiLCLGlSI6Imh2Iiwiw60iOiJpIiwixK0iOiJpIiwix5AiOiJpIiwiw64iOiJpIiwiw68iOiJpIiwi4bivIjoiaSIsIuG7iyI6ImkiLCLIiSI6ImkiLCLDrCI6ImkiLCLhu4kiOiJpIiwiyIsiOiJpIiwixKsiOiJpIiwixK8iOiJpIiwi4baWIjoiaSIsIsmoIjoiaSIsIsSpIjoiaSIsIuG4rSI6ImkiLCLqnboiOiJkIiwi6p28IjoiZiIsIuG1uSI6ImciLCLqnoMiOiJyIiwi6p6FIjoicyIsIuqehyI6InQiLCLqna0iOiJpcyIsIsewIjoiaiIsIsS1IjoiaiIsIsqdIjoiaiIsIsmJIjoiaiIsIuG4sSI6ImsiLCLHqSI6ImsiLCLEtyI6ImsiLCLisaoiOiJrIiwi6p2DIjoiayIsIuG4syI6ImsiLCLGmSI6ImsiLCLhuLUiOiJrIiwi4baEIjoiayIsIuqdgSI6ImsiLCLqnYUiOiJrIiwixLoiOiJsIiwixpoiOiJsIiwiyawiOiJsIiwixL4iOiJsIiwixLwiOiJsIiwi4bi9IjoibCIsIsi0IjoibCIsIuG4tyI6ImwiLCLhuLkiOiJsIiwi4rGhIjoibCIsIuqdiSI6ImwiLCLhuLsiOiJsIiwixYAiOiJsIiwiyasiOiJsIiwi4baFIjoibCIsIsmtIjoibCIsIsWCIjoibCIsIseJIjoibGoiLCLFvyI6InMiLCLhupwiOiJzIiwi4bqbIjoicyIsIuG6nSI6InMiLCLhuL8iOiJtIiwi4bmBIjoibSIsIuG5gyI6Im0iLCLJsSI6Im0iLCLhta8iOiJtIiwi4baGIjoibSIsIsWEIjoibiIsIsWIIjoibiIsIsWGIjoibiIsIuG5iyI6Im4iLCLItSI6Im4iLCLhuYUiOiJuIiwi4bmHIjoibiIsIse5IjoibiIsIsmyIjoibiIsIuG5iSI6Im4iLCLGniI6Im4iLCLhtbAiOiJuIiwi4baHIjoibiIsIsmzIjoibiIsIsOxIjoibiIsIseMIjoibmoiLCLDsyI6Im8iLCLFjyI6Im8iLCLHkiI6Im8iLCLDtCI6Im8iLCLhu5EiOiJvIiwi4buZIjoibyIsIuG7kyI6Im8iLCLhu5UiOiJvIiwi4buXIjoibyIsIsO2IjoibyIsIsirIjoibyIsIsivIjoibyIsIsixIjoibyIsIuG7jSI6Im8iLCLFkSI6Im8iLCLIjSI6Im8iLCLDsiI6Im8iLCLhu48iOiJvIiwixqEiOiJvIiwi4bubIjoibyIsIuG7oyI6Im8iLCLhu50iOiJvIiwi4bufIjoibyIsIuG7oSI6Im8iLCLIjyI6Im8iLCLqnYsiOiJvIiwi6p2NIjoibyIsIuKxuiI6Im8iLCLFjSI6Im8iLCLhuZMiOiJvIiwi4bmRIjoibyIsIserIjoibyIsIsetIjoibyIsIsO4IjoibyIsIse/IjoibyIsIsO1IjoibyIsIuG5jSI6Im8iLCLhuY8iOiJvIiwiyK0iOiJvIiwixqMiOiJvaSIsIuqdjyI6Im9vIiwiyZsiOiJlIiwi4baTIjoiZSIsIsmUIjoibyIsIuG2lyI6Im8iLCLIoyI6Im91Iiwi4bmVIjoicCIsIuG5lyI6InAiLCLqnZMiOiJwIiwixqUiOiJwIiwi4bWxIjoicCIsIuG2iCI6InAiLCLqnZUiOiJwIiwi4bW9IjoicCIsIuqdkSI6InAiLCLqnZkiOiJxIiwiyqAiOiJxIiwiyYsiOiJxIiwi6p2XIjoicSIsIsWVIjoiciIsIsWZIjoiciIsIsWXIjoiciIsIuG5mSI6InIiLCLhuZsiOiJyIiwi4bmdIjoiciIsIsiRIjoiciIsIsm+IjoiciIsIuG1syI6InIiLCLIkyI6InIiLCLhuZ8iOiJyIiwiybwiOiJyIiwi4bWyIjoiciIsIuG2iSI6InIiLCLJjSI6InIiLCLJvSI6InIiLCLihoQiOiJjIiwi6py/IjoiYyIsIsmYIjoiZSIsIsm/IjoiciIsIsWbIjoicyIsIuG5pSI6InMiLCLFoSI6InMiLCLhuaciOiJzIiwixZ8iOiJzIiwixZ0iOiJzIiwiyJkiOiJzIiwi4bmhIjoicyIsIuG5oyI6InMiLCLhuakiOiJzIiwiyoIiOiJzIiwi4bW0IjoicyIsIuG2iiI6InMiLCLIvyI6InMiLCLJoSI6ImciLCLhtJEiOiJvIiwi4bSTIjoibyIsIuG0nSI6InUiLCLFpSI6InQiLCLFoyI6InQiLCLhubEiOiJ0IiwiyJsiOiJ0IiwiyLYiOiJ0Iiwi4bqXIjoidCIsIuKxpiI6InQiLCLhuasiOiJ0Iiwi4bmtIjoidCIsIsatIjoidCIsIuG5ryI6InQiLCLhtbUiOiJ0IiwixqsiOiJ0IiwiyogiOiJ0IiwixaciOiJ0Iiwi4bW6IjoidGgiLCLJkCI6ImEiLCLhtIIiOiJhZSIsIsedIjoiZSIsIuG1tyI6ImciLCLJpSI6ImgiLCLKriI6ImgiLCLKryI6ImgiLCLhtIkiOiJpIiwiyp4iOiJrIiwi6p6BIjoibCIsIsmvIjoibSIsIsmwIjoibSIsIuG0lCI6Im9lIiwiybkiOiJyIiwiybsiOiJyIiwiyboiOiJyIiwi4rG5IjoiciIsIsqHIjoidCIsIsqMIjoidiIsIsqNIjoidyIsIsqOIjoieSIsIuqcqSI6InR6Iiwiw7oiOiJ1Iiwixa0iOiJ1Iiwix5QiOiJ1Iiwiw7siOiJ1Iiwi4bm3IjoidSIsIsO8IjoidSIsIseYIjoidSIsIseaIjoidSIsIsecIjoidSIsIseWIjoidSIsIuG5syI6InUiLCLhu6UiOiJ1IiwixbEiOiJ1IiwiyJUiOiJ1Iiwiw7kiOiJ1Iiwi4bunIjoidSIsIsawIjoidSIsIuG7qSI6InUiLCLhu7EiOiJ1Iiwi4burIjoidSIsIuG7rSI6InUiLCLhu68iOiJ1IiwiyJciOiJ1IiwixasiOiJ1Iiwi4bm7IjoidSIsIsWzIjoidSIsIuG2mSI6InUiLCLFryI6InUiLCLFqSI6InUiLCLhubkiOiJ1Iiwi4bm1IjoidSIsIuG1qyI6InVlIiwi6p24IjoidW0iLCLisbQiOiJ2Iiwi6p2fIjoidiIsIuG5vyI6InYiLCLKiyI6InYiLCLhtowiOiJ2Iiwi4rGxIjoidiIsIuG5vSI6InYiLCLqnaEiOiJ2eSIsIuG6gyI6InciLCLFtSI6InciLCLhuoUiOiJ3Iiwi4bqHIjoidyIsIuG6iSI6InciLCLhuoEiOiJ3Iiwi4rGzIjoidyIsIuG6mCI6InciLCLhuo0iOiJ4Iiwi4bqLIjoieCIsIuG2jSI6IngiLCLDvSI6InkiLCLFtyI6InkiLCLDvyI6InkiLCLhuo8iOiJ5Iiwi4bu1IjoieSIsIuG7syI6InkiLCLGtCI6InkiLCLhu7ciOiJ5Iiwi4bu/IjoieSIsIsizIjoieSIsIuG6mSI6InkiLCLJjyI6InkiLCLhu7kiOiJ5IiwixboiOiJ6Iiwixb4iOiJ6Iiwi4bqRIjoieiIsIsqRIjoieiIsIuKxrCI6InoiLCLFvCI6InoiLCLhupMiOiJ6IiwiyKUiOiJ6Iiwi4bqVIjoieiIsIuG1tiI6InoiLCLhto4iOiJ6IiwiypAiOiJ6IiwixrYiOiJ6IiwiyYAiOiJ6Iiwi76yAIjoiZmYiLCLvrIMiOiJmZmkiLCLvrIQiOiJmZmwiLCLvrIEiOiJmaSIsIu+sgiI6ImZsIiwixLMiOiJpaiIsIsWTIjoib2UiLCLvrIYiOiJzdCIsIuKCkCI6ImEiLCLigpEiOiJlIiwi4bWiIjoiaSIsIuKxvCI6ImoiLCLigpIiOiJvIiwi4bWjIjoiciIsIuG1pCI6InUiLCLhtaUiOiJ2Iiwi4oKTIjoieCJ9";
        var Latinise = {}; Latinise.latin_map = JSON.parse(decodeURIComponent(escape(atob(base64map))));
        String.prototype.latinise = function () {
            return this.replace(/[^A-Za-z0-9\[\] ]/g, function (x) { return Latinise.latin_map[x] || x; });
        };
        String.prototype.latinize = String.prototype.latinise;
        String.prototype.isLatin = function () { return this == this.latinise() }
        var placeSearch, shippingautocomplete;

        var shippingcomponentForm = {
            sublocality_level_1: 'ctl00_maincontent_txtupdateadd2,short_name',
            locality: 'ctl00_maincontent_txtupdatetown,long_name',
            postal_town: 'ctl00_maincontent_txtupdatetown,long_name',
            administrative_area_level_1: 'ctl00_maincontent_txtupdatestate,short_name',
            postal_code: 'ctl00_maincontent_txtupdatezip,short_name',
            country: 'ctl00_maincontent_drpupdatecountry,long_name'
        };

        function initAutocomplete() {

            shippingautocomplete = new google.maps.places.Autocomplete(
           (document.getElementById('ctl00_maincontent_txtupdateadd')),
          { types: ['address'] });
            shippingautocomplete.addListener('place_changed', fillInAddressShipping);

            AddKeyTrap();
        }
        function AddKeyTrap() {
            $("input[id*='txtsadd']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtsadd2']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txttown']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtstate']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtzip']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtsadd_Bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtadd2_Bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txttown_Bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtstate_Bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtzip_bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtstate_Bill']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtbillbusname']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txtbillname']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
            $("input[id*='txt_attnto']").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault()
                    e.stopPropagation();
                    return false;
                }
            });
        }

        function fillInAddressShipping() {
            // Get the place details from the autocomplete object.
            document.getElementById("ctl00_maincontent_txtupdateadd2").value = '';
            document.getElementById("ctl00_maincontent_txtupdatetown").value = '';

            document.getElementById("ctl00_maincontent_txtupdatestate").value = '';

            document.getElementById("ctl00_maincontent_txtupdatezip").value = '';
            var place = shippingautocomplete.getPlace();

            //Check that the country can be shipped to
            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];

                if (addressType === 'country') {
                    var val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]].toString().latinise();
                    var ctl = document.getElementById(shippingcomponentForm[addressType].split(',')[0]);
                    SetSelectedText(ctl, val);

                    if (val.toLowerCase() == "australia") {
                        document.getElementById('ctl00_maincontent_txtupdatestate').style.display = 'none';
                        document.getElementById('ctl00_maincontent_drpupdatestate').style.display = 'block';
                        //document.getElementById('ctl00_maincontent_rfvstate').style.display = 'none';
                        //ValidatorEnable(document.getElementById('<%= rfvstate.ClientID %>'), false);
                      } else {
                          document.getElementById('ctl00_maincontent_txtupdatestate').style.display = 'block';
                          document.getElementById('ctl00_maincontent_drpupdatestate').style.display = 'none';
                          //document.getElementById('ctl00_maincontent_rfvstate').disabled = true;
                          //document.getElementById('ctl00_maincontent_rfvstate').style.display = 'none';
                          //ValidatorEnable(document.getElementById('<%= rfvstate.ClientID %>'), true);
                      }

                      if (val.toLowerCase() == "united states") {

                          document.getElementById("ctl00_maincontent_drpupdatecountry").value = "US";
                          break;
                      }
                      else if (val.toLowerCase() == "germany") {

                          document.getElementById("ctl00_maincontent_drpupdatecountry").value = "DE";
                          break;
                      }
                      else if (val.toLowerCase() == "vietnam") {

                          document.getElementById("ctl00_maincontent_drpupdatecountry").value = "VN";

                          break;
                      }
                      else if (val.toLowerCase() == "hong kong") {

                          document.getElementById("ctl00_maincontent_drpupdatecountry").value = "HK";
                          break;
                      }
                      else if (val.toLowerCase() == "tanzania") {

                          document.getElementById("ctl00_maincontent_drpupdatecountry").value = "tanzania";
                          break;
                      }
                  }
              }

              //for (var component in shippingcomponentForm) {
              //    alert(shippingcomponentForm[component]);
              //    var ctl = document.getElementById(shippingcomponentForm[component].split(',')[0]);

              //    if (ctl.nodeName === 'INPUT') {
              //        ctl.value = '';
              //        ctl.disabled = false;
              //    }
              //}

              //Fill in address line1 need to do this separately because of google autopopulating unit numbers when not entered
              document.getElementById('ctl00_maincontent_txtupdateadd').value =
                 document.getElementById('ctl00_maincontent_txtupdateadd').value.split(',')[0];
              document.getElementById('ctl00_maincontent_txtupdateadd2').value = ''
              // Get each component of the address from the place details
              // and fill the corresponding field on the form.
              for (var i = 0; i < place.address_components.length; i++) {
                  var addressType = place.address_components[i].types[0];
                  //var state_val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]].toString().latinise();
                  //if (addressType == 'administrative_area_level_1') {
                  //    document.getElementById("ctl00_maincontent_drpstate").value = state_val;
                  //}
                  if (shippingcomponentForm[addressType]) {
                      var val = place.address_components[i][shippingcomponentForm[addressType].split(',')[1]];
                      // alert(shippingcomponentForm[addressType].split(',')[0]);
                      var ctl = document.getElementById(shippingcomponentForm[addressType].split(',')[0]);
                      //alert(addressType);
                      //alert(val);
                      //ctl.value = '';
                      if (addressType == 'administrative_area_level_1') {
                          document.getElementById("ctl00_maincontent_drpupdatestate").value = val;
                          document.getElementById("ctl00_maincontent_txtupdatestate").value = val;
                          if (ctl.value != "") {
                              $("#" + ctl.id).removeClass("error");
                              $("#" + ctl.id).nextAll("span").hide();
                          }
                          else {
                              $("#" + ctl.id).addClass("error");
                              $("#" + ctl.id).nextAll("span").show();
                          }
                      }

                      if (ctl.nodeName === 'INPUT') {
                          ctl.value = '';
                          if (ctl.value === '') {
                              ctl.value = val;
                          } else {
                              if (addressType === 'street_number') {
                                  ctl.value += '/' + val;
                              } else {
                                  ctl.value += ' ' + val;
                              }
                          }
                      } else if (ctl.nodeName === 'SELECT') {
                          SetSelectedText(ctl, val);
                      }
                      if (ctl.value != "") {
                          $("#" + ctl.id).removeClass("error");
                          $("#" + ctl.id).next("span").hide();
                      }


                  }
              }

          }

        <%-- $(".form-control").on("change paste keyup", function () {
        }--%>
        function SetSelectedText(dd, textToFind) {
            //  dd.selectedIndex = -1;
            for (var i = 0; i < dd.options.length; i++) {
                if (dd.options[i].text.toLowerCase() === textToFind.toLowerCase()) {
                    dd.selectedIndex = i;
                    break;
                }



            }
        }

        // Bias the autocomplete object to the user's geographical location,
        // as supplied by the browser's 'navigator.geolocation' object.
        function geolocate() {

            //if (navigator.geolocation) {
            //  navigator.geolocation.getCurrentPosition(function(position) {
            var geolocation = {
                lat: -37.874, //position.coords.latitude,
                lng: 145.0425 //position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: 20  //position.coords.accuracy
            });
            //billingautocomplete.setBounds(circle.getBounds());
            shippingautocomplete.setBounds(circle.getBounds());

            //});
            //}
        }
        function geolocate_bill() {
            //if (navigator.geolocation) {
            //  navigator.geolocation.getCurrentPosition(function(position) {
            var geolocation = {
                lat: -37.874, //position.coords.latitude,
                lng: 145.0425 //position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: 20  //position.coords.accuracy
            });
            billingautocomplete.setBounds(circle.getBounds());






            //shippingautocomplete.setBounds(circle.getBounds());
            //});
            //}
        }


    </script>


    <asp:Panel ID="pnlProfile" runat="server" DefaultButton="btnUpdate">
        <%--  <table align="center" width="558" border="0" cellspacing="0">
            <tr>
                <td align="left" class="tx_1">
                    <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                        font-weight: bolder; font-size: small; font-style: normal"> / </font>View User
                    Profile
                </td>
            </tr>
            <tr>
                <td class="tx_3">
                    <hr>
                </td>
            </tr>
        </table>--%>
        <div class="span9 box1" style="width: 580px; margin-left: 5px;">
            <h3 class="title1" align="left">View User Profile</h3>
            <table align="center" width="100%">
                <tr>
                    <td align="center">
                        <%-- &nbsp;<asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="false"
                        meta:resourcekey="LblStar" Width="1px"></asp:Label>
                    &nbsp;<asp:Label ID="Label4" runat="server" Visible="false" meta:resourcekey="LblReqField"
                        Class="lblNormalSkin"></asp:Label>--%>
                        <table width="100%" cellspacing="0" align="center">
                            <%--    <tr>
                            <td align="left">
                              <div class="box1">
                           <h3 class="title3">Contact Details</h3>
                                <table id="user" width="100%" cellpadding="0" cellspacing="0">
                                    <tr  >
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="Label5" Class="lblNormalSkin" runat="server" Text="Company Name"></asp:Label>
                                            <asp:Label ID="Label6" runat="server" Text=" "></asp:Label>
                                           </span>
                                        </td>
                                        <td>
                                            <span class="form_2">
                                            <asp:TextBox autocomplete="off" ReadOnly="true" ID="txtcompname" CssClass="input_dr"
                                                runat="server" MaxLength="40" Width="203px"></asp:TextBox>
                                               </span>
                                        </td>
                                        <td ></td>
                                    </tr>
                                    <tr>
                                      
                                        <td style="width: 30%">
                                             <span class="form_1">
                                            <asp:Label ID="lblFName" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFName"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 67%;">                                           
                                              <span class="form_2">
                                            <asp:TextBox autocomplete="off" ID="txtFname" ReadOnly="true" runat="server" MaxLength="40"
                                               CssClass="input_dr" Width="151px"></asp:TextBox></span>
                                            <span>
                                                 <a class="pop_btnwhite" onclick="ShowModal();" style="cursor:pointer;font-size: 12px; padding: 3px 6px; margin: 15px 0 0 0; display: inline-block;">Update Details</a>
                                            </span>
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvFname" runat="server" ControlToValidate="txtFname"
                                                Display="Dynamic" meta:resourcekey="rfvFname" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td>
                                        <span class="form_1">
                                            <asp:Label ID="lblAdd1" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd1"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAdd1" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvAdd1" meta:resourcekey="rfvAdd1" runat="server"
                                                ControlToValidate="txtAdd1" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAdd2" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd2"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAdd2" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAdd3" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd3"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAdd3" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox>
                                                </span> 
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblCity" Class="lblNormalSkin" runat="server" meta:resourcekey="lblCity"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtCity" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvCity" meta:resourcekey="rfvCity" runat="server"
                                                ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td style="height: 28px"><span class="form_1">
                                            <asp:Label ID="lblState" Class="lblNormalSkin" runat="server" meta:resourcekey="lblState"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="height: 28px">
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="drpState" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                          
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                        <td><span class="form_1">
                                            <asp:Label ID="lblZip" Class="lblNormalSkin" runat="server" meta:resourcekey="lblZip"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtZip" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvZip" meta:resourcekey="rfvZip" runat="server"
                                                ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td style="height: 28px">  <span class="form_1">
                                            <asp:Label ID="lblCountry" Class="lblNormalSkin" runat="server" meta:resourcekey="lblCountry"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="height: 28px">  <span class="form_2">
                                            <asp:DropDownList ID="drpCountry" runat="server" Width="210px" CssClass="input_dr"
                                                Enabled="false">
                                            </asp:DropDownList>
                                        </span> 
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                      
                                        <td>  <span class="form_1">
                                            <asp:Label ID="lblAltEmail" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAltEmail"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                          <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAltEmail" CssClass="input_dr"
                                                runat="server" MaxLength="40" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <%
                                                if (txtAltEmail.Text != null)
                                                {
                                            %>
                                           
                                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                                meta:resourcekey="valRegEx" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory" TabIndex="455"></asp:RegularExpressionValidator>
                                            <%} %>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblPhone" Class="lblNormalSkin" runat="server" meta:resourcekey="lblPhone"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtPhone" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvPhone" meta:resourcekey="rfvPhone" runat="server"
                                                ControlToValidate="txtPhone" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       

                                        <td style="height: 30px"><span class="form_1">
                                            <asp:Label ID="lblMobile" Class="lblNormalSkin" runat="server" meta:resourcekey="lblMobile"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="height: 30px">
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtMobile" CssClass="input_dr"
                                                runat="server" MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                       
                                        <td style="width: 30%">
                                        <span class="form_1">
                                            <asp:Label ID="lblFax" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFax"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 67%">
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtFax" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    </table>
                                </div>  
                                   
                            </td>
                        </tr>--%>

                            <tr>
                                <td align="left">
                                    <div class="box1">
                                        <h3 class="title3">Shipping Information</h3>
                                        <table id="Table2" width="100%" cellpadding="0" cellspacing="0">
                                            <%-- <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblshipTitle" runat="server" meta:resourcekey="lblShippingTitle" Class="lblStaticSkin"></asp:Label>
                                        </td>
                                    </tr>--%>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkShippingAdd" runat="server" Visible="false" AutoPostBack="true"
                                                        Class="CheckBoxSkin" Checked="false" meta:resourcekey="ChkShipTitle" OnCheckedChanged="ChkShippingAdd_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>

                                                <td style="width: 30%"><span class="form_1">
                                                    <asp:Label ID="Label8" runat="server" Text="Company Name" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td style="width: 70%">
                                                    <span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtShipCompName" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                                     <span>
                                                         <%
                                                             int UserID;
                                                             UserServices objUserServices = new UserServices();
                                                             HelperServices objHelperServices = new HelperServices();
                                                             UserID = objHelperServices.CI(Session["USER_ID"]); ;
                                                             if (Session["USER_ROLE"] != null && Session["USER_ROLE"].ToString()=="1")
                                                             {
                                                              %>
                                                                 <a class="pop_btnwhite" onclick="ShowModal();" style="cursor:pointer;font-size: 12px; padding: 3px 6px; margin: 15px 0 0 0; display: inline-block;">Update Details</a>
                                                   
                                                         <%}else{ %>
                                                                 <a class="pop_btnwhite" onclick="AdminUser();" style="cursor:pointer;font-size: 12px; padding: 3px 6px; margin: 15px 0 0 0; display: inline-block;" >Update Details</a>
                                                         <%} %>
                                                          </span>
                                                </td>
                                            </tr>
                                          
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipadd1" runat="server" meta:resourcekey="lblshipaddress1" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipadd1" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                                        Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshiadd2" runat="server" meta:resourcekey="lblshipaddress2" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipadd2" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>
                                           <%-- <tr>
                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipadd3" runat="server" meta:resourcekey="lblshipaddress3" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipadd3" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>--%>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipcity" runat="server" meta:resourcekey="lblshipCity" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipcity" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtshipcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipState" runat="server" meta:resourcekey="lblshipState" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="drpShipState" runat="server"
                                                        CssClass="input_dr" MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>

                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipZip" runat="server" meta:resourcekey="lblshipZip" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipzip" runat="server" CssClass="input_dr"
                                                        MaxLength="20" Width="203px"></asp:TextBox></span>


                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtshipzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipCountry" runat="server" meta:resourcekey="lblshipCountry" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:DropDownList ID="drpShipCountry" Enabled="false" runat="server" Width="210px"
                                                        Class="DropdownlistSkin">
                                                    </asp:DropDownList>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>
                                              <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="Label10" runat="server" meta:resourcekey="lblReceiveName" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtShipFname" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>
                                             <tr>
                                       
                                                <td style="width: 30%">
                                                <span class="form_1">
                                                    <asp:Label ID="Label3" Class="lblNormalSkin" runat="server" meta:resourcekey="lblDeliveryInst"></asp:Label>
                                                    </span>
                                                </td>
                                                <td style="width: 67%">
                                                <span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipDeliveryInst" CssClass="input_dr" runat="server"
                                                        MaxLength="20" Width="203px"></asp:TextBox>
                                                        </span>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                         <%--   <tr>
                                      
                                        <td>  <span class="form_1">
                                            <asp:Label ID="lblAltEmail" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAltEmail"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                          <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipemail" CssClass="input_dr"
                                                runat="server" MaxLength="40" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <%
                                            if (txtshipemail.Text != null)
                                                {
                                            %>
                                           
                                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtshipemail"
                                                meta:resourcekey="valRegEx" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory" TabIndex="455"></asp:RegularExpressionValidator>
                                            <%} %>
                                        </td>
                                    </tr>--%>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblshipPhone" runat="server" meta:resourcekey="lblshipPhone" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td>
                                                    <span class="form_2">
                                                        <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipphone" runat="server"
                                                            CssClass="input_dr" MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvsPhone" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtshipphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                       
                                        <td style="width: 30%">
                                        <span class="form_1">
                                            <asp:Label ID="lblFax" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFax"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 67%">
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtshipFax" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>

                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div class="box1">
                                        <h3 class="title3">Billing information</h3>
                                        <table id="Table1" width="100%" cellpadding="0" cellspacing="0">

                                            <tr>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="false" AutoPostBack="true"
                                                        Class="CheckBoxSkin" meta:resourcekey="ChkBillTitle" Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" />
                                                </td>
                                            </tr>
                                            <tr>

                                                <td style="width: 30%"><span class="form_1">
                                                    <asp:Label ID="Label12" runat="server" Text="Company Name"
                                                        Class="lblNormalSkin"></asp:Label></span>
                                                </td>
                                                <td style="width: 70%"><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillCompanyName" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>
                                                </td>
                                                <td></td>
                                            </tr>
                                           
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbilladdress1" runat="server" meta:resourcekey="lblbilladdress1"
                                                        Class="lblNormalSkin"></asp:Label></span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbilladd1" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                                        Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbilladdress2" runat="server" meta:resourcekey="lblbilladdress2"
                                                        Class="lblNormalSkin"></asp:Label></span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbilladd2" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>
                                                </td>
                                                <td></td>
                                            </tr>
                                           <%-- <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbilladdress3" runat="server" meta:resourcekey="lblbilladdress3"
                                                        Class="lblNormalSkin"></asp:Label></span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbilladd3" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>
                                                </td>
                                                <td></td>
                                            </tr>--%>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbillCity" runat="server" meta:resourcekey="lblbillCity" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillcity" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtbillcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbillState" runat="server" meta:resourcekey="lblbillState" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">

                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="drpBillState" runat="server"
                                                        CssClass="input_dr" MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbillZip" runat="server" meta:resourcekey="lblbillZip" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillzip" runat="server" CssClass="input_dr"
                                                        MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtbillzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbillCountry" runat="server" meta:resourcekey="lblbillCountry" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:DropDownList ID="drpBillCountry" runat="server" Enabled="false" Width="210px"
                                                        Class="DropdownlistSkin">
                                                    </asp:DropDownList></span>
                                                </td>
                                                <td></td>
                                            </tr>
                                             <tr>
                                                <td><span class="form_1">
                                                    <asp:Label ID="Label18" runat="server" meta:resourcekey="lblReceiveName"
                                                        Class="lblNormalSkin"></asp:Label></span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillFName" runat="server" CssClass="input_dr"
                                                        MaxLength="40" Width="203px"></asp:TextBox>
                                                </span>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                       
                                                <td style="width: 30%">
                                                <span class="form_1">
                                                    <asp:Label ID="Label2" Class="lblNormalSkin" runat="server" meta:resourcekey="lblDeliveryInst"></asp:Label>
                                                    </span>
                                                </td>
                                                <td style="width: 67%">
                                                <span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillDeliveryInst" CssClass="input_dr" runat="server"
                                                        MaxLength="20" Width="203px"></asp:TextBox>
                                                        </span>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                       <%--     <tr>
                                      
                                        <td>  <span class="form_1">
                                            <asp:Label ID="Label4" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAltEmail"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                          <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillemail" CssClass="input_dr"
                                                runat="server" MaxLength="40" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <%
                                            if (txtbillemail.Text != null)
                                                {
                                            %>
                                           
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbillemail"
                                                meta:resourcekey="valRegEx" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory" TabIndex="455"></asp:RegularExpressionValidator>
                                            <%} %>
                                        </td>
                                    </tr>--%>
                                    
                                            <tr>

                                                <td><span class="form_1">
                                                    <asp:Label ID="lblbillPhone" runat="server" meta:resourcekey="lblbillPhone" Class="lblNormalSkin"></asp:Label>
                                                </span>
                                                </td>
                                                <td><span class="form_2">
                                                    <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillphone" runat="server"
                                                        CssClass="input_dr" MaxLength="40" Width="203px"></asp:TextBox></span>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                                        ControlToValidate="txtbillphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                       
                                        <td style="width: 30%">
                                        <span class="form_1">
                                            <asp:Label ID="Label1" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFax"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 67%">
                                        <span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtbillFax" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="558" align="center">
                            <tr>
                                <td width="50%" align="center">
                                    <asp:Button ID="btnUpdate" Visible="false" runat="server" meta:resourcekey="btnUpdate"
                                        OnClick="btnUpdate_Click" ValidationGroup="Mandatory" class="button normalsiz btngreen btnmain" />




                                    <div id="editaddresspopup" class="modal-box" style="top: 20px;width:550px;left:0;right:0;top:0;bottom:0;height:585px;margin:auto; border: 6px solid #e6e6e5;">

                                        <div class="modal-body" style="padding: 1.5em 1.8em 1.1em;">
                                            <div class="mt10 mb20">
                                                <h2 class="pophead" style="font-size: 24px;">Shipping Address Update Request Form </h2>
                                            </div>
                                            <div class="mt10 mb20">
                                                <h4 class="pophead" style="font-size: 14px;">Please Fill in the form  below and click submit to update your shipping details</h4>
                                                <h4 class="pophead" style="font-size: 14px;">our Account Team will get back to you ASAP.</h4>
                                            </div>
                                            <div class="cmn_wrap">
                                                <div id="Div1" runat="server">

                                                    <%--<div class="form-group mb10 clearfix">
                                                    <label class="col-md-8 pl0">Bussiness Name</label>
                                                    <div class="col-md-12 mpl0">

                                                        <asp:TextBox runat="server" ID="txtComname" Text="" class="form-control" MaxLength="30" onkeyup="javascript:keyboardup(this);"></asp:TextBox>
                                                    </div>
                                                    <label style="font-size: 12px; font-weight: lighter; padding-top: 10px" class="mb15">A business name is required if your order is being delivered to a non-residential address.</label>
                                                </div>
                                                <div class="form-group mb10 clearfix">
                                                    <label class="col-md-8 pl0">Receivers Name</label>
                                                    <div class="col-md-12 mpl0">
                                                        <asp:TextBox runat="server" ID="txt_attnto" Text="" class="form-control" MaxLength="250"></asp:TextBox>
                                                    </div>
                                                </div>--%>

                                                    <div>
                                                        <div class="form-col-3-8">
                                                            <label style="margin: 9px 0px;">Account / Company Name</label>
                                                        </div>
                                                        <div class="form-col-4-8">
                                                            <asp:TextBox runat="server" ID="txtComname" Text="" class="form-control" MaxLength="30"></asp:TextBox>
                                                        </div>

                                                        <div class="clear"></div>
                                                    </div>


                                                </div>
                                                <div>

                                                    <label>Street Address<span class="required">*</span></label>
                                                    <div class="form-col-8-8">
                                                        <asp:TextBox runat="server" ID="txtupdateadd" Text="" onfocus="geolocate()" class="form-control mb10" MaxLength="30" autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvsadd" runat="server" Class="mandatory"
                                                            ErrorMessage="Required" ValidationGroup="updateRequest" Display="Dynamic" Text="Enter Street Address" ControlToValidate="txtupdateadd" ForeColor="Red" Style="float: left;"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" ID="txtupdateadd2" Text="" class="form-control" MaxLength="30"></asp:TextBox>

                                                    </div>
                                                    <div class="clear"></div>

                                                </div>


                                                <div>

                                                    <div class="form-col-4-8">
                                                        <label>Suburb / Town<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtupdatetown" Text="" class="form-control checkout_input" MaxLength="30"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtown" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="updateRequest" Display="Dynamic" Text="Enter Suburb/Town" ForeColor="Red" ControlToValidate="txtupdatetown" Style="float: left;"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="form-col-4-8">
                                                        <label>State Province<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtupdatestate" Text="" Style="display: none" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                        <%--   <asp:DropDownList runat="server" ID="drpupdatestate" CssClass="form-control" MaxLength="20"></asp:DropDownList>--%>
                                                        <asp:DropDownList ID="drpupdatestate" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvstate" runat="server" Class="mandatory"
                                                            ErrorMessage="Required" ValidationGroup="updateRequest" Display="Dynamic"
                                                            Text="Enter State/Province" ControlToValidate="txtupdatestate" ForeColor="Red" Style="float: left;">Enter State</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="clear"></div>

                                                </div>


                                                <div>
                                                    <div class="form-col-4-8">
                                                        <label>Postal Zipcode<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtupdatezip" Text="" class="form-control" MaxLength="10"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="updateRequest" Display="Dynamic" Text="Enter Post/Zip Code" ControlToValidate="txtupdatezip" ForeColor="Red" Style="float: left;"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                 <%--   <asp:FilteredTextBoxExtender ID="ftezip" runat="server" FilterMode="ValidChars" ValidChars="1234567890" TargetControlID="txtzip" />--%>
                                                    </div>
                                                    <div class="form-col-4-8">
                                                        <label>Country<span class="required">*</span></label>


                                                        <asp:DropDownList ID="drpupdatecountry" runat="server" onchange="drpcountry_change()" class="form-control"></asp:DropDownList>

                                                    </div>
                                                    <div class="clear"></div>

                                                </div>

                                                 <%--  <div>
                                                        <div class="form-col-3-8">
                                                            <label style="margin: 9px 0px;">ATTN / Receivers Name</label>
                                                        </div>
                                                        <div class="form-col-4-8">
                                                            <asp:TextBox runat="server" ID="txt_attnto" Text="" class="form-control" MaxLength="250"></asp:TextBox>
                                                        </div>
                                                    </div>--%>
                                 
                                                    <div>

                                                    <div class="form-col-4-8">
                                                        <label>ATTN / Receivers Name</label>
                                                        <asp:TextBox runat="server" ID="txt_attnto" Text="" class="form-control checkout_input" MaxLength="250"></asp:TextBox>
                                                    </div>
                                                    <div class="form-col-4-8">
                                                        <label>Phone Number<span class="required">*</span></label>
                                                        <asp:TextBox runat="server" ID="txtphonenumber" Text="" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvphonenumber" runat="server" Class="mandatory" ErrorMessage="Required" ValidationGroup="updateRequest" Display="Dynamic" Text="Enter Phone Number" ForeColor="Red" ControlToValidate="txtphonenumber" Style="float: left;"></asp:RequiredFieldValidator>
                                                        <asp:FilteredTextBoxExtender ID="ftephonenumber" runat="server" FilterMode="ValidChars"
                                                                FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtphonenumber" />
                                                    </div>
                                                    <div class="clear"></div>

                                                </div>

                                                
                                                <div>
                                                    <div class="form-col-8-8">
                                                        <label>Delivery Instructions</label>
                                                        <%--<div class="col-md-20 pl5 pr0">--%>
                                                        <asp:TextBox runat="server" ID="txtDELIVERYINST" Text="" class="form-control" MaxLength="30"></asp:TextBox>
                                                        <%-- </div>--%>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>

                                            </div>

                                            <div class="btns" style="margin-top: 15px;">
                                                <asp:Button ID="btnSubmitAddress" runat="server" Text="Submit" CssClass="pop_btngreen" ValidationGroup="updateRequest" OnClick="btnSubmitAddress_Click" />
                                                <asp:Button ID="btnCloseAddress" runat="server" Text="Close" CssClass="pop_btnwhite" UseSubmitBehavior="false" OnClientClick="HideModel();return true;" />
                                            </div>
                                        </div>


                                    </div>





                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>




        </div>
    </asp:Panel>



</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
