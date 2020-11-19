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
using System.Text.RegularExpressions;
using System.Collections.Generic;
//using System.Windows.Forms;
using TradingBell.WebCat;
using GCheckout.Checkout;
using GCheckout.Util;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class ProductFilter : System.Web.UI.Page
{

    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();

    protected void Page_PreInit()
    {
      
        Page.MasterPageFile = "~/AddtoCardPopup.Master";
       
    }
}


