using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to Get and Return all the Order related Methods and Functions
    /// </summary>
    /// <remarks>
    /// Used to get Order Details like Data Retrieve Functions,Shipping Functions,Inventory Data Retrieve Funtions,Invoice Functions..
    /// </remarks>
    /// <example>
    /// Order oOrder = new Order();
    /// </example>
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Order : System.Web.Services.WebService
    {

        HelperDB oHelper = new HelperDB();
        ErrorHandler oErrHand = new ErrorHandler();
        User oUser = new User();
        ConnectionDB oConStr = new ConnectionDB();

        /// <summary>
        /// Create Structure for Order Related Information(includes shipping,Billing)
        /// </summary>
        /// <example>
        /// Order.OrderInfo oOI = new Order.OrderInfo();
        /// oOI.ShipLName
        /// </example>
        public struct OrderInfo
        {
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
            /// <summary>
            /// Users's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Shipping Prefix
            /// </summary>
            public string ShipPrefix;
            /// <summary>
            /// Shipping First Name
            /// </summary>
            public string ShipFName;
            /// <summary>
            /// Shipping Last Name
            /// </summary>
            public string ShipLName;
            /// <summary>
            /// Shipping Middle Name
            /// </summary>
            public string ShipMName;
            /// <summary>
            /// Shipping Suffix
            /// </summary>
            public string ShipSuffix;
            /// <summary>
            /// Shipping Address 1
            /// </summary>
            public string ShipAdd1;
            /// <summary>
            /// Shipping Address 2
            /// </summary>
            public string ShipAdd2;
            /// <summary>
            /// Shipping Address 3
            /// </summary>
            public string ShipAdd3;
            /// <summary>
            /// Shipping City
            /// </summary>
            public string ShipCity;
            /// <summary>
            /// Shipping State
            /// </summary>
            public string ShipState;
            /// <summary>
            /// Shipping Zip Code
            /// </summary>
            public string ShipZip;
            /// <summary>
            /// Shipping Country
            /// </summary>
            public string ShipCountry;
            /// <summary>
            /// Shipping Phone Number
            /// </summary>
            public string ShipPhone;
            /// <summary>
            /// Shipping Notes
            /// </summary>
            public string ShipNotes;
            /// <summary>
            /// Billing First Name
            /// </summary>
            public string BillFName;
            /// <summary>
            /// Billing Last Name
            /// </summary>
            public string BillLName;
            /// <summary>
            /// Billing Middle Name
            /// </summary>
            public string BillMName;
            /// <summary>
            /// Billing Address 1
            /// </summary>
            public string BillAdd1;
            /// <summary>
            /// Billing Address 2
            /// </summary>
            public string BillAdd2;
            /// <summary>
            /// Billing Address 3
            /// </summary>
            public string BillAdd3;
            /// <summary>
            /// Billing City
            /// </summary>
            public string BillCity;
            /// <summary>
            /// Billing State
            /// </summary>
            public string BillState;
            /// <summary>
            /// Billing Zip Code
            /// </summary>
            public string BillZip;
            /// <summary>
            /// Billing Country
            /// </summary>
            public string BillCountry;
            /// <summary>
            /// Billing Phone Number
            /// </summary>
            public string BillPhone;
            /// <summary>
            /// Order Status
            /// </summary>
            public int OrderStatus;
            /// <summary>
            /// Product Total Price
            /// </summary>
            public decimal ProdTotalPrice;
            /// <summary>
            /// Order's Tax Amount
            /// </summary>
            public decimal TaxAmount;
            /// <summary>
            /// Order's Shipping Cost
            /// </summary>
            public decimal ShipCost;
            /// <summary>
            /// Order's Total Amount
            /// </summary>
            public decimal TotalAmount;
            /// <summary>
            /// Order's Shipping Method
            /// </summary>
            public string ShipMethod;
            /// <summary>
            /// Order's Shipping Status
            /// </summary>
            public bool IsShipped;
            /// <summary>
            /// Order's Tracking Number
            /// </summary>
            public string TrackingNo;
            /// <summary>
            /// Order's Est Delivery
            /// </summary>
            public string EstDelivery;
            /// <summary>
            /// Order's Shipping Company
            /// </summary>
            public string ShipCompany;
            /// <summary>
            /// Order's Shipping Confirmation
            /// </summary>
            public string ShipConf;
            /// <summary>
            /// Email Sent Status
            /// </summary>
            public bool isEmailSent;
            /// <summary>
            /// Invoice Sent Status
            /// </summary>
            public bool isInvoiceSent;
            /// <summary>
            /// Client IP Address
            /// </summary>
            public string ClientIPAddress;
            /// <summary>
            /// Invoice No
            /// </summary>
            public string InvoiceNo;
            /// <summary>
            /// Drop Ship
            /// </summary>
            public int DropShip;
            /// <summary>
            /// Created Date
            /// </summary>
            public DateTime CreatedDate;
            /// <summary>
            /// Modified Date
            /// </summary>
            public DateTime ModifiedDate;
            /// <summary>
            /// Modified User
            /// </summary>
            public int ModifiedUser;
            /// <summary>
            /// Created User
            /// </summary>
            public int CreatedUser;
            /// <summary>
            /// SHIP COMPANY NAME
            /// </summary>
            public string ShipCompName;
            /// <summary>
            /// DELIVERY INSTRUCTIONS
            /// </summary>
            public string DeliveryInstr;
            /// <summary>
            /// INVOICE DATE
            /// </summary>
            public DateTime InvoiceDate;
        }
        /// <summary>
        /// Create Structure for Order Item Information
        /// </summary>
        /// <example>
        /// Order.OrderItemInfo oItemInFo = new Order.OrderItemInfo();
        /// oItemInFo.OrderID
        /// </example>
        public struct OrderItemInfo
        {
            /// <summary>
            /// User's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
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
            /// <summary>
            /// Price Calculation Method(Percentage or Amount)
            /// </summary>
            public string PriceCalcMethod;
            /// <summary>
            /// Price Calculation Method(Percentage or Amount)
            /// </summary>
            public decimal PriceIncApplied;

        }
        /// <summary>
        /// Create Enum Values for Order Status Details
        /// </summary>
        /// <example>
        /// Order.OrderStatus.OPEN;
        /// </example>
        public enum OrderStatus
        {
            /// <summary>
            /// Open Order Status
            /// </summary>
            OPEN = 1,
            /// <summary>
            /// Paymeny Order Status
            /// </summary>
            PAYMENT = 2,
            /// <summary>
            /// Transit Order Status
            /// </summary>
            SHIPPED = 3,
            /// <summary>
            /// Completed Order Status
            /// </summary>
            COMPLETED = 4,
            /// <summary>
            /// Cancelled Order Status
            /// </summary>
            CANCELED = 5,
            /// <summary>
            /// Order Placed Order Status
            /// </summary>
            ORDERPLACED = 6,
            /// <summary>
            /// Manual Process Order Status(Manual Verify Users)
            /// </summary>
            MANUALPROCESS = 7,
            /// <summary>
            /// Place the Quote  
            /// </summary>
            QUOTEPLACED = 8,
            /// <summary>
            /// For CAU ORDERS - Order Completed Status
            /// </summary>
            CAU_READYTOVERIFY = 9,
            /// <summary>
            /// For CAU ORDERS - Order Manual check status
            /// </summary>
            CAU_MANUALPROCESS = 10,
            /// <summary>
            /// For CAU PENDING - Order status
            /// </summary>
            CAU_PENDING = 11,
            /// <summary>
            /// For Deleted- Order status
            /// </summary>
            DELETED = 12
        }
        /// <summary>
        /// Create Structure for Inventory Products
        /// </summary>
        /// <example>
        /// Order.Inventory Inv = new Order.Inventory();
        /// Inv.MinOrdQty
        /// </example>
        public struct Inventory
        {
            /// <summary>
            /// User ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Product ID
            /// </summary>
            public int ProductID;
            /// <summary>
            /// Quantity Available
            /// </summary>
            public decimal QtyAvail;
            /// <summary>
            /// Minimum Order Quantity
            /// </summary>
            public decimal MinOrdQty;
            /// <summary>
            /// Lead Time
            /// </summary>
            public string LeadTime;
            /// <summary>
            /// Sale Status
            /// </summary>
            public bool isOnSale;
            /// <summary>
            /// Discount Price
            /// </summary>
            public decimal Discount;
            /// <summary>
            /// List Price Valid Date
            /// </summary>
            public string ListPriceValidTill;
            /// <summary>
            /// Sales Price Valid Date
            /// </summary>
            public string SalePriceValidTill;
            /// <summary>
            /// Product's Status
            /// </summary>
            public int ProductStatus;
        }
        /// <summary>
        /// Create Enum Values for Product Status Details
        /// </summary>
        /// <example>
        /// oInv.ProductStatus.AVALIABLE;
        /// </example>
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
        /// <summary>
        /// Create Enum Values for Shipping Company Details
        /// </summary>
        /// <example>
        /// oInv.ShippingCompany.FEDEX;
        /// </example>
        public enum ShippingCompany
        {
            /// <summary>
            /// FEDEX Shipping Company
            /// </summary>
            ///
            FEDEX = 1,
            /// <summary>
            /// DHL Shipping Company
            /// </summary>
            DHL = 2,
            /// <summary>
            /// UPS Shipping Company
            /// </summary>
            UPS = 3,
            /// <summary>
            /// USPS Shipping Company
            /// </summary>
            USPS = 4
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Order()
        {

        }

        #region "Order Data Retrieve Functions"
        /// <summary>
        /// This is used to create the New Order in the Order Table
        /// </summary>
        /// <param name="oInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int order;
        ///     ...
        ///     order = oOrder.CreateOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int CreateOrder(OrderInfo oInfo)
        {
            try
            {
                string sSQL;
                sSQL = "INSERT INTO TBWC_ORDER(USER_ID,SHIP_PREFIX,SHIP_FNAME,SHIP_LNAME,SHIP_MNAME,SHIP_SUFFIX,SHIP_ADDRESS_LINE1,SHIP_ADDRESS_LINE2,SHIP_ADDRESS_LINE3,";
                sSQL = sSQL + "SHIP_CITY,SHIP_STATE,SHIP_ZIP,SHIP_COUNTRY,SHIP_PHONE,SHIP_NOTES,ORDER_STATUS,PRODUCT_TOTAL_PRICE,TAX_AMOUNT,SHIP_COST,TOTAL_AMOUNT,";
                sSQL = sSQL + "CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER,SHIP_METHOD,IS_SHIPPED,TRACKING_NO,EST_DELIVERY,SHIP_COMPANY,SHIP_CONF,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT) ";
                sSQL = sSQL + "VALUES(" + oInfo.UserID + ",'" + oInfo.ShipPrefix + "','" + oInfo.ShipFName + "','" + oInfo.ShipLName + "','" + oInfo.ShipMName + "','" + oInfo.ShipSuffix + "','" + oInfo.ShipAdd1 + "','" + oInfo.ShipAdd2 + "','" + oInfo.ShipAdd3 + "','";
                sSQL = sSQL + oInfo.ShipCity + "','" + oInfo.ShipState + "','" + oInfo.ShipZip + "','" + oInfo.ShipCountry + "','" + oInfo.ShipPhone + "','" + oInfo.ShipNotes + "'," + oInfo.OrderStatus + "," + oInfo.ProdTotalPrice + ",";
                sSQL = sSQL + oInfo.TaxAmount + "," + oInfo.ShipCost + "," + oInfo.TotalAmount + ",{fn now()},'" + oInfo.UserID + "',{fn now()},'" + oInfo.UserID + "','" + oInfo.ShipMethod + "',";
                sSQL = sSQL + oInfo.IsShipped + ",'" + oInfo.TrackingNo + "','" + oInfo.EstDelivery + "','" + oInfo.ShipCompany + "','" + oInfo.ShipConf + "'," + oInfo.isEmailSent + "," + oInfo.isInvoiceSent + ")";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to Insert the New Order in the Order Table
        /// </summary>
        /// <param name="oInfo">oInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int initOrder;
        ///     ...
        ///     initOrder = oOrder.InitilizeOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int InitilizeOrder(OrderInfo oInfo)
        {
            try
            {
                string sSQL = " INSERT INTO TBWC_ORDER(USER_ID,ORDER_STATUS,IS_SHIPPED,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT,CREATED_USER,MODIFIED_USER)";
                sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1,0,0,0," + oInfo.UserID + "," + oInfo.UserID + " )";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }

        }
        /// <summary>
        /// This is used to get the Order
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>OrderInfo</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     ...
        ///     oInfo = oOrder.GetOrder(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public OrderInfo GetOrder(int OrderID)
        {
            string sSQL;
            DataSet dsOD = new DataSet();
            OrderInfo rInfo = new OrderInfo();
            try
            {
                sSQL = "SELECT * FROM TBWC_ORDER WHERE ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                dsOD = oHelper.GetDataSet("Order");
                foreach (DataRow drOD in dsOD.Tables["Order"].Rows)
                {
                    rInfo.OrderID = oHelper.CI(drOD["ORDER_ID"]);
                    rInfo.ShipPrefix = oHelper.CS(drOD["SHIP_PREFIX"]);
                    rInfo.ShipFName = oHelper.CS(drOD["SHIP_FNAME"]);
                    rInfo.ShipLName = oHelper.CS(drOD["SHIP_LNAME"]);
                    rInfo.ShipMName = oHelper.CS(drOD["SHIP_MNAME"]);
                    rInfo.ShipSuffix = oHelper.CS(drOD["SHIP_SUFFIX"]);
                    rInfo.ShipAdd1 = oHelper.CS(drOD["SHIP_ADDRESS_LINE1"]);
                    rInfo.ShipAdd2 = oHelper.CS(drOD["SHIP_ADDRESS_LINE2"]);
                    rInfo.ShipAdd3 = oHelper.CS(drOD["SHIP_ADDRESS_LINE3"]);
                    rInfo.ShipCity = oHelper.CS(drOD["SHIP_CITY"]);
                    rInfo.ShipState = oHelper.CS(drOD["SHIP_STATE"]);
                    rInfo.ShipZip = oHelper.CS(drOD["SHIP_ZIP"]);
                    rInfo.ShipCountry = oHelper.CS(drOD["SHIP_COUNTRY"]);
                    rInfo.ShipPhone = oHelper.CS(drOD["SHIP_PHONE"]);
                    rInfo.ShipNotes = oHelper.CS(drOD["SHIP_NOTES"]);

                    rInfo.BillFName = oHelper.CS(drOD["BILL_FNAME"]);
                    rInfo.BillLName = oHelper.CS(drOD["BILL_LNAME"]);
                    rInfo.BillMName = oHelper.CS(drOD["BILL_MNAME"]);
                    rInfo.BillAdd1 = oHelper.CS(drOD["BILL_ADDRESS_LINE1"]);
                    rInfo.BillAdd2 = oHelper.CS(drOD["BILL_ADDRESS_LINE2"]);
                    rInfo.BillAdd3 = oHelper.CS(drOD["BILL_ADDRESS_LINE3"]);
                    rInfo.BillCity = oHelper.CS(drOD["BILL_CITY"]);
                    rInfo.BillState = oHelper.CS(drOD["BILL_STATE"]);
                    rInfo.BillZip = oHelper.CS(drOD["BILL_ZIP"]);
                    rInfo.BillCountry = oHelper.CS(drOD["BILL_COUNTRY"]);
                    rInfo.BillPhone = oHelper.CS(drOD["BILL_PHONE"]);

                    rInfo.OrderStatus = oHelper.CI(drOD["ORDER_STATUS"]);
                    rInfo.TaxAmount = oHelper.CDEC(drOD["TAX_AMOUNT"]);
                    rInfo.ShipCost = oHelper.CDEC(drOD["SHIP_COST"]);
                    rInfo.TotalAmount = oHelper.CDEC(drOD["TOTAL_AMOUNT"]);
                    rInfo.ProdTotalPrice = oHelper.CDEC(drOD["PRODUCT_TOTAL_PRICE"]);
                    rInfo.ShipMethod = oHelper.CS(drOD["SHIP_METHOD"]);
                    rInfo.IsShipped = (bool)drOD["IS_SHIPPED"];
                    rInfo.TrackingNo = oHelper.CS(drOD["TRACKING_NO"]);
                    rInfo.EstDelivery = oHelper.CS(drOD["EST_DELIVERY"]);
                    rInfo.ShipCompany = oHelper.CS(drOD["SHIP_COMPANY"]);
                    rInfo.InvoiceNo = oHelper.CS(drOD["INVOICENO"]);
                    rInfo.ShipConf = oHelper.CS(drOD["SHIP_CONF"]);
                    rInfo.isEmailSent = (bool)drOD["ORDER_EMAIL_SENT"];
                    rInfo.isInvoiceSent = (bool)drOD["ORDER_INVOICE_SENT"];
                    rInfo.CreatedDate = (DateTime)drOD["CREATED_DATE"];
                    rInfo.CreatedUser = oHelper.CI(drOD["CREATED_USER"]);
                    rInfo.ModifiedDate = (DateTime)drOD["MODIFIED_DATE"];
                    rInfo.ModifiedUser = oHelper.CI(drOD["MODIFIED_USER"]);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();                    
            }
            return rInfo;
        }
        /// <summary>
        /// This is used to Update all the Order Details
        /// </summary>
        /// <param name="oInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int UpdateOrderStatus;
        ///     ...
        ///     UpdateOrderStatus = oOrder.UpdateOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateOrder(OrderInfo oInfo)
        {

            try
            {
                string sSQL;
                sSQL = "UPDATE TBWC_ORDER SET SHIP_PREFIX='" + oInfo.ShipPrefix + "',SHIP_FNAME='" + oInfo.ShipFName + "',";
                sSQL = sSQL + "SHIP_LNAME='" + oInfo.ShipLName + "',SHIP_MNAME='" + oInfo.ShipMName + "',SHIP_SUFFIX='" + oInfo.ShipSuffix + "',";
                sSQL = sSQL + "SHIP_ADDRESS_LINE1='" + oInfo.ShipAdd1 + "',SHIP_ADDRESS_LINE2='" + oInfo.ShipAdd2 + "',SHIP_ADDRESS_LINE3='" + oInfo.ShipAdd3 + "',";
                sSQL = sSQL + "SHIP_CITY='" + oInfo.ShipCity + "',SHIP_STATE='" + oInfo.ShipState + "',SHIP_ZIP='" + oInfo.ShipZip + "',";
                sSQL = sSQL + "SHIP_COUNTRY='" + oInfo.ShipCountry + "',SHIP_PHONE='" + oInfo.ShipPhone + "',SHIP_NOTES='" + oInfo.ShipNotes + "',";

                sSQL = sSQL + "BILL_FNAME='" + oInfo.BillFName + "',BILL_LNAME='" + oInfo.BillLName + "',BILL_MNAME='" + oInfo.BillMName + "',";
                sSQL = sSQL + "BILL_ADDRESS_LINE1='" + oInfo.BillAdd1 + "',BILL_ADDRESS_LINE2='" + oInfo.BillAdd2 + "',BILL_ADDRESS_LINE3='" + oInfo.BillAdd3 + "',";
                sSQL = sSQL + "BILL_CITY='" + oInfo.BillCity + "',BILL_STATE='" + oInfo.BillState + "',BILL_ZIP='" + oInfo.BillZip + "',";
                sSQL = sSQL + "BILL_COUNTRY='" + oInfo.BillCountry + "',BILL_PHONE='" + oInfo.BillPhone + "',";

                sSQL = sSQL + "ORDER_STATUS=" + oInfo.OrderStatus + ",PRODUCT_TOTAL_PRICE=" + oInfo.ProdTotalPrice + ",TAX_AMOUNT=" + oInfo.TaxAmount + ",";
                sSQL = sSQL + "SHIP_COST=" + oInfo.ShipCost + ",TOTAL_AMOUNT=" + oInfo.TotalAmount + ",";
                sSQL = sSQL + "SHIP_METHOD='" + oInfo.ShipMethod + "',IS_SHIPPED='" + oInfo.IsShipped + "',TRACKING_NO='" + oInfo.TrackingNo + "',";
                sSQL = sSQL + "EST_DELIVERY='" + oInfo.EstDelivery + "',SHIP_COMPANY='" + oInfo.ShipCompany + "',SHIP_CONF='" + oInfo.ShipConf + "',";
                sSQL = sSQL + "ORDER_EMAIL_SENT='" + oInfo.isEmailSent + "',ORDER_INVOICE_SENT='" + oInfo.isInvoiceSent + "',DROPSHIP=" + oInfo.DropShip + ",";
                sSQL = sSQL + "SHIP_COMPNAME='" + oInfo.ShipCompName + "',DELIVERYINST='" + oInfo.DeliveryInstr + "',";
                sSQL = sSQL + "MODIFIED_USER=" + oInfo.UserID + ",CUSTOM_TEXT_FIELD1='" + oInfo.ClientIPAddress + "'  WHERE ORDER_ID=" + oInfo.OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }

        }
        /// <summary>
        /// This is used to set the Price of the Order
        /// </summary>
        /// <param name="oOrdInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdInfo = new Order.OrderInfo();
        ///     int UpdateOrdPr;
        ///     ...
        ///     UpdateOrdPr = oOrder.UpdateOrderPrice(oOrdInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateOrderPrice(OrderInfo oOrdInfo, bool ItemShipping)
        {
            int retVal;
            try
            {
                string sSQL = " UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = " + oOrdInfo.ProdTotalPrice + ",";
                sSQL = sSQL + " TAX_AMOUNT =" + oOrdInfo.TaxAmount + ",TOTAL_AMOUNT = " + oOrdInfo.TotalAmount;
                if (ItemShipping)
                {
                    sSQL = sSQL + ",SHIP_COST = " + oOrdInfo.ShipCost;
                }
                sSQL = sSQL + " WHERE ORDER_ID =" + oOrdInfo.OrderID;
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

        [WebMethod]
        public bool IsUserCanDropShip(int UserID)
        {
            bool isDropShip = false;
            try
            {
                string sSQL = string.Format("SELECT ORDDRPSHP FROM TBWC_COMPANY_BUYERS WHERE USER_ID={0}", UserID);
                oHelper.SQLString = sSQL;
                if (oHelper.GetValue("ORDDRPSHP").ToString() == "1")
                {
                    isDropShip = true;
                }
                else
                {
                    isDropShip = false;
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return isDropShip;
            }
            return isDropShip;
        }

        [WebMethod]
        public int UpdateCustomFields(OrderInfo oOrdInfo)
        {
            int retval = -1;

            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string sSQL = string.Format("SELECT COMPANY_ID,COMPANY_NAME FROM TBWC_COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            oHelper.SQLString = sSQL;
            string sShipCompany = string.IsNullOrEmpty(oOrdInfo.ShipCompName) ? oHelper.GetValue("COMPANY_NAME") : oOrdInfo.ShipCompName;
            int CompanyID = oHelper.CI(oHelper.GetValue("COMPANY_ID"));
            sSQL = string.Format("SELECT CUST_NAME FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            oHelper.SQLString = sSQL;
            string sBillCompany = oHelper.GetValue("CUST_NAME");
            sSQL = string.Format("SELECT BILLTO_ID FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            oHelper.SQLString = sSQL;
            int billto = oHelper.CI(oHelper.GetValue("BILLTO_ID"));
            sSQL = string.Format("UPDATE TBWC_ORDER SET WEBSITE_ID = {0}, SHIP_COMPNAME='{1}', BILL_COMPNAME='{2}', BILLTO_ID = {3}, COMPANY_ID={5} WHERE ORDER_ID = {4}", websiteid, sShipCompany, sBillCompany, billto, oOrdInfo.OrderID, CompanyID);
            oHelper.SQLString = sSQL;
            retval = oHelper.ExecuteSQLQuery();
            return retval;
        }
        /// <summary>
        /// This is used to Set the Price of Removed Items
        /// </summary>
        /// <param name="RemovedItemsPrice">decimal</param>
        /// <param name="OrderID">int</param>
        /// <param name="TaxAmount">decimal</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderId;
        ///     int retVal;
        ///     decimal Tax;
        ///     decimal RemovedItemsPrice;
        ///     ...
        ///     retVal = oOrder.UpdateRemovedItemsPrice(RemovedItemsPrice, OrderId, Tax);
        /// }
        /// </code>
        /// </example>
        public int UpdateRemovedItemsPrice(decimal RemovedItemsPrice, int OrderID, decimal TaxAmount, decimal ShipCost)
        {
            int retVal;
            try
            {
                decimal TotalAmount = RemovedItemsPrice + TaxAmount + ShipCost;
                string sSQL = "UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = PRODUCT_TOTAL_PRICE - " + RemovedItemsPrice + ",SHIP_COST = SHIP_COST - " + ShipCost + ",TAX_AMOUNT =TAX_AMOUNT - " + TaxAmount + ",TOTAL_AMOUNT = TOTAL_AMOUNT -" + TotalAmount + " WHERE ORDER_ID = " + OrderID;
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

        /// <summary>
        /// This is used to Set the Order Status
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="OrderStatus">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int OrdStatusID;
        ///     int UpdateOrdStatus;
        ///     ...
        ///     UpdateOrdStatus = oOrder.UpdateOrderStatus(OrderID, OrdStatusID)
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateOrderStatus(int OrderID, int OrderStatus)
        {
            int retVal = 0;
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET ORDER_STATUS = " + OrderStatus + " WHERE ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return retVal;
            }
            return retVal;
        }

        [WebMethod]
        public int UpdateQuoteID(int OrderID, int QuoteID)
        {
            int retVal = 0;
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET QUOTE_ID = " + QuoteID + " WHERE ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return retVal;
            }
            return retVal;
        }

        [WebMethod]
        public int GetOrderIDForQuote(int QuoteID)
        {
            int retVal = 0;
            try
            {
                string OrderId;
                string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE QUOTE_ID= " + QuoteID;
                oHelper.SQLString = sSQL;
                OrderId = oHelper.GetValue("ORDER_ID");
                if (OrderId == "")
                {
                    return 0;
                }
                else
                {
                    return oHelper.CI(OrderId);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return retVal;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to get the Order ID based on User ID
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     int OrdID;
        ///     ...
        ///     OrdID = oOrder.GetOrderID(UserID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetOrderID(int UserID)
        {
            try
            {
                string OrderID;
                string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID;
                oHelper.SQLString = sSQL;
                OrderID = oHelper.GetValue("ORDER_ID");
                if (OrderID == "")
                {
                    return 0;
                }
                else
                {
                    return oHelper.CI(OrderID);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }

        }
        /// <summary>
        /// This is used to get the Order ID based on User ID and Order Status
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="OrderStatus">int</param>
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     int OrderStatus;
        ///     int Id;
        ///     ...
        ///     Id = oOrder.GetOrderID(UserID, OrderStatus);
        /// }
        /// </code>
        /// </example>
        /// 
        [WebMethod]
        public int GetPendingOrderItems(int UserID, int OrderStatus, int OrderID)
        {
            try
            {
                string Order_ID;
                string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID + " AND ORDER_STATUS = " + OrderStatus + " AND ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                Order_ID = oHelper.GetValue("ORDER_ID");
                if (Order_ID == "")
                {
                    return 0;
                }
                else
                {
                    return oHelper.CI(Order_ID);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }

        [WebMethod]
        public int GetOrderID(int UserID, int OrderStatus)
        {
            try
            {
                string OrderID;
                string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID + " AND ORDER_STATUS = " + OrderStatus;
                oHelper.SQLString = sSQL;
                OrderID = oHelper.GetValue("ORDER_ID");
                if (OrderID == "")
                {
                    return 0;
                }
                else
                {
                    return oHelper.CI(OrderID);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }

        }
        /// <summary>
        /// This is used to get the Price Value for the Order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderPriceValues(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetOrderPriceValues(int OrderID)
        {
            try
            {
                string sSQL = " SELECT PRODUCT_TOTAL_PRICE,TAX_AMOUNT,SHIP_COST,TOTAL_AMOUNT FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get all the Orders
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="sStatus">string</param>
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     string sStatus;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oOrder.GetOrders(UserID, sStatus);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataTable GetOrders(int UserID, string sStatus)
        {
            try
            {
                string sStatusType = "";
                if (sStatus.Equals("HISTORY"))
                {
                    sStatusType = "2,3,4,5,6,7";
                }
                if (sStatus.Equals("STATUS"))
                {
                    sStatusType = "2,3,6,7";
                }
                if (sStatus.Equals("COMPANYORDS"))
                {
                    sStatusType = "2,3,4,5,6,7";
                }
                string sSQL = " SELECT TBP.PO_RELEASE AS ORDER_ID,TBO.ORDER_ID AS REFID,";
                sSQL = sSQL + " TBO.MODIFIED_DATE AS ORDERDATE,TBO.ORDER_STATUS,";
                sSQL = sSQL + " TBO.TOTAL_AMOUNT,TBO.SHIP_COMPANY,TBO.TRACKING_NO  FROM TBWC_ORDER TBO,TBWC_PAYMENT TBP";
                sSQL = sSQL + " WHERE TBO.ORDER_ID=TBP.ORDER_ID AND " + (sStatus != "COMPANYORDS" ? "TBO.[USER_ID]=" + UserID : "TBO.COMPANY_ID = " + Session["COMPANY_ID"]);
                sSQL = sSQL + " AND TBO.ORDER_STATUS IN(" + sStatusType + ") ORDER BY ORDERDATE DESC";

                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return null;
            }
        }


        public DataTable PendingOrders()
        {
            DataTable retvalue = null;

            try
            {
                //string sSQL = string.Format("select order_id [Reference], tcb.Contact, tbo.MODIFIED_DATE [Date], (select COUNT(*) from TBWC_ORDER_ITEM where ORDER_ID = tbo.ORDER_ID) Items, tbo.TOTAL_AMOUNT [Total Amount] from TBWC_ORDER tbo, TBWC_COMPANY_BUYERS tcb where tcb.user_id = tbo.MODIFIED_USER and ORDER_STATUS = 1 and tbo.company_id={0}", Convert.ToInt32(Session["COMPANY_ID"]));
                string sSQL = string.Format("SELECT ORDER_ID [ORDER], TCB.CONTACT, TBO.CREATED_DATE,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No], TBO.TOTAL_AMOUNT [TOTAL AMOUNT] FROM TBWC_ORDER TBO, TBWC_COMPANY_BUYERS TCB WHERE TCB.USER_ID = TBO.MODIFIED_USER AND ORDER_STATUS = 11 AND TBO.COMPANY_ID={0} Order by TBO.CREATED_DATE DESC", Convert.ToInt32(Session["COMPANY_ID"]));
                oHelper.SQLString = sSQL;
                retvalue = oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
            }
            return retvalue;
        }

        public DataTable GetPendingOrderProducts()
        {
            DataTable retvalue = null;

            try
            {
                string sSQL = string.Format("select * from tbwc_order_item where order_id in (select order_id from tbwc_order where order_id = {0})", Convert.ToInt32(Session["ORDER_ID"]));
                oHelper.SQLString = sSQL;
                retvalue = oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
            }
            return retvalue;
        }

        public DataTable GetOrderHistory()
        {
            DataTable retvalue = null;

            try
            {
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped' WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process' WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order] FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.COMPANY_ID = TBCB.COMPANY_ID");
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No],TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID ORDER BY TBO.CREATED_DATE DESC");
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],TBO.MODIFIED_DATE [Modified Date], TBO.ORDER_ID OrderID,TBP.PO_RELEASE  as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status],TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID INNER JOIN TBWC_PAYMENT AS TBP ON TBP.ORDER_ID = TBO.ORDER_ID ORDER BY TBO.CREATED_DATE DESC");
                string sSQL = string.Format("EXEC GetOrderHistory");
                oHelper.SQLString = sSQL;
                retvalue = oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
            }
            return retvalue;
        }

        public DataTable GetFilteredOrderHistory(string pInvoice, string pFromDate, string pToDate, string pUsers, int Companyid)
        {
            DataTable retvalue = null;

            try
            {
                string sSQL = string.Format("EXEC GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + " ");
                oHelper.SQLString = sSQL;
                retvalue = oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
            }
            return retvalue;
        }

        
        public int SentSignalInvoiceNotification(string InvoiceNo)
        {
            int retVal = 0;
            DataSet dsOD = new DataSet();
            try
            {
                string sSQL = string.Format("EXEC STP_REQINVOICE '{0}'", InvoiceNo);
                oHelper.SQLString = sSQL;
                dsOD = oHelper.GetDataSet("inv");
                foreach (DataRow drOD in dsOD.Tables["inv"].Rows)
                {
                    retVal = oHelper.CI(drOD["output"]);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return retVal;
            }
            return retVal;
        }


        public int SentSignalOrderNotification(string InvoiceNo)
        {
            int retVal = 0;
            try
            {
                string sSQL = string.Format("EXEC STP_REQORDER '{0}'", InvoiceNo);
                oHelper.SQLString = sSQL;
                retVal = oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return retVal;
            }
            return retVal;
        }

        /// <summary>
        /// This is used to get the Order Details
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderDetails(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetOrderDetails(int OrderID)
        {
            try
            {
                string sSQL;
                sSQL = "SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = P.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                sSQL = sSQL + "OI.QTY,OI.PRICE_EXT_APPLIED,OI.PRICE_INC_APPLIED,OI.PRICE_CALC_MTHD,(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =41 AND PRODUCT_ID = P.PRODUCT_ID) AS DESCRIPTION FROM TB_PRODUCT P,";
                sSQL = sSQL + "TBWC_ORDER_ITEM OI WHERE P.PRODUCT_ID=OI.PRODUCT_ID AND OI.ORDER_ID=" + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("OrderDetails");
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get the Order Status Details
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     string Orderstatus;
        ///     ...
        ///     Orderstatus = oOrder.GetOrderStatus(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetOrderStatus(int OrderID)
        {
            string sSQL;
            int StatusValue = 0;
            string sOrderStatus = "";
            DataSet dsOS = new DataSet();

            try
            {
                sSQL = "SELECT ORDER_STATUS FROM TBWC_ORDER WHERE ORDER_ID=" + OrderID;
                oHelper.SQLString = sSQL;
                dsOS = oHelper.GetDataSet("OrderStatus");
                if (dsOS != null)
                {
                    foreach (DataRow oDR in dsOS.Tables["OrderStatus"].Rows)
                    {
                        StatusValue = (int)oDR["ORDER_STATUS"];
                    }
                }
                if (StatusValue > 0)
                {
                    sOrderStatus = Enum.GetName(typeof(OrderStatus), StatusValue);
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return null;
            }
            return sOrderStatus;
        }
        /// <summary>
        /// This is used to get the Current Product Total Cost
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>decimal</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oOrder.GetCurrentProductTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public decimal GetCurrentProductTotalCost(int OrderID)
        {
            decimal retVal = 0;
            try
            {
                string sSQL = "SELECT PRODUCT_TOTAL_PRICE FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
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
                //oErrHand.CreateLog();
                retVal = -1;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to cancel the order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int CancelOrd;
        ///     ...
        ///     CancelOrd = oOrder.CancelOrder(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int CancelOrder(int OrderID)
        {
            try
            {
                string sSQL = " UPDATE TBWC_ORDER";
                sSQL = sSQL + " SET ORDER_STATUS=" + (int)OrderStatus.CANCELED;
                sSQL = sSQL + " WHERE ORDER_ID=" + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }

        #region "Order Item Functions.."
        /// <summary>
        /// This is used to insert the items into the Order Table
        /// </summary>
        /// <param name="oItem">OrderItemInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.oItemInFo oItem;
        ///     int addItem;
        ///     ...
        ///     addItem = oOrder.AddOrderItem(oItem);
        /// }
        /// </code>
        /// </example>
        [WebMethod]

        public int AddOrderItem(OrderItemInfo oItem)
        {
            try
            {
                string sSQL;
                sSQL = "INSERT INTO TBWC_ORDER_ITEM(ORDER_ID,PRODUCT_ID,QTY,PRICE_EXT_APPLIED,PRICE_CALC_MTHD,CREATED_USER,MODIFIED_USER) ";
                sSQL = sSQL + "VALUES(" + oItem.OrderID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",'" + oItem.PriceCalcMethod + "',";
                sSQL = sSQL + oItem.UserID + "," + oItem.UserID + ")";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return -1;
            }
        }
        /// <summary>
        /// This is used to get the order Items using order ID
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>DatSet</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderId;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderItems(OrderId);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetOrderItems(int OrderID)
        {
            try
            {
                //string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TOI.ORDER_ID,PF.FAMILY_ID,TOI.PRODUCT_ID,TOI.QTY,TOI.PRICE_APPLIED,TOI.PRICE_CALC_MTHD,";
                //sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,"; //TI.LEAD_TIME,";//TI.IS_ON_SALE,TI.DISCOUNT_PERCENT,TI.LIST_PRICE_VALID_TILL,";
                //sSQL = sSQL + " PRODUCT_STATUS";
                //sSQL = sSQL + " FROM TBWC_ORDER_ITEM TOI,TBWC_INVENTORY TI,TB_PRODUCT TP,TB_PROD_FAMILY PF";
                //sSQL = sSQL + " WHERE TOI.ORDER_ ID =" + OrderID;
                //sSQL = sSQL + " AND TOI.PRODUCT_ID = TI.PRODUCT_ID AND TOI.PRODUCT_ID = TP.PRODUCT_ID AND TOI.PRODUCT_ID = PF.PRODUCT_ID AND TP.PRODUCT_ID=PF.PRODUCT_ID";

                string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TOI.ORDER_ID,TOI.PRODUCT_ID,TOI.QTY,TOI.PRICE_INC_APPLIED,TOI.PRICE_EXT_APPLIED,TOI.PRICE_CALC_MTHD,";
                sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,"; //TI.LEAD_TIME,";//TI.IS_ON_SALE,TI.DISCOUNT_PERCENT,TI.LIST_PRICE_VALID_TILL,";
                sSQL = sSQL + " PRODUCT_STATUS,(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME='PROD_DSC') AND PRODUCT_ID = TP.PRODUCT_ID) AS DESCRIPTION";
                sSQL = sSQL + " FROM TBWC_ORDER_ITEM TOI,TBWC_INVENTORY TI,TB_PRODUCT TP";
                sSQL = sSQL + " WHERE TOI.ORDER_ID =" + OrderID;
                sSQL = sSQL + " AND TOI.PRODUCT_ID = TI.PRODUCT_ID AND TOI.PRODUCT_ID = TP.PRODUCT_ID ORDER BY TOI.CREATED_DATE DESC";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();

            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return null;
            }
        }
        /// <summary>
        /// This is used to count the Number of Items in Order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetOrderItemCount(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetOrderItemCount(int OrderID)
        {
            try
            {
                string sSQL = "SELECT COUNT(PRODUCT_ID) AS ITEMCOUNT FROM TBWC_ORDER_ITEM WHERE ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.CI(oHelper.GetValue("ITEMCOUNT"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return -1;
            }
        }
        /// <summary>
        /// This is used to get the Products Total Cost
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>decimal</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oOrder.GetProductTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public decimal GetProductTotalCost(int OrderID)
        {
            decimal retVal = 0;
            try
            {
                string sSQL = "SELECT SUM(PRICE_EXT_APPLIED)AS TOTALAMOUNT FROM TBWC_ORDER_ITEM WHERE ORDER_ID =" + OrderID;
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
        /// <summary>
        /// This is used to get the Order Items Quantity
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int OrderID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetOrderItemQty(ProductID, OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetOrderItemQty(int ProductID, int OrderID)
        {
            int retVal = 0;
            try
            {
                string sSQL = "SELECT QTY FROM TBWC_ORDER_ITEM WHERE PRODUCT_ID = " + ProductID + " AND ORDER_ID =" + OrderID;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrItemInfo"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetApproveOrderItems(int OrderID)
        {
            try
            {
                string sSQL = "SELECT TBO.ORDER_ID, TBO.SHIP_METHOD, TBO.SHIP_NOTES AS COMMENTS, TBP.PO_RELEASE FROM TBWC_ORDER TBO, ";
                sSQL = sSQL + "TBWC_PAYMENT TBP ";
                sSQL = sSQL + "WHERE TBO.ORDER_ID = TBP.ORDER_ID AND ";
                sSQL = sSQL + "TBO.ORDER_ID = " + OrderID; ;

                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();

            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return null;
            }
        }

        /// <summary>
        /// This is used to Update the Order Items 
        /// </summary>
        /// <param name="OrItemInfo">OrderItemInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderItemInfo oOrdItemInfo;
        ///     int UpdateOrdItem;
        ///     ...
        ///     UpdateOrdItem = oOrder.UpdateOrderItem(oOrdItemInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateOrderItem(OrderItemInfo OrItemInfo)
        {
            try
            {

                string sSQL = "UPDATE TBWC_ORDER_ITEM SET QTY=(" + OrItemInfo.Quantity + ") ,PRICE_EXT_APPLIED=" + OrItemInfo.PriceApplied;
                sSQL = sSQL + " WHERE PRODUCT_ID=" + OrItemInfo.ProductID + " AND ORDER_ID =" + OrItemInfo.OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                return -1;
            }
        }
        /// <summary>
        /// This is used to remove the Items from the "order item" table 
        /// </summary>
        /// <param name="ProductID">string</param>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     string ProductID;
        ///     int OrderId;
        ///     int remItem;
        ///     ...
        ///     remItem = oOrder.RemoveItem(ProductID, OrderId);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int RemoveItem(string ProductID, int OrderID, int UserID)
        {
            try
            {
                string sSQL;
                if (ProductID == "AllProd")
                {
                    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
                }
                else
                {
                    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE PRODUCT_ID IN(" + ProductID + ") AND ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
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

        [WebMethod]
        public int RemoveOrder(int OrderID, int UserID)
        {
            try
            {
                string sSQL = string.Empty;
                sSQL = "DELETE FROM TBWC_ORDER WHERE ORDER_ID=" + OrderID + " AND USER_ID = " + UserID;
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
        #region "Shipping Data Retrieve Functions"
        /// <summary>
        /// This is used to set the shipping details to order table 
        /// </summary>
        /// <param name="oOrdShipInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdShipInfo = new Order.OrderInfo();
        ///     int UpdateStatus;
        ///     ...
        ///     UpdateStatus = oOrder.UpdateShippDetailsToOrder(oOrdShipInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateShippDetailsToOrder(OrderInfo oOrdShipInfo)
        {
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET SHIP_PREFIX='" + oOrdShipInfo.ShipPrefix + "',SHIP_FNAME='" + oOrdShipInfo.ShipFName + "',";
                sSQL = sSQL + "SHIP_LNAME='" + oOrdShipInfo.ShipLName + "',SHIP_MNAME='" + oOrdShipInfo.ShipMName + "',SHIP_SUFFIX='" + oOrdShipInfo.ShipSuffix + "',";
                sSQL = sSQL + "SHIP_ADDRESS_LINE1='" + oOrdShipInfo.ShipAdd1 + "',SHIP_ADDRESS_LINE2='" + oOrdShipInfo.ShipAdd2 + "',SHIP_ADDRESS_LINE3='" + oOrdShipInfo.ShipAdd3 + "',";
                sSQL = sSQL + "SHIP_CITY='" + oOrdShipInfo.ShipCity + "',SHIP_STATE='" + oOrdShipInfo.ShipState + "',SHIP_ZIP='" + oOrdShipInfo.ShipZip + "',";
                sSQL = sSQL + "SHIP_COUNTRY='" + oOrdShipInfo.ShipCountry + "',SHIP_PHONE='" + oOrdShipInfo.ShipPhone + "',SHIP_NOTES='" + oOrdShipInfo.ShipNotes + "',SHIP_COST =" + oOrdShipInfo.ShipCost + ",";
                sSQL = sSQL + "TOTAL_AMOUNT =" + oOrdShipInfo.TotalAmount + ",TAX_AMOUNT =" + oOrdShipInfo.TaxAmount + ",SHIP_METHOD='" + oOrdShipInfo.ShipMethod + "',IS_SHIPPED=" + oHelper.CB(oOrdShipInfo.IsShipped) + ",TRACKING_NO='" + oOrdShipInfo.TrackingNo + "',";
                sSQL = sSQL + "EST_DELIVERY='" + oOrdShipInfo.EstDelivery + "',SHIP_COMPANY='" + oOrdShipInfo.ShipCompany + "',SHIP_CONF='" + oOrdShipInfo.ShipConf + "',";
                sSQL = sSQL + "MODIFIED_USER=" + oOrdShipInfo.UserID + ",";
                sSQL = sSQL + "ORDER_EMAIL_SENT=" + oHelper.CB(oOrdShipInfo.isEmailSent) + ",ORDER_INVOICE_SENT=" + oHelper.CB(oOrdShipInfo.isInvoiceSent) + " WHERE ORDER_ID=" + oOrdShipInfo.OrderID;

                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();

            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to set the billing details in the order table
        /// </summary>
        /// <param name="oOrdBillInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdBillInfo = new Order.OrderInfo();
        ///     int retUpdateStatus;
        ///     ...
        ///     retUpdateStatus = oOrder.UpdateBillDetailsToOrder(oOrdBillInfo);
        /// }
        /// </code>
        /// </example>
        public int UpdateBillDetailsToOrder(OrderInfo oOrdBillInfo)
        {
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET ";
                sSQL = sSQL + "BILL_ADDRESS_LINE1='" + oOrdBillInfo.BillAdd1 + "',BILL_ADDRESS_LINE2='" + oOrdBillInfo.BillAdd2 + "',BILL_ADDRESS_LINE3='" + oOrdBillInfo.BillAdd3 + "',";
                sSQL = sSQL + "BILL_CITY='" + oOrdBillInfo.BillCity + "',BILL_STATE='" + oOrdBillInfo.BillState + "',BILL_ZIP='" + oOrdBillInfo.BillZip + "',";
                sSQL = sSQL + "BILL_COUNTRY='" + oOrdBillInfo.BillCountry + "',BILL_PHONE='" + oOrdBillInfo.BillPhone + "'WHERE ORDER_ID=" + oOrdBillInfo.OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }
        #endregion
        #region "Inventry Data Retrieve Functions..."
        /// <summary>
        /// This is used to add the Products to the Inventory Table
        /// </summary>
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
        ///     Order oOrder = new Order();
        ///     Order.Inventory Inv = new Order.Inventory();
        ///     int addProd;
        ///     ...
        ///     addProd = oOrder.AddProductToInventory(Inv);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public int AddProductToInventory(Inventory oInv)
        {
            try
            {
                string sSQL;
                sSQL = "INSERT INTO TBWC_INVENTORY(PRODUCT_ID,QTY_AVAIL,MINORDQTY,LEAD_TIME,IS_ON_SALE,DISCOUNT_PERCENT,LIST_PRICE_VALID_TILL,";
                sSQL = sSQL + "SALE_PRICE_VALID_TILL,PRODUCT_STATUS,CREATED_DATA,CREATED_USER,MODIFIED_DATE,MODIFIED_USER) ";
                sSQL = sSQL + "VALUES(" + oInv.ProductID + "," + oInv.QtyAvail + "," + oInv.MinOrdQty + ",'" + oInv.MinOrdQty + "'," + oInv.isOnSale + ",";
                sSQL = sSQL + oInv.isOnSale + ",'" + oInv.ListPriceValidTill + "','" + oInv.SalePriceValidTill + "'," + oInv.ProductStatus;
                sSQL = sSQL + "',{fn now()},'" + oInv.UserID + "','{fn now()},'" + oInv.UserID + "')";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to set the products to Inventory list
        /// </summary>
        /// <param name="oInv">Inventory</param>
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
        ///     Order oOrder = new Order();
        ///     Order.Inventory Inv = new Order.Inventory();
        ///     int UpdateStatus;
        ///     ...
        ///     UpdateStatus = oOrder.UpdateProductDetailsToInventry(Inv);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateProductDetailsToInventry(Inventory oInv)
        {
            try
            {
                string sSQL;
                sSQL = "UPDATE TBWC_INVENTORY SET QTY_AVAIL =" + oInv.QtyAvail + ",MINIORDQTY =" + oInv.MinOrdQty + ",LEAD_TIME = '" + oInv.LeadTime + "'";
                sSQL = sSQL + ",IS_ON_SALE =" + oInv.isOnSale + ",DISCOUNT_PERCENT =" + oInv.Discount + ",LIST_PROCE_VALID_TILL ='" + oInv.ListPriceValidTill + "'";
                sSQL = sSQL + ",SALE_PRICE_VALID_TILL = '" + oInv.SalePriceValidTill + "',PRODUCT_STAUS =" + oInv.ProductStatus + ", MODIFIED_DATE = {fn now()},'MODIFIED_USER ='" + oInv.UserID + "'";
                sSQL = sSQL + "WHERE PRODUCT_ID = " + oInv.ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog(); 
                return -1;
            }
        }
        /// <summary>
        /// This is used to set the Available Quantity
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="Qty">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int Qty;
        ///     int UpdateQty;
        ///     ...
        ///     UpdateQty = oOrder.UpdateQuantity(ProductID, Qty);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateQuantity(int ProductID, int Qty)
        {
            int retVal = 0;
            try
            {
                string sSQL = "";
                if (Qty > 0)
                    sSQL = " UPDATE TBWC_INVENTORY SET QTY_AVAIL=" + Qty + ",PRODUCT_STATUS='AVAILABLE'";
                else
                    sSQL = " UPDATE TBWC_INVENTORY SET QTY_AVAIL=" + Qty;
                sSQL = sSQL + " WHERE PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                retVal = 1; //oHelper.ExecuteSQLQuery(); /// TBWC_INVENTORY DOESNOT APPLY TO WES CUSTOMER
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                retVal = -1;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to set the Tax Amount
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="TaxAmount">decimal</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal TaxAmount;
        ///     int retVal;
        ///      ...
        ///     retVal = oOrder.UpdateTaxAmount(OrderID,TaxAmount);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateTaxAmount(int OrderID, decimal TaxAmount)
        {
            int retVal = 0;
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET TAX_AMOUNT =" + TaxAmount + ",TOTAL_AMOUNT = PRODUCT_TOTAL_PRICE + SHIP_COST + " + TaxAmount + " WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                retVal = -1;
                return retVal;
            }

        }
        /// <summary>
        /// This is used to check the product having shipping charge or not
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>bool</returns>
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
        ///     Order oOrder = new Order();
        ///     bool IsShipping = false;
        ///     ...
        ///     IsShipping = oOrder.GetProductIsShipping(oHelper.CI(rItem["Product_id"].ToString()));
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool GetProductIsShipping(int ProductID)
        {
            bool retVal = false;
            try
            {
                string sSQL = "SELECT IS_SHIPPING FROM TBWC_INVENTORY WHERE PRODUCT_ID= " + ProductID;
                oHelper.SQLString = sSQL;
                if (oHelper.GetValue("IS_SHIPPING") == "True")
                {
                    retVal = true;
                }
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                retVal = false;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to set the shipping cost
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="ShipCost">decimal</param>
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
        ///     Order oOrder = new Order(); 
        ///     int OrdId;
        ///     decimal Shipcost;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.UpdateShippingCost(OrdId,Shipcost);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateShippingCost(int OrderID, decimal ShipCost)
        {
            int retVal = 0;
            try
            {
                string sSQL = "UPDATE TBWC_ORDER SET SHIP_COST =" + ShipCost + ",TOTAL_AMOUNT = PRODUCT_TOTAL_PRICE + TAX_AMOUNT + " + ShipCost + " WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                retVal = -1;
            }
            return retVal;
        }

        /// <summary>
        /// This is used to get the products available quantity
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int qtyAvail;
        ///     ...
        ///     qtyAvail = oOrder.GetProductAvilableQty(ProductID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetProductAvilableQty(int ProductID)
        {
            try
            {
                string sSQL;
                sSQL = "SELECT QTY_AVAIL FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.CI(oHelper.GetValue("QTY_AVAIL"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to get products minimum order quantity
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetProductMinimumOrderQty(ProductID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetProductMinimumOrderQty(int ProductID)
        {
            string sSQL;
            try
            {
                sSQL = "SELECT MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
                oHelper.SQLString = sSQL;
                return oHelper.CI(oHelper.GetValue("MIN_ORD_QTY"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                return -1;
            }
        }

        /// <summary>
        /// This is used to get ItemDetailsFrom Inventory based on ProductID
        /// </summary>
        /// <param name="ProductID">int</param>
        [WebMethod]
        public DataSet GetItemDetailsFromInventory(int ProductID)
        {
            DataSet oDs = new DataSet();
            try
            {
                string sSQL = "SELECT * FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
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


        #endregion
        #region "Invoice functions.."
        /// <summary>
        /// This is used to get the orders total cost
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>decimal</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal TotCost;
        ///     ...
        ///     TotCost = oOrder.GetOrderTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        public decimal GetOrderTotalCost(int OrderID)
        {
            decimal TotCst = 0.00M;
            try
            {
                string sSQL = "SELECT ISNULL(TOTAL_AMOUNT,0.00)AS TOTAL_AMOUNT  FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                TotCst = oHelper.CDEC(oHelper.GetValue("TOTAL_AMOUNT"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();

            }
            return TotCst;

        }
        /// <summary>
        /// This is used to get the shipping cost based on the order id
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>decimal</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal ShippCost;
        ///     ...
        ///     ShippCost = oOrder.GetShippingCost(OrderID);
        /// }
        /// </code>

        /// </example>

        public decimal GetShippingCost(int OrderID)
        {
            decimal ShippCst = 0.00M;
            try
            {
                string sSQL = "SELECT ISNULL(SHIP_COST,0.00)AS SHIP_COST FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                ShippCst = oHelper.CDEC(oHelper.GetValue("SHIP_COST"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
            }
            return ShippCst;

        }
        /// <summary>
        /// This is used to get the tax amount based on order id
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>decimal</returns>
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
        ///  Order oOrder = new Order();
        ///  int OrderID;
        ///  decimal Tax;
        ///  ...
        ///  Tax = oOrder.GetTaxAmount(OrderID);
        /// }
        /// </code>
        /// </example>
        public decimal GetTaxAmount(int OrderID)
        {
            decimal Tax = 0.00M;
            try
            {
                string sSQL = "SELECT ISNULL(TAX_AMOUNT,0.00) AS TAX_AMOUNT FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                Tax = oHelper.CDEC(oHelper.GetValue("TAX_AMOUNT"));
            }
            catch (Exception e)
            {
                oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();

            }
            return Tax;
        }
        #endregion

    }

}