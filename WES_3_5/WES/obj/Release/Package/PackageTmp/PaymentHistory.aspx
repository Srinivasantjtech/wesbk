<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.master" AutoEventWireup="true" Inherits="PaymentHistory" Codebehind="PaymentHistory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript" src="Scripts/jquery-1.5.1.min.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/jquery-ui-1.8.13.custom.min.js"></script>
    <link href="css/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
<script language=JavaScript>
    var message = "Right click not allowed this page!";
    function clickIE4() {
        if (event.button == 2) {
            alert(message);
            return false;
        }
    }
    function clickNS4(e) {
        if (document.layers || document.getElementById && !document.all) {
            if (e.which == 2 || e.which == 3) {
                alert(message);
                return false;
            }
        }
    }

    if (document.layers) {
        document.captureEvents(Event.MOUSEDOWN);
        document.onmousedown = clickNS4;
    }
    else if (document.all && !document.getElementById) {
        document.onmousedown = clickIE4;
    }

    document.oncontextmenu = new Function("alert(message);return false")
</script>

<script language="JavaScript">
    document.onkeypress = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onmousedown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
</script>
   <script language="javascript" type="text/javascript">
       function Numbersonly(e) {
           var keynum
           var keychar
           var numcheck
           // For Internet Explorer
           if (window.event) {
               keynum = e.keyCode
           }
           // For Netscape/Firefox/Opera
           else if (e.which) {
               keynum = e.which
           }
           keychar = String.fromCharCode(keynum)
           //List of special characters you want to restrict
           if (keychar == "1" || keychar == "2" || keychar == "3" || keychar == "4" || keychar == "5" || keychar == "6" || keychar == "7" || keychar == "8" || keychar == "9" || keychar == "0") {

               return true;
           }
           else {
               return false;
           }
       }
       function PopupOrder(url) {
           newwindow = window.open(url, 'name', 'scrollbars=1,left=130,top=50,height=530,width=700');
           if (window.focus) {
               newwindow.focus();
           }
           return false;
       }
</script>
<script type="text/javascript">
    (function () {

        var DEBUG = true,
	EXPOSED_NS = 'ForTheCosumer';

        var myApp = function () {

            return {
                DoSomething: function () { },
                DoSomethingElse: function () { }
            }
        } ();

        // expose my public methods
        window[EXPOSED_NS] = {
            doSomething: myApp.DoSomething,
            doSomethingElse: myApp.DoSomethingElse
        };

        if (DEBUG) {
            window.MyApp = myApp
        }
    } ());
</script>

<script type="text/javascript">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
    <div class="box1" style="width: 955px;">
    <H4 style="TEXT-ALIGN: left" class="title1"> PAYMENT DETAILS</H4>
   

     <table id="tblBase" width="100%" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"    align="center">  
    <tr>
     <td width="100%">
            <table id="tblBase" width="100%" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse" align="center">  
            <tr>
                <td align="center">
                    Transaction Type
                </td>
                <td align="center">
                    Order No or Invoice No
                </td>
                <td  align="center">
                    From Date
                </td>
                <td align="center">
                    To Date
                </td>
                <td  align="left">
                    Created User
                </td>
                <td >
                    &nbsp;
                </td>
                <td >
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                            <asp:DropDownList runat="server" ID="CboTranType" BackColor="White"  CssClass="inputtxt" Width="100px">                            
                            <asp:ListItem Selected="True" >Payment</asp:ListItem>
                            <asp:ListItem>Failed Transaction</asp:ListItem>
                            </asp:DropDownList>
                        </td>
             <td >
                            <asp:TextBox runat="server" ID="OrderNo" Width="115px" BackColor="White" CssClass="inputtxt" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:TextBox runat="server" ID="FromdateTextBox" Width="90px" BackColor="White" Height="19px" CssClass="inputtxt" BorderColor="#999999"
                                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        </td>
                        <td  align="center">
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
                          <%--  <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButton" />--%>
                        </td>
            </tr>
            <tr>
            <td colspan="7">
            <asp:Label runat="server" ID="MsgLabel" Width="250px"> &nbsp; </asp:Label>
            </td>
            </tr>
            </table>
     </td>
    </tr>  
    <tr>
        <td width="100%"  >
            <table  width="100%" id="test1" border="0" cellpadding="3" cellspacing="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse; border-color: Red;"
                bgcolor="#c1d8d9">
         
                <tr  class="rowEven">
                    <td align="left" >
                         Cust. Order Date
                    </td>
                    <td align="left" >
                         Cust. Order No.
                    </td>
                    <td align="left">
                         Invoice Date
                    </td>
                    <td  align="right" >
                         Invoice No
                    </td>
                    <td align="left" >
                         User
                    </td>
                    <td align="left" >
                         view Order / Invoice
                    </td>
                    <td align="left" >
                        Value
                    </td>
                    <td align="left" >
                         Payment Txn Id.
                    </td>
                    <td align="left" >
                         Payment Date /Time.
                    </td>
                   <%-- <td align="left" >
                         Refund
                    </td>--%>
                </tr> 
                <asp:Repeater ID="PaymentRepeater"   runat="server"  > 
                 
                    <ItemTemplate >
                        <tr id="tRow"  runat="server"  class="rowOdd">
                                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("ORDER_DATE","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                        <td style="text-align:left;"><%# Eval("CUST_ORDER_NO")%></td>
                                        <td style="text-align:left;"> <%# Eval("MODIFIED_DATE", "{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                         <td style="text-align:left;"><%# Eval("INVOICE_NO")%></td>
                                          <td style="text-align:left;"><%# Eval("USER")%></td>
                                          <td style="text-align:left;">
                                            <a id="HyperLink1" style="color: #3399FF" href="OrderReport.aspx?OrdId=<%# Eval("ORDERID")%>"onclick="return PopupOrder('OrderReport.aspx?OrdId=<%# Eval("ORDERID")%>')">
                                            View Order </a>
                                          </td>
                                          <td style="text-align:left;">$<%# Eval("AMOUNT_CHARGED")%></td>
                                          
                                        <td style="text-align:left;"><%# Eval("RESPONSE_TXN_ID")%></td>
                                         <td style="text-align:left;"><%# Eval("PAYMENTDATE","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                         <%-- <td style="text-align:left;"><a id="A1" style="color: #3399FF" href="PayOnlineCC.aspx?<%#  EncryptSP(Eval("ORDERID").ToString(),Eval("RESPONSE_TXN_ID").ToString()) %>">Refund </a></td>--%>
                       </tr>  
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="tRow"  runat="server" class="rowEven">
                                   <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("ORDER_DATE","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                        <td style="text-align:left;"><%# Eval("CUST_ORDER_NO")%></td>
                                        <td style="text-align:left;"> <%# Eval("MODIFIED_DATE", "{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                         <td style="text-align:left;"><%# Eval("INVOICE_NO")%></td>
                                          <td style="text-align:left;"><%# Eval("USER")%></td>
                                             <td style="text-align:left;">
                                            <a id="HyperLink1" style="color: #3399FF" href="OrderReport.aspx?OrdId=<%# Eval("ORDERID")%>"
                                            onclick="return PopupOrder('OrderReport.aspx?OrdId=<%# Eval("ORDERID")%>')">
                                            View Order </a>
                                          </td>
                                          <td style="text-align:left;">$<%# Eval("AMOUNT_CHARGED")%></td>
                                           <td style="text-align:left;"><%# Eval("RESPONSE_TXN_ID")%></td>
                                         <td style="text-align:left;"><%# Eval("PAYMENTDATE","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                          <%--<td style="text-align:left;">  <a id="A1" style="color: #3399FF" href="PayOnlineCC.aspx?<%#  EncryptSP(Eval("ORDERID").ToString(),Eval("RESPONSE_TXN_ID").ToString()) %>">Refund </a></td>--%>
                                       
                       </tr>  
                    </AlternatingItemTemplate>
               </asp:Repeater>                           
            </table>
        </td>
    </tr>
</table>
    </div>
</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
