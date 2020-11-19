<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" EnableEventValidation="false"
    AutoEventWireup="true"  Inherits="QuoteCart"
    Title="Untitled Page" Codebehind="QuoteCart.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Register Assembly="GCheckout" Namespace="GCheckout.Checkout" TagPrefix="GCCheckout" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="pnlEnter" runat="server">
        <script type="text/javascript" language="javascript">
            var pid = "";
            var UntPrice = 0;
            var ProdPrice = "";
            var pgValidated = "no";
            if (window.history.forward(1) != null)
                window.history.forward(1);
            function PopupAdd(FamilyID, ProductID, MinQty, MaxQty, refocus) {
                var Qty = window.open("ProductFeatures.aspx?Page=QUOTE&Fid=" + FamilyID + "&Pid=" + ProductID, "Qty", "width=350,height=250,left=150,top=200,resizable=yes,scrollbars=1");
                if (Qty >= MinQty) {
                    alert(document.forms[0].elements["Returnname"].value);
                    var href = window.parent.location = "QuoteCart.aspx?Pid=" + ProductID + "&Qty=" + document.forms[0].elements["Returnname"].value;
                }
            }
            function EditText() {
                alert('Invalid Quantity!');
                //window.document.getElementById("txtQty") 
                document.forms[0].cheText.disabled = "false";
            }
            function BuildValue(Productid, Qty1, Pprice, Id) {
                var rst;
                if (Id.checked) {
                    pid = pid + "," + Productid;
                    ProdPrice = ProdPrice + "," + Pprice;
                    UntPrice += Pprice;
                    var elem = document.forms[0].elements
                    var c = 0;
                    for (var i = 0; i < elem.length; i++) {
                        var ObjName = "Chk" + i;
                        if (elem[i].type == "checkbox") {
                            if (elem[i].name == "Chk" + c) {
                                c += 1
                            }
                        }
                    }

                    var ObjChkStatus = 0;
                    for (var j = 0; j < c; j++) {
                        var Oname = "Chk" + j;
                        if (document.forms[0].elements[Oname].checked == true) {
                            ObjChkStatus += 1;
                        }
                    }
                    if (c == ObjChkStatus) {
                        document.forms[0].elements["ChkAllSel"].checked = true;
                    }
                }
                else {
                    pid = pid.replace("," + Productid, "");
                    ProdPrice = ProdPrice.replace("," + Pprice, "");
                    if (UntPrice > 0) {
                        UntPrice = UntPrice - Pprice;
                    }
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        document.forms[0].elements["ChkAllSel"].checked = false;
                    }
                }

            }
            function Send() {
                window.location.href = "orderdetails.aspx";

                if (pid != "") {
                    pid = pid.substr(1, pid.length);
                }
                else {
                    pid = 'AllProd';
                    UntPrice = '0';
                    ProdPrice = "";
                }
                window.location.href = 'orderdetails.aspx?&bulkorder=1&SelPid=' + pid + '&SelProdPrice=' + UntPrice;

            }


            function Check(Id, MQty, MinQty, PreQty) {
                var Qty = window.document.forms[0].elements["txtQty" + Id].value;
                if (isNaN(Qty) || Qty == "" || Qty <= 0 || Qty.indexOf(".") != -1) {
                    alert('Invalid Quantity!');
                    window.document.forms[0].elements["txtQty" + Id].style.borderColor = "red";
                    window.document.forms[0].elements["txtQty" + Id].value = PreQty;
                    window.document.forms[0].elements["txtQty" + Id].focus();
                    return false;
                }
                else if (parseInt(Qty) > parseInt(MQty)) {
                    alert('Quantity entered exceeds Maximum limit.');
                    window.document.forms[0].elements["txtQty" + Id].style.borderColor = "red";
                    window.document.forms[0].elements["txtQty" + Id].value = PreQty;
                    window.document.forms[0].elements["txtQty" + Id].focus();
                    return false;
                }
                else if (parseInt(Qty) < parseInt(MinQty)) {
                    alert('Minimum quantity for this product is :' + MinQty);
                    window.document.forms[0].elements["txtQty" + Id].style.borderColor = "red";
                    window.document.forms[0].elements["txtQty" + Id].value = PreQty;
                    window.document.forms[0].elements["txtQty" + Id].focus();
                    return false;
                }
            }


            function SendRemoveProducts() {
                if (String(pid) != "") {
                    pid = pid.substr(1, pid.length);
                    ProdPrice = ProdPrice.substr(1, ProdPrice.length);
                    window.location.href = "quotecart.aspx?SelPid=" + pid + "&SelProdPrice=" + UntPrice + "&ProdPrice=" + ProdPrice;
                    return true;
                }
                else {
                    UntPrice = 0;
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        pid = "AllProd";
                        SelProdPrice = 0;
                        ProdPrice = "";
                        window.location.href = "quotecart.aspx?SelPid=AllProd";
                        return true;

                    }
                    else {

                        var elem = document.forms[0].elements;
                        var c = 0;
                        //Get the check box count 
                        for (var i = 0; i < elem.length; i++) {
                            if (elem[i].type == "checkbox") {
                                if (elem[i].name == "Chk" + c) {
                                    c += 1;
                                }
                            }
                        }
                        for (var j = 0; j < c; j++) {
                            var Oname = "Chk" + j;
                            if (document.forms[0].elements[Oname].checked == true) {
                                pid = pid + "," + parseInt(document.forms[0].elements[Oname].value);
                                UntPrice += parseFloat(document.forms[0].elements["txtPrdTprice" + j].value);
                                ProdPrice = ProdPrice + "," + parseFloat(document.forms[0].elements["txtPrdTprice" + j].value);
                            }
                        }
                        if (String(pid) != "" && document.forms[0].elements["ChkAllSel"].checked == false) {
                            pid = pid.substr(1, pid.length);
                            ProdPrice = ProdPrice.substr(1, ProdPrice.length);
                            window.location.href = "Quotecart.aspx?SelPid=" + pid + "&SelProdPrice=" + UntPrice + "&ProdPrice=" + ProdPrice;
                            return true;
                        }
                    }
                }
            }
            function DelayR() {
                setTimeout('SendRemoveProducts()', 6000);
            }
            function CheckSelectAll() {
                var elem = document.forms[0].elements
                var c = 0;
                for (var i = 0; i < elem.length; i++) {
                    var ObjName = "Chk" + i;
                    if (elem[i].type == "checkbox") {
                        if (elem[i].name == "Chk" + c) {
                            c += 1
                        }
                    }
                }
                for (var j = 0; j < c; j++) {
                    var Oname = "Chk" + j;
                    if (document.forms[0].elements["ChkAllSel"].checked == true) {
                        //   alert("all recort select");
                        if (document.forms[0].elements[Oname].checked == false) {
                            document.forms[0].elements[Oname].checked = true;
                        }
                        pid = "";
                        ProdPrice = "";
                    }
                    else {
                        document.forms[0].elements[Oname].checked = false;
                        UntPrice = 0;
                        ProdPrice = "";
                    }
                }
            }
        </script>
        <%
            HelperServices oHelper = new HelperServices();
            ErrorHandler objErrorHandler = new ErrorHandler();
            OrderServices objOrderServices = new OrderServices();
            ProductServices oProd = new ProductServices();
            //ProductFamily oProdFam = new ProductFamily();
            DataSet dsOItem = new DataSet();
            QuoteServices oQuote = new QuoteServices();
            //Order.OrderInfo ordOrder=new Order.OrderInfo();
            int QuoteID = 0;
            int Userid;
            int ProductId;
            decimal subtot = 0.00M;
            decimal tax;
            decimal taxamt = 0.00M;
            decimal Total = 0.00M;
            string SelProductId = "";
            string QuoteStatus = "";
            int OpenQuoteStatusID = (int)QuoteServices.QuoteStatus.OPEN;
            Userid = oHelper.CI(Session["USER_ID"].ToString());
            if (oQuote.GetQuoteID(Userid, OpenQuoteStatusID) != 0)
                QuoteID = oQuote.GetQuoteID(Userid, OpenQuoteStatusID);
            else if (Session["QuoteID"] != null)
                QuoteID = oHelper.CI(Session["QuoteID"].ToString());
            QuoteStatus = oQuote.GetQuoteStatus(QuoteID);
            ProductId = oHelper.CI(Request.QueryString["Pid"]);
        %>
        <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <td align="left" class="tx_1">
                    <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                        font-weight: bolder; font-size: small; font-style: normal"> / </font>Quote Cart
                </td>
            </tr>
            <tr>
                <td class="tx_3">
                    <hr>
                </td>
            </tr>
        </table>
        <br />
        <table id="BaseTable" cellpadding="0" cellspacing="0" border="0" width="558">
            <tr>
                <td>
                    <table id="SiteMapTable">
                        <tr class="tablerow">
                            <td class="StaticText">
                                <b>
                                    <asp:Label ID="lblCheck" runat="Server" meta:resourcekey="lblCheck"></asp:Label></b>
                                <b>
                                    <asp:Label ID="lblQuoteCart" runat="Server" meta:resourcekey="lblQuoteCart" ForeColor="Blue"></asp:Label></b>
                                >
                                <asp:Label ID="lblShip" runat="Server" meta:resourcekey="lblShip"></asp:Label>
                                >
                                <asp:Label ID="lblBill" runat="Server" meta:resourcekey="lblPayment"></asp:Label>
                                >
                                <asp:Label ID="lblReviewOrder" runat="Server" meta:resourcekey="lblReviewOrder"></asp:Label>
                                >
                                <asp:Label ID="lblConfirm" runat="Server" meta:resourcekey="lblConfirm"></asp:Label>
                                <cc1:msgBox ID="MsgBox1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            </tr>
            <tr class="TableRow">
                <td class="TableRowHead" colspan="3" align="left">
                    <asp:Label ID="Label1" runat="server" Text="Quote No : "></asp:Label>
                    <asp:Label ID="lblQteNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td class="TableRowHead" align="left">
                    <asp:Label ID="QuoteHeader" runat="server" meta:resourcekey="lblQuoteHeader"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Dynamic Content" width="558" cellpadding="0" cellspacing="0" class="BaseTable"
                        border="1">
                        <tr>
                            <td align="left" colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UpdateMsg" runat="server" meta:resourcekey="btnUpdateMsg" Class="lblComPhoneSkin"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CancelMesg" runat="server" meta:resourcekey="btnCancelMsg" Class="lblComPhoneSkin"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="center">
                                <asp:Button runat="server" ID="btnPlaceOrder" Class="btnNormalSkin" meta:resourcekey="btnPlaceOrder"
                                    Width="70px" OnClick="btnPlaceOrder_Click" /><%--style="left: 0%; position: relative; top: 0px;"  OnClick="btnPlaceOrder_Click --%>
                                <asp:Button ID="btnBotRemove" runat="Server" Class="btnNormalSkin" meta:resourcekey="btnRemoveCart"
                                    OnClick="btnRemoveCart_Click" OnClientClick="javascript:return  setTimeout('SendRemoveProducts()',100);"
                                    Width="70px" />
                                <asp:Button ID="btnUpdateCartBot" Class="btnNormalSkin" runat="Server" meta:resourcekey="btnUpdateQuote"
                                    OnClick="btnUpdateQuote_Click" Width="74px" />
                                <asp:Button ID="btnPlaceQuote" Class="btnNormalSkin" runat="Server" meta:resourcekey="btnPlaceQuote"
                                    OnClick="btnPlaceQuote_Click" Width="82px" Visible="false" />
                                <asp:Button ID="btnQuoteCanceled" Class="btnNormalSkin" runat="server" meta:resourcekey="btnQuoteCanceled"
                                    OnClick="btnQuoteCancel_Click" Width="74px" />
                            </td>
                        </tr>
                        <tr class="TableRowHead">
                            <td class="TableRowHead" align="center" width="80">
                                <% Response.Write("<input type =\"CheckBox\"  Name =\"ChkAllSel\" runat =\"server\" value =\"SelAll\"onclick =\"javascript:CheckSelectAll( );\"");%>
                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="" Class="CheckBoxSkin" Visible="false"
                                    OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td class="TableRowHead" align="center" style="width: 200px">
                                <asp:Label ID="lblItem" runat="server" meta:resourcekey="lblItem"></asp:Label>
                            </td>
                            <td class="TableRowHead" align="center" width="100">
                                <asp:Label ID="lblQty" runat="server" meta:resourcekey="lblQty"></asp:Label>
                            </td>
                            <td class="TableRowHead" align="center" style="width: 130px">
                                <asp:Label ID="lblCost" runat="server" meta:resourcekey="lblCost"></asp:Label>
                            </td>
                            <td class="TableRowHead" align="center" width="130">
                                <asp:Label ID="lblAmount" runat="server" meta:resourcekey="lblAmount"></asp:Label>
                            </td>
                        </tr>
                        <%       
    				     	      	                   	     
                            dsOItem = oQuote.GetQuoteItems(QuoteID);
                            string cSymbol = oHelper.GetOptionValues("CURRENCYFORMAT").ToString();

                            SelProductId = "";
                            decimal prdTotPrice = oQuote.GetCurrentProductTotalCost(QuoteID);
                            tax = CalculateTaxAmount(prdTotPrice);
                            if (QuoteStatus == QuoteServices.QuoteStatus.OPEN.ToString() || QuoteStatus == Quote.QuoteStatus.QUOTEUPDATEFLAG.ToString())
                            {
                                if (dsOItem != null)
                                {
                                    int i = 0;
                                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                    {
                                        decimal ProductUnitPrice;
                                        int pid;
                                        int maxqty;
                                        int minQty;
                                        pid = oHelper.CI(rItem["PRODUCT_ID"].ToString());
                                        int FId = oProd.GetFamilyID(pid);
                                        int pQty = oQuote.GetQuoteItemQty(pid, QuoteID);
                                        maxqty = oHelper.CI(rItem["QTY_AVAIL"].ToString());
                                        maxqty = maxqty + oHelper.CI(Request.Form["txtQty"] + pQty);

                                        minQty = oHelper.CI(rItem["MIN_ORD_QTY"].ToString());
                                        ProductUnitPrice = oHelper.CDEC(rItem["PRICE_APPLIED"].ToString());
                                        ProductUnitPrice = oHelper.CDEC(ProductUnitPrice.ToString("N2"));
                                        int Qty = oHelper.CI(rItem["QTY"].ToString());
                                        decimal ProdTotal = Qty * ProductUnitPrice;
                                        subtot = subtot + ProdTotal;

                                        if (Request["SelAll"] != "1")
                                        {
                                            SelProductId = "";
                                            Session["SelProduct"] = null;
                                            CheckBox chk = new CheckBox();
                        %>
                        <tr class="TableRow">
                            <td class="TableRow" align="center">
                                <%Response.Write("<input type =\"CheckBox\" Name =\"Chk" + i + "\" value =" + pid + "\" onclick =\"javascript:BuildValue(" + pid + "," + Qty + "," + ProdTotal + ",this);\">");%>
                            </td>
                            <td class="TableRow" align="left">
                                <%  Response.Write("<a href =productdetails.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td class="Numeric" align="left">
                                <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\"   Name =\"txtQty" + i + "\" size=\"7\"  runat =\"server\" onBlur=\"javascript:return Check(" + i + "," + maxqty + "," + minQty + "," + Qty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td class="NumericField" align="center" style="width: 130px">
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                            <td class="NumericField" align="center">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMinQty" + i + "\" runat=\"server\" value=\"" + minQty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%  
                              i = i + 1;
                                               }
                                               else if (Request["SelAll"] == "0")
                                               {
                                                   SelProductId = "";
                                                   Session["SelProduct"] = null;
                        %>
                        <tr class="TableRow">
                            <td class="TableRow" align="center">
                                <%Response.Write("<input type =\"CheckBox\" Name =\"Chk" + i + "\" value =" + pid + "\"  onclick =\"javascript:Click(" + pid + "," + Qty + "," + i + ");\">");%>
                            </td>
                            <td class="TableRow" align="left">
                                <% Response.Write("<a href =productdetails.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td class="Numeric" align="left">
                                <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\"  Name =\"txtQty" + i + "\" size=\"7\"  runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td class="NumericField" align="center" style="width: 130px">
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                            <td class="NumericField" align="center" width="20%">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%
                              i = i + 1;
                     }
                                               else
                                               { 
                        %>
                        <tr class="TableRow">
                            <td class="TableRow" align="center">
                                <%Response.Write("<input type =\"CheckBox\" Name =\"Chk" + i + "\" value =" + pid + "\" checked=\"checked\" onclick =\"javascript:Click(" + pid + "," + Qty + "," + i + ");\">");%>
                            </td>
                            <%--<td class="TableRow" align="left" style="width: 200px"><% Response.Write("<a href =ProductDetails.aspx?Pid=" + pid + ">" + rItem["CATALOG_ITEM_NO"] + "</a>");%></td>--%>
                            <td class="TableRow" align="left">
                                <%  Response.Write("<a href =productdetails.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td class="Numeric" align="left">
                                <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\"  Name =\"txtQty" + i + "\" size=\"7\"   runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td class="NumericField" align="center" style="width: 130px">
                                <%Response.Write(cSymbol + " " + ProductUnitPrice.ToString("#,#0.00"));%>
                            </td>
                            <td class="NumericField" align="center">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
                        <%   
                              SelProductId = SelProductId + "," + pid;
                              i = i + 1;
                     } //End of SelAll
                                           // } 
                                       } //End of for each.
                                       dsOItem.Dispose();
                                   }//End of dataset empty. 
                               } // End Of Order Status Check
                               if (SelProductId != "")
                               {
                                   SelProductId = SelProductId.Substring(1, SelProductId.Length - 1);
                                   Session["SelProduct"] = SelProductId;
                               }
                        %>
                        <!-- End Up Here-->
                        <tr>
                            <td class="NumericField" colspan="4">
                                Sub Total
                            </td>
                            <td class="NumericField">
                                <%Response.Write(oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + subtot.ToString("#,#0.00"));%>
                            </td>
                        </tr>
                        <tr>
                            <td class="NumericField" colspan="4" style="height: 21px">
                                Tax
                            </td>
                            <td class="NumericField" style="height: 21px">
                                <%
                                    taxamt = CalculateTaxAmount(subtot);
                                    Session["TaxAmt"] = oHelper.CDEC(taxamt);
                                    Response.Write(oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + oHelper.FixDecPlace(oHelper.CDEC(taxamt)));  							    
                                %>
                            </td>
                        </tr>
                        <tr>
                            <td class="NumericField" colspan="4" style="height: 21px">
                                <strong>Total</strong>
                            </td>
                            <td class="NumericField" style="height: 21px">
                                <strong>
                                    <%
                                        //Session["ProdTotalCost"] = subtot;				        
                                        Total = subtot + taxamt;
                                        Total = oHelper.CDEC(oHelper.FixDecPlace(Total));
                                        //Session["ProductTotalCost"] = Total;			        
                                        Response.Write(oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + Total.ToString("#,#0.00"));
    //btnPlaceOrder.Click += new System.EventHandler(btnPlaceOrder_Click);				        
                                    %>
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="height: 27px">
                                <input type="hidden" runat="Server" id="txtHiddenPid" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="center">
                                <asp:Button runat="server" ID="btnPlaceOrder1" Class="btnNormalSkin" meta:resourcekey="btnPlaceOrder"
                                    Width="70px" OnClick="btnPlaceOrder_Click" /><%--style="left: 0%; position: relative; top: 0px;"  OnClick="btnPlaceOrder_Click --%>
                                <asp:Button ID="btnBotRemove1" runat="Server" Class="btnNormalSkin" meta:resourcekey="btnRemoveCart"
                                    OnClick="btnRemoveCart_Click" OnClientClick="javascript:return  setTimeout('SendRemoveProducts()',100);"
                                    Width="70px" />
                                <asp:Button ID="btnUpdateCartBot1" Class="btnNormalSkin" runat="Server" meta:resourcekey="btnUpdateQuote"
                                    OnClick="btnUpdateQuote_Click" Width="74px" />
                                <asp:Button ID="btnPlaceQuote1" Class="btnNormalSkin" runat="Server" meta:resourcekey="btnPlaceQuote"
                                    OnClick="btnPlaceQuote_Click" Width="82px" Visible="false" />
                                <asp:Button ID="btnQuoteCanceled1" Class="btnNormalSkin" runat="server" meta:resourcekey="btnQuoteCanceled"
                                    OnClick="btnQuoteCancel_Click" Width="74px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblRemoveMsg" runat="server" meta:resourcekey="lblRemoveMsg" Visible="false"
                        Class="lblErrorSkin"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
