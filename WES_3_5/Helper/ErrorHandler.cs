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
using System.Threading;



namespace TradingBell.WebCat.Helpers
{
    /*********************************** J TECH CODE ***********************************/
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
        /*********************************** DECLARATION ***********************************/
        StreamWriter sw;
        Exception smException;
        string strlog;
        /*********************************** DECLARATION ***********************************/
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
        public string ExeTimelog
        {
            get
            {
                return strlog;
            }
            set
            {
                strlog = value;
            }
        }
        #endregion



        #region "Functions"
        /*********************************** OLD CODE ***********************************/
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
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE LAST ERROR  ***/
        /********************************************************************************/

        public string GetLastError()
        {
            return smException.Message;
        }
        /*********************************** OLD CODE ***********************************/
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
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE LOG FILE TO HOLD ERROR DETAIL TEXT DOCUMENT ***/
        /********************************************************************************/
        public void CreateLog()
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "Log/log" + DateTime.Now.ToShortDateString().Replace("/", "").Trim() + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (!(File.Exists(Path)))
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE FILE FOR ONLINE PAYMENT  ***/
        /********************************************************************************/
        public void CreateLog(string Request_id,string xml)
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                

                string FName = "Pay/" + Request_id + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");

                if (!(Directory.Exists(sPath.BaseDirectory + "Pay")))
                    Directory.CreateDirectory(sPath.BaseDirectory + "Pay");

                if (!(File.Exists(Path)))
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                try
                {
                    sw = new StreamWriter(Path, true);
                    sw.WriteLine(xml);                 
                    sw.Flush();
                    sw.Close();                    
                }
                catch (Exception ex)
                {
                    
                }
            }
            catch (Exception ex)
            {
                smException = ex;
                //oErrHand.CreateLog();
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE TIME LOG TO SHOW ERROR WRITTEN TIME  ***/
        /********************************************************************************/
        public void CreateTimeLog()
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "Log/log" + DateTime.Now.ToShortDateString().Replace("/", "").Trim() + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (!(File.Exists(Path)))
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                bool rst = WriteTimeLog(Path);
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO WRITE ERROR DETAILS INTO TEXT DOCUMENT  ***/
        /********************************************************************************/
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO WRITE TIME DETAILS INTO TEXT DOCUMENT  ***/
        /********************************************************************************/
        public bool WriteTimeLog(string strPathName)
        {
            string strException = string.Empty;
            bool bReturn = false;
            try
            {
                sw = new StreamWriter(strPathName, true);
                sw.WriteLine("Estimated Time : " + strlog  );
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

            public void CreateLog(string STRVALUE)
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "Lognew/TimeRecord"  + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (!(File.Exists(Path)))
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                bool rst = WriteErrorLog(Path,STRVALUE);
            }
            catch (Exception ex)
            {
                smException = ex;
                //oErrHand.CreateLog();
            }
        }

        public bool WriteErrorLog(string strPathName,string str)
        {
            string strException = string.Empty;
            bool bReturn = false;
            try
            {
                sw = new StreamWriter(strPathName, true);
                //sw.WriteLine("Source        : " + objException.Source.ToString().Trim());
                sw.WriteLine("Method        : " + str.ToString());
                sw.WriteLine("Date        : " + DateTime.Now.TimeOfDay);
                sw.WriteLine("Time        : " + DateTime.Now.ToShortDateString());
                //sw.WriteLine("Computer    : " + Dns.GetHostName().ToString());
                //sw.WriteLine("Error        : " + objException.Message.ToString().Trim());
                //sw.WriteLine("Stack Trace    : " + objException.StackTrace.ToString().Trim());
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

        public void CreatePayLog(string strvalue)
        {
            try
            {
                Thread thlog = new Thread(new ParameterizedThreadStart(CreatePayLogThread));
                thlog.Start(strvalue);
            }
            catch (Exception ex)
            {
                smException = ex;
            }
        }

        public void CreatePayLogThread(object strvalue)
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "PayLog/PayRecord" +DateTime.Now.ToString("ddMMyyyy")+".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (File.Exists(Path) == false)
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                bool rst = WriteErrorLog(Path, (string)strvalue);
            }
            catch (Exception ex)
            {
                smException = ex;
            }
        }


        #endregion
    }
    /*********************************** J TECH CODE ***********************************/
}