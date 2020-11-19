using System;
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
    /// This is used to get all User's Login Details
    /// </summary>
    /// <remarks>
    /// Used to Track the User Session
    /// </remarks>
    /// <example>
    /// UserSession UserSess=new UserSession();
    /// </example>
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserSession : System.Web.Services.WebService
    {

        HelperDB oHelper = new HelperDB();
        User oUser = new User();
        ErrorHandler oErr = new ErrorHandler();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserSession()
        {
        }
        /// <summary>
        /// Create the Structure for manipulating User session Information
        /// </summary>
        /// <example>
        /// UserSession.UserSessionInfo oUSI ;
        /// oUSI.Session_ID;
        /// </example>
        public struct UserSessionInfo
        {
            /// <summary>
            /// Session ID
            /// </summary>
            public string Session_ID;
            /// <summary>
            ///  Last IP Address
            /// </summary>
            public string Last_IP;
            /// <summary>
            /// Session Data
            /// </summary>
            public string Session_Data;
            /// <summary>
            /// Referal URL
            /// </summary>
            public string Referal_URL;
            /// <summary>
            /// User ID
            /// </summary>
            public int User_ID;

        }


        /// <summary>
        /// This is used to insert the track session details
        /// </summary>
        /// <param name="oUSI">UserSessionInfo</param>
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
        ///     UserSession oUS = new UserSession();
        ///     UserSession.UserSessionInfo oUSI = new UserSession.UserSessionInfo();
        ///     int TrkSession;
        ///     ...
        ///     TrkSession = oUS.TrackSession(oUSI);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int TrackSession(UserSessionInfo oUSI)
        {
            try
            {
                string sSQL = " INSERT INTO TBWC_USER_SESSION(";
                sSQL = sSQL + " SESSION_ID,LAST_IP_ADDRESS,";
                sSQL = sSQL + " SESSION_DATA,";
                sSQL = sSQL + " REFERAL_URL,CREATED_USER,";
                sSQL = sSQL + " USER_ID) VALUES ('";
                sSQL = sSQL + oUSI.Session_ID + "','" + oUSI.Last_IP + "','";
                sSQL = sSQL + oUSI.Session_Data + "','";
                sSQL = sSQL + oUSI.Referal_URL + "'," + TradingBell.Common.User._StoreAdminID + ",";
                sSQL = sSQL + oUSI.User_ID + ")";

                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
                return -1;
            }

        }

    }

}
