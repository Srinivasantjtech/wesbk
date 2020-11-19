<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_ProductFilter" Codebehind="ProductFilter.ascx.cs" ViewStateMode="Enabled" %>
<%--<%@ Import Namespace="TradingBell.Common" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Import Namespace ="System.Data.OleDb" %> 
<%@ Import Namespace ="System.IO" %>  
<%@ Import Namespace ="System.Data" %>
<%--<script type="text/javascript" src="../scripts/jquery-1.4.1.min.js"></script>--%>
<script type="text/javascript" src="../scripts/jquery-1.10.2.min.js"></script>
<script src="/Scripts/AC_RunActiveContent.js" type="text/javascript"></script>
  <script src="Scripts/Productfilter.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>

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
    
</script>

<div class="westabouter_dummy">
	<div class="westabouter">
		<div class="westabtitle">Product Fast Finder</div>
		<div id="horizontalTab" class=box1>
			<ul>
				<li><a href="#tab-1">Cable & Adaptor Finder</a> </li>
				<li><a href="#">Battery Finder </a> </li>
				<li><a href="#tab-3" onclick="onTabClick('3');" >Mobile Phone Accessory Finder</a></li>
			</ul>
			<div id="tab-1">
				<div class="leftpanel">
                  
  
                    


    <div id="Cableyhs" style="display:none;">
                        

                    </div> 
   
  


					<div class="panelouter" onmousemove="ChangeCableTitle('1');" >
						<div class="paneltitle">1. First Connection Side</div>
						<div class="panelcontant">
							<div class="searchouter" onmouseover="ShowCableDropDown('1');"   >
								
							</div>
                           
						</div>
					</div>
					<div class="panelouter" onmousemove="ChangeCableTitle('2');">
						<div class="paneltitle">2. Second Connection Side</div>
						<div class="panelcontant">
							<div class="searchouter"  onmouseover="ShowCableDropDown('2');" >
								
							</div>
                             
						</div>
					</div>
                     
                    <a  runat="server"  class="button normalsiz btngreen"  style="width: 248px; line-height: 2.5; text-align: center; font-weight: bold;" onclick="GetCableFinderURL();"  > Find My Cable</a>
				</div>
				<div class="rightpanel">
					<div class="panelouter">
						                     
					</div>
				</div>
				<div class="cl"></div>
			</div>
			<div id="tab-2">
				<p> </p>
			</div>
			<div id="tab-3">
				<div class="leftpanel">
					<div class="panelouter" onmousemove="ChangeBrandTitle('1');" >
						<div class="paneltitle">1. Brand List</div>
						<div class="panelcontant">
							<div class="searchouter">
								<input name="" type="text" id="txtBrand" class="saerchinput" placeholder="Search..."  onclick="ChangeBrandTitle('1');"  onkeyup="GetBrandData(txtBrand);"   />
								<%--<input name="" type="button" class="searchbtn" />--%>
							</div>
							<ul class="list01" id ="Brand_list">
								
							</ul>
						</div>
					</div>
					<div class="panelouter" onmousemove="ChangeBrandTitle('2');">
						<div class="paneltitle">2. Model List</div>
						<div class="panelcontant">
							<div class="searchouter">
								<input name="" type="text" id="txtModel" class="saerchinput" placeholder="Search..." onclick="ChangeBrandTitle('2');" onkeyup="GetModelData(txtModel,txtBrand,'');" />
								<%--<input name="" type="button" class="searchbtn" />--%>
							</div>
							<ul class="list01" id ="Model_list">
								
							</ul>
						</div>
					</div>
                    <a id="A1"  runat="server"  class="button normalsiz btngreen"  style="width: 248px; line-height: 2.5; text-align: center; font-weight: bold;" onclick="GetBrandFinderURL();"  > Find My Brand & Model</a>
				</div>
				<div class="rightpanel">
					<div class="panelouter">
						<div class="paneltitle" id="BrandImagetitle" >1. Brand List</div>
						<div class="panelcontant">
							<ul class="productslist" id ="Brand_images">								
								<div class="cl"></div>
							</ul>
                            <ul class="productslist" id ="Model_images">								
								<div class="cl"></div>
							</ul>
						</div>                        
					</div>
				</div>
				<div class="cl"></div>
			</div>
		</div>
	</div>
</div>
<div class="container">
	<div class="profinder-wrapper">
    	<h2>Product Finder</h2>
    	<div class="profinder_tabs">
        	
        	<div class="tab-border">
        	<h3>Select Your Porduct Type</h3>
            </div>
        	<ul class="tabs">
                <li>
                  <input type="radio" checked name="tabs" id="tab1">
                  <label for="tab1" class="tab1">Cable and <br/>Adapters</label>
                   <div id="tab-content1" class="tab-content animated fadeIn">
                        <div class="profind_cnt">
                        	
                        	<div class="select-container clear">
                            		<div class="selectfirst" onmouseout="HideCableDropDown('1');">
                                <div class="productsearch"  >
                                	    <div class="numbox">
                                    	    1                                        
                                        </div>
                                        <span id="spncablelint" ><strong>Select FIRST Cable  Side From Bellow..</strong></span>
                                        <span class="darkgray" style="float:left;display:none;" id="spncablel" >First Cable Side</span>
                                        <span style="display:none;" id="spncablelH" ><h3>Apple</h3></span>
                                        <div class="select-wrap" onmousemove="ChangeCableTitle('1');" > 
                                               <input name="" type="text" id="txtCable1" class="saerchinput" placeholder="Search..."  onclick="ChangeCableTitle('1');ShowCableDropDown('1');"  onkeyup="GetCableLData(txtCable1);ShowCableDropDown('1');"  onblur="ShowCableDropDown('1');"   autocomplete="off"  />
								            <input name="" type="button" id ="btncloseCable1" class="FilterClosebtn" onclick="removetextCable('1');"  />
                                             <div class="DrpdownLi" id="divCableLi1" >
							            <ul class="list01" id ="Cable_1">								
							            </ul>

                                        </div>
                                        </div>
                                      
                                    </div>
                                      <div class="searched_image" id="cable1_selectedimage" >
                                    	    
                                            </div>
                                </div>
                                
                                <div class="connection-line">
                                	<div class="arobox">
                                        <img src="images/connection-aro.png">
                                    </div>
                                </div>
                                
                                <div class="selectsecond" onmouseout="HideCableDropDown('2');">
                                     	<div class="productsearch">
                                	<div class="numbox">
                                    	2
                                    </div>
                                    <span  id="spncableRint"><strong>Select SECOND Cable Side From Bellow..</strong></span>
                                      <span class="darkgray" style="float:left;display:none;" id="spncableR" >First Cable Side</span>
                                        <span style="display:none;" id="spncableRH" ><h3>Apple</h3></span>
                                    <div class="select-wrap">
                                           <input name="" type="text" id="txtCable2" class="saerchinput" placeholder="Search..." onclick="ChangeCableTitle('2');ShowCableDropDown('2');" onkeyup="GetCableLRData(txtCable2,txtCable1,'');ShowCableDropDown('2');" onblur="ShowCableDropDown('2');"  autocomplete="off" />
								        <input name="" type="button" id ="btncloseCable2" class="FilterClosebtn" onclick="removetextCable('2');"  />
                                        <div class="DrpdownLi" id="divCableLi2" >
							        <ul class="list01" id ="Cable_2">
								
							        </ul>
                                    </div>
                                    </div>
                                    </div>
                                  <div class="searched_image" id="cable2_selectedimage">
                                    	
                                    </div>
                                </div>
                            </div>
                            
                            
                            <div class="progrid_wrapper">
                                <div class="paneltitle" id="CableImagetitle" style="display:none;" >1. First Connection Side</div>
						       
							        <div class="productslist" id ="Cable1_images">								
								        <div class="cl"></div>
							        </div>
                                    <div class="productslist" id ="Cable2_images">								
								        <div class="cl"></div>
							        </div>
						       
                            	<%--<div>
                                    <div class="pro_grid">
                                        <img src="images/cable-1.jpg">
                                        <h5>HDMI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-2.jpg">
                                        <h5>DVI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-3.jpg">
                                        <h5>VGA</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-4.jpg">
                                        <h5>S-VIDEO</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-5.jpg">
                                        <h5>RCA</h5>
                                    </div>
                               
                                
                                
                               
                                    <div class="pro_grid">
                                        <img src="images/cable-1.jpg">
                                        <h5>HDMI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-2.jpg">
                                        <h5>DVI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-3.jpg">
                                        <h5>VGA</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-4.jpg">
                                        <h5>S-VIDEO</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-5.jpg">
                                        <h5>RCA</h5>
                                    </div>
                                
                                
                                
                                
                                    <div class="pro_grid">
                                        <img src="images/cable-1.jpg">
                                        <h5>HDMI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-2.jpg">
                                        <h5>DVI</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-3.jpg">
                                        <h5>VGA</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-4.jpg">
                                        <h5>S-VIDEO</h5>
                                    </div>
                                    <div class="pro_grid">
                                        <img src="images/cable-5.jpg">
                                        <h5>RCA</h5>
                                    </div>
                                </div>--%>

                            </div>
                            
                        </div>
                   </div>
                   </li>
                <li>
                  <input type="radio" name="tabs" id="tab2">
                  <label for="tab2" class="tab2">Mobile Phone and<br />Wireless Data</label>
                   <div id="tab-content2" class="tab-content animated fadeIn">
                    
 
                    
                  </div>
                  </li>
                <li>
                  <input type="radio" name="tabs" id="tab3">
                  <label for="tab3" class="tab3">Vechicle Specific<br /> Accessories</label>
                    <div id="tab-content3" class="tab-content animated fadeIn">

                            
                  </div>
                  </div>
                </li>                
        	</ul>       
        </div>    	
    </div>
<script>
	 $('#list').click(function(){$('#progrid_wrapper .product-items').addClass('list-group-item');});
	 $('#grid').click(function(){$('#progrid_wrapper .product-items').removeClass('list-group-item');});
	 <!-- grid and list view -->
	 $("#list").click(function () {
    $(this).addClass('lightblue');
	$('#grid').removeClass('lightblue');
});
$('#grid').addClass('lightblue');
	 $("#grid").click(function () {
    $(this).addClass('lightblue');
	$('#list').removeClass('lightblue');
});
 <!-- grid and list view ends-->
 
</script>