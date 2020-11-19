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
using System.Diagnostics;
using System.Security.Cryptography;
namespace TradingBell.Common 
{
    public delegate System.Security.Cryptography.ICryptoTransform GetEncryptengine();
    /// <summary>
    /// Establishing the Connection.      
    /// <para>  
    /// Make Connection.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Used to Connect the Database 
    /// </remarks>
    /// <example>
    /// Connection oCon = new Connection();
    /// </example>

    public class Connection
    {
        private string smConnectionString;
        OleDbConnection smConnection;
        SqlConnection wesConnection;
        Hashtable tbGlobals = new Hashtable();
        IDbTransaction mTransaction;
        ErrorHandler oErrHand;
        # region "Properties"

        /// <summary>
        /// Get the Connection State from OleDbConnection
        /// </summary>        
        public ConnectionState GetConnectionState
        {
            get
            {
                return smConnection.State;
            }
        }
        /// <summary>
        /// Returns the Connection string 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return smConnectionString;
            }
            set
            {
                ConnectionString = smConnectionString;
            }
        }
        /// <summary>
        /// Returns the Transaction Value
        /// </summary>
        public IDbTransaction Transaction
        {
            get
            {
                return mTransaction;
            }
            set
            {
                mTransaction = value;
            }
        }
        #endregion

        # region "Initialization Functions "
        /// <summary>
        /// Establishing Connection String      
        /// </summary>       
        public Connection()
        {
            oErrHand = new ErrorHandler();
            string constring;
            try
            {
                constring = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                //Decrypt the Connection String.
                //System.Security.Cryptography.SymmetricAlgorithm R_Encryption_engine;
                //R_Encryption_engine = Get_Encryption_Engine();
                //GetEncryptengine Decryptor = new GetEncryptengine(R_Encryption_engine.CreateDecryptor);
                smConnectionString = constring;// System.Text.Encoding.Default.GetString(Transform(Convert.FromBase64String(constring), Decryptor()));
                smConnection = new OleDbConnection();
                //wesConnection = new SqlConnection(); 
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
        }
        /// <summary>
        /// Establishing Connection using Connection String      
        /// </summary>
        /// <param name="sConnectionString">string</param>
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
        ///     string sConnectionString;
        ///     ...
        ///     Connection oCon = new Connection(sConnectionString); 
        /// }
        /// </code> 
        /// </example>

        public Connection(string sConnectionString)
        {
            oErrHand = new ErrorHandler();
            try
            {
                smConnectionString = sConnectionString;
                smConnection = new OleDbConnection();
                
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
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
        #endregion

        # region "Connection Related Functions"
        /// <summary>
        /// This is used to open the OleDbConnection   
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
        ///     Connection oCon = new Connection();
        ///     ...
        ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
        /// }
        /// </code>
        /// </example>
        /// <returns>OleDbConnection</returns>

        public OleDbConnection GetConnection()
        {
            oErrHand = new ErrorHandler();
            try
            {
                OpenConnection();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
            return smConnection;
        }
        /// <summary>
        /// This is used to Get Connection using Connection String     
        /// </summary>
        /// <param name="sConnectionString">string</param>
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
        ///     Connection oCon = new Connection();
        ///     string sConnectionString;
        ///     ...
        ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection(sConnectionString));
        /// }
        /// </code>
        /// </example>
        /// <returns>OleDbConnection</returns>

        public OleDbConnection GetConnection(string sConnectionString)
        {
            oErrHand = new ErrorHandler();
            try
            {
                smConnectionString = sConnectionString;
                OpenConnection();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
            return smConnection;
        }
     
        /// <summary>
        /// This is used to Open the Connection     
        /// </summary>
        /// <param></param>
        /// <returns>True or False</returns>
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
        ///     Connection oCon = new Connection();
        ///     bool isConnected;
        ///     ...
        ///     isConnected = oCon.OpenConnection();
        /// }
        /// </code>
        /// </example>
        public bool OpenConnection()
        {
            bool isConnected = false;
            oErrHand = new ErrorHandler();
            try
            {
                //if (!(smConnection.State == System.Data.ConnectionState.Open))
                //{
                    smConnection.ConnectionString = smConnectionString;
                     smConnection.Open();
                    isConnected = true;
                //}
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                isConnected = false;
                //oErrHand.CreateLog();
            }
            return isConnected;
        }
        /// <summary>
        /// This is used to close the Connection      
        /// </summary>
        /// <returns>True or False</returns>
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
        ///     Connection oCon = new Connection(); 
        ///     bool isClose;
        ///     ...
        ///     isClose = oCon.CloseConnection();
        /// } 
        /// </code>
        /// </example>
        public bool CloseConnection()
        {
            oErrHand = new ErrorHandler();
            bool isClose;
            try
            {
                smConnection.Close();
                isClose = true;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                isClose = false;
            }
            return isClose;
        }
        #endregion

    }
}
