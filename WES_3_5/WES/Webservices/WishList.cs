using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;


/// <summary>
/// Summary description for WishList
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WishList : System.Web.Services.WebService 
{
    Helper oHelper = new Helper();
    ErrorHandler oErrHand = new ErrorHandler();
    User oUser = new User();
    
    public struct WishListInfo
    {
        public int WishListID;
        public int UserID;
        public int WishListStatus;
        public decimal ProdTotalPrice;
    }

    public WishList () 
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    public struct WishListItemInfo
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public int WishListID;
        /// <summary>
        /// Users's ID
        /// </summary>
        public int UserID;
        /// <summary>
        /// Product ID
        /// </summary>
        public int ProductID;
        /// <summary>
        /// Product's Quantity
        /// </summary>
        public decimal Quantity;
        /// <summary>
        /// Price Applied for each Product
        /// </summary>
        public decimal PriceApplied;
    }

    public enum ProductStatus
    {
        /// <summary>
        /// Available Products
        /// </summary>
        AVALIABLE = 1,
        /// <summary>
        /// Not Available Products
        /// </summary>
        NA = 2,
        /// <summary>
        /// Discontinued Products
        /// </summary>
        DISCOUNTINUED = 3
    }

    public enum WishListStatus
    {
        OPEN = 1,
        CLOSED = 2,
    }

    [WebMethod]

    public string HelloWorld() 
    {
        return "Hello World";
    }

    #region "WishList Data Retrieve Functions"

    public int InitilizeWishList(WishListInfo oInfo)
    {
        try
        {
            string sSQL = " INSERT INTO TBWC_WISHLIST(USER_ID,WISHLIST_STATUS,CREATED_USER,MODIFIED_USER)";
            sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1," + oInfo.UserID + "," + oInfo.UserID + " )";
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return -1;
        }
    }

    public int CreateWishList(WishListInfo oInfo)
    {
        try
        {
            string sSQL = " INSERT INTO TBWC_WISHLIST(USER_ID,WISHLIST_STATUS,CREATED_USER,MODIFIED_USER)";
            sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1," + oInfo.UserID + "," + oInfo.UserID + " )";
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return -1;
        }
    }

    public WishListInfo GetWishList(int WishListID)
    {
        string sSQL;
        DataSet dsOD = new DataSet();
        WishListInfo rInfo = new WishListInfo();
        try
        {
            sSQL = "SELECT * FROM TBWC_WISHLIST WHERE WISHLIST_ID = " + WishListID;
            oHelper.SQLString = sSQL;
            dsOD = oHelper.GetDataSet("WishList");
            foreach (DataRow drOD in dsOD.Tables["WishList"].Rows)
            {
                rInfo.WishListID = Convert.ToInt16(drOD["WISHLIST_ID"]);
                rInfo.UserID = Convert.ToInt16(drOD["USER_ID"]);
                rInfo.WishListStatus = Convert.ToInt16(drOD["WISHLIST_STATUS"]);
                rInfo.ProdTotalPrice = Convert.ToDecimal(drOD["PRODUCT_TOTAL_PRICE"]);
            }
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
        }
        return rInfo;
    }

    public int UpdateWishList(WishListInfo oWishListInfo)
    {
        try
        {
            string sSQL;
            sSQL = "UPDATE TBWC_WISHLIST SET PRODUCT_TOTAL_PRICE=" + oWishListInfo.ProdTotalPrice;
            sSQL = sSQL + ",MODIFIED_USER=" + oWishListInfo.UserID + " WHERE WISHLIST_ID=" + oWishListInfo.WishListID;
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return -1;
        }
    }

    public int UpdateRemovedItemsPrice(decimal RemovedItemsPrice, int WishListID)
    {
        int retVal;
        try
        {
            string sSQL = "UPDATE TBWC_WISHLIST SET PRODUCT_TOTAL_PRICE = PRODUCT_TOTAL_PRICE - " + RemovedItemsPrice + " WHERE WISHLIST_ID = " + WishListID;
            oHelper.SQLString = sSQL;
            retVal = oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            retVal = -1;
        }
        return retVal;
    }

    public int UpdateWishListStatus(int WishListID, int WListStatus)
    {
        int retVal = 0;
        try
        {
            string sSQL = "UPDATE TBWC_WISHLIST SET WISHLIST_STATUS = " + WListStatus + " WHERE WISHLIST_ID = " + WishListID;
            oHelper.SQLString = sSQL;
            retVal = oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return retVal;
        }
        return retVal;
    }

    public int GetWishListID(int UserID, int WishListStatus)
    {
        try
        {
            string WlistID;
            string sSQL = "SELECT WISHLIST_ID FROM TBWC_WISHLIST WHERE USER_ID = " + UserID + " AND WISHLIST_STATUS = " + WishListStatus;
            oHelper.SQLString = sSQL;
            WlistID = oHelper.GetValue("WISHLIST_ID");
            if (WlistID == "")
            {
                return 0;
            }
            else
            {
                return oHelper.CI(WlistID);
            }
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return -1;
        }
    }

    public DataSet GetWishListPriceValue(int WishListID)
    {
        try
        {
            string sSQL = " SELECT PRODUCT_TOTAL_PRICE FROM TBWC_WISHLIST WHERE WISHLIST_ID =" + WishListID;
            oHelper.SQLString = sSQL;
            return oHelper.GetDataSet();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return null;
        }
    }

    public DataSet GetWishListDetails(int WishListID)
    {
        try
        {
            string sSQL;
            sSQL = "SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = P.PRODUCT_ID) AS CATALOG_ITEM_NO,";
            sSQL = sSQL + "OI.QTY,OI.PRICE_APPLIED FROM TB_PRODUCT P,";
            sSQL = sSQL + "TBWC_WISHLIST_ITEM OI WHERE P.PRODUCT_ID=OI.PRODUCT_ID AND OI.WISHLIST_ID=" + WishListID;
            oHelper.SQLString = sSQL;
            return oHelper.GetDataSet("WishListDetails");
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return null;
        }
    }

    public string GetWishListStatus(int WishListID)
    {
        string sSQL;
        int StatusValue = 0;
        string sOrderStatus = "";
        DataSet dsOS = new DataSet();

        try
        {
            sSQL = "SELECT WISHLIST_STATUS FROM TBWC_WISHLIST WHERE WISHLIST_ID=" + WishListID;
            oHelper.SQLString = sSQL;
            dsOS = oHelper.GetDataSet("WishListStatus");
            if (dsOS != null)
            {
                foreach (DataRow oDR in dsOS.Tables["WishListStatus"].Rows)
                {
                    StatusValue = (int)oDR["WISHLIST_STATUS"];
                }
            }
            if (StatusValue > 0)
            {
                sOrderStatus = Enum.GetName(typeof(WishListStatus), StatusValue);
            }
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return null;
        }
        return sOrderStatus;
    }

    public decimal GetCurrentProductTotalCost(int WishListID)
    {
        decimal retVal = 0;
        try
        {
            string sSQL = "SELECT PRODUCT_TOTAL_PRICE FROM TBWC_WISHLIST WHERE WISHLIST_ID =" + WishListID;
            oHelper.SQLString = sSQL;
            retVal = oHelper.CDEC(oHelper.GetValue("PRODUCT_TOTAL_PRICE"));
            if (retVal == -1)
            {
                retVal = 0;
            }
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            retVal = -1;
        }
        return retVal;
    }
    #endregion

    #region "WishList Item Functions.."

    public int AddWishListItem(WishListItemInfo oItem)
    {
        try
        {
            string sSQL;
            sSQL = "INSERT INTO TBWC_WISHLIST_ITEM(WISHLIST_ID,PRODUCT_ID,QTY,PRICE_APPLIED,CREATED_USER,MODIFIED_USER) ";
            sSQL = sSQL + "VALUES(" + oItem.WishListID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",";
            sSQL = sSQL + oItem.UserID + "," + oItem.UserID + ")";
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return -1;
        }
    }

    public DataSet GetWishListItems(int WishListID)
    {
        try
        {
            string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TOI.WISHLIST_ID,TOI.PRODUCT_ID,TOI.QTY,TOI.PRICE_APPLIED,";
            sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,";
            sSQL = sSQL + " PRODUCT_STATUS";
            sSQL = sSQL + " FROM TBWC_WISHLIST_ITEM TOI,TBWC_INVENTORY TI,TB_PRODUCT TP";
            sSQL = sSQL + " WHERE TOI.WISHLIST_ID =" + WishListID;
            sSQL = sSQL + " AND TOI.PRODUCT_ID = TI.PRODUCT_ID AND TOI.PRODUCT_ID = TP.PRODUCT_ID";
            oHelper.SQLString = sSQL;
            return oHelper.GetDataSet();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            oErrHand.CreateLog();
            return null;
        }
    }

    public int GetWishListItemCount(int WishListID)
    {
        try
        {
            string sSQL = "SELECT COUNT(PRODUCT_ID)AS ITEMCOUNT FROM TBWC_WISHLIST_ITEM WHERE WISHLIST_ID = " + WishListID;
            oHelper.SQLString = sSQL;
            return oHelper.CI(oHelper.GetValue("ITEMCOUNT"));
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            return -1;
        }
    }

    public decimal GetProductTotalCost(int WishListID)
    {
        decimal retVal = 0;
        try
        {
            string sSQL = "SELECT SUM(PRICE_APPLIED)AS TOTALAMOUNT FROM TBWC_WISHLIST_ITEM WHERE WISHLIST_ID =" + WishListID;
            oHelper.SQLString = sSQL;
            retVal = oHelper.CDEC(oHelper.GetValue("TOTALAMOUNT"));
            if (retVal < 0) retVal = 0.00M;
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            retVal = -1;
        }
        return retVal;
    }

    public int GetWishListItemQty(int ProductID, int WishListID)
    {
        int retVal = 0;
        try
        {
            string sSQL = "SELECT QTY FROM TBWC_WISHLIST_ITEM WHERE PRODUCT_ID = " + ProductID + " AND WISHLIST_ID =" + WishListID;
            oHelper.SQLString = sSQL;
            retVal = oHelper.CI(oHelper.GetValue("QTY"));
        }
        catch (Exception e)
        {

            oErrHand.ErrorMsg = e;
            retVal = -1;
        }
        return retVal;
    }

    public int UpdateWishListItem(WishListItemInfo WLItemInfo)
    {
        try
        {

            string sSQL = "UPDATE TBWC_WISHLIST_ITEM SET QTY=" + WLItemInfo.Quantity + ",PRICE_APPLIED=" + WLItemInfo.PriceApplied;
            sSQL = sSQL + " WHERE PRODUCT_ID=" + WLItemInfo.ProductID + " AND WISHLIST_ID =" + WLItemInfo.WishListID;
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            return -1;
        }
    }

    public int RemoveItem(string ProductID, int WishListID)
    {
        try
        {
            string sSQL;
            if (ProductID == "AllProd")
            {
                sSQL = "DELETE FROM TBWC_WISHLIST_ITEM WHERE WISHLIST_ID=" + WishListID;
            }
            else
            {
                sSQL = "DELETE FROM TBWC_WISHLIST_ITEM WHERE PRODUCT_ID IN(" + ProductID + ") AND WISHLIST_ID=" + WishListID;
            }
            oHelper.SQLString = sSQL;
            return oHelper.ExecuteSQLQuery();
        }
        catch (Exception e)
        {
            oErrHand.ErrorMsg = e;
            return -1;
        }
    }
    #endregion
}

