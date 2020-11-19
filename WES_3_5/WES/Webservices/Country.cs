using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.Common;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to get all the Country and State Details
    /// </summary>
    /// <remarks>
    /// Used to get the Countries List,States List,Company Names,Tax for each State...
    /// </remarks>
    /// <example>
    /// Country oCountry = new Country();
    /// </example>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Country : System.Web.Services.WebService
    {
        HelperDB oHelper = new HelperDB();
        ErrorHandler oErrHand = new ErrorHandler();
        #region "Tax Related Functions"
        /// <summary>
        /// This is Used to get the Countries list
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
        ///     Country oCountry = new Country();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetCountries();
        /// }
        /// </code>
        /// </example>
///
        [WebMethod]
        public DataSet GetCountries()
        {
            string sSQL;
            //DataSet dsOD = new DataSet();
            try
            {
                sSQL = "SELECT COUNTRY_NAME,COUNTRY_CODE FROM TBWC_COUNTRY ORDER BY COUNTRY_NAME";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("CountryDetails");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get the Country name using Country Code
        /// </summary>
        /// <param name="CountryCode">Sring</param>
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
        ///     Country oCountry = new Country();
        ///     string CountCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetCountryName(CountCode);
        /// }
        /// </code>
        /// </example>
        public DataSet GetCountryName(string CountryCode)
        {
            string sSQL;
            DataSet dsOS = new DataSet();
            try
            {
                sSQL = "SELECT COUNTRY_NAME FROM TBWC_COUNTRY WHERE COUNTRY_CODE ='" + CountryCode + "'";
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
        /// This is used to get all States using Country Code
        /// </summary>
        /// <param name="CountryCode">string</param>
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
        ///     Country oCountry = new Country();
        ///     string CountryCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetStates("CountryCode");
        /// }
        /// </code>
        /// </example>
        public DataSet GetStates(string CountryCode)
        {
            string sSQL;
            try
            {
                sSQL = "SELECT STATE_NAME,STATE_CODE,COUNTRY_CODE FROM TBWC_STATE WHERE COUNTRY_CODE ='" + CountryCode + "'";
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
        /// This is used to get the State Name using State Code
        /// </summary>
        /// <param name="StateCode">string</param>
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
        ///     Country oCountry = new Country();
        ///     string StateCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetStateName(StateCode);
        /// }
        /// </code>
        /// </example>
        public DataSet GetStateName(string StateCode)
        {
            string sSQL;
            try
            {
                sSQL = "SELECT STATE_NAME FROM TBWC_STATE WHERE STATE_CODE ='" + StateCode + "'";
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
        /// This is used to get the State Tax Value based on State Code
        /// </summary>
        /// <param name="CountryCode">string</param>
        /// <param name="StateCode">string</param>
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
        ///     Country oCountry = new Country();
        ///     string CountryCode;
        ///     string StateCode;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oCountry.GetStateTax(CountryCode, StateCode);
        /// }
        /// </code>
        /// </example>
        public decimal GetStateTax(string CountryCode, string StateCode)
        {
            string sSQL;
            decimal retVal;
            try
            {
                sSQL = "SELECT TAX_AMOUNT FROM TBWC_TAX WHERE STATE_CODE ='" + StateCode + "'and COUNTRY_CODE ='" + CountryCode + "'";
                oHelper.SQLString = sSQL;
                retVal = oHelper.CDEC(oHelper.GetValue("TAX_AMOUNT"));

            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retVal = 0;
            }
            return retVal;
        }

        //:::                                                                         :::
        //:::  This routine calculates the distance between two points (given the     :::
        //:::  latitude/longitude of those points). It is being used to calculate     :::
        //:::  the distance between two ZIP Codes or Postal Codes using our           :::
        //:::  ZIPCodeWorld(TM) and PostalCodeWorld(TM) products.                     :::
        //:::                                                                         :::
        //:::  Definitions:                                                           :::
        //:::    South latitudes are negative, east longitudes are positive           :::
        //:::                                                                         :::
        //:::  Passed to function:                                                    :::
        //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
        //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
        //:::    unit = the unit you desire for results                               :::
        //:::           where: 'M' is statute miles                                   :::
        //:::                  'K' is kilometers (default)                            :::
        //:::                  'N' is nautical miles                                  :::
        //:::                                                                         :::
        //:::  United States ZIP Code/ Canadian Postal Code databases with latitude   :::
        //:::  & longitude are available at http://www.zipcodeworld.com               :::
        //:::                                                                         :::
        //:::                                                                         :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        public double GetDistanceUsingZip(string ZipCode)
        {
            double Distance=0;
            try
            {
                string sSQL = "SELECT * FROM TBWC_ZIPCODES WHERE ZIPCODE='" + ZipCode + "'";
                oHelper.SQLString = sSQL;
                DataSet oDSZip = new DataSet();
                oDSZip = oHelper.GetDataSet();
                if (oDSZip != null)
                {
                    double DestLatitude = oHelper.CD(oDSZip.Tables[0].Rows[0]["Latitude"].ToString());
                    double DestLongitude = oHelper.CD(oDSZip.Tables[0].Rows[0]["Longitude"].ToString());
                    double SourceLatitude = 38.69067;
                    double SourceLongitude = -77.221509;
                    Distance = FindDistance(SourceLatitude, SourceLongitude, DestLatitude, DestLongitude, 'M');
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
            }
            return Distance;
        }

        private double FindDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }


        #endregion



    }
}

