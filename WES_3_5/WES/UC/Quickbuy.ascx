<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_QuickOrder"
    EnableTheming="true" Codebehind="Quickbuy.ascx.cs" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc2" %>
<input id="HidItemCode" type="hidden" clientidmode="Static" accesskey="HidItemCode"
    runat="server" />
<input id="HidQty" type="hidden" clientidmode="Static" accesskey="HidQty" runat="server" />
<script type="text/javascript">
    function ValidateQCQty1() {
        var i = 0;
        var chkval = 0;
        var tempqty = "";
        var tempitem = "";
        var strg = "";
        for (i = 1; i <= 3; i++) {
            strg = strg.replace(/^\s+|\s+$/g, '');
            if (document.forms[0].elements["txitem" + i].value.length != 0 && document.forms[0].elements["txitem" + i].value != 'Item#' && document.forms[0].elements["txitem" + i].value != null) {
                if (document.forms[0].elements["txitem" + i].value.indexOf(",") >= 0) {
                    alert("Invalid Item#");
                    window.document.getElementById("txitem" + i).style.borderColor = "red";
                    window.document.getElementById("txqty" + i).style.borderColor = "red";
                    return false;
                }
                tempitem = tempitem + document.forms[0].elements["txitem" + i].value + ",";
            }
            if (document.forms[0].elements["txqty" + i].value.length != 0 && document.forms[0].elements["txqty" + i].value != null && document.forms[0].elements["txqty" + i].value != "0") {
                tempqty = tempqty + document.forms[0].elements["txqty" + i].value + ",";
            }
            if (document.forms[0].elements["txitem" + i].value == 'Item#' && document.forms[0].elements["txqty" + i].value.length == 0) {
                chkval = chkval + 1;
            }
        }

        for (i = 1; i <= 3; i++) {
            if (document.forms[0].elements["txitem" + i].value != 'Item#' && ((document.forms[0].elements["txqty" + i].value == '') || (document.forms[0].elements["txqty" + i].value <= 0))) {
                window.document.getElementById("txitem" + i).style.borderColor = "red";
                window.document.getElementById("txqty" + i).style.borderColor = "red";
                window.document.getElementById("txqty" + i).value = "";
            }

            if (document.forms[0].elements["txitem" + i].value == 'Item#' && document.forms[0].elements["txqty" + i].value > 0) {
                window.document.getElementById("txitem" + i).style.borderColor = "red";
                window.document.getElementById("txqty" + i).style.borderColor = "red";
            }
        }
        
        document.forms[0].elements["HidItemCode"].value = tempitem;
        document.forms[0].elements["HidQty"].value = tempqty;
        var myattr = new Array();
        var myattr1 = new Array();
        myattr = tempitem.split(",");
        myattr1 = tempqty.split(",");

        if (chkval == 3) {
            // alert("Item# and Qty cannot be empty !"); 
            //alert("1");
            return false;
        }
        else if (myattr.length < myattr1.length) {
            alert("Item# cannot be empty !");
            return false;
        }
        else if (myattr.length > myattr1.length) {
            alert("Qty cannot be empty !");
            return false;
        }
        else
            return true;
    }

    function FillValue1(ctl) {
        if (ctl.value == '' || ctl.value == null) {
            ctl.value = 'Item#';
        }
    }

    function Check1(Id) {
        var Qty = window.document.forms[0].elements["txqty" + Id].value;
        var Code = window.document.forms[0].elements["txitem" + Id].value;
        if ((isNaN(Qty) && Code != "Item#") || (Qty <= 0 && Code != "Item#") || (Qty.indexOf(".") != -1 && Code != "Item#") || (Qty == "" && Code != "Item#")) {
            alert('Invalid Quantity!');
            window.document.getElementById("txitem" + Id).style.borderColor = "red";
            window.document.getElementById("txqty" + Id).style.borderColor = "red";
            window.document.getElementById("txqty" + Id).value = "";
            window.document.getElementById("txqty" + Id).focus();
            return false;
        }

        if (Code == '' || Code == 'Item#' || Code.length == 0 || Code == null) {
            alert('Invalid Item!');
            window.document.getElementById("txitem" + Id).style.borderColor = "red";
            window.document.getElementById("txqty" + Id).style.borderColor = "red";
            window.document.getElementById("txitem" + Id).value = "";
            window.document.getElementById("txitem" + Id).focus();
            return false;
        }

        if ((Code != '' || Code != null || Code.lenth > 0) && (Qty > 0)) {
            window.document.getElementById("txitem" + Id).style.borderColor = "ActiveBorder";
            window.document.forms[0].elements["txqty" + Id].style.borderColor = "ActiveBorder";
        }
    }

    function Focus1(ctl) {
        if (ctl.value == 'Item#') {
            ctl.value = '';
        }
    }
</script>
<table width="180px" height="110px" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="back_5">
            <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="138px" height="30" class="tx_6" align="left">
                        &nbsp;&nbsp;&nbsp;&nbsp;QUICK BUY
                    </td>
                    <td align="center">
                        &nbsp;<img src="images/ico_11.gif" width="14" height="17" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="back_tb_cost">
            <table width="180" border="0" cellpadding="5" cellspacing="0" class="tx_1">
                <tr>
                    <td height="90px">
                        <asp:UpdatePanel runat="server" ID="updpnlQB" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="150px" align="left" cellpadding="5" cellspacing="0" border="0">
                                    <tr>
                                        <td colspan="2">
                                            <table id="tblitembox" cellpadding="0" cellspacing="0" border="0">
                                                <tr class="tx_4table">
                                                    <td align="left">
                                                        <strong>Item Number</strong>
                                                    </td>
                                                    <td align="left">
                                                        <strong>&nbsp;&nbsp;&nbsp;&nbsp;Qty</strong>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <input type="text" id="txitem1" <%=(!IsEcomenabled() ? "ReadOnly" : "")%> style="width: 85px;
                                                            background-color: #FFFFFF; color: #000000;" size="17" value="Item#" onblur="FillValue1(this)"
                                                            onfocus="Focus1(this)" />
                                                    </td>
                                                    <td align="right">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="txqty1" <%=(!IsEcomenabled() ? "ReadOnly" : "")%>
                                                            style="width: 30px; background-color: #FFFFFF; color: #000000;" size="2" onblur="javascript:return Check1(1);" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <input type="text" id="txitem2" <%=(!IsEcomenabled() ? "ReadOnly" : "")%> style="width: 85px;
                                                            background-color: #FFFFFF; color: #000000;" size="17" value="Item#" onblur="FillValue1(this)"
                                                            onfocus="Focus1(this)" />
                                                    </td>
                                                    <td align="right">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="txqty2" <%=(!IsEcomenabled() ? "ReadOnly" : "")%>
                                                            style="width: 30px; background-color: #FFFFFF; color: #000000;" size="2" onblur="javascript:return Check1(2);" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <input type="text" id="txitem3" <%=(!IsEcomenabled() ? "ReadOnly" : "")%> style="width: 85px;
                                                            background-color: #FFFFFF; color: #000000;" size="17" value="Item#" onblur="FillValue1(this)"
                                                            onfocus="Focus1(this)" />
                                                    </td>
                                                    <td align="right">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="txqty3" <%=(!IsEcomenabled() ? "ReadOnly" : "")%>
                                                            style="width: 30px; background-color: #FFFFFF; color: #000000;" size="2" onblur="javascript:return Check1(3);" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="BtnAddToCart" ImageUrl="~/images/but_submit.gif" Width="154px"
                                                Height="27px" runat="server" OnClientClick="return ValidateQCQty1();" OnClick="btnAddtoCart_ServerClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <% if (IsEcomenabled())
                                               { %>
                                            <a id="A1" href="~/BulkOrder.aspx?txtcnt=20" class="tx_14" runat="server">Bulk Order
                                                Forms</a>
                                            <%} %>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnAddToCart" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <cc2:msgBox ID="MsgBox1" runat="server"></cc2:msgBox>
            </table>
        </td>
    </tr>
</table>
