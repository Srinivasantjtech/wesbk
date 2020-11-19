using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using TradingBell.Common; 
using TradingBell.WebServices;


public partial class UI_Invoice : System.Web.UI.UserControl
{
    Helper oHelper = new Helper();
    ErrorHandler oErr = new ErrorHandler();
    Order oOrder = new Order();
    Payment oPay = new Payment();
    Payment.PayInfo oPayInfo = new Payment.PayInfo();
    Order.OrderInfo oOrderInfo = new Order.OrderInfo();
    Order.OrderInfo ModifiedUser = new Order.OrderInfo();
    User Usr = new User();
    User.UserInfo oUserInfo = new User.UserInfo();
    
    int OrderID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        //  Page.Title = oHelper.GetOptionValues("BROWSER TITLE"].ToString();
        //int OrderID = oOrder.GetOrderID(oHelper.CI(Session["USER_ID"].ToString()),1);

        if (OrderID == 0)
        {
            OrderID = oHelper.CI(Request["OrdId"].ToString());
        }

        string BillAdd;
        string ShippAdd;

        oPayInfo = oPay.GetPayment(OrderID);
        oOrderInfo = oOrder.GetOrder(OrderID);
        //ModifiedUser = oOrder.ModifiedUser(OrderID);
        int UserID = oHelper.CI(Session["USER_ID"].ToString());
        oUserInfo = Usr.GetUserInfo(UserID);
        lblOrderID.Text = oPayInfo.PORelease;
        BillAdd = BuildBillAddress();
        ShippAdd = BuildShippAddress();
        lblBillContent.Text = BillAdd;
        lblShippContent.Text = ShippAdd;
        Label1.Text = ModifiedUser.ToString();
        //ApprovedUserLabel.Text = ModifiedUser.ToString();
        //InvoiceNoLabel2.Text = oOrderInfo.InvoiceNo.ToString();
        //CreatedByuserLabel.Text = oOrderInfo.Contact.ToString();        
        ShippedbyLabel.Text = oOrderInfo.ShipMethod.ToString();
                        
        //if (BillAdd == "")
        //{
        //    lblBillAdd.Visible = false;
        // }
        if (ShippAdd == "")
        {
            //lblSAdd.Visible = false;
        }
        if (oOrderInfo.ShipCompany == null)
        {
            //lblShipProvider.Visible = false;
            //lblShippProName.Visible = false;
        }
        else
        {
            //lblShippProName.Text = oOrderInfo.ShipCompany;
        }
        if (oOrderInfo.ShipMethod == null)
        {
            //lblShipMethod.Visible = false;
            //lblShipMethodName.Visible = false;
        }
        else
        {
            //lblShipMethodName.Text = oOrderInfo.ShipMethod;
        }

    }
    #region "Function.."
    //public string BuildBillAddress()
    //{
    //    Payment.PayInfo oBillInfo;
    //    oBillInfo = (Payment.PayInfo)Session["BILLING INFO"];

    //    oOrder.GetOrder(OrderID);
    //    string sBillAdd = "";
    //    if (oOrderInfo.BillFName != null)
    //    {
    //        sShippAdd = oOrderInfo.ShipFName + "<br>";
    //        sBillAdd = sBillAdd + oOrderInfo.BillFName + " ";
    //    }
    //    if (oOrderInfo.BillLName != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillLName + "<br>";
    //    }
    //    if (oOrderInfo.BillAdd1 != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd1 + "<br>";
    //    }
    //    if (oOrderInfo.BillAdd2 != "")
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd2 + "<br>";
    //    }
    //    else
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd2;
    //    if (oOrderInfo.BillAdd3 != "")
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd3 + "<br>";
    //    }
    //    else
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillAdd3;
    //    }
    //    if (oOrderInfo.BillCity != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillCity + "<br>";
    //    }
    //    if (oOrderInfo.BillState != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillState + "<br>";
    //    }
    //    if (oOrderInfo.BillCountry != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillCountry + "<br>";
    //    }
    //    if (oOrderInfo.BillZip != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillZip + "<br>";
    //    }
    //    if (oOrderInfo.BillPhone != null)
    //    {
    //        sBillAdd = sBillAdd + oOrderInfo.BillPhone + "<br>";
    //    }
    //    return sBillAdd;
    //}
    public string BuildBillAddress()
    {
        try
        {
            string sBillAdd = "";
            if (oOrderInfo.BillFName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillFName) + " ";
            }
            if (oOrderInfo.BillMName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillMName) + " ";
            }
            if (oOrderInfo.BillLName != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillLName) + "<br>";
            }
            if (oOrderInfo.BillAdd1 != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd1) + "<br>";
            }
            if (oOrderInfo.BillAdd2 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2) + "<br>";
            }
            else
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2);
            if (oOrderInfo.BillAdd3 != "")
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3) + "<br>";
            }
            else
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3);
            }
            if (oOrderInfo.BillCity != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCity) + "<br>";
            }
            if (oOrderInfo.BillState != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillState) + "-";
            }
            if (oOrderInfo.BillZip != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillZip) + "<br>";
            }
            if (oOrderInfo.BillCountry != null)
            {
                sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCountry) + "<br>";
            }
            if (oOrderInfo.BillPhone != null)
            {
                sBillAdd = sBillAdd + "Phone No:" + WrapText(oOrderInfo.BillPhone) + "<br>";
            }
            return sBillAdd;
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            //oErr.CreateLog();
            return null;
        }
    }
    public string BuildShippAddress()
    {
        try
        {
            string sShippAdd = "";

            if (oOrderInfo.ShipFName != null)
            {
                sShippAdd = WrapText(oOrderInfo.ShipFName) + " ";
            }
            if (oOrderInfo.ShipMName != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipMName) + " ";
            }
            if (oOrderInfo.ShipLName != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipLName) + "<br>";
            }
            if (oOrderInfo.ShipAdd1 != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd1) + "<br>";
            }
            if (oOrderInfo.ShipAdd2 != "")
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2) + "<br>";
            }
            else
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2);
            if (oOrderInfo.ShipAdd3 != "")
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3) + "<br>";
            }
            else
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3);
            if (oOrderInfo.ShipCity != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCity) + "<br>";
            }
            if (oOrderInfo.ShipState != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipState) + "-";
            }
            if (oOrderInfo.ShipZip != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipZip) + "<br>";
            }
            if (oOrderInfo.ShipCountry != null)
            {
                sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCountry) + "<br>";
            }
            if (oOrderInfo.ShipPhone != null)
            {
                sShippAdd = sShippAdd + "Phone No:" + WrapText(oOrderInfo.ShipPhone) + "<br>";
            }
            return sShippAdd;
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            //oErr.CreateLog();
            return null;
        }
    }
    public string WrapText(string BillAdd)
    {
        string newline = " \n ";
        if (BillAdd.Length > 50 & BillAdd.Length <= 100)
            BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51) + newline;
        else if (BillAdd.Length > 100 & BillAdd.Length <= 150)
            BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51, 49) + newline + BillAdd.Substring(101);
        return BillAdd;
    }
    #endregion
}
