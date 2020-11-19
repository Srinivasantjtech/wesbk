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
using System.Security.Cryptography;
using System.Security;
using System.Text;


namespace TradingBell.WebCat.Helpers
{
    /*********************************** J TECH CODE ***********************************/
    public class Security
    {
        /*********************************** DECLARATION ***********************************/
        ErrorHandler objErrorHandler = new ErrorHandler();
        /*********************************** DECLARATION ***********************************/

        /*********************************** CONSTRUCTOR ***********************************/
        public Security()
        {
            //GetInitialDetails();
        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING USING TRIPLE DES  ***/
        /********************************************************************************/
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
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING VALUE USING TRIPLE DES  ***/
        /********************************************************************************/
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
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        ///<summary>
        ///  This is used to Decrypt the String
        /// </summary>
        /// <param name="DecryptStrValue">string</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING   ***/
        /********************************************************************************/
        public string StringDeCrypt(string DecryptStrValue)
        {
            try
            {
                string Decryptext = Decrypt(DecryptStrValue, true,"");
                return Decryptext;
            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog();
                objErrorHandler.CreateLog(DecryptStrValue + ex.Message.ToString());
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING   ***/
        /********************************************************************************/
        public string StringDeCrypt(string DecryptStrValue,string key)
        {
            try
            {
                string Decryptext = Decrypt(DecryptStrValue, true, key);
                return Decryptext;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING VALUE   ***/
        /********************************************************************************/
        public string StringEnCrypt(string EncryptStrValue)
        {
            try
            {
                string EncryText = Encrypt(EncryptStrValue, true,"");
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING VALUE   ***/
        /********************************************************************************/
        public string StringEnCrypt(string EncryptStrValue,string key)
        {
            try
            {
                string EncryText = Encrypt(EncryptStrValue, true, key);
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  RETRIVE ENCRYPTION MODE,KEY DETAILS  ***/
        /********************************************************************************/
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
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT WITH KEY VALUE  ***/
        /********************************************************************************/
        public static string Encrypt(string toEncrypt, bool useHashing,string EnKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = string.Empty;
            if (EnKey == "")
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file
                key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            }
            else
                key = EnKey;

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
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT WITH KEY VALUE  ***/
        /********************************************************************************/
        public static string Decrypt(string cipherString, bool useHashing,string deKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = string.Empty;
            if (deKey == "")
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            }
            else
            {
                key = deKey;
            }
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REDIRECT PAGE URL INTO SECURE LAYERS ***/
        /********************************************************************************/
        public void RedirectSSL(HttpContext context, bool redirectWww, bool redirectSsl)
        {
            if (context == null)
                return;

            var redirect = false;
            var uri = context.Request.Url;
            var scheme = uri.GetComponents(UriComponents.Scheme, UriFormat.Unescaped);
            var host = uri.GetComponents(UriComponents.Host, UriFormat.Unescaped);
            var pathAndQuery = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            ////if (HttpContext.Current.Session != null && HttpContext.Current.Session.Count > 0)
            ////{
            ////    //if (string.IsNullOrEmpty(HttpContext.Current.Session["USER_ID"].ToString()) && Convert.ToInt32(HttpContext.Current.Session["USER_ID"]) > 0)
            ////    if (HttpContext.Current.Session["USER_ID"].ToString() != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
            ////    {
            ////        scheme = "https";
            ////        redirect = true;
            ////    }
            ////    else
            ////    {
            ////        if (redirectSsl == true && scheme.Equals("https") == false)
            ////        {
            ////            scheme = "https";
            ////            redirect = true;
            ////        }
            ////        if (redirectSsl == false && scheme.Equals("https") == true)
            ////        {
            ////            scheme = "http";
            ////            redirect = true;
            ////        }
            ////    }
            ////}
            ////else
            ////{
            ////    if (redirectSsl == true && scheme.Equals("https") == false)
            ////    {
            ////        scheme = "https";
            ////        redirect = true;
            ////    }
            ////    if (redirectSsl == false && scheme.Equals("https") == true)
            ////    {
            ////        scheme = "http";
            ////        redirect = true;
            ////    }

            ////}

            if ((redirectSsl) && !(scheme.Equals("https")))
            {
                scheme = "https";
                redirect = true;
            }
            if (!(redirectSsl) && (scheme.Equals("https")))
            {
                scheme = "http";
                redirect = true;
            }

            if (redirectWww && !host.StartsWith("www", StringComparison.OrdinalIgnoreCase))
            {
                host = "www." + host;
                redirect = true;
            }

            if (redirect)
            {
                context.Response.Status = "301 Moved Permanently";
                context.Response.StatusCode = 301;
                context.Response.AddHeader("Location", scheme + "://" + host + pathAndQuery);
            }
        }

    }
    /*********************************** J TECH CODE ***********************************/
}