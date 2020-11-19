using System;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebServices
{
    /// <summary>   
    /// This is used to get all the Product Details 
    /// </summary>
    ///<remarks>
    /// Used to get the Product List,Produt Family List,Attributes of the Product ,Specifications and Descriptions of the Products..
    ///</remarks>
    ///<example>
    /// Product oProd = new Product();
    ///</example>


    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]


    public class Product : System.Web.Services.WebService
    {
        HelperDB oHelper = new HelperDB();
        ErrorHandler oErrHand = new ErrorHandler();
        ConnectionDB oCon = new ConnectionDB();
        string sCategoryNames = "";
        string retVal = "";

        /// <summary>   
        /// Default constructor.
        /// </summary>

        public Product()
        {

        }

        # region "Functions"
        /// <summary>   
        /// This is used to get all the Product Details using Product ID 
        /// </summary>
        /// <param name="productID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int productID;
        ///     DataSet oDS = new DataSet():
        ///     ...
        ///     oDS = oProd.GetProduct(productID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProduct(int productID)
        {
            try
            {
                string sSQL = " SELECT TP.PRODUCT_ID, TP.CATEGORY_ID, ";
                sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =2 AND PRODUCT_ID = TP.PRODUCT_ID) AS MFG_PART_NO,";
                sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =4 AND PRODUCT_ID = TP.PRODUCT_ID) AS SUPPLIER_NAME,";
                sSQL = sSQL + " (SELECT NUMERIC_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =3 AND PRODUCT_ID = TP.PRODUCT_ID) AS BASE_PRICE,  ";
                sSQL = sSQL + " TPF.FAMILY_ID FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF ";
                sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' AND  TP.PRODUCT_ID = " + productID;
                sSQL = sSQL + " ORDER BY SORT_ORDER ";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the Product Details 
        /// </summary>
        /// <returns>DataTable</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetProductList();
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataTable GetProductList()
        {
            try
            {
                DataTable dtProd = new DataTable();
                string sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID,";
                sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                sSQL = sSQL + " TPF.FAMILY_ID AS FAMILY_ID ";
                sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF";
                sSQL = sSQL + " WHERE TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'";
                sSQL = sSQL + " AND TPF.FAMILY_ID > 0";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the Product List using core Attributes
        /// </summary>
        /// <param name="ProdID">int</param>
        /// <param name="CoreAttribute1">string</param>
        /// <param name="CoreAttribute2">string</param>
        /// <param name="CoreAttribute4">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string CoreAttribute_1;
        ///     string CoreAttribute_2;
        ///     string CoreAttribute_4;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductList(0, CoreAttribute_1, CoreAttribute_2, CoreAttribute_4);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductList(int ProdID, string CoreAttribute1, string CoreAttribute2, string CoreAttribute4)
        {
            try
            {
                DataTable dtProd = new DataTable();
                string sSQL = "";

                if (ProdID == 0)
                {
                    sSQL = " SELECT PRODUCT_ID AS PRODUCT_ID, CATEGORY_ID,";
                    sSQL = sSQL + " CATALOG_ITEM_NO AS '" + CoreAttribute1 + "', MFG_PART_NO AS '" + CoreAttribute2 + "', SUPPLIER_NAME AS '" + CoreAttribute4 + "'";
                    sSQL = sSQL + " FROM TB_PRODUCT ";
                }
                else
                {
                    sSQL = " SELECT PRODUCT_ID AS PRODUCT_ID, CATEGORY_ID,";
                    sSQL = sSQL + " CATALOG_ITEM_NO AS '" + CoreAttribute1 + "', MFG_PART_NO AS '" + CoreAttribute2 + "', SUPPLIER_NAME AS '" + CoreAttribute4 + "'";
                    sSQL = sSQL + " FROM TB_PRODUCT WHERE PRODUCT_ID IN(" + ProdID + ")";
                }
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the product list
        /// </summary>
        /// <param name="SupplierName">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string SupplierName;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductList(SupplierName);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductList(string SupplierName)
        {
            try
            {
                DataSet oDS = GetCoreAttributesName();
                string CoreAttr1 = oDS.Tables[0].Rows[0].ItemArray[0].ToString();

                string sSQL = " SELECT PRODUCT_ID,SUPPLIER_NAME AS 'SUPPLIER NAME',CATALOG_ITEM_NO AS '" + CoreAttr1 + "' FROM TB_PRODUCT WHERE SUPPLIER_NAME = '" + SupplierName + "'";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog()
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the family product list
        /// If restricted product is Enabled that product is not able to Purchase.
        /// 
        /// </summary>
        /// <param name="ProductFamilyID">int</param>
        /// <param name="EnabledRestrictedProducts">bool</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductFamilyID;
        ///     bool EnabledRestrictedProducts;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetFamilyProductList(ProductFamilyID, EnabledRestrictedProducts,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetRestrictedFamilyProductList(int FamilyID, bool EnabledRestrictedProducts,int CatalogID)
        {
            try
            {
                string sSQL = "";
                if (EnabledRestrictedProducts)
                {
                    sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID, TP.CATEGORY_ID,TPF.FAMILY_ID,TBI.MIN_ORD_QTY AS MIN_ORD_QTY,";
                    sSQL = sSQL + " TBI.QTY_AVAIL AS QTY_AVAIL,TBI.IS_SHIPPING AS IS_SHIPPING, TPS.STRING_VALUE AS RESTRICTED";
                    sSQL = sSQL + " FROM TB_PRODUCT TP,TB_PROD_FAMILY TPF, TBWC_INVENTORY TBI, TB_PROD_SPECS TPS,TB_CATALOG_PRODUCT TCP ";
                    sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' AND TPS.PRODUCT_ID=TP.PRODUCT_ID";
                    sSQL = sSQL + " AND TBI.PRODUCT_ID=TPF.PRODUCT_ID AND TBI.PRODUCT_STATUS='AVAILABLE'AND TPF.FAMILY_ID IN(" + FamilyID + " )";
                    sSQL = sSQL + " AND TPS.ATTRIBUTE_ID = (SELECT ATTRIBUTE_ID AS RESTRICTED FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME ='~Restricted')";
                    sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID =" + CatalogID;
                    sSQL = sSQL + " ORDER BY SORT_ORDER";
                }
                else
                {
                    sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID, TP.CATEGORY_ID,TPF.FAMILY_ID,TBI.MIN_ORD_QTY AS MIN_ORD_QTY,TBI.QTY_AVAIL AS QTY_AVAIL,TBI.IS_SHIPPING AS IS_SHIPPING ";
                    sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF, TBWC_INVENTORY TBI,TB_CATALOG_PRODUCT TCP WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'";
                    sSQL = sSQL + " AND TBI.PRODUCT_ID=TPF.PRODUCT_ID AND TBI.PRODUCT_STATUS='AVAILABLE' AND TPF.FAMILY_ID IN(" + FamilyID + " )"; 
                    sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID ="+ CatalogID;
                    sSQL =sSQL + " ORDER BY SORT_ORDER";
                }
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();

            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }

        /// <summary>   
        /// This is used to get the Restricted Products from the Products List
        /// </summary>
        /// <param name="ProductID">int</param> 
        /// <returns>string</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     string retVal;
        ///     ...
        ///     retVal = oProd.GetRestrictedProduct(ProductID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]

        public string GetRestrictedProduct(int ProductID)
        {
            string retVal = "";
            try
            {
                string sSQL = "SELECT TPS.STRING_VALUE AS RESTRICTED FROM TB_PRODUCT TP,TB_PROD_FAMILY TPF, TB_PROD_SPECS TPS ";
                sSQL = sSQL + "WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' ";
                sSQL = sSQL + "AND TPS.PRODUCT_ID=TP.PRODUCT_ID ";
                sSQL = sSQL + "AND TPS.PRODUCT_ID IN(" + ProductID + " )";
                sSQL = sSQL + "AND TPS.ATTRIBUTE_ID = (SELECT ATTRIBUTE_ID AS RESTRICTED FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME ='~Restricted') ";
                oHelper.SQLString = sSQL;
                retVal = oHelper.GetValue("RESTRICTED");
                return retVal;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                return "";
            }
            //return retVal;
        }

        /// <summary>   
        /// This is used to get all the product attribute values using family and product id
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="FamilyID">int</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int FamilyID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductsAttributesValues(ProductID,FamilyID,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductsAttributesValues(int ProductID, int FamilyID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE  FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
                sSQL = sSQL + " WHERE TPS.PRODUCT_ID =" + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID IN(1,2,4)AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " UNION";
                sSQL = sSQL + " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
                sSQL = sSQL + " WHERE TPS.PRODUCT_ID = " + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID <>3";
                sSQL = sSQL + " AND TA.ATTRIBUTE_TYPE =1 AND TA.PUBLISH2WEB=1 AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_PROD_FAMILY_ATTR_LIST WHERE FAMILY_ID=" + FamilyID + ")";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID; 
                sSQL = sSQL + " ORDER BY TPS.PRODUCT_ID,TA.ATTRIBUTE_ID";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();

            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the product attribute values
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int CatalogID;
        ///     DataSet oDS =new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductsAllAttributesValues(ProductID,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductsAllAttributesValues(int ProductID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE  FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                sSQL = sSQL + " WHERE TPS.PRODUCT_ID =" + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID IN(1,2,4)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " UNION";
                sSQL = sSQL + " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE   FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                sSQL = sSQL + " WHERE TPS.PRODUCT_ID = " + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID <>3";
                sSQL = sSQL + " AND TA.ATTRIBUTE_TYPE =1 AND TA.PUBLISH2WEB=1 AND TPS.STRING_VALUE <>''";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID;
                sSQL = sSQL + " ORDER BY TPS.PRODUCT_ID,TA.ATTRIBUTE_ID";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();

            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>   
        /// This is used to get all the Products Specification details using attribute ID 
        /// </summary>
        /// <param name="productID">int</param>
        /// <param name="AttributesID">string</param>
        /// <returns>Dataset</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int productID;
        ///     string AttributesID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductSpecsValues(productID, AttributesID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductSpecsValues(int productID, String AttributesID)
        {

            try
            {
                string sSQL = "SELECT ATTRIBUTE_ID,STRING_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
                sSQL = sSQL + " WHERE ATTRIBUTE_ID IN( " + AttributesID + ")";
                sSQL = sSQL + " AND PRODUCT_ID = " + productID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the Products Specification Details
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="isValues">bool</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     bool isValues;
        ///     int CatalogID
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductSpecsValues(ProductID,isValues,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductSpecsValues(int ProductID, bool isValues,int CatalogID)
        {
            try
            {
                string sSQL = "";
                if (isValues)
                {
                    sSQL = " SELECT TPS.STRING_VALUE AS ATTRIBUTE_VALUE, TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID ";
                    sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
                    sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                    sSQL = sSQL + " AND TA.PUBLISH2WEB = 1 AND TA.ATTRIBUTE_ID IN (1,2,4) AND PRODUCT_ID = " + ProductID;

                }
                else
                {
                    sSQL = "SELECT  TPS.STRING_VALUE AS ATTRIBUTE_VALUE, TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID  ";
                    sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID ";
                    sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID + " AND TA.PUBLISH2WEB = 1 AND PRODUCT_ID = " + ProductID;
                }
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        [WebMethod]
        public int GetMaxAvailQty(int ProductID)
        {
            int retVal = 0;
            try
            {
                string sSQL = "SELECT QTY_AVAIL FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProductID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.CI(oHelper.GetValue("QTY_AVAIL"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                retVal = -1;
            }
            return retVal;
        }

        [WebMethod]
        public int GetMinOrdQty(int ProductID)
        {
            int retVal = 0;
            try
            {
                string sSQL = "SELECT MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProductID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.CI(oHelper.GetValue("MIN_ORD_QTY"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                retVal = -1;
            }
            return retVal;
        }

        /// <summary>   
        /// This is used to get the Product Image
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="CatalogID">int</param>    
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductImages(ProductID,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductImages(int ProductID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TPS.OBJECT_TYPE, TPS.STRING_VALUE,TPS.OBJECT_NAME,TA.ATTRIBUTE_NAME,";
                sSQL = sSQL + " TPS.ATTRIBUTE_ID FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND STRING_VALUE <>''";
                sSQL = sSQL + " AND TPS.OBJECT_TYPE IS NOT NULL AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " AND TA.PUBLISH2WEB = 1 AND PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the Product Base Price 
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>Double</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     Double price;
        ///     ...
        ///     price = oProd.GetProductBasePrice(ProductID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public Double GetProductBasePrice(int ProductID)
        {
            double price = 0.0;
            try
            {
                string sSQL = " SELECT ISNULL(NUMERIC_VALUE,0) AS BASE_PRICE FROM TB_PROD_SPECS";
                sSQL = sSQL + " WHERE PRODUCT_ID = " + ProductID + " AND ATTRIBUTE_ID =4";
                OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                price = oHelper.CD(oCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                price = -1;
            }
            return price;
        }
        /// <summary>   
        /// This is used to get the Products Base Price 
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductBasePriceValues(ProductID);
        /// }   
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductBasePriceValues(int ProductID)
        {
            try
            {
                string sSQL = " SELECT ATTRIBUTE_ID,NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
                sSQL = sSQL + " WHERE PRODUCT_ID =" + ProductID + " AND ATTRIBUTE_ID = 3 ";

                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the Products Price Values based on Product ID and Attribute ID(string).
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="AttributesID">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     String AttributesID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductPriceValues(oHelper.CI(rProd["PRODUCT_ID"]), AttributesID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductPriceValues(int ProductID, String AttributesID)
        {
            try
            {
                string sSQL = "SELECT ATTRIBUTE_ID,NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
                sSQL = sSQL + " WHERE ATTRIBUTE_ID IN( " + AttributesID + ")";
                sSQL = sSQL + " AND PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the Products Price Values based on Product ID and Attribute ID
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="AttributeID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int BGPriceID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductPriceValue(ProductID, BGPriceID);
        ///     /// Where BGPriceID is the Buyer Group Price Attribute ID
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProductPriceValue(int ProductID, int AttributeID)
        {

            try
            {
                string sSQL = " SELECT TA.ATTRIBUTE_NAME,TPS.NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA";
                sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND TPS.ATTRIBUTE_ID =" + AttributeID + " AND TPS.PRODUCT_ID =" + ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
       
        /// <summary>   
        /// This is used to get the price value of the Buyer Group for Every User
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int UserID;
        ///     int retPriceID;
        ///     ...
        ///     retPriceID = oProd.GetBuyerGroupPrice(UserID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public int GetBuyerGroupPrice(int UserID)
        {
            int retPriceID = 0;
            try
            {
                string sSQL = "";
                if (UserID > 0)
                {
                    sSQL = " SELECT TBG.PRICE_ATTRIBUTE_ID FROM TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB,TBWC_BUYER_GROUP TBG";
                    sSQL = sSQL + " WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TBG.BUYER_GROUP = TC.BUYER_GROUP";
                    sSQL = sSQL + " AND TCB.USER_ID = " + UserID;
                }
                else if (UserID == 0)
                {
                    sSQL = "SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = 'DEFAULTBG'";
                }
                oHelper.SQLString = sSQL;
                retPriceID = oHelper.CI(oHelper.GetValue("PRICE_ATTRIBUTE_ID"));
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retPriceID = -1;
            }
            return retPriceID;
        }
        /// <summary>   
        /// This is used to get the Product ID
        /// </summary>
        /// <param name="CatalogItemNo">string</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string CatalogItemNo;
        ///     int ProductID;
        ///     ...
        ///     ProductID = oProd.GetProductID(CatalogItemNo);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public int GetProductID(string CatalogItemNo)
        {
            int ProductID = 0;
            try
            {
                string sSQL = " SELECT PRODUCT_ID FROM TB_PRODUCT";
                sSQL = sSQL + " WHERE CATALOG_ITEM_NO = N'" + CatalogItemNo + "'";
                OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                ProductID = oHelper.CI(oCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
            return ProductID;
        }
        /// <summary>   
        /// This is used to get the Document Details based on Product ID
        /// <param name="productID">int</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int productID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetDocuments(productID,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetDocuments(int productID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TPS.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TPS.OBJECT_NAME AS IMAGENAME,STRING_VALUE AS IMAGEFILE,TPS.OBJECT_TYPE AS IMAGETYPE ";
                sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                sSQL = sSQL + " WHERE TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID";
                sSQL = sSQL + " AND PRODUCT_ID = " + productID + " AND TPS.STRING_VALUE <>''AND TPS.OBJECT_TYPE ='pdf' ";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =3 AND PUBLISH2WEB =1 )";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " ORDER BY TA.ATTRIBUTE_NAME";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the Product Count based on family ID 
        /// </summary>
        /// <param name="familyID">int</param>
        /// <param name="CatalogID">int</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int familyID;
        ///     int CatalogID;
        ///     int productCount;
        ///     ...
        ///     productCount = oProd.GetProductCount(familyID,CatalogID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public int GetProductCount(int familyID,int CatalogID)
        {
            int productCount = 0;
            try
            {
                string sSQL = " SELECT COUNT(TP.PRODUCT_ID) AS PRODUCTCOUNT";
                sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF,TBWC_INVENTORY TI,TB_CATALOG_PRODUCT TCP";
                sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TI.PRODUCT_ID = TP.PRODUCT_ID";
                sSQL = sSQL + " AND  TPF.FAMILY_ID = " + familyID;
                sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID =" + CatalogID;
                //OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                oHelper.SQLString = sSQL;
                productCount =oHelper.CI(oHelper.GetValue("PRODUCTCOUNT").ToString());
                //productCount = oHelper.CI(oCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
            return productCount;
        }
        /// <summary>   
        /// This is used to get all the Product Details
        /// </summary>
        /// <param name="CateID">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string CateID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProducts(CateID);
        /// }
        /// </code>
        /// </example>

        [WebMethod]
        public DataSet GetProducts(string CateID)
        {
            try
            {
                string sSQL = "SELECT product_id FROM tb_prod_family WHERE family_id in( ";
                sSQL = sSQL + "SELECT DISTINCT(family_id) FROM tb_family WHERE category_id ='" + CateID + "')";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }


        [WebMethod]
        public DataSet GetProducts(int familyId,int catalogID)
        {
            try
            {
                string sSQL = "SELECT TPF.PRODUCT_ID FROM TB_PROD_FAMILY TPF,TB_CATALOG_PRODUCT TCP WHERE TPF.FAMILY_ID=" + familyId +" AND TPF.PRODUCT_ID IN(SELECT PRODUCT_ID FROM TBWC_INVENTORY WHERE PRODUCT_STATUS='AVAILABLE') AND TCP.PRODUCT_ID=TPF.PRODUCT_ID AND TCP.CATALOG_ID="+catalogID;                
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }

        [WebMethod]
        public DataSet GetProductsSpecs(string product_id)
        {
            
            try
            {
                string cmdd = "SELECT STRING_VALUE,PRODUCT_ID,ATTRIBUTE_ID,NUMERIC_VALUE,OBJECT_TYPE,[OBJECT_NAME],(SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = PS.ATTRIBUTE_ID) AS ATTRIBUTE_TYPE FROM TB_PROD_SPECS PS  WHERE PRODUCT_ID=" + product_id ;
                oHelper.SQLString = cmdd;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
            
        }
        /// <summary>   
        /// This is used to get the Category Name using Product ID 
        /// </summary>
        /// <param name="isHirearchy">int</param> 
        /// <param name="ProductID">bool</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     bool isHirearchy;
        ///     string retVal;
        ///     ...
        ///     retVal = oProd.GetCategoryName(ProductID,isHirearchy);
        /// }
        /// </code>
        ///</example>  


        [WebMethod]
        public string GetCategoryName(int ProductID, bool isHirearchy)
        {
            string retVal;
            try
            {
                string sSQL = "SELECT CATEGORY_ID FROM TB_PRODUCT WHERE PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.GetValue("CATEGORY_ID");
                if (isHirearchy)
                {
                    retVal = GetParentCategory(retVal);
                }
                else
                {
                    oHelper.SQLString = " SELECT CATEGORY_NAME FROM TB_CATEGORY WHERE CATEGORY_ID ='" + retVal + "'";
                    retVal = oHelper.GetValue("CATEGORY_NAME");
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retVal = null;
            }

            return retVal;
        }
        /// <summary>   
        /// This is used to get the Parent Category Name
        /// </summary>
        /// <param name="CategoryID">string</param> 
        /// <returns>string</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string CategoryID;
        ///     string _ParentCategory;
        ///     ...
        ///     _ParentCategory = oProd.GetParentCategory(CategoryID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetParentCategory(string CategoryID)
        {
            
            try
            {
                string _ParentCategory;
                string sSQL = "SELECT CATEGORY_NAME, PARENT_CATEGORY FROM TB_CATEGORY WHERE CATEGORY_ID = '" + CategoryID + "' ";
                oHelper.SQLString = sSQL;
                _ParentCategory = oHelper.GetValue("PARENT_CATEGORY");
                if (_ParentCategory != "0")
                {
                    retVal = ">>" + oHelper.GetValue("CATEGORY_NAME") + retVal;
                    sCategoryNames = retVal;
                    GetParentCategory(_ParentCategory);
                }
                retVal = oHelper.GetValue("CATEGORY_NAME") + sCategoryNames;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retVal = null;
            }
            return retVal;
        }
        /// <summary>   
        /// This is used to get the attribute names
        /// </summary>
        /// <param name="CatalogID">int</param>
        ///<returns>DataTable</returns> 
        ///<example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     int CatalogID;
        ///     Product oProd = new Product();
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetAttributesName(CatalogID);
        /// }
        /// </code>
        ///</example> 

        [WebMethod]
        public DataTable GetAttributesName(int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE ";
                sSQL = sSQL + " FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_ID IN(1,2,4)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " UNION";
                sSQL = sSQL + " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_TYPE IN(1,2)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID <> 3 AND TA.PUBLISH2WEB =1";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID ;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the attribute name using Family ID
        /// </summary>
        /// <param name="FamilyID">int</param> 
        /// <returns>DataTable</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int FamilyID;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetAttributesName(FamilyID);
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataTable GetAttributesName(int FamilyID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE ";
                sSQL = sSQL + " FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_ID IN(1,2,4)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " UNION";
                sSQL = sSQL + " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_TYPE IN(1,2)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID <> 3 AND TA.PUBLISH2WEB =1";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN (SELECT DISTINCT ATTRIBUTE_ID FROM TB_PROD_FAMILY_ATTR_LIST WHERE FAMILY_ID =" + FamilyID + ")";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;

                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get the Price Attribute Name based on the Price ID
        /// </summary>
        /// <param name="BGPriceID">int</param> 
        /// <returns>DataTable</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     int BGPriceID;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetBGAttributesName(BGPriceID);
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataTable GetBGAttributesName(int BGPriceID)
        {
            try
            {
                string sSQL = " SELECT ATTRIBUTE_ID AS ATTRIBUTEID,ATTRIBUTE_NAME AS ATTRIBUTENAME,ATTRIBUTE_TYPE AS ATTRIBUTETYPE";
                sSQL = sSQL + " FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + BGPriceID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get core attributes(itemno,suppliername,mfg,price)names
        /// </summary>
        /// <returns>DataSet</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetCoreAttributesName();
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataSet GetCoreAttributesName()
        {
            try
            {
                oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN(1,2,4)AND PUBLISH2WEB=1";
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>   
        /// This is used to get all the Supplier Name Index
        /// </summary>
        /// <returns>DataSet</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetSupplierNameIndex();
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataSet GetSupplierNameIndex()
        {
            DataSet dsRet = new DataSet();
            try
            {
                string sSQL = " SELECT DISTINCT(SUBSTRING(STRING_VALUE,1,1))AS SUPPLIERNAMEINDEX  FROM TB_PROD_SPECS WHERE STRING_VALUE <>''AND ATTRIBUTE_ID=4";
                sSQL = sSQL + " ORDER BY SUBSTRING(STRING_VALUE,1,1)";
                oHelper.SQLString = sSQL;
                dsRet = oHelper.GetDataSet();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                dsRet = null;
            }
            return dsRet;
        }
        /// <summary>   
        /// This is used to get all the Suppliers List
        /// </summary>
        /// <param name="NameIndex">string</param> 
        /// <returns>DataSet</returns> 
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string NameIndex;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetSupplierrList(NameIndex);
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataSet GetSupplierrList(string NameIndex)
        {
            DataSet dsRet = new DataSet();
            try
            {
                string sSQL = " SELECT DISTINCT STRING_VALUE  FROM TB_PROD_SPECS WHERE STRING_VALUE LIKE '" + NameIndex + "%'";
                sSQL = sSQL + " ORDER BY STRING_VALUE";
                oHelper.SQLString = sSQL;
                dsRet = oHelper.GetDataSet();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                dsRet = null;
            }
            return dsRet;
        }
        
        /// <summary>
        /// Get the Restricted product list using ZipCode
        /// </summary>
        /// <param name="ZipCode">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string ZipCode;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetRestProdDS(ZipCode);
        /// }
        /// </code>
        /// </example>
        
        [WebMethod]
        public DataSet GetRestrictedProdDS(string ZipCode)
        {
            DataSet oDS = new DataSet();
            try
            {
                oHelper.SQLString = "SELECT * FROM TBWC_PROD_RESTRICT WHERE ZIPCODE='" + ZipCode + "'";
                oDS = oHelper.GetDataSet();
                if (oDS.Tables[0].Rows.Count == 0)
                {
                    oDS = null;
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                oDS = null;
            }
            return oDS;
        }

        /// <summary>
        /// To get productIDs
        /// </summary>
        /// <param name="ItemCode">string</param>
        /// <returns>Dataset</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Product oProd = new Product();
        ///     string[] ItemCode;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetProdIDDS(ItemCode);
        ///     ...
        /// </code>
        /// </example>
        [WebMethod]

        public DataSet GetProdIDDS(string[] ItemCode)
        {
            DataSet oProdListDS = new DataSet();
            string ItemList = "";
            for (int l = 0; l < ItemCode.Length; l++)
            {
                if (ItemCode.Length - 1 == l)
                {
                    ItemList = ItemList + "'" + ItemCode[l].ToString() + "'";
                }
                else
                {
                    ItemList = ItemList + "'" + ItemCode[l].ToString() + "',";
                }
            }
            oHelper.SQLString = "SELECT DISTINCT (SELECT TOP 1 PRODUCT_ID FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE=PS.STRING_VALUE) PRODUCT_ID,STRING_VALUE FROM TB_PROD_SPECS PS WHERE STRING_VALUE IN (" + ItemList + ") AND ATTRIBUTE_ID=1";
            oProdListDS = oHelper.GetDataSet();
            return oProdListDS;
        }

        public int GetProductAvailability(int ProdID)
        {
            int retValue = 0;
            string Avlbty = "";
            try
            {
                oHelper.SQLString = "SELECT PRODUCT_STATUS FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProdID;
                Avlbty = oHelper.GetValue("PRODUCT_STATUS");
                if (Avlbty == "AVAILABLE")
                    retValue = 1;
                else if (Avlbty == "N/A" || Avlbty == "DISCONTINUED")
                {
                    retValue = 0;
                }
            }
            catch
            {
                retValue = 0;
                return retValue;
            }
            return retValue;
        }
        #endregion
    }

}

