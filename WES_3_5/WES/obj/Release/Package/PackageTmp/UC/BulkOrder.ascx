<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_BulkOrder" Codebehind="BulkOrder.ascx.cs" ViewStateMode="Enabled" %>
<%--<%@ Import Namespace="TradingBell.Common" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Import Namespace ="System.Data.OleDb" %> 
<%@ Import Namespace ="System.IO" %>  
<%@ Import Namespace ="System.Data" %>
<input id="HidItemCode1" name="HidItemCode1" clientidmode="Static" type="hidden" class="autosuggest" runat="server" value="" />
<input id="HidQty1" name="HidQty1" clientidmode="Static" type="hidden" runat="server" />
<input id="HidTarget" name="HidTarget" clientidmode="Static" type="hidden" runat="server" />
<input id="HidtxtCnt" name="HidtxtCnt" clientidmode="Static" type="hidden" runat="server" />
<input id="Hidtxtbulk" name="Hidtxtbulk" clientidmode="Static" type="hidden" runat="server" />
<input id="HidItemCode2" name="HidItemCode2" clientidmode="Static" type="hidden" runat="server" />
<input id="HidQty2" name="HidQty2" clientidmode="Static" type="hidden" runat="server" />
<asp:HiddenField ID="hidSourceID" runat="server" />
      <link rel="Stylesheet" href="css/thickboxNew.css" type="text/css" />
       <link rel="Stylesheet" href="../css/jquery-ui.css" type="text/css" />               
       <script language="javascript" src="Scripts/thickboxNew.js" type="text/javascript" />
       <script language="javascript" src="Scripts/jquery-Thickbox-New.js" type="text/javascript" />  
<script src="../Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    function Hidepopup() {
        $find('BtnItemClarify').hide();
    
    }
    var integer_only_warned = false;
    function integeronly(obj) {
        var value_entered = obj.value;
        if (!integer_only_warned) {
            if (value_entered.indexOf(".") > -1) {
                alert('Please enter an integer only. No decimal places.');
                integer_only_warned = true;
                obj.value = value_entered.replace(/[^0-9]/g, '');
            }
        }
        obj.value = value_entered.replace(/[^0-9]/g, '');
        
    }

    function dotransfer(eventTarget, eventArgument1, eventArgument2, eventArgument3) {
        document.getElementById("HidTarget").value = eventTarget;
        document.getElementById("HidItemCode1").value = eventArgument1;
        document.getElementById("HidQty1").value = eventArgument2;
        document.getElementById("HidtxtCnt").value = eventArgument3;
        //document.forms[0].submit();
    }

    function NoEnter() {
        if (window.event.keyCode == 13) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        }
    }

    function ResetDisabledControls() {
        var tb = document.getElementById("tblitembox");
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            document.forms[0].elements["txtitem" + i].disabled = false;
            document.forms[0].elements["txtqty" + i].disabled = false;
        }
        window.document.getElementById("ctl00_maincontent_ctl00_lnkbtnmore").disabled = false;
        document.getElementById("ctl00_maincontent_ctl00_txtCopyPaste").disabled = false;
    }

    function linkbtnclear() {
        var _isQtyAlert = true;
        var _isItemAlert = true;
        var i = 0;
        var tempqty = "";
        var tempitem = "";
        var txtcnt = 27;
        var chk = 1;
        var tb = document.getElementById("tblitembox");
        var txtcnt = "<%=intTxtCount%>";
        txtcnt = tb.rows.length;
        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                tempqty = tempqty + document.forms[0].elements["txtqty" + i].value + ",";
                tempitem = tempitem + document.forms[0].elements["txtitem" + i].value + ",";
            }
        }
        document.forms[0].elements["<%=HidItemCode2.ClientID%>"].value = tempitem;
        document.forms[0].elements["<%=HidQty2.ClientID%>"].value = tempqty;
    }

    function DisableLineEntry() {
        var tb = document.getElementById("tblitembox");
        txtcnt = tb.rows.length - 1;
        for (i = 1; i <= txtcnt; i++) {
            document.forms[0].elements["txtitem" + i].disabled = true;
            document.forms[0].elements["txtqty" + i].disabled = true;
        }
        document.forms[0].elements["ctl00_maincontent_ctl00_lnkbtnmore"].href = "#";
    }

    function DisableBulkEntry(SourceID) {
    }

    function dispmsg(SourceID) {
        var _isQtyAlert = true;
        var _isItemAlert = true;
        var i = 0;
        var tempqty = "";
        var tempitem = "";
        var txtcnt = 27;
        var chk = 1;
        var hidSourceID =document.getElementById("<%=hidSourceID.ClientID%>");
        hidSourceID.value = SourceID;     
        var tb = document.getElementById("tblitembox");
        var txtcnt = "<%=intTxtCount%>";
        txtcnt = tb.rows.length ;      
        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                tempqty = tempqty + document.forms[0].elements["txtqty" + i].value + ",";
                tempitem = tempitem + document.forms[0].elements["txtitem" + i].value + ",";
            }
            else if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                if (document.forms[0].elements["txtqty" + i].value.length == 0 || document.forms[0].elements["txtqty" + i].value == null || document.forms[0].elements["txtqty" + i].value == "0" || document.forms[0].elements["txtitem" + i].value.indexOf(',') != "-1") {
                    if (_isQtyAlert == true || _isQtyAlert == 'true') {
                        alert('Qty cannot be empty');
                        _isQtyAlert = false;
                    }

                    window.document.getElementById("txtitem" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).value = "";
                    window.document.getElementById("txtqty" + i).focus();
                    return false;
                }
            }
            else if (document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                if (document.forms[0].elements["txtitem" + i].value.length == 0 || document.forms[0].elements["txtitem" + i].value == 'Item#' || document.forms[0].elements["txtitem" + i].value == null || document.forms[0].elements["txtitem" + i].value.indexOf(',') != "-1") {
                    if (_isItemAlert == true || _isItemAlert == 'true') {
                        alert('Item# cannot be empty');
                        _isItemAlert = false;
                    }

                    window.document.getElementById("txtitem" + i).style.borderColor = "red";
                    window.document.getElementById("txtqty" + i).style.borderColor = "red";
                    window.document.getElementById("txtitem" + i).value = "";
                    window.document.getElementById("txtitem" + i).focus();
                    return false;
                }
            }
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value == "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                chk = 0;
            }
        }
        for (i = 1; i <= txtcnt; i++) {
            if (document.forms[0].elements["txtitem" + i].value != 'Item#' && ((document.forms[0].elements["txtqty" + i].value == '') || (document.forms[0].elements["txtqty" + i].value <= 0))) {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).value = "";
                return false;
            }
            if (document.forms[0].elements["txtqty" + i].value > 0 && document.forms[0].elements["txtitem" + i].value == 'Item#') {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
                return false;
            }
            if ((document.forms[0].elements["txtitem" + i].value != '' || document.forms[0].elements["txtitem" + i].value != null || document.forms[0].elements["txtitem" + i].length > 0) && (document.forms[0].elements["txtqty" + i].value > 0)) {
                window.document.getElementById("txtitem" + i).style.borderColor = "ActiveBorder";
                window.document.forms[0].elements["txtqty" + i].style.borderColor = "ActiveBorder";
            }

        }
        if (chk == 0) {
            alert('Item# and Qty cannot be empty');
            return false;
        }
        else if (tempitem == "" || tempqty == "") {
            var result = checkbulk();
            if (!result) {
                alert("Item# and Qty cannot be empty !");
                document.forms[0].elements["HidItemCode1"].value = "";
                document.forms[0].elements["HidQty1"].value = "";
            }
            return result;
        }
        else {
            dotransfer('BULKORDER', tempitem, tempqty, txtcnt);
            return true;

        }
    }
    function checkbulk() {
        var str = document.forms[0].elements["<%=txtCopyPaste.ClientID %>"].value;
        var i = 0;
        var chk = 0;
        var _ItemStatus = true;

        var myattr = new Array();
        if (str.length <= 0) {
            _ItemStatus = false;
            //return false;
        }

        myattr = str.split("\n");
        for (i = 0; i < myattr.length; i++) {
            if (myattr[i].length > 0) {
                myattr[i] = myattr[i].toString().replace("\t", " ");
                var myattr1 = new Array();
                if (myattr[i].toString().indexOf(",") >= 0) {
                    myattr1 = myattr[i].split(",");
                }
                else if (myattr[i].toString().indexOf(" ") >= 0) {
                    while (myattr[i].toString().indexOf("  ") > 0) {
                        myattr[i] = myattr[i].toString().replace("  ", " ");
                    }
                    myattr1 = myattr[i].split(" ");
                }
                else {
                    chk = 1;
                    break;
                }
                if (myattr1[1].length <= 0) {
                    alert("Qty cannot be empty !");
                    _ItemStatus = false;
                    //return false;
                }

                if (myattr1[0].length <= 0) {
                    alert("Item# cannot be empty !");
                    _ItemStatus = false;
                    //return false;
                }
                if (myattr1.length != 2) {
                    chk = 1;
                    break;
                }
            }
        }
        if (chk == 1) {
            alert("Qty cannot be empty !");
            _ItemStatus = false;
        } 
        return _ItemStatus;
    }
    function FillValue(ctl) {
        
        if (ctl.value == '' || ctl.value == null) {
            ctl.value = 'Item#';
        } 
    }
    function Focus(ctl) {
        SearchText(ctl);
        if (ctl.value == 'Item#') {
            ctl.value = '';
        }
        var txtbxID = ctl;
        var txtid = txtbxID.id;         
        if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            if (txtid == 'txtitem1') {
                $(".autosuggest").focusout(function (e) {
                   $('ul.ui-autocomplete').removeClass('autosuggest').addClass('ui-helper-hidden-accessible ');
               });

               $(".inputbackground").focus(function () {
                   $('ul.ui-autocomplete').removeAttr('style').hide();
               });
            }
        }

       
    }
    function Focus1(ctl) {
        var txtbxID = ctl;
        var txtid = txtbxID.id;
           if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            $(".autosuggest").focusin(function (e) {
                $('ul.ui-autocomplete').removeClass('ui-helper-hidden-accessible').addClass('test');
      
               });
        }

        if (ctl.value != '' || ctl.value != null || ctl.value != 'Item#') {
            $(".inputbackground").focusin(function (e) {
                $('ul.ui-autocomplete').removeClass('test').addClass('ui-helper-hidden-accessible ');
                    $('ul.ui-autocomplete').removeAttr('style').hide();
                    $('autosuggest').removeAttr('test').hide();
                    $('.autosuggest').removeClass("ui-autocomplete-loading");
                    $('.autosuggest').data = "";
                    $('.autosuggest').autocomplete('close');
                    $('ui-autocomplete ui-menu ui-widget ui-widget-content ui-corner-all').removeAttr('style').hide();
                    $('.ui-menu-item').removeAttr('style').hide();
                    $('.ui-corner-all').removeAttr('style').hide();
                    $('.ui-active-menuitem').removeAttr('style').hide();
                    $('.ui-helper-hidden-accessible').removeAttr('style').hide();
                    $('.ui-menu-item').hide();
                    $('.autosuggest').autocomplete("search", "");
                    $(".ui-autocomplete").css({ "display": "none" });
                    if (!e) e = window.event;
                    if (e.keyCode == '9') {
                        $('.autosuggest').autocomplete('close');
                        $('.autosuggest').autocomplete("search", "");
                        return false;
                    }
                

            });
       }
            function HideAutoCompleteHack() {
                $(".ui-autocomplete").hide();
            }              
    }
    function Check(Id) {
        
        var Qty = window.document.getElementById("txtqty" + Id).value;  //window.document.forms(0).elements("txtqty" + Id).value;
        var Code = window.document.forms(0).elements("txtitem" + Id).value;      
        if ((isNaN(Qty) && Code != "Item#") || (Qty <= 0 && Code != "Item#") || (Qty.indexOf(".") != -1 && Code != "Item#") || (Qty == "" && Code != "Item#")) {
            alert('Invalid Quantity!');
            window.document.getElementById("txtitem" + Id).style.borderColor = "red";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "red";
            window.document.getElementById("txtqty" + Id).value = "";
            window.document.getElementById("txtqty" + Id).focus();
            return false;
        }
        if (Code == '' || Code == 'Item#' || Code.length == 0 || Code == null) {
            alert('Invalid Item!');
            window.document.forms[0].elements["txtitem" + Id].style.borderColor = "red";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "red";
            window.document.getElementById("txtitem" + Id).value = "Item#";
            window.document.getElementById("txtitem" + Id).focus();
            return false;
        }
        if ((Code != '' || Code != null || Code.lenth > 0) && (Qty > 0)) {
            window.document.getElementById("txtitem" + Id).style.borderColor = "ActiveBorder";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "ActiveBorder";
        }
        if (document.getElementById("txtqty" + Id).focus == true) {
            alert("hshkdashk");
        }
        
    }
    function GetValues() {
        for (var i = 1; i <= 10; i++) {
            alert(document.forms[0].elements["txtQty" + i].value);
        }
    }
    function Here() {
        alert('this');
    }
</script>  
    <script type="text/javascript">
        $(document).ready(function () {
        });    
        function SearchText(ctl) {

            $(".autosuggest").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "BulkOrder.aspx/WestestAutoCompleteData",
                        data: "{'strvalue':'" + ctl.value + "'}",
                        dataType: "json",
                        success: function (data) {
                            $(".ui-autocomplete").css({ "display": "none" });
                            $('.autosuggest').removeClass("ui-autocomplete-loading");
                            $('.autosuggest').data = "";
                            $('.autosuggest').autocomplete('close');
                            $(".ui-autocomplete").hide();
                            $('.ui-autocomplete').css({ "display": "none" });
                            $("body").click(function () {
                                HideAutoCompleteHack();
                            });
                            $(document).keyup(function (e) {
                                if (e.keyCode == 9) { //esc
                                    HideAutoCompleteHack();
                                    $('.ui-autocomplete').autocomplete('close');

                                }
                            });
                            function HideAutoCompleteHack() {
                                $(".ui-autocomplete").hide();
                            }
                            $(".inputbackground").focus(function () {
                                $('ul.ui-autocomplete').removeAttr('style').hide();
                            });
                            $(document).ready(function () {
                                if ($.browser.msie) {
                                    HideAutoCompleteHack();
                                    $(".inputbackground").focus(function () {
                                        $('ul.ui-autocomplete').removeAttr('style').hide();
                                   });                               
                                }
                                else if ($.browser.mozilla) {
                                    $('ul.ui-autocomplete').removeAttr('style').hide();
                                }

                            });
                           response(data.d);
                            var txtbxID = ctl;
                            var txtid = txtbxID.id;
                            window.document.getElementById(txtid).setAttribute('class', 'autosuggest');
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                }
            });

        }
	</script>

        
    
<%
HelperServices objHelperServices = new HelperServices();
OrderServices objOrderServices = new OrderServices(); 
DataSet dstemplate = new DataSet();





if (Request.Url.ToString().ToLower().Contains("orderdetails.aspx"))
{
%>

<% 
}
else if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
{
%>
<table align="center" width="785" border="0" cellspacing="0" >
    <tr>
        <td align="left" >
            <div class="breadcrumb_outer1">
             <a href="home.aspx" style="float: left" class="toplinkatest" style="text-decoration:none!important;" >HOME >&nbsp;</a>
             <div class="breadcrumb1"> 
  <a href="OrderTemplate.aspx"  class="breadcrumb_txt1" style="text-transform:none;">ORDER TEMPLATE</a>
  <a href="home.aspx" class="breadcrumb_close1" >    
  </a>
</div>
</div>
</td>
</tr>
</table>
   <div runat="server" id="divclrify"  >
 <table width="760" id="BaseTable0" border="0" cellpadding="0" cellspacing="0" style="padding-left:0px;" >
                <%
                    //int DupProdCount = 0;
                    //string LeaveDuplicateProds = "";
                    //string url = "";
                    //if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                    //{
                    //    url = "ordertemplate.aspx";
                    //}
                    //else if (Request.Url.ToString().ToLower().Contains("bulkorder.aspx"))
                    //{
                    //     url="bulkorder.aspx";
                    //}
                    //if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
                    //{
                    //    if (Request["rma"] != null)
                    //    {
                    //        string _rma = Request["rma"].ToString();
                    //        string _rmitem = Request["Item"].ToString();
                    //        string _rmqty = "";
                    //        double CalItem_ID = 0;
                    //        if (Request.QueryString["DelQty"] != null)
                    //        {
                    //            _rmqty = Request["DelQty"].ToString();
                    //        }
                    //        if (Request.QueryString["cla_id"] != null)
                    //        {
                    //            CalItem_ID = Convert.ToDouble(Request["cla_id"].ToString());
                    //        }
                    //        if (_rma == "NF")
                    //        {
                                
                    //            objOrderServices.Remove_Clarification_item(CalItem_ID);  
                    //        }
                    //        if (_rma == "CI")
                    //        {
                                
                    //            objOrderServices.Remove_Clarification_item(CalItem_ID); 
                    //        }
                    //    }
            
                    //       int T_id = 0;
                    //        if (HttpContext.Current.Request.QueryString["Tempid"] != null)                            
                    //            T_id =  objHelperServices.CI(  HttpContext.Current.Request.QueryString["Tempid"].ToString());
                    //        else
                    //            T_id = 0;
                    //   // LeaveDuplicateProds = GetLeaveDuplicateProducts();

                    //    ///DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(OrderID, LeaveDuplicateProds);
                    //    DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(T_id, "TEMP_ITEM_ERROR");
                    //    DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(T_id, "TEMP_ITEM_CHK");
                                                


                    //    //DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(0, LeaveDuplicateProds);
                    //    //if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
                    //    //{                                                   
                    //    //    DupProdCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
                    //    //}
                                                
                    //    //if (Session["ITEM_ERROR"] != null && (Session["ITEM_ERROR"].ToString().Replace(",", "") != "" || Session["ITEM_CHK"].ToString() != "") ||( DupProdCount>0))
                    //    if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0) || (DupProdCount > 0))                                                    
                    //    {
                            %>
                                                <tr>
                        <td align="left" colspan="2" width="100%">
                          <div class="quickorder3">        
                            <H3 class="title2">Order Clarification/Errors</H3>
                            <table id="SiteMapTable0"  class="orderdettable">
                                  <tr>
                                    <td align="left" colspan="4" bgColor="#cccccc">
                                        <b>Please Check Below</b>
                                    </td>
                                </tr>
                                 <tr>
                                        <td  align="left" bgcolor="#f2f2f2"   >
                                            ITEMCODE
                                        </td>
                                        <td align="left" bgcolor="#f2f2f2" >
                                            CLARIFICATION REQUIRED
                                        </td>
                                        <td colspan="2" bgcolor="#f2f2f2" >
                                            &nbsp;
                                        </td>
                                    </tr>  
                         
                                   
                                                         <%// string TempNotFoundItem = "";
                                                           //string _NotFoundItem = "";
                                                           //string _ClaItem_ID = "0";
                                                           //if (tbErrorItem != null && tbErrorItem.Rows.Count>0)
                                                           //{

                                                           //    RepeaterError.DataSource = tbErrorItem;
                                                           //    RepeaterError.DataBind();
                                                               //foreach (DataRow RItem in tbErrorItem.Rows)
                                                               //{
                                                               //    _NotFoundItem = RItem["PRODUCT_DESC"].ToString();
                                                               //    _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                               //    if (_NotFoundItem.Trim() != "")
                                                               //    {
                                                               //        if (_NotFoundItem.Trim() != TempNotFoundItem.Trim())
                                                               //        {
                                                                           
                                                        %>
                                                              <asp:Repeater ID="RepeaterError" runat="server" OnItemCommand="RepeaterItemCommand_click">
                                                            <ItemTemplate>
                                                        <tr>
                                                            <td align="left" >
                                                                <%# Eval("PRODUCT_DESC")%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="red" style="font-weight: bold;">Not Found / Incorrect Code</font>
                                                            </td>
                                                            <td align="left">
                                                                <a href="#bulkorder" style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                                                    Please Re-Enter Below</a>
                                                            </td>
                                                            <td align="left">
                                                               <%-- <a href="<%=url %>?bulkorder=1&amp;Tempid=<%=T_id%>&amp;rma=NF&amp;item=<%=_NotFoundItem%>&amp;cla_id=<%=_ClaItem_ID%>" style="font-weight: bold; text-decoration: none; color: #1589FF;">Delete Item</a>--%>
                                                                <asp:LinkButton ID="lnkbtnDeleteError" style="font-weight: bold; text-decoration: none; color: #1589FF;"
                                                               OnClientClick="return dispmsg(this.id);" runat="server"  CommandName="NF" CommandArgument='<%# Eval("CLARIFICATION_ID") %>' >Delete Item</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        </ItemTemplate>
                                                                  </asp:Repeater>
                                                        <%
                                                            //}
                                                            //           TempNotFoundItem = _NotFoundItem;
                                                            //           }
                                                            //       }
                                                           //}
                                                          
                                                           //  string TempClarifyItem = "";
                                                           //string _ClarifyItem = "";
                                                           //Int32  _orderQty = 0;
                                                           
                                                           //if (tbErrorChk!=null && tbErrorChk.Rows.Count > 0)
                                                           //{
                                                           //    //foreach (DataRow RItem in tbErrorChk.Rows)
                                                           //    //{
                                                           //    //    _ClarifyItem = RItem["PRODUCT_DESC"].ToString();
                                                           //    //    _orderQty = Convert.ToInt32(RItem["QTY"].ToString());
                                                           //    //    _ClaItem_ID = RItem["CLARIFICATION_ID"].ToString();
                                                           //    //    if (_ClarifyItem.Trim() != "")
                                                           //    //    {
                                                           //    //        if (_ClarifyItem.Trim() != TempClarifyItem.Trim())
                                                           //    //        {

                                                           //    RepeaterClarify.DataSource = tbErrorChk;
                                                           //    RepeaterClarify.DataBind();    
                                                        %>
                                                          <asp:Repeater ID="RepeaterClarify" runat="server" OnItemCommand="RepeaterItemCommand_click" >
                                                            <ItemTemplate>
                                                        <tr>
                                                            <td align="left">
                                                                 <%# Eval("PRODUCT_DESC")%>
                                                            </td>
                                                            <td align="left">
                                                                <font color="#ff9900" style="font-weight: bold">Not unique Code</font>
                                                            </td>
                                                            <td align="left">
                                                              <%--  <a class="thickbox" href="SubProducts.aspx?Item=<%# Eval("PRODUCT_DESC") %>&amp;height=400&amp;width=600&amp;modal=true&amp;Tempid=<%# Eval("ORDER_ID") %>&amp;ClrQty=<%# Eval("QTY")%>&amp;cla_id=<%# Eval("CLARIFICATION_ID")%>&ordTemp=T"
                                                                     style="font-weight: bold; text-decoration: none; color: #1589FF;" onclick="">Clarify Now</a>--%>
                                                                    <asp:LinkButton ID="lnkbtnclarify" runat="server"  style="font-weight: bold; text-decoration: none; color: #1589FF;" 
                                                              OnClientClick="return dispmsg(this.id);"      CommandName="ItemCI"  CommandArgument='<%# Eval("CLARIFICATION_ID") +"," +Eval("QTY") +","+ Eval("PRODUCT_DESC") +","+ Eval("ORDER_ID") %>'>Clarify Now</asp:LinkButton>

                                                            </td>
                                                            <td align="left">
                                                            <%--    <a href="ordertemplate?bulkorder=1&amp;Tempid=<%=T_id%>&amp;rma=CI&amp;item=<%= _ClarifyItem %>&amp;DelQty=<%=_orderQty%>&amp;cla_id=<%=_ClaItem_ID%> "
                                                                    style="font-weight: bold; text-decoration: none; color: #1589FF;">Delete Item</a>
--%>
                                                                       
                                                                     <asp:LinkButton ID="lnkbtnDelete" runat="server"  style="font-weight: bold; text-decoration: none; color: #1589FF;" 
                                                                    OnClientClick="return dispmsg(this.id);" CommandName="CI"  CommandArgument='<%# Eval("CLARIFICATION_ID")%>'>Delete Item</asp:LinkButton>
                                                                  
                                                            </td>
                                                        </tr>
                                                         </ItemTemplate>
                                                                  </asp:Repeater>
                                                        <%  
                                                            
                                                               //        }

                                                               //        TempClarifyItem = _ClarifyItem;
                                                                       
                                                               //    }
                                                               //}
                                                          //  }                                                            
                                                        %>
                                             
                                                   </table>    
                            </div>
                            <br />
                        </td>
                    </tr>
                                        <%  // }
                                            //}
                                            //else
                                            //{
                                            //    Session["ITEM_ERROR"] = "";
                                            //   Session["ITEM_CHK"] = "";
                                            //   Session["QTY_CHK"] = "";
                                            //}
          
                                        %>
                                        </table>
                                        </div>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>

<%}
else
{
%>
<table align="center" width="785" border="0" cellspacing="0" >
    <tr>
        <td align="left" >
            <div class="breadcrumb_outer1">
             <a href="home.aspx" style="float: left" class="toplinkatest" style="text-decoration:none!important;" >HOME >&nbsp;</a>
             <div class="breadcrumb1">
  <a href="BulkOrder.aspx?txtcnt=27"  class="breadcrumb_txt1" style="text-transform:none;">Quick Order</a>
  <a href="home.aspx" class="breadcrumb_close1" >
  </a>
</div>
</div>
</td>
</tr>
</table>
  <%}  %>


<%
if (Request.Url.ToString().ToLower().Contains("orderdetails.aspx"))
{
%>
        <div class="box1" style="width:745px;">
       <h4 class="title3" style="text-align:left;">Add Addtional Items to Cart - Quick Order</h4>
<% }
else if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
{        
         %>
         <div class="box1" style="width:750px;">
<h4 class="title3" style="text-align:left;">Order Template</h4>
         <%
}
else
{        
         %>
         <div class="box1" style="width:750px;">
<h4 class="title3" style="text-align:left;">Quick Order</h4>
         <%
}
         %>
<P class=p3>Enter part numbers using either <STRONG>line entry fields</STRONG>, <STRONG>bulk entry field</STRONG>, or <STRONG>excel file upload</STRONG>.</P>
                         <% 
                             
if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
{
    dstemplate = GetOrderTemplateDetails();
    if (dstemplate != null && dstemplate.Tables.Count > 0 && dstemplate.Tables[0].Rows.Count > 0)
    {
        TxtTemplateName.Text = dstemplate.Tables[0].Rows[0]["TEMPLATE_NAME"].ToString();
        TxtDesc.Text = dstemplate.Tables[0].Rows[0]["NOTES"].ToString();
    }
    

                                    %>
                         <table cellpadding="0" cellspacing="0" border="0" width="750px">
                             <tr>
                                <td  align="left">
                              
                                 <div class="quickorder3">
                                   <H3 class="title1">Template Name / Notes</H3>   
                                     <table    cellpadding="2" cellspacing="2" border="0"   > 
          
                                            <tr>
                                            <td width="100px">
                                            Template Name :<span class="redx">*</span>
                                            </td>
                                            <td>
                                             <asp:TextBox  style="Width:200px;BackColor:White;" ID="TxtTemplateName" runat="server"  class="input_dr"></asp:TextBox>
                                       
                                            </td>
                                            <td>
                                             <asp:Label Width="150px" ID="txterr" runat="server" ForeColor="red" />
                                            </td>
                                            </tr>
                                             <tr>
                                            <td>
                                             Notes : 
                                            </td>
                                            <td>
                                             <asp:TextBox ID="TxtDesc"  runat="server"  Font-Size="12px" CssClass="input_dr" Width="400px"   Font-Names="arial"  MaxLength="50"  
                                            
                                             />
                                               
                                             
                                            </td>
                                            <td>
                                             
                                            </td>
                                            </tr>
               
                                            </table>          
                                    </div>
                                 
                                    </td>
                            </tr>
                         </table>
                         <Br />
                            <% 
                                 
//  if (dstemplate != null && dstemplate.Tables.Count > 0 && dstemplate.Tables[0].Rows.Count > 0)
// {
//   TxtTemplateName.Text= dstemplate.Tables[0].Rows[0]["TEMPLATE_NAME"].ToString();
//                                   TxtDesc.Text = dstemplate.Tables[0].Rows[0]["DESCRIPTION"].ToString();
//                              }
}
                                 %>
                        <table cellpadding="0" cellspacing="0" border="0">
                            
                            <tr>
                                <td  align="left">
                                <div class="quickorder1" style="margin-right: 4%;">
                                <h3 class="title1" style="text-align:left;">Product Line Entry
                                                </h3>
                                                <p class="p2">Please TAB Key to quickly jump to next field for faster entry </p>
                                                 <div class="QOordercode">
                                                <p class="p2"><strong>Order Code</strong></p>
                                              </div>
                                              <div class="QOqty">
                                                <p class="p2"><strong>Qty</strong></p>
                                              </div>

                                <%--    <table cellpadding="0" cellspacing="0" border="0" >                                        
                                        <tr>
                                            <td >--%>
                                                <%
//HelperServices objHelperServices = new HelperServices();
StringBuilder oStrCtrls = new StringBuilder();





int i = 1;
int txtCount = 10;
if (HidtxtCnt.Value.ToString() != "")
{
    txtCount = objHelperServices.CI(HidtxtCnt.Value.ToString());

    if (txtCount > 50)
    {
        txtCount = 50;
    }
}
DataSet ds = new DataSet();
    DataSet tmpds = new DataSet();
    string CtrlID = string.Empty;
    if (Request.Form[hidSourceID.UniqueID] != null && Request.Form[hidSourceID.UniqueID] != string.Empty)
    {
        CtrlID = Request.Form[hidSourceID.UniqueID];
    }
       
if (HttpContext.Current.Session["fileData"] != null)
{
    ds = (DataSet)HttpContext.Current.Session["fileData"];
    txtCount = ds.Tables[0].Rows.Count;
}
else if (HttpContext.Current.Session["linkmoredata"] != null)
{
    tmpds = (DataSet)HttpContext.Current.Session["linkmoredata"];
    txtCount = Convert.ToInt16(HttpContext.Current.Session["linkmoredatatxtcount"]);

}                                             
else if ((Request.Url.ToString().ToLower().Contains("ordertemplate.aspx") && (CtrlID.Contains("btnSaveTemplate") == true || CtrlID.Contains("lnkbtnDelete") == true|| CtrlID.Contains("lnkbtnclarify") == true )) ||  (Request.Url.ToString().ToLower().Contains("bulkorder.aspx") && CtrlID.Contains("btnSaveasTemplate") == true) )
{

ds = GetEntrySaveData();
if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
{
    txtCount = ds.Tables[0].Rows.Count;
    dstemplate = null;
}
    if (txtCount<=10)
        txtCount=10;
    else        
        txtCount = (int)(Math.Round(txtCount / 10.0) * 10);
    
}
else if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx") && dstemplate != null && dstemplate.Tables.Count > 0 && dstemplate.Tables[0].Rows.Count > 0)
{
    txtCount = dstemplate.Tables[0].Rows.Count;
    ds = null;
    if (txtCount <= 10)
        txtCount = 10;
    else
        txtCount = (int)(Math.Round(txtCount / 10.0) * 10);
}
       
                                                   
                                                    


                                                    
oStrCtrls.Append("<table id=\"tblitembox\" cellpadding=\"3\" cellspacing=\"10\" border=\"0\" style=\"vertical-align:top; \">");
//oStrCtrls.Append("<tr  class=\"tx_4table\"><td valign=\"top\"><strong>Order Code</strong></td><td><strong>Qty</strong></td></tr>");


for (i = 1; i <= txtCount; i++)
{
    string _code = "Item#";
    string _qty = "";

    if (ds != null && ds.Tables.Count > 0)
    {
        if (ds.Tables[0].Rows.Count >= i)
        {
            if (ds.Tables[0].Rows[i - 1][0].ToString() != "")
            {
                _code = ds.Tables[0].Rows[i - 1][0].ToString();
                _qty = ds.Tables[0].Rows[i - 1][1].ToString();
            }
        }
    }

    if (tmpds != null && tmpds.Tables.Count > 0)
    {
        if (tmpds.Tables[0].Rows.Count >= i)
        {
            if (tmpds.Tables[0].Rows[i - 1][0].ToString()!= "")
            {
                _code = tmpds.Tables[0].Rows[i - 1][0].ToString();
                _qty = tmpds.Tables[0].Rows[i - 1][1].ToString();
            }
        }
    }
    if (dstemplate != null && dstemplate.Tables.Count > 0)
    {
        if (dstemplate.Tables[0].Rows.Count >= i)
        {
            if (dstemplate.Tables[0].Rows[i - 1]["Pcode"].ToString() != "")
            {
                _code = dstemplate.Tables[0].Rows[i - 1]["Pcode"].ToString();
                _qty = dstemplate.Tables[0].Rows[i - 1]["Qty"].ToString();
            }
        }
    }
    //oStrCtrls.Append(System.Environment.NewLine + "<tr><td><input type=\"text\" class=\"autosuggest\"  name=\"itembox\" id=\"txtitem" + i + "\" style=\"width:200px\"  class=\"inputbackground\" value=\"" + _code + "\" onblur=\"FillValue(txtitem" + i + ")\" onfocus=\"Focus(txtitem" + i + ")\"></td><td><input type=\"text\" value=\"" + _qty + "\" name=\"qtybox\" id=\"txtqty" + i + "\" class=\"inputbackground\" onkeyup=\"integeronly(this)\" style=\"width:50px\" runat=\"server\" onBlur=\"javascript:return Check(" + i + ");\" onfocus=\"Focus1(txtqty" + i + ")\"></td></tr>" + System.Environment.NewLine);
    oStrCtrls.Append(System.Environment.NewLine + "<tr><td width=\"74%\">");
    oStrCtrls.Append("<input type=\"text\" class=\"autosuggest\"  name=\"itembox\" id=\"txtitem" + i + "\" style=\"width:94%\"  class=\"inputbackground\" value=\"" + _code + "\" onblur=\"FillValue(txtitem" + i + ")\" onfocus=\"Focus(txtitem" + i + ")\">");
    oStrCtrls.Append("</td>");
    oStrCtrls.Append("<td>");
    oStrCtrls.Append("<input type=\"text\" value=\"" + _qty + "\" maxlength=\"6\" name=\"qtybox\" id=\"txtqty" + i + "\" class=\"input_dr\" onkeyup=\"integeronly(this)\" style=\"width:90%\" runat=\"server\" onBlur=\"javascript:return Check(" + i + ");\" onfocus=\"Focus1(txtqty" + i + ")\">");
    oStrCtrls.Append("</tr>");

}

oStrCtrls.Append("</table>");
Response.Write(oStrCtrls.ToString());
                                                %>
                                           <%-- </td>
                                        </tr>
                                    </table>--%>
                                    <div align="left">
                                     <asp:LinkButton ID="lnkbtnmore" runat="server" Style="font-size: 11px;
                                        " OnClientClick="linkbtnclear();"  class="toplinkatest" OnClick="lnkbtnmore_Click">+ Add more fields</asp:LinkButton>
                                      <%--  <%  if (!Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                                            { %>
                                        <br /><asp:LinkButton ID="lnkBtnLoadTemplate" runat="server" Style="font-size: 11px;
                                        " OnClientClick="linkbtnclear();"  class="toplinkatest" OnClick="lnkBtnLoadTemplate_Click">+ Load from Template</asp:LinkButton>
                                        <%} %>--%>
                                        </div>
                                    </div>
                                
                                    <div class="quickorder1" >
                                     <h3 class="title1" style="text-align:left;">Product Bulk Entry</h3>
                                     <p class="p2">Copy & paste your order from your file into the space below or type in manually with one item per line. Code No. first followed by the required Qty, seperated by a space or comma.</p>
                                     <p>
                                     <textarea id="txtCopyPaste" name="txtCopyPaste" rows="0" cols="38" runat="server" style="width:320px; height:280px" ></textarea>
                                     </p>                                  
                                    </div>
                                    
                                    
                             
                                    <br/>
                                </td>
                            </tr>
                            <tr>                            
                            <td align="left">
                            <br />
                                   <div class="quickorder3">
                                    <H3 class="title1">Order File Upload</H3>                                    
                                    <P class="p2">Upload your excel file for quick order. Enter the Order Code in column "A" and quantity in column "B".</P>
                                    <p class="pad10" style="padding:0px 0px;">  
                                      <asp:LinkButton id="LinkButton2" 
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                 OnClick="LinkButton_Click"  runat="server" class="toplinkatest" > 
                                                 <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:285px;">Click Here to Download example Excel upload order sheet</p></div>     
                                                       <%-- <div class="toplinkatestbulk"><p style="margin: 0 0 0 21px;width:282px;">Click Here to Download example Excel upload order sheet</p></div>                     --%>
                                                </asp:LinkButton>
                                    </p>
                                    <div>
                                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="inputtxtfile"   /> 
                                    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" class="inputupload" style="height:23px;margin-left:0px;border:'1'px solid #465C71;font-family:Arial;"/>
                                    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
                                    </div>
                                    </div>
                            </td>
                            </tr>


                           <%--  <tr>
                                <td >            
                                
                                   
                                        <p align="left" Style="font-family: Arial; font-size: 11px; margin:0; color:#00AFFF;">
                                       
                                       <img src="images/Icon_Excel.png" alt="" />
                                        <asp:LinkButton id="LinkButton1"  
                                                Text="Click Here to Download example Excel upload order sheet" 
                                                Font-Names="Arial" Font-Size="11px"  OnClick="LinkButton_Click"  runat="server" 
                                                ForeColor="#00AFFF"/>
                                         
                                       </p> 
                                       <br />                                           
                                    
                                </td>
                                                               
                            </tr>
                          --%>
                         
                            <tr>
                            <td  align="left">
                                <asp:RegularExpressionValidator  id="RegularExpressionValidator1" runat="server"  ErrorMessage="Invalid File Type.Only xls, xlsx or csv files are allowed!"  ValidationExpression="^.+(.xls|.XLS|.xlsx|.XLSX|.csv|.CSV)$"   ControlToValidate="FileUploadControl"></asp:RegularExpressionValidator>
                            
                               </td>
                            </tr>
                           

                            <tr>
                              
                                <td align="center" width="100%">
                                        <%
if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
{ %>
                                                                                               
                                                      <asp:Button ID="btnSaveTemplate" Text="Save Template" runat="server" OnClick="BtnSaveOrderTemplate_Click"  
                                                     OnClientClick="return dispmsg(this.id);" class="button normalsiz btngreen btnmain" />
                                          <% }
else
{ %>
                                     <asp:Button ID="btnAddCart1" Text="Add to Cart" runat="server" 
                                                     OnClientClick="return dispmsg(this.id);"  OnClick="BtnAddtoCart_Click"  class="button normalsiznew btngreen btnmain"  />
                                                     <%      if (Request.Url.ToString().ToLower().Contains("bulkorder.aspx"))
                                                             { %> 
                                                      <asp:Button ID="btnSaveasTemplate" Text="Save as New Template" runat="server"  OnClick="btnSaveasOrdTemplate_Click"
                                                     OnClientClick="return dispmsg(this.id);" class="buttongray normalsiznew btngray"  />
                                                     <%}%>
                                            <%} %>
                                                <asp:Button ID="btnResetCart" Text="Reset" runat="server" Style="width: 100%; height: 40px;"
                                                    OnClick="btnResetCart_Click" Visible="false" />
                                    
                                </td>
                              
                            </tr>
                        </table>
              
</div>
                    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>
                <div id="divOrderTemplate">
                    <asp:Panel ID="plnOrderTemplate" runat="server" Style="display: none" BackColor="White"
                        Height="150px" Width="550px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#b81212">                        
                          <H3 class="title1">Template Name / Notes</H3>   

                                     <table  width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold;" align="center"  > 
          
                                            <tr>
                                            <td width="120px">
                                            Template Name :<span class="redx"> *</span>
                                            </td>
                                            <td>
                                             <asp:TextBox  style="Width:200px;BackColor:White;" ID="TxtdivTemplateName" runat="server"  MaxLength="50" class="input_dr"></asp:TextBox>
                                       
                                            </td>
                                            <td>
                                             <asp:Label Width="150px" ID="lbldiverror" runat="server" ForeColor="red" />
                                            </td>
                                            </tr>
                                             <tr>
                                            <td>
                                             Notes : 
                                            </td>
                                            <td>
                                             <asp:TextBox ID="TxtdivDesc"  runat="server"  Font-Size="12px" CssClass="input_dr" Width="200px"  Font-Names="arial"  MaxLength="50"/>                                                                                           
                                            </td>
                                            <td>                                             
                                            </td>
                                            </tr>
                                             <tr>
                                            <td colspan="3"  align ="center" height="5px">
                                            </td>
                                            </tr>
                                            <tr>
                                            <td colspan="3"  align ="center">
                                            <asp:Button ID="btnOTSave" OnClick="btnSaveOrdTemplate" runat="server" Text="Save" Width="55px" Font-Bold="true" ForeColor="#1589FF" />
                                             <asp:Button ID="btnOTClose" runat="server" OnClick="btnOTClose_click" Text="Cancel" Width="55px" Font-Bold="true" ForeColor="#1589FF" />                                              
                                            </td>
                                            </tr>
               
                                            </table>         
                    </asp:Panel>
                </div>
               <div id="DivAlert">
                    <asp:Panel ID="pnlAlert" runat="server" Style="display: none" BackColor="White"
                        Height="100px" Width="350px" BorderStyle="Solid" BorderWidth="2px" BorderColor="#0077cc">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                            font-family: Arial; font-size: 12px; font-weight: bold; color: #FF0000;" align="center">
                            <tr style="height: 15px">
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <td width="5%">
                                    &nbsp;
                                </td>
                                <td width="80%" align="center">
                                    
                                    <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                                </td>                               
                                <td width="5%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                             <td colspan="3" align="center">
                                  <asp:Button ID="btnok" runat="server" Text="Ok" Width="55px" Font-Bold="true" OnClick="btnOTClose_click"
                                        ForeColor="#1589FF" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

                <div id="DivItemClarify">
<%--                       <asp:ImageButton ID="IBClose" runat="server" class="testbutton" OnClick="IBClose_Click"  />--%>
                        
                    <asp:Panel ID="PnlItemClarify" runat="server" Style="display: none;" BackColor="White"
                        Height="400px" Width="600px" BorderStyle="Solid" BorderWidth="1px" BorderColor="#0077cc" >
                        <a href = "javascript:Hidepopup()" class="testbuttonBO" ></a>
                       <div  style="width:600px;height:400px;overflow:scroll;">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                      <h3 class="titleblue"><b>Order Clarification/Errors</b></h3>
                                         <table width="100%" cellspacing="0" cellpadding="5">                                            
                                            <tr>
                                                <td colspan="4">
                                                    Product Item Clarification: <font color="red"><strong>                                                      
                                          <asp:Label ID="lblitemClarify" runat="server" Text=""></asp:Label></strong></font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    Please select an item from below to update order with:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%" class="tx_bohead1">
                                                    &nbsp;
                                                </td>
                                                <td width="15%" class="tx_bohead1">
                                                    Code
                                                </td>
                                                <td width="40%" class="tx_bohead1">
                                                    Description
                                                </td>
                                                <td width="35%" class="tx_bohead1">                
                                                    Add to Order Template                
                                                </td>
            
                                            </tr>
                                               <asp:Repeater ID="RepeaterItemClarify" runat="server" OnItemCommand="RepeaterItemCommand_click" >
                                                <ItemTemplate>
                                           <tr>
                                           <td>
                                           <img class="tx_img" src='<%# GetFilePath(Eval("IMG").ToString()) %>'/>
                                           </td>
                                           <td><%#Eval("CODE") %></td>
                                           <td><%#Eval("DESC") %></td>
                                           <td class="">
                                          <%-- <a href="OrderTemplate.aspx?pid=6141&amp;Qty=1&amp;bulkorder=1&amp;rma=CI&amp;DelQty=1&amp;cla_id=116&amp;Item=dvd&amp;Tempid=90"
                                            style="font-weight: bold; text-decoration: none; color: #1589FF;">
                                            Update Order Template</a>--%>
                                               <asp:LinkButton ID="lnkbtnclarifyInsert" runat="server"  style="font-weight: bold; text-decoration: none; color: #1589FF;" 
                                                         OnClientClick="return dispmsg(this.id);"             CommandName="CIInsert"  CommandArgument='<%# Eval("PRODUCT_ID")+ ","+ Eval("CIQty") +","+ Eval("TEMP_ID") +","+ Eval("CLARIFICATION_ID") +","+ Eval("CODE") %>'>Update Order Template</asp:LinkButton>

                                            </td>
                                           </tr>
                                              </ItemTemplate>
                                         </asp:Repeater>
                                           </table>
                                    </td>
                             </tr>
                         </tbody>
                      </table>
                      </div>
                    </asp:Panel>
                </div>