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
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public partial class UI_QuickOrder1 : System.Web.UI.UserControl
{
    #region "Object Declaration"
    HelperDB oHelper = new HelperDB();
    
    ErrorHandler oErr = new ErrorHandler();
    Order oOrder = new Order();
    User oUser = new User();
    ConnectionDB oCon = new ConnectionDB();
    Product oProd = new Product();
    #endregion "Object Declaration"

    #region "Variable Declaration"
    int[] ProdID = new int[6];
    int[] ProdQnty = new int[6];
    int i = 0;
    #endregion "Variable Declaration"

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnAddtoCart_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string TempProdQtys;
            string TempProdItems;
            TempProdQtys = HidQty.Value.ToString();
            TempProdItems = HidItemCode.Value.ToString();

            if (TempProdQtys != string.Empty && TempProdItems != string.Empty)
            {
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    Session["QCKORDER_QTY"] = TempProdQtys;
                    Session["QCKORDER_CODE"] = TempProdItems;
                    Response.Redirect("BulkOrder.aspx?txtcnt=5",false);
                }
            }
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            oErr.CreateLog();
        }
    }

    protected void btnCPAddtoCart_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string strItems = "";
            string[] AItems = new string[50];
            string[] TempItems = new string[50];
            string ItemCode = "";
            string ItemQty = "";
            int ItemCount = 0;
            strItems = txtCopyPaste.Value.ToString();
            if (strItems.Trim() != string.Empty)
            {
                AItems = Regex.Split(strItems, "\r\n");
                for (int i = 0; i < AItems.Length; i++)
                {
                    if (AItems[i].Contains(",") == true)
                    {
                        TempItems = Regex.Split(AItems[i].ToString(), ",");
                    }
                    else if (AItems[i].Contains("\t") == true)
                    {
                        TempItems = Regex.Split(AItems[i].ToString(), "\t");
                    }
                    if ((TempItems != null) && (TempItems.Length >= 2) && (TempItems[0] != null) && (TempItems[1] != null) && (TempItems[0] != string.Empty) && (TempItems[1] != string.Empty) && (TempItems[0] != "") && (TempItems[1] != ""))
                    {
                        ItemQty = ItemQty + TempItems[0].ToString().Trim() + ",";
                        ItemCode = ItemCode + TempItems[1].ToString().Trim() + ",";
                        TempItems[0] = "";
                        TempItems[1] = "";
                        ItemCount++;
                    }
                }
            }

            if (ItemCode != string.Empty && ItemQty != string.Empty)
            {
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    Session["QCKORDER_QTY"] = ItemQty;
                    Session["QCKORDER_CODE"] = ItemCode;
                    Response.Redirect("BulkOrder.aspx?txtcnt=" + ItemCount.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            oErr.CreateLog();
        }
    }
}
