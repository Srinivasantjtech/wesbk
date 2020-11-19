using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using System.Text;


namespace TradingBell.Common
{
    /// <summary>
    /// Helping objects for entire class
    /// </summary>
    /// <remarks>
    /// This is used to get the DataSet values and get the DataTable using Table Names
    /// </remarks>
    /// <example>
    /// Helper oHelper=new Helper();
    /// </example>
    public class Helper
    {
        Connection oCon = new Connection();

        SqlConnection GCon = new SqlConnection();
        string StrConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
        /// <summary>
        /// Hash Table
        /// </summary>
        public static Hashtable WebCatGlb = new Hashtable();
        private string sSqlStr;
        ErrorHandler oErrHand = new ErrorHandler();
        public Helper()
        {
            //GetInitialDetails();
        }
        #region "Properties"
        /// <summary>
        /// Set the Property for SQLString
        /// </summary>
        public string SQLString
        {
            get
            {
                return sSqlStr;
            }
            set
            {
                sSqlStr = value;
            }
        }
        #endregion
        #region "Functions"
        /// <summary>
        ///This is used to get the data from the Table and fill it into the Dataset
        /// </summary>
        /// <param name="sTableName">string</param>
        /// <returns>
        /// Set of Data from the Table sTableName
        /// </returns>
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
        ///     Helper oHelper=new Helper();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oHelper.GetDataSet("CategoryList");
        ///     /// Here "CategoryList" is TableName
        /// }
        /// </code>
        /// </example>
        public DataSet GetDataSet(string sTableName)
        {
            DataSet oDs = new DataSet();
            try
            {
                OleDbDataAdapter oDA = new OleDbDataAdapter(SQLString, oCon.ConnectionString);
                oDA.Fill(oDs, sTableName);
                oDA.Dispose();
                if (oDs.Tables[sTableName].Rows.Count == 0)
                {
                    oDs = null;
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                //oDs = null;
            }
            finally
            {
                //oDs.Dispose();
            }
            return oDs;

        }

        /// <summary>
        /// This is used to get the Dataset Values
        /// </summary>
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
        ///     Helper oHelper=new Helper();
        ///     ...
        ///     DataSet oDS = new DataSet();
        ///     oDS = oHelper.GetDataSet();
        /// }
        /// </code>
        /// </example>
        public DataSet GetDataSet()
        {
            DataSet oDs = new DataSet();
            try
            {
                OleDbDataAdapter oDA = new OleDbDataAdapter(SQLString, oCon.ConnectionString);
                oDA.Fill(oDs);
                oDA.Dispose();
                if (oDs.Tables[0].Rows.Count == 0)
                {
                    oDs = null;
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                //oDs = null;
            }
            return oDs;
        }
        /// <summary>
        /// This is used to get the values and return as Data Table
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
        ///     Helper oHelper=new Helper();
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = Helper.GetDataTable();
        /// }
        /// </code>
        /// </example>
        public DataTable GetDataTable()
        {
            DataTable oDt = new DataTable();
            try
            {
                OleDbDataAdapter oDA = new OleDbDataAdapter(SQLString, oCon.ConnectionString);
                oDA.Fill(oDt);
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                oDt = null;
            }
            finally
            {
                oDt.Dispose();
            }
            return oDt;
        }
        /// <summary>
        /// This is used to Execute the SQL Query 
        /// </summary>
        /// <returns>integer</returns>
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
        ///     Helper oHelper=new Helper();
        ///     int rValue;
        ///     ...
        ///     rValue = oHelper.ExecuteSQLQuery();
        /// }
        /// </code>
        /// </example>
        public int ExecuteSQLQuery()
        {
            int rValue;
            OleDbConnection con;
            con = new OleDbConnection(oCon.ConnectionString);
            try
            {
                con.Open();
                OleDbCommand oCmd = new OleDbCommand(SQLString, con);
                rValue = oCmd.ExecuteNonQuery();
                oCmd.Dispose();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                rValue = -1;
            }
            finally
            {
                con.Close();
            }

            return rValue;
        }
        /// <summary>
        /// This is used to get the Value from the Column
        /// </summary>
        /// <param name="ColumnName">string</param>
        /// <returns>String</returns>
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
        ///     Helper oHelper=new Helper();
        ///     string rValue;
        ///     ...
        ///     rValue = oHelper.GetValue("COMPANY_STATUS");
        /// //  Here "COMPANY_STATUS" is the Column Name
        /// }
        /// </code>
        /// </example>
        public string GetValue(string ColumnName)
        {
            string rValue = "";
            DataSet oDs = new DataSet();
            try
            {
                OleDbDataAdapter oDA = new OleDbDataAdapter(SQLString, oCon.ConnectionString);
                oDA.Fill(oDs);
                oDA.Dispose();
                if (oDs != null)
                    foreach (DataRow oRow in oDs.Tables[0].Rows)
                    {
                        if (oRow[ColumnName] != DBNull.Value)
                        {
                            rValue = oRow[ColumnName].ToString();
                        }
                        else
                        {
                            rValue = "-1";
                        }
                    }

            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                rValue = "-1";
            }
            return rValue;
        }
        /// <summary>
        /// This is used to Read the Config XML File
        /// </summary>
        ///// <example>
        ///// <code>
        ///// using System;
        ///// using System.IO;
        ///// using System.Web;
        ///// using System.Data;
        ///// using TradingBell.Common;
        ///// using TradingBell.WebServices;
        ///// 
        ///// protected void Page_Load(object sender, EventArgs e)
        ///// {
        /////     Helper oHelper=new Helper();
        /////     ...
        /////     oHelper.ReadConfigXML();
        ///// }
        ///// </code>
        ///// </example>
        public void ReadConfigXML()
        {
            DataSet oDs = new DataSet();
            try
            {
                WebCatGlb = new Hashtable();
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;
                oDs.ReadXml(sPath.BaseDirectory + @"\WebCat.xml");
                oDs.Dispose();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
            }
            finally
            {
                oDs.Dispose();
            }
        }
        /// <summary>
        /// This is used to get the Initail Details of Store Options Table
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
        ///     Helper oHelper=new Helper();
        ///     ...
        ///     oHelper.GetInitialDetails();
        /// }
        /// </code>
        /// </example>
        //public void GetInitialDetails()
        //{
        //    WebCatGlb = new Hashtable();
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        sSqlStr = " SELECT OPTION_NAME,OPTION_VALUE FROM TBWC_STORE_OPTIONS";
        //        oDs = GetDataSet();
        //        foreach (DataRow oDr in oDs.Tables[0].Rows)
        //        {
        //            WebCatGlb.Add(oDr[0], oDr[1]);
        //        }
        //        oDs.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //    }
        //}
        /// <summary>
        /// This is used to get the Values of Store Option Values based on the Object Name
        /// </summary>
        /// <param name="oName">string</param>
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
        ///     Helper oHelper=new Helper();
        ///     string retVal;
        ///     ...
        ///     retVal = oHelper.GetOptionValues("CURRENCY");
        ///     // Here "CURRENCY" is the OPTION NAME
        /// }
        /// </code>
        /// </example>
        public string GetOptionValues(string oName)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sSqlStr = "SELECT OPTION_VALUE FROM TBWC_STORE_OPTIONS WHERE OPTION_NAME= '" + oName + "' and WEBSITE_ID = " + websiteid;
                SQLString = sSqlStr;
                return GetValue(("OPTION_VALUE").ToString());
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return "";
            }
        }
        #endregion
        # region "Utility Functions.."

        /// <summary>
        /// This is used to replace the single Quotes as string
        /// </summary>
        /// <param name="sValue">string</param>
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
        ///     Helper oHelper=new Helper();
        ///     string sValue;
        ///     string sReturnValue;
        ///     ...
        ///     sReturnValue = oHelper.Prepare(sValue);
        /// }
        /// </code>
        /// </example>
        /// 

        public string Encrypt(string Input, string key)
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(Input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return "";
            }
        }

        public string Decrypt(string input, string key)
        {
            try
            {
                input = input.Replace(" ", "+");
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                return "";
            }
        }

        public string Prepare(string sValue)
        {
            try
            {
                string sReturnValue;
                sReturnValue = sValue.Replace("'", "''");
                return sReturnValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return "";
            }
        }

        /// <summary>
        /// This is used to convert the object into String
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>string></returns>
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     string sRetValue;
        ///     ...
        ///     sRetValue = oHelper.CS(ObjectName);
        /// }
        /// </code>
        /// </example>
        public string CS(object obj)
        {
            try
            {
                string sRetValue = null;
                if (obj != null && obj != DBNull.Value)
                {
                    sRetValue = Convert.ToString(obj);
                }
                return sRetValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return "";
            }
        }
        /// <summary>
        ///  This is used to fix 2 Decimal places
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>String</returns>
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
        ///     Helper oHelper=new Helper();
        ///     decimal ObjectName;
        ///     string sRetValue;
        ///     ...
        ///     sRetValue = oHelper.FixDecPlace(ObjectName);
        /// }
        /// </code>
        /// </example>
        public string FixDecPlace(decimal obj)
        {
            try
            {
                string sRetValue = null;
                sRetValue = obj.ToString("N2");
                return sRetValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return "";
            }
        }
        /// <summary>
        ///  This is used to convert the object into integer
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>integer</returns>
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     int retValue;
        ///     ...
        ///     retValue = oHelper.CI(ObjectName);
        /// }
        /// </code>
        /// </example>
        public int CI(object obj)
        {
            try
            {
                int retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt32(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Long Integer
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Int64</returns>
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     Int64 retValue;
        ///     ...
        ///     retValue = oHelper.CLI(ObjectName);
        /// }
        /// </code>
        /// </example>
        public Int64 CLI(object obj)
        {
            try
            {
                Int64 retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt64(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return -1;
            }
        }

        /// <summary>
        ///  This is used to convert the object into Double
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>double</returns>
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     double retValue;
        ///     ...
        ///     retValue = oHelper.CD(ObjectName);
        /// }
        /// </code>
        /// </example>
        public double CD(object obj)
        {
            try
            {
                double retValue = 0.0;
                if (obj != null && obj != DBNull.Value)
                {
                    retValue = Convert.ToDouble(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Decimal Values
        /// </summary>
        /// <param name="obj">Object</param>
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     decimal retValue;
        ///     ...
        ///     retValue = oHelper.CDEC(ObjectName);
        /// }
        /// </code>
        /// </example>
        public decimal CDEC(object obj)
        {
            try
            {
                decimal retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToDecimal(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Bool Value(T/F)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>integer</returns> 
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
        ///     Helper oHelper=new Helper();
        ///     object ObjectName;
        ///     int retValue;
        ///     ...
        ///     retValue = oHelper.CB(ObjectName);
        /// }
        /// </code>
        /// </example>
        public int CB(object obj)
        {
            try
            {
                int retValue = 0;
                if ((bool)obj)
                {
                    retValue = 1;
                }
                return retValue;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to Encrypt the String
        /// </summary>
        /// <param name="EncryptStrValue">string</param>
        /// <returns>string></returns>
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
        ///     Helper oHelper=new Helper();
        ///     string sEnValue;
        ///     string eEncrypt;
        ///     ...
        ///     eEncrypt = oHelper.StringEnCrypt(EncryptStrValue);
        /// }
        /// </code>
        /// </example>
        //public string EnCrypt(string sEnValue)
        //{   
        //    try
        //    {
        //     return sEnValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return "";
        //    }
        //}
        /// 
        //public string DeCrypt(string sDeValue)
        //{
        //    try
        //    {
        //    return sDeValue;
        //    }
        //    catch (Exception ex)
        //    {
        //    oErrHand.ErrorMsg = ex;
        //    //oErrHand.CreateLog();
        //    return "";
        //    }
        //}
        public string StringEnCrypt(string EncryptStrValue)
        {
            try
            {
                string EncryText = Encrypt(EncryptStrValue, true);
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return null;
            }
        }

        

        ///<summary>
        ///  This is used to Decrypt the String
        /// </summary>
        /// <param name="DecryptStrValue">string</param>
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
        ///     Helper oHelper=new Helper();
        ///     string sDeValue;
        ///     string sDecrypt;
        ///     ...
        ///     sDecrypt = oHelper.StringDeCrypt(DecryptStrValue);
        /// }
        /// </code>
        /// </example>
        public string StringDeCrypt(string DecryptStrValue)
        {
            try
            {
                string Decryptext = Decrypt(DecryptStrValue, true);

                //System.Security.Cryptography.SymmetricAlgorithm R_Encryption_engine;
                //R_Encryption_engine = Get_Encryption_Engine();
                //GetEncryptengine Decryptor = new GetEncryptengine(R_Encryption_engine.CreateDecryptor);
                //string Decryptext = System.Text.Encoding.Default.GetString(Transform(Convert.FromBase64String(DecryptStrValue), Decryptor()));
                return Decryptext;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                return null;
            }
        }
        public static System.Security.Cryptography.SymmetricAlgorithm Get_Encryption_Engine()
        {
            System.Security.Cryptography.SymmetricAlgorithm Encryption_engine;
            Encryption_engine = new System.Security.Cryptography.RijndaelManaged();
            Encryption_engine.Mode = System.Security.Cryptography.CipherMode.CBC;
            Encryption_engine.Key = Convert.FromBase64String("U1fknVDCPQWERTYGZfRqvAYCK7gFpUukYKOqsCuN8XU=");
            Encryption_engine.IV = Convert.FromBase64String("vEQWERTYRMrovjV+NXos5g==");
            return Encryption_engine;
        }
        public byte[] Transform(byte[] Source, System.Security.Cryptography.ICryptoTransform Transformer)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cryptographic_stream = new System.Security.Cryptography.CryptoStream(stream, Transformer, System.Security.Cryptography.CryptoStreamMode.Write);
            cryptographic_stream.Write(Source, 0, Source.Length);
            cryptographic_stream.FlushFinalBlock();
            cryptographic_stream.Close();
            return stream.ToArray();
        }


        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        /// 

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        #endregion
        # region "SQL Connection"
        public SqlDataReader ExecuteReader(string strSql)
        {
            SqlDataReader SqlDr=null;
            try
            {


                SqlConnection _SQLConn = new SqlConnection(StrConnectionString.Substring(StrConnectionString.IndexOf(';') + 1));

                SqlCommand sqlCommand = new SqlCommand(strSql, _SQLConn);
                _SQLConn.Open(); 
                SqlDr= sqlCommand.ExecuteReader();
                
            }
            catch (Exception ex) { 

            }
            return SqlDr;
        }
        public DataTable  GetDataTable(string strSql)
        {
            DataSet Dst = new DataSet();
            try
            {
                SqlConnection _SQLConn = new SqlConnection(StrConnectionString.Substring(StrConnectionString.IndexOf(';') + 1));
                
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, _SQLConn);
                sqlAdapter.Fill(Dst);
                _SQLConn.Close(); 

            }
            catch (Exception ex)
            {

            }
            return Dst.Tables[0] ;
        }        

        #endregion

    }


}