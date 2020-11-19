<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_contactus" Codebehind="contactus.ascx.cs" %>
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&signed_in=true&libraries=places"></script>

<script type="text/javascript">
  google.maps.event.addDomListener(window, 'load', initialize);
    function initialize() {
        //if (navigator.geolocation) {
        //    navigator.geolocation.getCurrentPosition(function (position) {
        //        var pos = {
        //            lat: position.coords.latitude,
        //            lng: position.coords.longitude
        //        };
        //        //alert(position.coords);
        //        getAddressFromLatLang(position.coords.latitude, position.coords.longitude);
        //        //  infoWindow.setPosition(pos);
        //        //    infoWindow.setContent('Location found.');
        //        // infoWindow.open(map);
        //        //  map.setCenter(pos);
        //    }, function () {
        //        // handleLocationError(true, infoWindow, map.getCenter());
        //    });
        //}
        var options = {

            componentRestrictions: { country: "AU" }
        };
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById("txtfromaddress"), options);
        google.maps.event.addListener(autocomplete, 'place_changed', function () {

            // Get the place details from the autocomplete object.
            var place = autocomplete.getPlace();
            embedmap(document.getElementById("txtfromaddress").value);
            //var location = "<b>Address</b>: " + place.formatted_address + "<br/>";
            //location += "<b>Latitude</b>: " + place.geometry.location.A + "<br/>";
            //location += "<b>Longitude</b>: " + place.geometry.location.F;
            //document.getElementById('lblResult').innerHTML = location
        });
    }
    function initialize_buttonclick() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                //alert(position.coords);
                getAddressFromLatLang(position.coords.latitude, position.coords.longitude);
                //  infoWindow.setPosition(pos);
                //    infoWindow.setContent('Location found.');
                // infoWindow.open(map);
                //  map.setCenter(pos);
            }, function () {
                // handleLocationError(true, infoWindow, map.getCenter());
            });
        }
        //var options = {

        //    componentRestrictions: { country: "AU" }
        //};
        //var autocomplete = new google.maps.places.Autocomplete(document.getElementById("txtfromaddress"), options);
        //google.maps.event.addListener(autocomplete, 'place_changed', function () {

        //    // Get the place details from the autocomplete object.
        //    var place = autocomplete.getPlace();
        //    embedmap(document.getElementById("txtfromaddress").value);
        //    //var location = "<b>Address</b>: " + place.formatted_address + "<br/>";
        //    //location += "<b>Latitude</b>: " + place.geometry.location.A + "<br/>";
        //    //location += "<b>Longitude</b>: " + place.geometry.location.F;
        //    //document.getElementById('lblResult').innerHTML = location
        //});
    }
</script>

    <script type="text/javascript">

        var map, infoWindow;

        function getAddressFromLatLang(lat, lng) {

            var geocoder = new google.maps.Geocoder();
            var latLng = new google.maps.LatLng(lat, lng);
            geocoder.geocode({ 'latLng': latLng }, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        // alert(results[1].formatted_address);
                        //  document.getElementById("txtfromaddress").value = results[1].formatted_address;

                        embedmap(results[1].formatted_address);
                    }

                } else {
                    embeddefault();
                }
            });

        }

        function embedmap(fromaddress) {

            $("address").each(function () {
                var tooadd = document.getElementById("ctl00_maincontent_ctl00_toaddress").value;
               
                //   var fromadd = document.getElementById("txtfromaddress").value;
                var fromadd;
                if (fromaddress != '') {
                    fromadd = fromaddress;
                }
                else {
                    fromadd = document.getElementById("txtfromaddress").value;
                }
                //    var embed = "<div class='google-maps'><iframe width='380' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

                var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://www.google.com/maps/embed/v1/directions?key=AIzaSyD6KpDELswcok62eMQN_L2qBpBud3mqpGM&origin=" + fromadd + "&destination=" + tooadd + "&avoid=tolls|highways'></iframe></div>";
                if (embed != null) {
                    $(this).html(embed);
                }
                else {
                    embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + tooadd + "&amp;output=embed'></iframe></div>";
                    $(this).html(embed);
                }


            });

        }
        $(document).ready(function () {
            $("address").each(function () {
                var tooadd = encodeURIComponent($(this).text());
                var fromadd = document.getElementById("txtfromaddress").value;

                var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

                $(this).html(embed);

            });
        });
        function embeddefault() {

            $("address").each(function () {
                var tooadd = document.getElementById("ctl00_maincontent_ctl00_toaddress").value;

                //    var embed = "<div class='google-maps'><iframe width='380' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + encodeURIComponent($(this).text()) + "&amp;output=embed'></iframe></div>";

                var embed = "<div class='google-maps'><iframe width='100%' height='350' frameborder='0' scrolling='no' style='border:0'   src='https://maps.google.com/maps?&amp;q=" + tooadd + "&amp;output=embed'></iframe></div>";
                $(this).html(embed);



            });

        }
  </script>
<%
    Response.Write(ST_contactus());
%>         
   <asp:HiddenField ID="toaddress" runat="server" Value=" WES Australiasia,  84-90 Parramatta Road, Ashfield NSW 2130,Australia" />