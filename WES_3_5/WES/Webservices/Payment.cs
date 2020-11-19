using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to get all the Payment Details
    /// </summary>    
    /// <remarks>
    /// Used to Create and Update the Payment Records in the Payment Table.
    /// </remarks>
    /// <example>
    /// Payment oPay = new Payment();
    /// </example>
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Payment : System.Web.Services.WebService
    {
        HelperDB oHelper = new HelperDB();
        User oUser = new User();
        ErrorHandler oErr = new ErrorHandler();
        #region "Declarations.."
        /// <summary>
        /// CreateEnum values for getting Payment Types
        /// </summary>
        /// <example>
        /// Payment.PaymentType.CCPayment;
        /// </example>
        public enum PaymentType
        {
            /// <summary>
            /// Cash on Delivery
            /// </summary>
            CODPayment = 1,
            /// <summary>
            /// Crdit Card
            /// </summary>
            CCPayment = 2,
            /// <summary>
            /// Cheque Payment
            /// </summary>
            CHEPayment = 3,
            /// <summary>
            /// Payment Declined
            /// </summary>
            CCPaymentDeclined = 4
        }
        /// <summary>
        /// Create Structure "PayInfo" for getting Payment related Values
        /// </summary>
        /// <example>
        /// Payment.PayInfo oPayInfo = new Payment.PayInfo();
        /// </example>
        public struct PayInfo
        {
            /// <summary>
            /// Payment Type
            /// </summary>
            public PaymentType PaymentType;
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
            /// <summary>
            /// User Id
            /// </summary>
            public int UserId;
            /// <summary>
            /// Purchase Order Number
            /// </summary>
            public string PONumber;
            /// <summary>
            ///  Purchase Order Release Number 
            /// </summary>
            public string PORelease;
            /// <summary>
            /// Bank ABA
            /// </summary>
            public string BankABA;
            /// <summary>
            /// Bank Name
            /// </summary>
            public string BankName;
            /// <summary>
            /// Bank Account Number
            /// </summary>
            public string BankACNo;
            /// <summary>
            /// Crddit Card Type
            /// </summary>
            public string CardType;
            /// <summary>
            /// Bank City
            /// </summary>                
            public string BankCity;
            /// <summary>
            /// Bank State
            /// </summary>
            public string BankState;
            /// <summary>
            /// Expiry Date
            /// </summary>
            public string Exp_Date;
            /// <summary>
            /// Name On Credit Card
            /// </summary>
            public string NameOnCard;
            /// <summary>
            /// CVV Code
            /// </summary>
            public string CVVCode;
            /// <summary>
            /// Billing First Name
            /// </summary>
            public string BillFName;
            /// <summary>
            /// Billing Last Name
            /// </summary>
            public string BillLName;
            /// <summary>
            /// Address 1
            /// </summary>
            public string Address1;
            /// <summary>
            /// Address 2
            /// </summary>
            public string Address2;
            /// <summary>
            /// Address 3
            /// </summary>
            public string Address3;
            /// <summary>
            /// City
            /// </summary>
            public string City;
            /// <summary>
            /// State
            /// </summary>
            public string State;
            /// <summary>
            /// Zip
            /// </summary>
            public string Zip;
            /// <summary>
            /// Country
            /// </summary>
            public string Country;
            /// <summary>
            /// Phone Number
            /// </summary>
            public string Phone;
            /// <summary>
            /// Payment Amount
            /// </summary>
            public decimal Amount;
            /// <summary>
            /// Payment Response from Gateway
            /// </summary>
            public string PayResponse;

        }
        #endregion
        /// <summary>
        /// Default constructor
        /// </summary>
        public Payment()
        {

        }

        #region "Functions..."
        /// <summary>
        /// This is used to create the PaymentRecord in the Payment Table
        /// </summary>
        /// <param name="oPayInfo">PayInfo</param>
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
        ///      Payment oPay = new Payment();
        ///      Payment.PayInfo oPayInfo = new Payment.PayInfo();
        ///      int rValue;
        ///      ...
        ///      rValue = oPay.CreatePayment(oPayInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int CreatePayment(PayInfo oPayInfo)
        {
            String sSQL;
            int rValue = 0;

            try
            {
                if (oPayInfo.PaymentType == PaymentType.CODPayment)
                {
                    sSQL = " INSERT INTO TBWC_PAYMENT(ORDER_ID,PAYMENT_TYPE,PO_NUMBER,PO_RELEASE,AMOUNT_CHARGED,CREATED_USER,MODIFIED_USER)";
                    sSQL = sSQL + " VALUES( " + oPayInfo.OrderID + "," + (int)oPayInfo.PaymentType + ",'" + oPayInfo.PONumber + "','" + oPayInfo.PORelease + "',";
                    sSQL = sSQL + "Replace('" + oPayInfo.Amount + "',',','.')" + "," + oPayInfo.UserId + "," + oPayInfo.UserId + ")";
                    oHelper.SQLString = sSQL;
                    return oHelper.ExecuteSQLQuery();

                }
                else if (oPayInfo.PaymentType == PaymentType.CCPayment)
                {
                    sSQL = "INSERT INTO TBWC_PAYMENT (ORDER_ID,PAYMENT_TYPE,PO_NUMBER,PO_RELEASE,";
                    sSQL = sSQL + "CC_EXP_DATE,CC_NAME_ON_CARD,CC_CVV_CODE,CC_ADDRESS_LINE1,CC_ADDRESS_LINE2,";
                    sSQL = sSQL + "CC_ADDRESS_LINE3,CC_CITY,CC_STATE,CC_ZIP,CC_COUNTRY,";
                    sSQL = sSQL + "AMOUNT_CHARGED,CC_PAY_RESPONSE,BANK_ACT_NO,CREATED_USER,MODIFIED_USER) ";
                    sSQL = sSQL + "VALUES (" + oPayInfo.OrderID + "," + (int)oPayInfo.PaymentType + ",'" + oPayInfo.PONumber + "','" + oPayInfo.PORelease + "','";
                    sSQL = sSQL + oPayInfo.Exp_Date + "','" + oPayInfo.NameOnCard + "','" + oPayInfo.CVVCode + "','" + oPayInfo.Address1 + "','" + oPayInfo.Address2 + "','";
                    sSQL = sSQL + oPayInfo.Address3 + "','" + oPayInfo.City + "','" + oPayInfo.State + "','" + oPayInfo.Zip + "','" + oPayInfo.Country + "',";
                    sSQL = sSQL + "Replace('" + oPayInfo.Amount + "',',','.')" + ",'" + oPayInfo.PayResponse + "','" + oPayInfo.BankACNo + "'," + oPayInfo.UserId + "," + oPayInfo.UserId + ")";
                    oHelper.SQLString = sSQL;
                    rValue = oHelper.ExecuteSQLQuery();
                }
                else if (oPayInfo.PaymentType == PaymentType.CHEPayment)
                {
                    sSQL = "INSERT INTO TBWC_PAYMENT (ORDER_ID,PAYMENT_TYPE,PO_NUMBER,PO_RELEASE,";
                    sSQL = sSQL + "BANK_ABA,BANK_NAME,BANK_ACT_NO,BANK_CITY,BANK_STATE,";
                    sSQL = sSQL + "AMOUNT_CHARGED,CREATED_USER,MODIFIED_USER) ";
                    sSQL = sSQL + "VALUES (" + oPayInfo.OrderID + "," + (int)oPayInfo.PaymentType + ",'" + oPayInfo.PONumber + "','" + oPayInfo.PORelease + "','";
                    sSQL = sSQL + oPayInfo.BankABA + "','" + oPayInfo.BankName + "','" + oPayInfo.BankACNo + "','" + oPayInfo.BankCity + "','" + oPayInfo.BankState + "',";
                    sSQL = sSQL + "Replace('" + oPayInfo.Amount + "',',','.')" + "," + oPayInfo.UserId + oPayInfo.UserId + ")";
                    oHelper.SQLString = sSQL;
                    rValue = oHelper.ExecuteSQLQuery();
                }
                else if (oPayInfo.PaymentType == PaymentType.CCPaymentDeclined)
                {
                    sSQL = "INSERT INTO TBWC_PAYMENT (ORDER_ID,PAYMENT_TYPE,PO_NUMBER,PO_RELEASE,";
                    sSQL = sSQL + "CC_EXP_DATE,CC_NAME_ON_CARD,CC_CVV_CODE,CC_ADDRESS_LINE1,CC_ADDRESS_LINE2,";
                    sSQL = sSQL + "CC_ADDRESS_LINE3,CC_CITY,CC_STATE,CC_ZIP,CC_COUNTRY,";
                    sSQL = sSQL + "AMOUNT_CHARGED,CC_PAY_RESPONSE,BANK_ACT_NO,CREATED_USER,MODIFIED_USER) ";
                    sSQL = sSQL + "VALUES (" + oPayInfo.OrderID + "," + (int)oPayInfo.PaymentType + ",'" + oPayInfo.PONumber + "','" + oPayInfo.PORelease + "','";
                    sSQL = sSQL + oPayInfo.Exp_Date + "','" + oPayInfo.NameOnCard + "','" + oPayInfo.CVVCode + "','" + oPayInfo.Address1 + "','" + oPayInfo.Address2 + "','";
                    sSQL = sSQL + oPayInfo.Address3 + "','" + oPayInfo.City + "','" + oPayInfo.State + "','" + oPayInfo.Zip + "','" + oPayInfo.Country + "',";
                    sSQL = sSQL + "Replace('" + oPayInfo.Amount + "',',','.')" + ",'" + oPayInfo.PayResponse + "','" + oPayInfo.BankACNo + "'," + oPayInfo.UserId + "," + oPayInfo.UserId + ")";
                    oHelper.SQLString = sSQL;
                    rValue = oHelper.ExecuteSQLQuery();
                }
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                rValue = -1;
            }

            return rValue;
        }

        /// <summary>
        /// This is used to get all the Payment Details
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>PayInfo</returns>
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
        ///     Payment oPay = new Payment();
        ///     int orderID;
        ///     Payment.PayInfo oPayInfo= new Payment.PayInfo();
        ///     ...
        ///     oPayInfo = oPay.GetPayment(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public PayInfo GetPayment(int OrderID)
        {
            String sSQL;
            DataSet dsPayment = new DataSet();
            PayInfo Paymentinfo = new PayInfo();

            try
            {
                sSQL = "SELECT * FROM TBWC_PAYMENT WHERE ORDER_ID = " + OrderID;
                oHelper.SQLString = sSQL;
                dsPayment = oHelper.GetDataSet("Payment");

                foreach (DataRow drPayment in dsPayment.Tables["Payment"].Rows)
                {
                    Paymentinfo.PaymentType = (PaymentType)drPayment["PAYMENT_TYPE"];
                    Paymentinfo.PONumber = oHelper.CS(drPayment["PO_NUMBER"]);
                    Paymentinfo.PORelease = oHelper.CS(drPayment["PO_RELEASE"]) + "";
                    Paymentinfo.BankABA = oHelper.CS(drPayment["BANK_ABA"]);
                    Paymentinfo.BankName = oHelper.CS(drPayment["BANK_NAME"]);
                    Paymentinfo.BankACNo = oHelper.CS(drPayment["BANK_ACT_NO"]);
                    Paymentinfo.BankCity = oHelper.CS(drPayment["BANK_CITY"]);
                    Paymentinfo.BankState = oHelper.CS(drPayment["BANK_STATE"]);
                    Paymentinfo.Exp_Date = oHelper.CS(drPayment["CC_EXP_DATE"]);
                    Paymentinfo.NameOnCard = oHelper.CS(drPayment["CC_NAME_ON_CARD"]);
                    Paymentinfo.CVVCode = oHelper.CS(drPayment["CC_CVV_CODE"]);
                    Paymentinfo.Address1 = oHelper.CS(drPayment["CC_ADDRESS_LINE1"]);
                    Paymentinfo.Address2 = oHelper.CS(drPayment["CC_ADDRESS_LINE2"]);
                    Paymentinfo.Address3 = oHelper.CS(drPayment["CC_ADDRESS_LINE3"]);
                    Paymentinfo.City = oHelper.CS(drPayment["CC_CITY"]);
                    Paymentinfo.State = oHelper.CS(drPayment["CC_STATE"]);
                    Paymentinfo.Zip = oHelper.CS(drPayment["CC_ZIP"]);
                    Paymentinfo.Country = oHelper.CS(drPayment["CC_COUNTRY"]);
                    Paymentinfo.Amount = oHelper.CDEC(drPayment["AMOUNT_CHARGED"]);
                    Paymentinfo.PayResponse = oHelper.CS(drPayment["CC_PAY_RESPONSE"]);
                    Paymentinfo.OrderID = oHelper.CI(drPayment["ORDER_ID"]);
                }
                return Paymentinfo;
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                return Paymentinfo;
            }
        }
        /// <summary>
        /// This is used to update the record in the payment table
        /// </summary>
        /// <param name="oPayInfo">PayInfo</param>
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
        ///     Payment oPay = new Payment();
        ///     Payment.PayInfo CardInfo = new Payment.PayInfo();
        ///     int UpdatePaymt;
        ///     ...
        ///     UpdatePaymt = oPay.UpdatePayment(CardInfo);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdatePayment(PayInfo oPayInfo)
        {
            try
            {
                String sSQL;
                sSQL = "UPDATE TBWC_PAYMENT SET PAYMENT_TYPE=" + (int)oPayInfo.PaymentType + ",PO_NUMBER='" + oPayInfo.PONumber + "',";
                sSQL = sSQL + "PO_RELEASE='" + oPayInfo.PORelease + "',BANK_ABA='" + oPayInfo.BankABA + "',BANK_NAME='" + oPayInfo.BankName + "',";
                sSQL = sSQL + "BANK_ACT_NO ='" + oPayInfo.BankACNo + "',BANK_CITY='" + oPayInfo.BankCity + "',BANK_STATE='" + oPayInfo.BankState + "',";
                sSQL = sSQL + "CC_EXP_DATE='" + oPayInfo.Exp_Date + "',CC_NAME_ON_CARD='" + oPayInfo.NameOnCard + "',CC_CVV_CODE='" + oPayInfo.CVVCode + "',";
                sSQL = sSQL + "CC_ADDRESS_LINE1='" + oPayInfo.Address1 + "',CC_ADDRESS_LINE2='" + oPayInfo.Address2 + "',CC_ADDRESS_LINE3='" + oPayInfo.Address3 + "',";
                sSQL = sSQL + "CC_CITY='" + oPayInfo.City + "',CC_STATE='" + oPayInfo.State + "',CC_ZIP='" + oPayInfo.Zip + "',CC_COUNTRY='" + oPayInfo.Country + "',";
                sSQL = sSQL + "AMOUNT_CHARGED=" + oPayInfo.Amount + " WHERE ORDER_ID=" + oPayInfo.OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to set the payment response value
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="sResponse">string</param>
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
        ///     Payment oPay = new Payment();
        ///     int OrderID;
        ///     string sResponse;
        ///     int iUpdateResponse;
        ///     ...
        ///     iUpdateResponse = oPay.UpdateResponse(OrderID,sResponse)
        /// } 
        /// </code>
        /// </example>
        [WebMethod]
        public int UpdateResponse(int OrderID, string sResponse)
        {
            try
            {
                String sSQL;
                sSQL = "UPDATE TBWC_PAYMENT SET CC_PAY_RESPONSE='" + sResponse + "' WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to get the payment response based on the order id 
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
        ///     Payment oPay = new Payment();
        ///     int OrderID;
        ///     string response;
        ///     ...
        ///     response = oPay.GetResponse(OrderID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetResponse(int OrderID)
        {
            String sSQL;
            String pResponse = "";
            DataSet dsPayment = new DataSet();
            try
            {
                sSQL = "SELECT CC_PAY_RESPONSE FROM TBWC_PAYMENT WHERE ORDER_ID =" + OrderID;
                oHelper.SQLString = sSQL;
                dsPayment = oHelper.GetDataSet("ResponseStatus");
                if (dsPayment != null)
                {
                    foreach (DataRow pDR in dsPayment.Tables["ResponseStatus"].Rows)
                    {
                        pResponse = pDR["CC_PAY_RESPONSE"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                pResponse = "";
            }
            return pResponse;
        }
        #endregion
    }

}