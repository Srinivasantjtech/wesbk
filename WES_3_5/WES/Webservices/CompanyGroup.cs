using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
namespace TradingBell.WebServices
{
    /// <summary>
    /// This is used to get the Company Details
    /// </summary>
    /// <remarks>
    /// Used to get the Company Details like Company Name,Status of Company,Get Company Admin Name List...
    /// </remarks>
    /// <example>
    /// CompanyGroup oCom = new CompanyGroup();
    /// </example>
    [WebService(Namespace = "http://WebCat.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CompanyGroup : System.Web.Services.WebService
    {
        /// <summary>
        /// Create the Structure for Company Related Information
        /// </summary>
        /// <example>
        /// CompanyGroup.CompanyInfo oCI = new CompanyGroup.CompanyInfo();
        /// oCI.CompanyName;
        /// </example>
        public struct CompanyInfo
        {
            /// <summary>
            /// Company ID
            /// </summary>
            public string CompanyID;
            /// <summary>
            /// Company Name
            /// </summary>
            public string CompanyName;
            /// <summary>
            /// Company TaxID
            /// </summary>
            public string TaxID;
            /// <summary>
            /// Company Address 1
            /// </summary>
            public string Address1;
            /// <summary>
            /// Company Address 2
            /// </summary>
            public string Address2;
            /// <summary>
            /// Company Address 3
            /// </summary>
            public string Address3;
            /// <summary>
            /// Company City
            /// </summary>
            public string City;
            /// <summary>
            /// Company State
            /// </summary>
            public string State;
            /// <summary>
            /// Company Zip Code
            /// </summary>
            public string Zip;
            /// <summary>
            /// Company Country
            /// </summary>
            public string Country;
            /// <summary>
            /// Company Phone Number 1
            /// </summary>
            public string Phone1;
            /// <summary>
            /// Company Phone Number 2
            /// </summary>
            public string Phone2;
            /// <summary>
            /// Company TollFree Number
            /// </summary>
            public string TollFree;
            /// <summary>
            /// Company Fax Number
            /// </summary>
            public string Fax;
            /// <summary>
            /// Company Status
            /// </summary>
            public CompanyStatus Status;
            /// <summary>
            /// Company Email Id
            /// </summary>
            public string Email;
            /// <summary>
            /// Company CSC Code
            /// </summary>
            public string CSC;
            /// <summary>
            /// Company WebSite Address
            /// </summary>
            public string Web;
            /// <summary>
            /// Company Contact Person 
            /// </summary>
            public string ContactName;
            /// <summary>
            /// Company Pay Method
            /// </summary>
            public string PayMethod;
            /// <summary>
            /// Buyer's Group
            /// </summary>
            public string BuyerGroup;

        }
        HelperDB oHelper = new HelperDB();
        ErrorHandler oErr = new ErrorHandler();
        private int _mCompanyUser;
        private string _CompID_Type = "custom";

        /// <summary>
        /// Create Enum for Company Status Information
        /// </summary>
        public enum CompanyStatus
        {
            /// <summary>
            /// New Comapny
            /// </summary>
            NEWCOMPANY = 0,
            /// <summary>
            /// Active Company
            /// </summary>
            ACTIVE = 1,
            /// <summary>
            /// InActive Company
            /// </summary>
            INACTIVE = 2

        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CompanyGroup()
        {

        }
        #region "Properties"

        /// <summary>
        /// Set the Property for Company User 
        /// </summary>
        public int CompanyUser
        {
            get
            {
                return _mCompanyUser;
            }
            set
            {
                _mCompanyUser = value;
            }
        }
        public string CompID_Type
        {
            get
            {
                return _CompID_Type;
            }
            set
            {
                _CompID_Type = value;
            }
        }
        #endregion

        #region "Functions..."
        /// <summary>
        /// This is used to create Profile of the Company.
        /// </summary>
        /// <param name="oCI">CompanyInfo</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     CompanyGroup.CompanyInfo oCI;
        ///     int retVal;
        ///     ...
        ///     retVal = oCom.CreateProfile(oCI);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int CreateProfile(CompanyInfo oCI)
        {
            try
            {
                string sSQL = " INSERT INTO TBWC_COMPANY(";
                sSQL = sSQL + " COMPANY_ID,COMPANY_NAME,EIN_OR_TAXID,ADDRESS_LINE1,";
                sSQL = sSQL + " ADDRESS_LINE2,ADDRESS_LINE3,CITY,STATE,";
                sSQL = sSQL + " ZIP,COUNTRY,PHONE1,PHONE2,";
                sSQL = sSQL + " TOLLFREE_PHONE,FAX,EMAIL,";
                sSQL = sSQL + " COMPANY_STATUS,CORP_SECURITY_CODE,WEB,";
                sSQL = sSQL + " CREATED_USER,MODIFIED_USER,";
                sSQL = sSQL + " PRIMARY_CONTACT_NAME,PREFFERED_PAY_MTHD,BUYER_GROUP)";
                sSQL = sSQL + " VALUES('" + oCI.CompanyID + "','" + oCI.CompanyName.Replace("'","''") + "','" + oCI.TaxID + "','" + oCI.Address1.Replace("'","''") + "','";
                sSQL = sSQL + oCI.Address2.Replace("'", "''") + "','" + oCI.Address3.Replace("'", "''") + "','" + oCI.City.Replace("'", "''") + "','" + oCI.State.Replace("'", "''") + "','";
                sSQL = sSQL + oCI.Zip + "','" + oCI.Country.Replace("'", "''") + "','" + oCI.Phone1 + "','" + oCI.Phone2 + "','";
                sSQL = sSQL + oCI.TollFree + "','" + oCI.Fax + "','" + oCI.Email.Replace("'", "''") + "','";
                sSQL = sSQL + oCI.Status + "','" + oCI.CSC + "','" + oCI.Web + "',";
                sSQL = sSQL + _mCompanyUser + "," + _mCompanyUser + ",'";
                sSQL = sSQL + oCI.ContactName.Replace("'", "''") + "','" + oCI.PayMethod + "','" + oCI.BuyerGroup + "')";
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
        /// <summary>
        /// This is used to get the Company Id
        /// </summary>
        /// <param name="CompanyName">string</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyName;
        ///     string CompanyID;
        ///     ...
        ///     CompanyID = oCom.GetCompanyID(CompanyName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetCompanyID(string CompanyName)
        {
            try
            {
                string CompanyID = "";
                DataTable dtCompany = new DataTable();
                string sSQL = " SELECT COMPANY_ID FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_NAME='" + CompanyName + "'";
                oHelper.SQLString = sSQL;
                dtCompany = oHelper.GetDataTable();
                if (dtCompany.Rows.Count > 0)
                {
                    CompanyID = dtCompany.Rows[0].ItemArray[0].ToString();
                }
                return CompanyID;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
        }
        /// <summary>
        /// This is used to check whether the Company Exists or Not.
        /// </summary>
        /// <param name="CompanyID">string</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     bool isValid;
        ///      ...
        ///     isValid = oCom.IsCompanyExist(CompanyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool IsCompanyExist(string CompanyID)
        {
            bool isValid = false;
            try
            {
                DataTable dtCompany = new DataTable();
                string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                sSQL = sSQL + " AND COMPANY_STATUS ='" + CompanyStatus.ACTIVE.ToString() + "'";
                oHelper.SQLString = sSQL;
                dtCompany = oHelper.GetDataTable();
                if (dtCompany.Rows.Count > 0)
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
            }
            return isValid;
        }
        /// <summary>
        /// This is used to check the User of the Company Exist or Not
        /// </summary>
        /// <param name="CompanyID">string</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     bool isExist;
        ///      ...
        ///     isExist = oCom.IsCompanyUserExist(CompanyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool IsCompanyUserExist(string CompanyID)
        {
            bool isValid = false;
            try
            {
                DataTable dtCompany = new DataTable();
                string sSQL = " SELECT USER_ID FROM TBWC_COMPANY_BUYERS";
                sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                oHelper.SQLString = sSQL;
                dtCompany = oHelper.GetDataTable();
                if (dtCompany.Rows.Count > 0)
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
            }
            return isValid;
        }
        /// <summary>
        /// This is used to check the Company Status
        /// </summary>
        /// <param name="UserID">int</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     int Userid;
        ///     string chkStatus; 
        ///      ...
        ///     chkStatus = oCom.CheckCompanyStatus(Userid);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string CheckCompanyStatus(int UserID)
        {
            string CompanyStatus = "";
            try
            {
                string sSQL = " SELECT COMPANY_STATUS FROM TBWC_COMPANY WHERE COMPANY_ID =(";
                sSQL = sSQL + " SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = " + UserID + ")";
                oHelper.SQLString = sSQL;
                CompanyStatus = oHelper.GetValue("COMPANY_STATUS");
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
            }
            return CompanyStatus;
        }
        /// <summary>
        /// This is used to get the Company Informations using Company Name
        /// </summary>
        /// <param name="CompanyName">string</param>
        /// <returns>CompanyInfo</returns>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompName;
        ///     CompanyInfo oCI;
        ///     ...
        ///     oCI = oCom.GetCompanyInfo(CompName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public CompanyInfo GetCompanyInfo(string CompanyName)
        {
            DataSet dsCom = new DataSet();
            CompanyInfo oCI = new CompanyInfo();
            try
            {
                string sSQL = " SELECT * FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_NAME ='" + CompanyName.Replace("'","''") + "'";
                oHelper.SQLString = sSQL;
                dsCom = oHelper.GetDataSet("Company");
                foreach (DataRow row in dsCom.Tables["Company"].Rows)
                {
                    oCI.CompanyID = row["COMPANY_ID"].ToString();
                    oCI.CompanyName = oHelper.CS(row["COMPANY_NAME"]);
                    oCI.Address1 = oHelper.CS(row["ADDRESS_LINE1"]);
                    oCI.Address2 = oHelper.CS(row["ADDRESS_LINE2"]);
                    oCI.Address3 = oHelper.CS(row["ADDRESS_LINE3"]);
                    oCI.City = oHelper.CS(row["CITY"]);
                    oCI.State = oHelper.CS(row["STATE"]);
                    oCI.Zip = oHelper.CS(row["ZIP"]);
                    oCI.Country = oHelper.CS(row["COUNTRY"]);
                    oCI.Phone1 = oHelper.CS(row["PHONE1"]);
                    oCI.Phone2 = oHelper.CS(row["PHONE2"]);
                    oCI.TollFree = oHelper.CS(row["TOLLFREE_PHONE"]);
                    oCI.Fax = oHelper.CS(row["FAX"]);
                    oCI.Web = oHelper.CS(row["WEB"]);
                    oCI.Email = oHelper.CS(row["EMAIL"]);
                    oCI.ContactName = oHelper.CS(row["PRIMARY_CONTACT_NAME"]);
                    oCI.CSC = oHelper.CS(row["CORP_SECURITY_CODE"]);

                    oCI.PayMethod = oHelper.CS(row["PREFFERED_PAY_MTHD"]);
                    oCI.Status = (CompanyStatus)row["COMPANY_STATUS"];
                    oCI.BuyerGroup = oHelper.CS(row["BUYER_GROUP"]);
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();

            }
            return oCI;
        }
        /// <summary>
        /// This is used to get the Company Name
        /// </summary>
        /// <param name="CompanyID">string</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     string CompGroup;
        ///     ...
        ///     CompGroup = oCom.GetCompanyName(CompanyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetCompanyName(string CompanyID)
        {
            string RetVal = "";
            try
            {
                string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                oHelper.SQLString = sSQL;
                RetVal = oHelper.GetValue("COMPANY_NAME");

            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
                RetVal = "-1";
            }
            return RetVal;
        }

        /// <summary>
        /// This is used to get the Details of the Company Administrator
        /// </summary>
        /// <param name="CompanyID">int</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     string RetVal;
        ///     ...
        ///     RetVal = oCom.GetCompanyAdmin(CompanyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetCompanyAdmin(string CompanyID)
        {
            string RetVal = "";
            try
            {
                string sSQL = " SELECT WCU.USER_EMAIL FROM TBWC_USER WCU,TBWC_COMPANY_BUYERS WCC";
                sSQL = sSQL + " WHERE WCU.USER_ID = WCC.USER_ID  ";
                sSQL = sSQL + " AND WCU.USER_ROLE ='" + TradingBell.Common.User.UserRole.COMPANYADMIN.ToString() + "'";
                sSQL = sSQL + " AND  WCC.COMPANY_ID ='" + CompanyID + "'";
                oHelper.SQLString = sSQL;
                RetVal = oHelper.GetValue("USER_EMAIL");

            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
                RetVal = "-1";
            }
            return RetVal;
        }
        /// <summary>
        /// This is used to check whether the Company Details Exists or Not
        /// </summary>
        /// <param name="CompanyName">string</param>
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
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyName;
        ///     bool isExists;
        ///     ...
        ///     isExists = oCom.isCompanyNameExists(CompanyName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public bool isCompanyNameExists(string CompanyName)
        {
            bool RetVal = false;
            string ComName = "";
            try
            {
                string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_Name='" + CompanyName.Replace("'","''") + "'";
                oHelper.SQLString = sSQL;
                ComName = oHelper.GetValue("COMPANY_NAME");
                if (ComName != string.Empty)
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                RetVal = true;
            }
            return RetVal;
        }

        [WebMethod]
        public bool isCompanyIDExists(string CompanyID)
        {
            bool RetVal = false;
            string ComID = "";
            try
            {
                string sSQL = " SELECT COMPANY_ID FROM TBWC_COMPANY";
                sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                oHelper.SQLString = sSQL;
                ComID = oHelper.GetValue("COMPANY_ID");
                if (ComID != string.Empty)
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                RetVal = true;
            }
            return RetVal;
        }

        [WebMethod]
        public string GenerateCompanyID()
        {
            int TempCompID = 0;
            //string CompanyID = "";
            if (_CompID_Type == "auto")
            {
                string sSql = "SELECT MAX(CONVERT(INT,COMPANY_ID)) AS COMPANY_ID FROM TBWC_COMPANY";
                oHelper.SQLString = sSql;
                if (oHelper.GetValue("COMPANY_ID") == null)
                {
                    TempCompID = 10000;
                }
                else
                {
                    TempCompID = oHelper.CI(oHelper.GetValue("COMPANY_ID").ToString());
                }
                TempCompID = TempCompID + 1;
            }
            return TempCompID.ToString();
        }
        #endregion
    }

}