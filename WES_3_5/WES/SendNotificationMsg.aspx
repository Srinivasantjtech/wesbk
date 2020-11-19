<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendNotificationMsg.aspx.cs" Inherits="WES.SendNotificationMsg" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<script src="/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
  <script src="./scripts/encryption/helpers.js"></script>
  <script src="./scripts/encryption/hmac.js"></script>
  <script src="./scripts/encryption/hkdf.js"></script>
  <script src="./scripts/encryption/encryption-factory.js"></script>
  <script src="./scripts/libs/snippets.js"></script>
  <script src="./scripts/libs/idb-keyval.js"></script>
     <script src="./scripts/app-controller.js"></script>
<%-- <script src="/Scripts/notification_send.js"></script>--%>
    <script language="javascript" type="text/javascript">

        function SendNotification() {
            var hfsubs = $("#" + '<%= HiddenField1.ClientID %>').val();
            
            var hfendpoint = $("#" + '<%= HiddenField2.ClientID %>').val();
          
            var hfmessage = $("#" + '<%= HiddenField3.ClientID %>').val();
         

            var myendpoint = "https://updates.push.services.mozilla.com/wpush/v1/gAAAAABYrVSlMDljh_lMSedjiMEAzUlWKm4_YxlQ0YxeTKSjOrioyRdR0PnyU84iqcR5kKS1qD6DeswFmW4P1AevMLdPFhGdybYACfa2gvSIMTpvOsq3AoY5oQpn2OuPG23BBQ4YBU4P";

            sendPushMessage(myendpoint, hfsubs,
     hfmessage, hfendpoint);

            return false;
        }
            </script>
</head>
<body>
    <form id="form1" runat="server" style="font-family: sans-serif">
        <asp:HiddenField ID="HiddenField1" runat="server" />
          <asp:HiddenField ID="HiddenField2" runat="server" />
         <asp:HiddenField ID="HiddenField3" runat="server" />
    <div>
        <table style="width:100%">
            <h2>Send Notification Details</h2>
            <tr>
                <td style="width:30%">
                    <asp:Label ID="Label3" runat="server" Text="Notifucation Text:"></asp:Label> 
                </td>
                <td>
                     <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td style="width:30%">
                      <asp:Label ID="Label4" runat="server" Text="Notification URL:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    
       
          
      
    </div>
    </form>

</body>
</html>
