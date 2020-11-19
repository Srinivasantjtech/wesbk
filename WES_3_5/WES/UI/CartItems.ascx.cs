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
//using TradingBell.Common;
//using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_CartItems : System.Web.UI.UserControl
{
    HelperDB oHelper;
    ErrorHandler oErr;
    Category oCat;
    ProductFamily oPF;
    Order oOrder;
    string imgPath;
    string CatLogo;
    string FamLogo;
    string CartText = "Item(s) in Cart";

    protected void Page_Load(object sender, EventArgs e)
    {
        oHelper = new HelperDB();
        oErr = new ErrorHandler();
        oOrder = new Order();

        int CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
        int OpenOrdStatusID = (int)Order.OrderStatus.OPEN;


        /* To display the No of items in cart*/
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
        {
            int OrderID = 0;
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = oOrder.GetOrderID(oHelper.CI(Session["USER_ID"]), OpenOrdStatusID);
            }

            string OrderStatus = oOrder.GetOrderStatus(OrderID);
            if (OrderID > 0 && (OrderStatus == Order.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
            {
                if (oOrder.GetOrderItemCount(OrderID) == 0)
                    lblItemCount.Text = "No " + CartText;
                else
                    lblItemCount.Text = oOrder.GetOrderItemCount(OrderID) + " " + CartText;
                if (oOrder.GetCurrentProductTotalCost(OrderID) > 0)
                // lblItemCount.Text = oOrder.GetOrderItemCount(OrderID) + "  " + GetGlobalResourceObject("CategoryNavigator", "Items");
                {
                    lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + oOrder.GetCurrentProductTotalCost(OrderID);
                    lblCost.Visible = true;
                }
                else
                    lblCost.Visible = false;
                // lblItemCount.Text = lblItemCount.Text + "  in Cart";
            }
            else
            {
                lblItemCount.Text = "No Item(s) in Cart";
                lblCost.Visible = false;
                //  lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " 0.00";
            }
        }
        else
        {
            lblItemCount.Text = "No Item(s) in Cart";
            lblCost.Visible = false;
            // lblCostValue.Text = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " 0.00";
        }

    }
}
