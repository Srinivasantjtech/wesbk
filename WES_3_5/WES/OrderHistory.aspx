<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.master" AutoEventWireup="true" Inherits="OrderHistory1" Codebehind="OrderHistory.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>    
 
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <script language="javascript" type="text/javascript" src="/Scripts/jquery-1.5.1.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Scripts/jquery-ui-1.8.13.custom.min.js"></script>
    <link href="/css/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/progress.js" type="text/javascript"></script>
    <link href="/css/loadingbox.css" rel="Stylesheet" />
    
    <script language="javascript" type="text/javascript">
        var Flag = false;

        $(document).ready(function msg() {

            $("ProcessDiv").hide();

            if (Flag == false) {

            }
            $(".HyperLink2").click(function () {

                if (Flag == true) {
                    $("ProcessDiv").show();


                }
            });
        });

        function ShowDiv(obj) {
            var dataDiv = document.getElementById(obj);
            dataDiv.style.display = "block";
            alert("h2");
        }

        function countdown_clear() {
            clearTimeout(countdown);
        }


        function ShowFileMessage() {
            alert('Invalid Specification!');
        }

        function showalert(Url) {
            Flag = confirm('Do you want to proceed the download?');
            if (Flag == true) {
                //alert(Url);
                window.location.href = "OrderHistory.aspx?Key=" + Url;
            }
            return flag;
        }

        function ShowFailureMessage() {
            alert('Invoice currently Unavailable. Please try again later');
        }

        function ShowLoader() {
            setTimeout("setImage();", 500);
            //$.hideprogress();
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'none';
            //document.getElementById('ctl00_maincontent_DivLoader').style.visibility = 'hidden';
            //document.getElementById('ctl00_maincontent_LoadPanel').style.visibility = 'hidden';
            alert("h2");
        }

        function setImage() {
            //$.showprogress();
            window.document.getElementById('ctl00_maincontent_DivLoader').style.display = 'block';
        }

        function PopupOrder(url) {
            newwindow = window.open(url, 'name', 'scrollbars=1,left=130,top=50,height=530,width=700');
            if (window.focus) {
                newwindow.focus();
            }
            return false;
        }

        function payonlinepopup_open(url) {

           // var hrefroot = 'payonline.aspx?OrdId=';
           // var url = hrefroot + url;
            // alert(url);

            
//                var hrefroot = 'payonline.aspx?OrdId=';
//                var hrefURL = hrefroot + url;
//                popupWindow = window.open(hrefURL, "_blank", "directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=700, height=530,top=50,left=130");
//                if (window.focus) {
//                    popupWindow.focus();
//                }
//                return false

        }

       
        
        

        function initinvoicereq() {
            var ret = confirm("Do you want to proceed")
            return ret;
        }
        $(function () {
            $("#<%=FromdateTextBox.ClientID %>").removeClass();
            $("#<%=FromdateTextBox.ClientID %>").datepicker({
                //                onSelect: function (currentText) {
                //                    var d1 = new Date(currentText);
                //                    d1 = d1.format('MM/dd/yyyy');
                //                    var sub = d1.split('/');
                //                    var mon = sub[0] - 1;
                //                    var Newdate = new Date(mon + "/" + sub[1] + "/" + sub[2]);
                //                    Newdate = Newdate.format('MM/dd/yyyy');
                //                    document.getElementById("ctl00_maincontent_FromdateTextBox").value = Newdate;
                //                },
                beforeShow: function (input, inst) {
                    var d1 = new Date();
                    d1 = d1.format('MM/dd/yyyy');
                    var sub = d1.split('/');
                    var mon = sub[0] - 1;
                    var Newdate = new Date(mon + "/" + sub[1] + "/" + sub[2]);
                    Newdate = Newdate.format('dd/MM/yyyy');

                    if (document.getElementById("ctl00_maincontent_FromdateTextBox").value == null || document.getElementById("ctl00_maincontent_FromdateTextBox").value == '') {
                        document.getElementById("ctl00_maincontent_FromdateTextBox").value = Newdate;
                    }
                },
                defaultDate: '-1m',
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                showOn: "button",     //false,                          
                buttonImage: "images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });

            $("#<%=TodateTextBox.ClientID %>").removeClass();
            $("#<%=TodateTextBox.ClientID %>").datepicker({
                maxDate: "+0M +0D",
                dateFormat: 'dd/mm/yy',
                showOn: "button",
                buttonImage: "images/Calendar2.PNG",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function Focus() {
            alert('dd');
            SearchText1();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //alert("sdasdas");
            // SearchText('txtitem1');

        });
        function SearchText() {

            $('a.HyperLink2').click({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "OrderHistory.aspx/WestestAutoCompleteData",
                        //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                        data: "{'strvalue':'A'}",
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
        function SearchText1() {
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "OrderHistory.aspx/WestestAutoCompleteData",
                //  data: "{'username':'" + document.getElementById(ctl).value + "'}",
                data: "{'strvalue':'A'}",
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
        function asyncServerCall(userid) {
            jQuery.ajax({
                url: 'WebForm1.aspx/GetData',
                type: "POST",
                data: "{'userid':" + userid + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert(data.d);
                }

            });
        }

	</script>
    <script type="text/javascript" language="javascript">
        function toggle(inv, dis) {
            var e = document.getElementById(inv);
            e.style.display = dis;
        }
        function GetInvoice(OrderInv, inv) {
            toggle(inv, "")
            PageMethods.SendInvoiceSignal(OrderInv, OnSuccess, OnFailure);
        }
        function OnSuccess(result) {
            if (result.indexOf("inv") != -1) {
                var invno = result.replace("inv", "");

                document.getElementById('<%=ImageButton1.ClientID %>').click();
                toggle(invno, "none")
            }
            else {
                var invno1 = result.replace("inv", "");
                toggle(invno1, "none")
                alert("Invoice PDF temopary not Available.Please try again later");
            }

        }
        function OnFailure(error) {
            var invno = result.replace("inv", "");
            toggle(invno, "none")
            alert("Invoice PDF temopary not Available.Please try again later");
            //alert(error);

        }
</script>
     <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
        <tr>
            <td width="100%" align="left">
                <font color="#333333" face="sans-serif" size="4">
                    <asp:Label runat="server" ID="Label1"> Web Order History </asp:Label>
                </font>
            </td>
        </tr>
    </table>     
    <table width="970px" border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse;
        visibility: visible" bgcolor="white">
        <tr>
            <td width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                    <tr>
                        <td style="font-family: Arial; font-size: 11px; font-weight: bold; color: Red; text-align: center">
                            <asp:Label runat="server" ID="MsgLabel" Width="250px"> &nbsp; </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="970px" style="border-collapse: collapse; border-width: 1px; border-style: solid;
        border-color: #c6d3de">
        <tr>
            <td width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                    <tr>
                        <td align="left" style="background-color: #c6d3de">
                            <asp:Label runat="server" ID="Label2">Filter Orders By:</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            
            <td width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                    <tr>
                        <td width="15%">
                            Order No or Invoice No
                        </td>
                        <td width="20%" align="center">
                            From Date
                        </td>
                        <td width="20%" align="center">
                            To Date
                        </td>
                        <td width="35%" align="left">
                            Created User
                        </td>
                        <td width="5%">
                            &nbsp;
                        </td>
                        <td width="5%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                    <tr>
                        <td width="15%">
                            <asp:TextBox runat="server" ID="OrderNo" Width="115px" BackColor="White" CssClass="inputtxt" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        </td>
                        <td width="20%" align="center">
                            <asp:TextBox runat="server" ID="FromdateTextBox" Width="90px" BackColor="White" Height="19px" CssClass="inputtxt" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        </td>
                        <td width="20%" align="center">
                            <asp:TextBox runat="server" ID="TodateTextBox" Width="90Px" BackColor="White" Height="19px" CssClass="inputtxt" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        </td>
                        <td width="25%" align="left">
                            <asp:DropDownList runat="server" ID="CreatedUserDropDownlist" BackColor="White"  CssClass="inputtxt" Width="190px">
                            </asp:DropDownList>
                        </td>
                        <td width="10%" align="center" valign="middle">
                            <asp:Button runat="server" ID="SearchButton"  CssClass="inputbtn"
                                Text="Search" OnClick="SearchButton_Click" />
                        </td>
                        <td width="10%" align="center" valign="middle">
                            <asp:Button runat="server" ID="ResetButton"  CssClass="inputbtn"
                                Text="Reset"  OnClick="ResetButton_Click" />                            
                        </td>
                        <td width="1" align="center" valign="middle">
                            <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButtonOH" />
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="OrderNo"
                                runat="server" ErrorMessage="RequiredFieldValidator">OrderNo cannot be blank</asp:RequiredFieldValidator>--%>
                            &nbsp;
                        </td>
                        <td width="20%" align="center">
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="FromDateTextBox"
                                runat="server" ErrorMessage="RequiredFieldValidator">From Date cannot be blank</asp:RequiredFieldValidator>--%>
                            &nbsp;
                        </td>
                        <td width="20%" align="center">
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ToDateTextBox"
                                runat="server" ErrorMessage="RequiredFieldValidator">To Date cannot be blank</asp:RequiredFieldValidator>--%>
                            &nbsp;
                        </td>
                        <td width="25%" align="left">
                            &nbsp;
                        </td>
                        <td width="10%">
                            &nbsp;
                        </td>
                        <td width="10%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <% 
                    try
                    {
                        HelperServices objHelperServices = new HelperServices();
                        Security objSecurity = new Security();
                        
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt = null;
                        //int Companyid = Convert.ToInt16(Session["COMPANY_ID"]);
                        int Companyid = Convert.ToInt32(Session["COMPANY_ID"]);

                        if (OrderNoHiddenField.Value.Trim() != "" || FromDateHiddenField.Value.Trim() != "" || ToDateHiddenField.Value.Trim() != "" || UserHiddenField.Value.Trim() != "")
                        {
                            DateTime Fromdate1 = new DateTime();
                            DateTime Todate1 = new DateTime();

                            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);

                            if (FromDateHiddenField.Value.Trim() != "")
                            {
                                // Fromdate1 = DateTime.Parse(FromDateHiddenField.Value.ToString(), culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                            else
                            {
                                FromDateHiddenField.Value = null;
                            }

                            if (ToDateHiddenField.Value.Trim() != "")
                            {
                                //  Todate1 = DateTime.Parse(ToDateHiddenField.Value.ToString(), culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                            }
                            else
                            {
                                ToDateHiddenField.Value = null;
                            }

                            if (OrderNoHiddenField.Value.Trim() == "")
                            {
                                OrderNoHiddenField.Value = null;
                            }

                            if (UserHiddenField.Value.Trim() == "")
                            {
                                UserHiddenField.Value = null;
                            }
                                                                                    
                            oDt = objOrderServices.GetFilteredOrderHistory(OrderNoHiddenField.Value, FromDateHiddenField.Value, ToDateHiddenField.Value, UserHiddenField.Value, Companyid);                            
                        }
                        else
                        {
                            oDt = objOrderServices.GetOrderHistory();
                        }

                        if (oDt != null && oDt.Rows.Count > 0)
                        {
                %>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse">
                    <tr style="height: 20px">
                        <td style="background-color: #c6d3de; width: 15%;" align="center">
                            Cust. Order Date
                        </td>
                        <td style="background-color: #c6d3de; width: 10%;" align="center">
                            Cust. Order No.
                        </td>
                        <td style="background-color: #c6d3de; width: 15%;" align="center">
                            Invoice Date
                        </td>
                        <td style="background-color: #c6d3de; width: 10%;" align="center">
                            Invoice No
                        </td>
                        <td style="background-color: #c6d3de; width: 10%;" align="center">
                            User
                        </td>
                        <td style="background-color: #c6d3de; width: 10%;" align="center">
                            Order Status
                        </td>
                        <td style="background-color: #c6d3de; width: 14%;" align="center">
                            Shipping Track & Trace
                        </td>
                        <td style="background-color: #c6d3de; width: 8%;" align="center">
                            Submitted Order
                        </td>
                        <td style="background-color: #c6d3de" width="8%" align="center">
                            View Invoice
                        </td>
                    </tr>
                    <%  
                            string bgcolor = "";
                            bool bodrow = true;

                            foreach (DataRow oDr in oDt.Rows)
                            {
                                bgcolor = bodrow ? "white" : "#e7efef";
                                bodrow = !bodrow;


                                if (oDr["CC_PAY_RESPONSE"].ToString().ToLower() != "yes" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {
                                    bgcolor = "#fff98c";
                                }
                    %>
                    <tr style="height: 20px">
                        <td style="background-color: <%=bgcolor%>; width: 15%;" align="center">
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Order Date"])%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 10%;" align="center">
                            <%=oDr["Cust.Order No"].ToString()%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 15%;" align="center">
                            <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["Modified Date"])%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 10%;" align="center">
                            <%=oDr["Invoice No"].ToString()%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 10%;" align="center">
                            <%=oDr["User"].ToString()%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 10%;" align="center">
                            <%=oDr["Order Status"].ToString()%>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 14%;" align="center">
                            <font color="#3399FF">
                            <% 
                                
                                if (oDr["CC_PAY_RESPONSE"].ToString().ToLower() != "yes" && (oDr["Order Status"].ToString().Trim() == "Payment Required" || oDr["Order Status"].ToString().Trim() == "Proforma Payment Required"))
                                {
                                    %>
                           <%       if (objOrderServices.IsNativeCountry(Convert.ToInt32( oDr["ORDERID"].ToString())) == 1)
                    { 
                                        
                                         %>
                                      <a id="A1" style="color: #3399FF" href="paysp.aspx?<%= EncryptSP(oDr["ORDERID"].ToString()) %>">

                             
                                          <img id="Img1" src="images/paynow_btn.png" alt="" />
                                     </a>
                                <% } else { %>
                                  <a id="A3" style="color: #3399FF" href="PayOnline_International.aspx?<%= oDr["ORDERID"].ToString() %>"+ "#####" + "PaySP">

                             
                                          <img id="Img2" src="images/paynow_btn.png" alt="" />
                                     </a>
                                 <% } %>
                               
                            
                                <% }
                                else if (!string.IsNullOrEmpty(oDr["SHIPTRACKURL"].ToString().Trim()))
                                { %>
                                <img id="ExternalLinkImg" src="images/External_Link.gif" height="12px" width="12px" alt="" /> 
                                &nbsp;
                                <a href="<% = oDr["SHIPTRACKURL"].ToString() %>" style="color: #0099da; text-decoration:none;"
                                    onclick="window.open('<% = oDr["SHIPTRACKURL"].ToString() %>','popup','width=800,height=600,scrollbars=yes,resizable=yes,toolbar=no,directories=no,location=no,menubar=no,modal=yes,status=yes,left=150,top=25'); return false">
                                    <%=oDr["Shipping Track & Trace"].ToString().Trim()%> </a>
                                 <% }
                                else if (!string.IsNullOrEmpty(oDr["Shipping Track & Trace"].ToString().Trim()))
                                {%>
                                   <%=oDr["Shipping Track & Trace"].ToString().Trim()%>
                                  <%}%>
                                      
                            </font>
                        </td>
                        <td style="background-color: <%=bgcolor%>; width: 8%;" align="center">
                            <a id="HyperLink1" style="color: #3399FF" href="OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>"
                                onclick="return PopupOrder('OrderReport.aspx?OrdId=<% = oDr["ORDERID"].ToString() %>')">
                                View Order </a>
                        </td>
                        <td style="background-color: <%=bgcolor%>; color: #3399FF" width="8%;" align="center">
                            <%
                                // oDr["Invoice No"].ToString().Trim() != string.Empty &&
                                if ((oDr["Order Status"].ToString().Trim() == "Processing Order" || oDr["Order Status"].ToString().Trim() == "Order Shipped" || oDr["Order Status"].ToString().Trim() == "Completed" || oDr["Order Status"].ToString().Trim() == "Shipped" || oDr["Order Status"].ToString().Trim() == "Payment Required"))
                                {
                                    string InvNo= oDr["Invoice No"].ToString();
                                    string EncryptKeyString = string.Format("{0}|{1}", oDr["ORDERID"].ToString(), oDr["Invoice No"].ToString().Trim());
                                    string PdfNameValue = objSecurity.Encrypt(EncryptKeyString, Session.SessionID);
                                    string pdfn = Server.HtmlEncode(PdfNameValue).ToString();
                            %>
                            <%--  <a id="HyperLink2" href="OrderHistory.aspx?pdfName=In<% = oDr["Invoice No"] %>&CustOrderNo=<%=oDr["ORDERID"].ToString()%>"
                                style="color: #3399FF; cursor: pointer;">View Invoice </a>--%>

                                
                               <%--  <a class="HyperLink2" id="A1" href="OrderHistory.aspx?Key=<%=Server.HtmlEncode(PdfNameValue)%>" style="color: #3399FF; cursor: pointer;" > View Invoice </a> 
                               --%>
                                <a class="HyperLink2" id="A2" style="color: #3399FF; cursor: pointer;" onclick="GetInvoice('<%=EncryptKeyString%>','<%=InvNo%>');" > 

                                 <%
                                     if (oDr["Order Status"].ToString().Trim() != "Proforma Payment Required")
                                     {
                                     %>
                                 View Invoice 
                                 <%
                                     }
                                     else
                                     { %>
                                     View Proforma Invoice
                                     <%} %>
                                
                                </a>                               
                                <div id="<%=InvNo%>" style="display:none;" ><img src="/Images/Invloading.gif"/ ></div>
                                <asp:ImageButton ID="ImageButton1" runat="server" onclick="cmdUpdateField_Click"    style="display:none;" ></asp:ImageButton>

                          <%-- <a class="HyperLink2" id="HyperLink2"  onclick="return showalert('<%=Server.HtmlEncode(PdfNameValue)%>');"
                            <%-- onclick="return confirm('Do you want to proceed the download?')"--%>
                                <%--style="color: #3399FF; cursor: pointer;"> View Invoice </a>--%>
         
                            
                            <% } %>
                        </td>
                    </tr>
                    <% } %>
                </table>
                <%
                        }
                        else
                        {
                            MsgLabel.Text = "No Records Found!";
                            MsgLabel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgLabel.Text = ex.Message;
                        MsgLabel.Visible = true;
                    }
                %>
            </td>
        </tr>
    </table>
     
   


    <div id="PopDiv" class="containerOH">
        <asp:Panel ID="SignalPopupPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOH">
            <div class="containerOH">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                        <td width="10px">
                        <asp:ImageButton ID="ImageButton2" runat="server" OnClick="btncancel_Click" ImageUrl="~/images/btn_images/11_32_2.png" ImageAlign="Right" />                        
                         </td>
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOH" colspan="2" style="font-size: medium; font-weight: bold">
                          &nbsp;&nbsp;Your Invoice has been generated.
                        <br/>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOH" colspan="3">
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Click here to Download.." OnClick="btnClose_Click"  Font-Size="Medium" Font-Underline="True" ForeColor="Maroon" Font-Bold="False" />
                            <%--<asp:Button ID="btnCancel" runat="server" Text="Click here to Download" CssClass="ButtonStyle" OnClick="btnClose_Click"/>--%>
                        </td>
                        
                        
                    </tr>
           
                </table>
            </div>
        </asp:Panel>
    </div>
    <div id="PopDiv1" class="containerOH">
        <asp:Panel ID="SignalProcessPanel" runat="server" Style="display: none;" CssClass="ModalPopupStyleOH">
            <div class="containerOH">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOH">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOH">
                           Invoice PDF temopary not Available. 
                            <br />
                            Please try again later
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOH">
                            <%--<asp:Button ID="ExitButton" runat="server" Text="Close"  Visible="false" OnClick="btncancel_Click" CssClass="ButtonStyle"/>--%>
                            <asp:Button ID="Button1" runat="server" Text="Close"  OnClick="btncancel_Click" CssClass="ButtonStyleOH"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
  <%--      <div id="message-div" visible="false" style="position: fixed; top: 40%; left: 50%;"  >
        <div> <img src="images/ajax-loader(2).gif" alt="" />  </div>--%>
        <%--<div id="message" style="position: fixed; top: 40%; left: 50%;"  >
        <div> <img src="images/ajax-loader(2).gif" /> </div>--%>
    
    </div>
     
     <div id="Div1" class="containerOH">
        <asp:Panel ID="Panel1" runat="server" Style="display: none;" CssClass="ModalPopupStyleOH">
            <div class="containerOH">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">                   
                    <tr>
                        <td class="TableColumnStyleOH">
                            <asp:Button ID="ExitButton" runat="server" Text="Close"   OnClick="btncancel_Click" CssClass="ButtonStyle"/>                            
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel> 
    </div>
    <div id="ProcessDiv" class="containerOH" runat="server"> 
         <asp:Panel ID="ShowProcessPanel" runat="server" Style="display: none;"  CssClass="ModalPopupStyleOH">
         <div class="containerOH">
                   <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;">
                    <tr class="TableColumnStyleOH">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleOH">
                           Please wait for a moment.
                            <br />
                            Invoice will be available shortly
                        </td>
                    </tr>
                   </table>          
       <%--      <div id="message" runat="server">
        <div><table bgcolor="white"><tr>
        <td><img src="images/loader_New.gif" alt="Processing......Please Wait...." /></td></tr></table>
       <table><tr><td><font size="2" color="#0066ff"><b>Processing......Please Wait....</b></font></td></tr></table></div>       
    </div>--%></div>
        </asp:Panel>
       
    </div>
    <asp:HiddenField ID="FromDateHiddenField" runat="server"/>
    <asp:HiddenField ID="ToDateHiddenField" runat="server" />
    <asp:HiddenField ID="UserHiddenField" runat="server" />
    <asp:HiddenField ID="OrderNoHiddenField" runat="server" />


       <asp:Timer ID="Timer1" runat="server" OnTick="timer1_Tick" Interval="3000" Enabled="false" />
  <asp:updatepanel id="updatepaneltimer" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"/>
         </Triggers>
   
    
       </asp:updatepanel>

     
       
</asp:Content>


<%--<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>--%>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>--%>
 