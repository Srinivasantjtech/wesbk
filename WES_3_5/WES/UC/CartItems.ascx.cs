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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_CartItems : System.Web.UI.UserControl
{
    //HelperDB oHelper;
    //ErrorHandler oErr;
    //CategoryServices  oCat;
    //ProductFamily oPF;

    CategoryServices objCategoryServices =new CategoryServices();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler  objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    OrderServices objOrderServices = new OrderServices();
    string imgPath;
    string CatLogo;
    string FamLogo;
    string CartText = "Item(s) in Cart";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

        int CatalogID = objHelperServices.CI(objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString());
        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
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
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);
            }

            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
            if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
            {
                if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                    lblItemCount.Text = "No " + CartText;
                else
                    lblItemCount.Text = objOrderServices.GetOrderItemCount(OrderID) + " " + CartText;
                if (objOrderServices.GetCurrentProductTotalCost(OrderID) > 0)
                // lblItemCount.Text = objOrderServices.GetOrderItemCount(OrderID) + "  " + GetGlobalResourceObject("CategoryNavigator", "Items");
                {
                    lblCostValue.Text = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + objOrderServices.GetCurrentProductTotalCost(OrderID);
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
                //  lblCostValue.Text = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " 0.00";
            }
        }
        else
        {
            lblItemCount.Text = "No Item(s) in Cart";
            lblCost.Visible = false;
            // lblCostValue.Text = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " 0.00";
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
        }

    }
}
