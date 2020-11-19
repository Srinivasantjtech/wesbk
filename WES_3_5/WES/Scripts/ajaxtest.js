//$(document).ready(function () {
//    //alert("sdasdas");
//    // SearchText('txtitem1');

//});
function SearchText(ctl) {

    $(".marbot8").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PowerSearch.aspx/WestestAutoCompleteData",
                //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                data: "{'strvalue':'" + ctl.value + "'}",
                dataType: "json",
                success: function (data) {
                    //alert("success");
                    response(data.d);
                },
                error: function (result) {
                    alert("Error");
                }
            });
        }
    });
}