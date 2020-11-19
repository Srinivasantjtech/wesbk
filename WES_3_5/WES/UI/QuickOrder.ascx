<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_QuickOrder1"
    EnableTheming="true" Codebehind="QuickOrder.ascx.cs" %>
<%@ Register Assembly="msgBox" Namespace="BunnyBear" TagPrefix="cc2" %>
<input id="HidItemCode" type="hidden" runat="server" />
<input id="HidQty" type="hidden" runat="server" />
<script type="text/javascript">
    function ValidateQCQty() {
        var i = 0;
        var tempqty = "";
        var tempitem = "";
        var strg = "";
        for (i = 1; i <= 5; i++) {
            //        alert(document.forms[0].elements["txtitem" + i].value);
            strg = strg.replace(/^\s+|\s+$/g, '');
            if (document.forms[0].elements["txtitem" + i].value.length != 0 && document.forms[0].elements["txtitem" + i].value != 'Item#' && document.forms[0].elements["txtitem" + i].value != null && document.forms[0].elements["txtqty" + i].value.length != 0 && document.forms[0].elements["txtqty" + i].value != null && document.forms[0].elements["txtqty" + i].value != "0" && document.forms[0].elements["txtitem" + i].value.indexOf(',') == "-1") {
                tempqty = tempqty + document.forms[0].elements["txtqty" + i].value + ",";
                tempitem = tempitem + document.forms[0].elements["txtitem" + i].value + ",";
            }
        }

        for (i = 1; i <= 5; i++) {
            if (document.forms[0].elements["txtitem" + i].value != 'Item#' && ((document.forms[0].elements["txtqty" + i].value == '') || (document.forms[0].elements["txtqty" + i].value <= 0))) {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).value = "1";
            }

            if (document.forms[0].elements["txtitem" + i].value == 'Item#' && document.forms[0].elements["txtqty" + i].value > 0) {
                window.document.getElementById("txtitem" + i).style.borderColor = "red";
                window.document.getElementById("txtqty" + i).style.borderColor = "red";
            }
        }

        document.forms[0].elements["ctl00_WorkingArea_QuickOrder1_HidItemCode"].value = tempitem;
        document.forms[0].elements["ctl00_WorkingArea_QuickOrder1_HidQty"].value = tempqty;

        if (tempitem == "" || tempqty == "") {
            alert("Item# and Qty cannot be empty !");
        }
    }

    function FillValue(ctl) {
        if (ctl.value == '' || ctl.value == null) {
            ctl.value = 'Item#';
        }
    }

    function Check(Id) {
        var Qty = window.document.forms[0].elements["txtqty" + Id].value;
        var Code = window.document.forms[0].elements["txtitem" + Id].value;
        if ((isNaN(Qty) && Code.value != "Item#") || (Qty <= 0 && Code != "Item#") || (Qty.indexOf(".") != -1 && Code != "Item#") || (Qty == "" && Code != "Item#")) {
            alert('Invalid Quantity!');
            window.document.getElementById("txtitem" + Id).style.borderColor = "red";
            window.document.getElementById("txtqty" + Id).style.borderColor = "red";
            window.document.getElementById("txtqty" + Id).value = "1";
            window.document.getElementById("txtqty" + Id).focus();
            return false;
        }

        if (Code == '' || Code == 'Item#' || Code.length == 0 || Code == null) {
            alert('Invalid Item!');
            window.document.getElementById("txtitem" + Id).style.borderColor = "red";
            window.document.getElementById("txtqty" + Id).style.borderColor = "red";
            window.document.getElementById("txtitem" + Id).value = "";
            window.document.getElementById("txtitem" + Id).focus();
            return false;
        }


        if ((Code != '' || Code != null || Code.lenth > 0) && (Qty > 0)) {
            window.document.getElementById("txtitem" + Id).style.borderColor = "ActiveBorder";
            window.document.forms[0].elements["txtqty" + Id].style.borderColor = "ActiveBorder";
        }
    }

    function Focus(ctl) {
        if (ctl.value == 'Item#') {
            ctl.value = '';
        }
    }
</script>
<table cellpadding="5" cellspacing="0" border="1px" width="120px" style="border-color: Black">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="120px">
                <tr>
                    <td colspan="2">
                        <%
                            StringBuilder oStrCtrls = new StringBuilder();
                            int i = 1;
                            oStrCtrls.Append("<table cellpadding=\"0\" cellspacing=\"10\" border=\"0\">");
                            oStrCtrls.Append("<tr><th>Qty</th><th>Item#</th></tr>");
                            for (i = 1; i <= 5; i++)
                            {
                                oStrCtrls.Append(System.Environment.NewLine + "<tr><td><input type=\"text\" id=\"txtqty" + i + "\" style=\"width:30px\" runat=\"server\" onBlur=\"javascript:return Check(" + i + ");\"></td><td><input type=\"text\" id=\"txtitem" + i + "\" style=\"width:85px\" size=\"2\" runat=\"server\" value=\"Item#\" onblur=\"FillValue(txtitem" + i + ")\" onfocus=\"Focus(txtitem" + i + ")\"></td></tr>" + System.Environment.NewLine);
                            }
                            oStrCtrls.Append("</table>");
                            Response.Write(oStrCtrls.ToString());
                        %>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <input id="BtnAddToCart" type="button" value="Add to Cart" style="font-family: Arial;
                            font-size: 11px;" runat="server" onclick="ValidateQCQty();" onserverclick="btnAddtoCart_ServerClick" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="LinkButton1" runat="server" SkinID="CommonLinkSkin" PostBackUrl="~/BulkOrder.aspx?txtcnt=20">Bulk Order Pad. . .</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="140px">
                <tr>
                    <td align="left" style="font-family: Arial; font-size: 11px; width: 150px;">
                        Copy & paste the Quantities and Item #s from your file
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-family: Arial; font-size: 11px; width: 150px;">
                        Enter one item per line:
                        <br />
                        Qty. [TAB or COMMA] Item #
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">
                        <br />
                        <textarea id="txtCopyPaste" runat="server" style="width: 170px; height: 151px;"></textarea>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 150px">
                        <input id="btnCPAddtoCart" type="button" value="Add to Cart" runat="server" onserverclick="btnCPAddtoCart_ServerClick"
                            style="font-family: Arial; font-size: 11px;" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <cc2:msgBox ID="MsgBox1" runat="server"></cc2:msgBox>
</table>
