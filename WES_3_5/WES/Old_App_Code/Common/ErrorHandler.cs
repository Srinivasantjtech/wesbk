using System;
using System.IO;
using System.Net;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



namespace TradingBell.Common
{
    /// <summary>
    ///  Establishing Error related objects
    /// </summary>
    /// <remarks>
    /// Used to create the Log file for each Exception
    /// </remarks>
    /// <example>
    /// ErrorHandler oErr = new ErrorHandler();
    /// </example>
    public class ErrorHandler
    {
        StreamWriter sw;
        Exception smException;
        
        #region "Properties"
        /// <summary>
        /// Set the Property for displaying Error Message
        /// </summary>
        public Exception ErrorMsg
        {
            get
            {
                return smException;
            }
            set
            {
                smException = value;
            }
        }
        #endregion



        #region "Functions"

        /// <summary>
        /// This is used to return the Exception message      
        /// </summary>
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
        ///     ErrorHandler oErr = new ErrorHandler();
        ///     string lastErr;
        ///     ...
        ///     lastErr = oErr.GetLastError();
        /// }
        /// </code>
        /// </example>

        public string GetLastError()
        {
            return smException.Message;
        }

        /// <summary>
        /// This is used to Create the Log file for Every Exception      
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
        ///     ErrorHandler oErr = new ErrorHandler();
        ///     ...
        ///     oErr.CreateLog();
        /// }
        /// </code>
        /// </example>

        public void CreateLog()
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "Log/log" + DateTime.Now.ToShortDateString().Replace("/", "").Trim() + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (File.Exists(Path) == false)
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                bool rst = WriteErrorLog(Path, smException);
            }
            catch (Exception ex)
            {
                smException = ex;
                //oErrHand.CreateLog();
            }
        }
        /// <summary>
        /// This is used to write the Exception in the Log File      
        /// </summary>
        /// <param name="strPathName">string</param>
        /// <param name="objException">Exception</param>
        /// <returns>True or False</returns>


        public bool WriteErrorLog(string strPathName, Exception objException)
        {
            string strException = string.Empty;
            bool bReturn = false;
            try
            {
                sw = new StreamWriter(strPathName, true);
                sw.WriteLine("Source        : " + objException.Source.ToString().Trim());
                sw.WriteLine("Method        : " + objException.TargetSite.Name.ToString());
                sw.WriteLine("Date        : " + DateTime.Now.ToLongTimeString());
                sw.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
                sw.WriteLine("Computer    : " + Dns.GetHostName().ToString());
                sw.WriteLine("Error        : " + objException.Message.ToString().Trim());
                sw.WriteLine("Stack Trace    : " + objException.StackTrace.ToString().Trim());
                sw.WriteLine("^^-------------------------------------------------------------------^^");
                sw.Flush();
                sw.Close();
                bReturn = true;
            }
            catch (Exception ex)
            {
                smException = ex;
                bReturn = false;
            }
            return bReturn;
        }

        #endregion
    }
}