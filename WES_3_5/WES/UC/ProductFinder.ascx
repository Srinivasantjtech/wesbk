<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_ProductFinder" Codebehind="ProductFinder.ascx.cs" ViewStateMode="Enabled" %>
<%--<%@ Import Namespace="TradingBell.Common" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Import Namespace ="System.Data.OleDb" %> 
<%@ Import Namespace ="System.IO" %>  
<%@ Import Namespace ="System.Data" %>
<%@ Register Src="/UC/ProductFinderPL.ascx" TagName="ProductFinderPL" TagPrefix="uc4" %>
<%--<script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>--%>
<%--<script type="text/javascript" src="/scripts/jquery-1.10.2.min.js"></script>--%>
<%--<script src="/Scripts/AC_RunActiveContent.js" type="text/javascript"></script>--%>
<%--
<script  type="text/javascript">
    window.onload = function (e) {
        onTabClick("1")
    }
    function onTabClick(tab) {
        var txt
        if (tab == "1") {
            txt = document.getElementById("txtCable1");
            GetCableLData(txt);
        }
        else if (tab == "2") {
        }
        else if (tab == "3") {
            txt = document.getElementById("txtBrand");
            GetBrandData(txt);
        }
    }
    </script>
<script  type="text/javascript">
    function ImgLoadCable(cable1) {
        if (cable1 == "1") {
            var c1 = document.getElementById("Cable1_images");
            c1.style.display = "block";
            var c2 = document.getElementById("Cable2_images");
            c2.style.display = "none";
        }
        else
        {
            var c1 = document.getElementById("Cable1_images");
        c1.style.display = "none";
        var c2 = document.getElementById("Cable2_images");
        c2.style.display = "block";
        }
}
function ShowCableDropDown(bm) {
    if (bm == "1") {
        var c1 = document.getElementById("divCableLi1");
        c1.style.display = "block";
        var c2 = document.getElementById("divCableLi2");
        c2.style.display = "none";
    }
    else {
        var c1 = document.getElementById("divCableLi1");
        c1.style.display = "none";
        var c2 = document.getElementById("divCableLi2");
        c2.style.display = "block";
    }
}

function HideCableDropDown(bm) {
    if (bm == "1") {
        var c2 = document.getElementById("divCableLi1");
        c2.style.display = "none";
    }
    else {
        var c1 = document.getElementById("divCableLi2");
        c1.style.display = "none";
        
    }
}
    window.onload = function (e) {      
        var txtCa = document.getElementById("txtCable1");
        GetCableLData(txtCa);
    }
    function SetBtnclose() {
        var t1 = document.getElementById("txtCable1");
        var t2 = document.getElementById("txtCable2");
        var btn1 = document.getElementById("btncloseCable1");
        if (t1.value != "")
            btn1.style.display = "block";
        else
            btn1.style.display = "none";

        var btn2 = document.getElementById("btncloseCable2");

        if (t2.value != "")
            btn2.style.display = "block";
        else
            btn2.style.display = "none";
    }
    function GetCableLData(strv) {
        ImgLoadCable("1");
        var txtv = strv.value;
       

        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/FindCableL",
            data: "{'strfindvalue':'" + txtv + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnCableLSuccess,
            failure: function (response) {
               
                //alert(response.d);
            },
            error: function (response) {
               
                //alert(response.d);
            }
        });

        SetBtnclose();

    }

    function OnCableLSuccess(response) {


        var Cables;
        var CableList = document.getElementById("Cable_1");
        var CableImageList = document.getElementById("Cable1_images");
        CableList.innerHTML = '';
        CableImageList.innerHTML = '';
        

        
        
       
        if (response.d != null) {
            var Cables = response.d.split("#####");
            
            for (var i = 0; i < Cables.length; i++) {
                AddCableLi(CableList, Cables[i], "1");
                AddCableLiImage(CableImageList, Cables[i], "1");
            }
            var t1 = document.getElementById("txtCable1");
            var t2 = document.getElementById("txtCable2");
            t2.value = '';
            GetCableLRData(t2, t1, 'H');
        }
    }


    function GetCableLRData(ctl2, ctl1,h) {
        
        var txt1v = ctl1.value;
        var tx2tv = ctl2.value;
        if (h != "H")
            ImgLoadCable("2");


        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/FindCableLR",
            data: "{'strfindvalue':'" + tx2tv + "','strCable1value':'" + txt1v + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnCableLRSuccess,
            failure: function (response) {
                
                //alert(response.d);
            },
            error: function (response) {
                
                //alert(response.d);
            }
        });

        var t1 = document.getElementById("txtCable1");

        var t2 = document.getElementById("txtCable2");

        if(t1.value=="" && t2.value=="")
            YHS('','')        
        SetBtnclose();
    }
    function OnCableLRSuccess(response) {


        var Cables;
        var CableList = document.getElementById("Cable_2");
        var CableImageList = document.getElementById("Cable2_images");
        var t2 = document.getElementById("txtCable2");
        CableList.innerHTML = '';
        CableImageList.innerHTML = '';
       

        
        if (response.d != null) {
            var Cables = response.d.split("#####");
            for (var i = 0; i < Cables.length; i++) {
                AddCableLi(CableList, Cables[i], '2');
                AddCableLiImage(CableImageList, Cables[i], '2');
            }

        }
    }

    function OnclickCableList1(xvalue,yvalue) {

        var t1 = document.getElementById("txtCable1");

        var t2 = document.getElementById("txtCable2");
       
        var c1 = document.getElementById("spncablelint");
        var c11 = document.getElementById("spncablel");
        var C111 = document.getElementById("spncablelH");
        var C1img = document.getElementById("cable1_selectedimage");

        t1.value = xvalue;
        C111.innerHTML = "<h3>" + xvalue + "</h3>";
        c1.style.display = "none";
        c11.style.display = "block";
        C111.style.display = "block";
        C1img.innerHTML = "<img src='" + yvalue + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  />" 
        GetCableLData(t1);

        GetCableLRData(t2, t1, 'H');
        SetBtnclose();
        //YHS(xvalue, '');
        HideCableDropDown('1');
    }
    function OnclickCableList2(xvalue,yvalue) {

        var t1 = document.getElementById("txtCable1");

        var t2 = document.getElementById("txtCable2");

        var c1 = document.getElementById("spncableRint");
        var c11 = document.getElementById("spncableR");
        var C111 = document.getElementById("spncableRH");
        var C1img = document.getElementById("cable2_selectedimage");


        t2.value = xvalue;
        C111.innerHTML = "<h3>" + xvalue + "</h3>";
        c1.style.display = "none";
        c11.style.display = "block";
        C111.style.display = "block";
        C1img.innerHTML = "<img src='" + yvalue + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  />" 

        GetCableLRData(t2, t1, '');

        SetBtnclose();
       // YHS(t1.value,xvalue);
        HideCableDropDown('2');

    }

    function removetextCable(cable1) {

        var t1 = document.getElementById("txtCable1");

        var t2 = document.getElementById("txtCable2");

        if (cable1 == "1") {
            var c1 = document.getElementById("txtCable1");
            c1.value = "";
            GetCableLData(t1);
            YHS('', '');
        }
        else {
            var c2 = document.getElementById("txtCable2");
            c2.value = "";



            GetCableLRData(t2, t1, '');
            YHS(t1.value, '');
        }

    }
    function ChangeCableTitle(xval) {

        if (xval == "1") {
            ImgLoadCable("1");
            CableImagetitle.innerHTML = "1. First Connection Side";
        }
        else {
            ImgLoadCable("2");
            CableImagetitle.innerHTML = "2. Second Connection Side";
        }
    }
    function AddCableLi(CableList, xvalue, onclk) {
        var v = xvalue.split("&&&&&");
        var li = document.createElement("li");
        li.setAttribute("class", "cableLi");

        if (onclk == '1') {
            li.setAttribute("onclick", "OnclickCableList1('" + v[0] + "','"+ v[1] +"');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList1('" + v[0] + "','" + v[1] + "');\" >" + v[0] + "</a>";
        }
        else {
            li.setAttribute("onclick", "OnclickCableList2('" + v[0] + "');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList2('" + v[0] + "','" + v[1] + "');\" >" + v[0] + "</a>";
        }

        CableList.appendChild(li);
      
    }
    function AddCableDiv(CableList, xvalue, onclk) {
        var v = xvalue.split("&&&&&");
        var li = document.createElement("Div");
        li.setAttribute("class", "pro_grid");

        if (onclk == '1') {
            li.setAttribute("onclick", "OnclickCableList1('" + v[0] + "');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList1('" + v[0] + "');\" >" + v[0] + "</a>";
        }
        else {
            li.setAttribute("onclick", "OnclickCableList2('" + v[0] + "');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList2('" + v[0] + "');\" >" + v[0] + "</a>";
        }

        CableList.appendChild(li);
    }
    function AddCableLiImage(CableImageList, xvalue, onclk) {

     /*   var v = xvalue.split("&&&&&");
        var li = document.createElement("li");
       
        li.setAttribute("class", "cableimageLi"); 

        if (onclk == '1')
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList1('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\" /><div>" + v[0] + "</div></a>";
        else
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList2('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  /><div>" + v[0] + "</div></a>";

        CableImageList.appendChild(li);*/


        var v = xvalue.split("&&&&&");
        var li = document.createElement("Div");

        li.setAttribute("class", "pro_grid");

        if (onclk == '1')
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList1('" + v[0] + "','" + v[1] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\" /><h5>" + v[0] + "</h5></a>";
        else
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList2('" + v[0] + "','" + v[1] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  /><h5>" + v[0] + "</h5></a>";

        CableImageList.appendChild(li);
    }
    function AddCableDivImage(CableImageList, xvalue, onclk) {

        var v = xvalue.split("&&&&&");
        var li = document.createElement("Div");

        li.setAttribute("class", "pro_grid");

        if (onclk == '1')
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList1('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\" /><div>" + v[0] + "</div></a>";
        else
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickCableList2('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  /><div>" + v[0] + "</div></a>";

        CableImageList.appendChild(li);
    }
   
    function CalbeRedirect(response) {
        if (response.d != null && response.d != "")
            window.location.href = response.d;

    }

    function YHS(c1, c2) {

        var CableImageList1 = document.getElementById("Cable1_images");
        var CableImageList2 = document.getElementById("Cable2_images");
            
        var yhs="<div class=\"current_seldction\"> YOUR CURRENT SELECTION </div>";
        var yhs1="";
        var yhs2 ="";
       if (c1 != "") {
            yhs1 = "<div class=\"itemcategory\"><a onclick=\"removetextCable('1');\" class=\"remove\" style=\"cursor:pointer;\" ></a>";
            yhs1 = yhs1 + "<span class=\"removertxt\"><strong style=\"padding-left: 5px;\">Item CableL</strong><br><a style=\"cursor:pointer;\" onclick=\"GetCableFinderRedirect('" + c1 + "','');\">" + c1 + "</a></span></div>"
       }
       if (c2 != "") {
            yhs2 = "<div class=\"itemcategory\"><a onclick=\"removetextCable('2');\" class=\"remove\"></a>";
            yhs2 = yhs2 + "<span class=\"removertxt\"><strong style=\"padding-left: 5px;\">Item CableLR</strong><br><a style=\"cursor:pointer;\" onclick=\"GetCableFinderRedirect('" + c1 + "','" + c2 + "');\">" + c2 + "</a></span></div>"
       }


if (yhs1 != "" && yhs2 != "") {
    CableImageList1.style.height = "522px";
    CableImageList2.style.height = "522px";
}
else if (yhs1 != "" && yhs2 == "") {
    CableImageList1.style.height = "490px";
    CableImageList2.style.height = "490px";
}
else
{
    CableImageList1.style.height = "425px";
    CableImageList2.style.height = "425px";
}

       if (yhs1!="" ||  yhs2!="") {
           Cableyhs.innerHTML = yhs + yhs1 + yhs2;
           Cableyhs.style.display = "block";

        }
        else {
            Cableyhs.innerHTML = "";
            Cableyhs.style.display = "none";

        }
       

    }
    function GetCableFinderRedirect(cable1,cable2) {
        
          $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/GetFindMyCableURL",
            data: "{'Cable1':'" + cable1 + "','Cable2':'" + cable2 + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: CalbeRedirect,
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });
    }
    function GetCableFinderURL() {

        var cable1 = "";
        var cable2 = "";
        var f1;
        var f2;

        if (txtCable1.value != 'All Cable') 
            cable1 = txtCable1.value;


        if (txtCable2.value != 'All Cable') 
            cable2 = txtCable2.value;

      GetCableFinderRedirect(cable1,cable2);

    }
    
</script>

<script  type="text/javascript">

  
    function ImgLoadBrand(bm) {
        if (bm == "1") {
            var c1 = document.getElementById("Brand_images");
            c1.style.display = "block";
            var c2 = document.getElementById("Model_images");
            c2.style.display = "none";
        }
        else {
            var c1 = document.getElementById("Brand_images");
            c1.style.display = "none";
            var c2 = document.getElementById("Model_images");
            c2.style.display = "block";
        }
    }

   

    function GetBrandData(strv) {
        ImgLoadBrand("1");
        var txtv = strv.value;


        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/FindMyBrand",
            data: "{'strfindvalue':'" + txtv + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnBrandSuccess,
            failure: function (response) {

                //alert(response.d);
            },
            error: function (response) {

                //alert(response.d);
            }
        });



    }
    function OnBrandSuccess(response) {


        var brands;
        var brandList = document.getElementById("Brand_list");
        var brandImageList = document.getElementById("Brand_images");
        brandList.innerHTML = '';
        brandImageList.innerHTML = '';

        if (response.d != null) {
            var brands = response.d.split("#####");
            for (var i = 0; i < brands.length; i++) {
                AddBrandModelLi(brandList, brands[i], "1");
                AddBrandModelLiImage(brandImageList, brands[i], "1");
            }
            var t1 = document.getElementById("txtBrand");
            var t2 = document.getElementById("txtModel");

            GetModelData(t2, t1, 'H');
        }
    }


    function GetModelData(ctl2, ctl1, h) {

        var txt1v = ctl1.value;
        var tx2tv = ctl2.value;
        if (h != "H")
            ImgLoadBrand("2");


        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/FindMyModel",
            data: "{'strfindvalue':'" + tx2tv + "','strBrandvalue':'" + txt1v + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnModelSuccess,
            failure: function (response) {

                //alert(response.d);
            },
            error: function (response) {

                //alert(response.d);
            }
        });

    }
    function OnModelSuccess(response) {


        var Models;
        var ModelList = document.getElementById("Model_list");
        var ModelImageList = document.getElementById("Model_images");
        var t2 = document.getElementById("txtModel");
        ModelList.innerHTML = '';
        ModelImageList.innerHTML = '';
        t2.value = '';


        if (response.d != null) {
            var Models = response.d.split("#####");
            for (var i = 0; i < Models.length; i++) {
                AddBrandModelLi(ModelList, Models[i], '2');
                AddBrandModelLiImage(ModelImageList, Models[i], '2');
            }

        }
    }

    function OnclickBrandList1(xvalue) {
        var t1 = document.getElementById("txtBrand");
        var t2 = document.getElementById("txtModel");
        t1.value = xvalue;
      

        GetModelData(t2, t1, 'H');
    }
    function OnclickModelList2(xvalue) {
        var t1 = document.getElementById("txtBrand");
        var t2 = document.getElementById("txtModel");
        t2.value = xvalue;

    }
    function ChangeBrandTitle(xval) {

        if (xval == "1") {
            ImgLoadBrand("1");
            BrandImagetitle.innerHTML = "1. Brand List";
        }
        else {
            ImgLoadBrand("2");
            BrandImagetitle.innerHTML = "2. Model List";
        }
    }




    function AddBrandModelLi(BrandList, xvalue, onclk) {
        var v = xvalue.split("&&&&&");
        var li = document.createElement("li");
        li.setAttribute("class", "cableLi");

        if (onclk == '1') {
            li.setAttribute("onclick", "OnclickBrandList1('" + v[0] + "');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickBrandList1('" + v[0] + "');\" >" + v[0] + "</a>";
        }
        else {
            li.setAttribute("onclick", "OnclickModelList2('" + v[0] + "');");
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickModelList2('" + v[0] + "');\" >" + v[0] + "</a>";
        }

        BrandList.appendChild(li);
    }
    function AddBrandModelLiImage(BrandImageList, xvalue, onclk) {

        var v = xvalue.split("&&&&&");
        var li = document.createElement("li");

        li.setAttribute("class", "cableimageLi");

        if (onclk == '1')
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickBrandList1('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\" /><div>" + v[0] + "</div></a>";
        else
            li.innerHTML = "<a class=\"cablelink\" onclick=\"OnclickModelList2('" + v[0] + "');\"><img src='" + v[1] + "' alt=\"img\" style=\"max-width:100px;height:50px;\"  /><div>" + v[0] + "</div></a>";

        BrandImageList.appendChild(li);
    }


    function BrandRedirect(response) {
        if (response.d != null && response.d != "")
            window.location.href = response.d;

    }
    function GetBrandFinderURL() {

        var brand = "";
        var model = "";
        var f1;
        var f2;

        if (txtBrand.value != 'All Brand')
            brand = txtBrand.value;


        if (txtModel.value != 'All Model')
            model = txtModel.value;

        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/GetFindMyBrandURL",
            data: "{'Brand':'" + brand + "','Model':'" + model + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: BrandRedirect,
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });

    }
    
</script>--%>
<script type="text/javascript">
    function GetCableLData(strv) {
        var txtv = strv.value;
        var txt2 = "";
        var txtct = "";
        var cb1 = document.getElementById("txtcable1");
        var ct = document.getElementById("ConnectorType");
        

       

        if (ct != null) {
            if (ct.selectedIndex != -1)
                txtct = ct.options[ct.selectedIndex].text;
            else
                txtct = "Cables";
        }
        else {
            txtct = "Cables";
        }

        if (cb1 != null) {
            txt2 = cb1.value;
        }
        else {
            txt2 = "";
        }
        $.ajax({
            type: "POST",
            url: "ProductFinder.aspx/FindCableLeftRightImages",
            data: "{'strvalue':'" + txtv + "','strCable1value':'" + txt2 + "','strconnector_type':'" + txtct + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnCableLSuccess,
            failure: function (response) {

                //alert(response.d);
            },
            error: function (response) {

                //alert(response.d);
            }
        });

        //SetBtnclose();

    }

    function OnCableLSuccess(response) {

        var Cables;

        var CableImageList = document.getElementById("divCableLRImage");
        CableImageList.innerHTML = '';
        if (response.d != null) {

            CableImageList.innerHTML = response.d;
        }
    }


    function GetCableLRData(ctl2, ctl1, h) {

        var txt1v = ctl1.value;
        var tx2tv = ctl2.value;
        if (h != "H")
            ImgLoadCable("2");


        $.ajax({
            type: "POST",
            url: "GblWebMethods.aspx/FindCableLR",
            data: "{'strfindvalue':'" + tx2tv + "','strCable1value':'" + txt1v + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnCableLRSuccess,
            failure: function (response) {

                //alert(response.d);
            },
            error: function (response) {

                //alert(response.d);
            }
        });

        var t1 = document.getElementById("txtCable1");

        var t2 = document.getElementById("txtCable2");

        if (t1.value == "" && t2.value == "")
            YHS('', '')
        SetBtnclose();
    }
    function OnCableLRSuccess(response) {


        var Cables;
        
        var CableImageList = document.getElementById("divCableLRImage");        
        CableImageList.innerHTML = '';
        if (response.d != null) {

            CableImageList.innerHTML = response.d;
        }
    }
    function GotoTabs(strtab) {
 
        if (strtab == "1") {
            window.location.href = "/productfinder.aspx?tab=1";
        }
        else if (strtab == "2") {
            window.location.href = "/productfinder.aspx?tab=2";
        }
        else if (strtab == "3") {
            window.location.href = "/productfinder.aspx?tab=3";
        }
    }
</script>
<div class="container">
	<div class="profinder-wrapper">
    	<h2>Product Finder</h2>
    	<div class="profinder_tabs">
        	
        	<div class="tab-border">
        	<h3>Select Your Product Type</h3>
            </div>
        	<ul class="pftabs">
                <li>
                    
                  <input type="radio"  name="tabs" id="tab1" <%= (Request.QueryString["tab"] == null || Request.QueryString["tab"] == "1")? "checked":"" %> >
                <label for="tab1" class="pftab1" onclick="GotoTabs('1');">Cable and <br/>Adapters</label>
                   <div id="tab-content1" class="pftab-content animated fadeIn">
                   <% if (Request.QueryString["tab"] == null || Request.QueryString["tab"] == "" || Request.QueryString["tab"] == "" || Request.QueryString["tab"] == "1")
                      {
                       %>

                    <% if (Request.QueryString["l"] != null && Request.QueryString["l"] != "" && Request.QueryString["r"] != null && Request.QueryString["r"] != "" && Request.QueryString["ea"] != null && Request.QueryString["ea"] != "")
                       {
                            %>
                           <uc4:ProductFinderPL ID="ProductFinderPL1" runat="server" />                  
                            <%
                       }
                       else
                       {
                           %>
                             
                        
                               <div class="profind_cnt">
                                  <div class="pfclear"></div>
                        	<div class="search-panel">
                            	<div class="searchbox">

                                <div class="filter-select">
                                        <select onchange="this.options[this.selectedIndex].value &amp;&amp; (window.location = this.options[this.selectedIndex].value);" id="ConnectorType" style="width: 168px; position: absolute; opacity: 0; height: 38px; font-size: 17px;" class="filter-option hasCustomSelect">
                                            <option Value="/productFinder.aspx?tab=1&ct=Cables" <%=cblconnector_type=="Cables" ? "SELECTED":"" %> >Cables</option>
                                            <option Value="/productFinder.aspx?tab=1&ct=Adaptors" <%=cblconnector_type=="Adaptors" ? "SELECTED":"" %>>Adaptors</option>                           
                                        </select>
                      <span style="display: inline-block;" class="customSelect filter-option">
                        <span style="width: 152px; display: inline-block;" class="customSelectInner"><%=cblconnector_type%></span>
                      </span>
                    </div>

                                     
                                    
                                </div>
                            </div>
                         
                        	<div class="select-container pfclear" >
                            	
                           
                                       <%=GetCableLeft()%>
                            

                             
                                <div class="connection-line <%=((Request.QueryString["l"] != null && Request.QueryString["l"] != "") && (Request.QueryString["r"] == null || Request.QueryString["r"] == ""))? "selectarrow":"" %>">
                                        <div class="arobox">
                                            <img src="images/connection-aro.png" />
                                        </div>
                               		 </div>
                                
                                <%=GetCableRight()%>
                              
                           </div>
                            
                            <div class="search-panel">
                            	<div class="searchbox">
                                    <input type="Hidden"  id="txtcable1" value="<%= GetCable1Value()  %>" >
                                	<input type="text" placeholder="Quick Search"  id="txtcable" onkeyup="GetCableLData(txtcable);" >
                                    
                                </div>
                            </div>
                            <div class="progrid_wrapper" id="divCableLRImage">	
                            	
                                  <%=GetCableLeftRight() %>
                                

                            </div>
                            
                        </div>
                    <% }
                       %>

                       <% } %>
                        </div>
                   
                   </li>
                <li>
                  <input type="radio" <%= ( Request.QueryString["tab"] == "2")? "checked":"" %> name="tabs" id="tab2">
                 <label for="tab2" class="pftab2" onclick="GotoTabs('2');">Mobile Phone and<br />Wireless Data</label>
                   <div id="tab-content2" class="pftab-content animated fadeIn">
                      <% if (Request.QueryString["tab"] != null && Request.QueryString["tab"] != "" && Request.QueryString["tab"] == "2")
                         {
                       %>


                          <% if (Request.QueryString["l"] != null && Request.QueryString["l"] != "" && Request.QueryString["r"] != null && Request.QueryString["r"] != "" && Request.QueryString["ea"] != null && Request.QueryString["ea"] != "")
                       {
                            %>
                            <uc4:ProductFinderPL ID="ProductFinderPL2" runat="server" />                  
                            <%
                       }
                       else
                       {
                           %>

                           <div class="profind_cnt">
                        	
                        	<div class="select-container pfclear" >
                            	
                                    <%=GetLeftBrand()%>
                                     
                            

                             
                                <div class="connection-line <%=((Request.QueryString["l"] != null && Request.QueryString["l"] != "") && (Request.QueryString["r"] == null || Request.QueryString["r"] == ""))? "selectarrow":"" %>">
                                        <div class="arobox">
                                            <img src="images/connection-aro.png" />
                                        </div>
                               		 </div>
                                
                              <%=GetRightModel()%>
                              
                           </div>
                              <div class="marbtm30"></div>                      
                            <div class="progrid_wrapper" id="div1">	
                            	
                                <%=GetBrandModelImagelist()%> 
                                

                            </div>
                            
                        </div>



                             <%
                             }
                       %>


                       <%} %>
                       
                    
                  </div>
                  </li>
                <li>
                  <input type="radio" name="tabs" id="tab3" <%= (Request.QueryString["tab"] == "3")? "checked":"" %>   >
                  <label for="tab3" class="pftab3" onclick="GotoTabs('3');">Vechicle Specific<br /> Accessories</label>
                    <div id="tab-content3" class="pftab-content animated fadeIn">
                      <% if (Request.QueryString["tab"] != null && Request.QueryString["tab"] != "" && Request.QueryString["tab"] == "3")
                         {
                       %>


                          <% if (Request.QueryString["l"] != null && Request.QueryString["l"] != "" && Request.QueryString["r"] != null && Request.QueryString["r"] != "" && Request.QueryString["ea"] != null && Request.QueryString["ea"] != "")
                       {
                            %>
                            <uc4:ProductFinderPL ID="ProductFinderPL3" runat="server" />                  
                            <%
                       }
                       else
                       {
                           %>

                           <div class="profind_cnt">
                        	
                        	<div class="select-container pfclear" >
                            	
                                    <%=GetLeftVechicleBrand()%>
                                     
                            

                             
                                <div class="connection-line <%=((Request.QueryString["l"] != null && Request.QueryString["l"] != "") && (Request.QueryString["r"] == null || Request.QueryString["r"] == ""))? "selectarrow":"" %>">
                                        <div class="arobox">
                                            <img src="images/connection-aro.png" />
                                        </div>
                               		 </div>
                                
                              <%=GetRightVechicleModel()%>
                              
                           </div>
                              <div class="marbtm30"></div>                      
                            <div class="progrid_wrapper" id="div2">	
                            	
                                <%=GetVechicleBrandModelImagelist()%> 
                                

                            </div>
                            
                        </div>



                             <%
                             }
                       %>


                       <%} %>
                       
                            
                  </div>
                
                </li>                
        	</ul>       
        </div>    	
    </div>
</div>
<script>
	 $('#list').click(function(){$('#progrid_wrapper .product-items').addClass('list-group-item');});
	 $('#grid').click(function(){$('#progrid_wrapper .product-items').removeClass('list-group-item');});
	 <!-- grid and list view -->
	 $("#list").click(function () {
    $(this).addClass('lightblue');
	$('#grid').removeClass('lightblue');
    $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmlviewmode").val("LV");
    SetViewType('LV');
});
$('#grid').addClass('lightblue');
	 $("#grid").click(function () {
    $(this).addClass('lightblue');
	$('#list').removeClass('lightblue');
     $("#ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmlviewmode").val("GV");
     SetViewType('GV');
});
 <!-- grid and list view ends-->
 
</script>
<script language="javascript" type="text/javascript">

    function SetViewType(viewtype) {

        var type = viewtype;
        $.ajax({
            type: "POST",
            url: "ProductFinder.aspx/SetViewType",
            data: "{'viewtype':'" + type + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: false,
            success: OncartSuccess,
            error: OncartFailure

        });

    }

    function OnFailure(result) {
        alert("failure");
    }
    function OnSuccess(result) {
    }
</script>