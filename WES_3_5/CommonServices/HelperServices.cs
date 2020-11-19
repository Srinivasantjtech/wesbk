using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class HelperServices
    {
        /*********************************** DECLARATION ***********************************/      
        ErrorHandler objErrorHandler = new ErrorHandler();
        CatalogDB.HelperDB objHelper = new CatalogDB.HelperDB();
        string _tempstring = string.Empty;
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE OPTION NAME VALUES  ***/
        /********************************************************************************/

        public string GetOptionValues_withoutsession(string oName)
        {
            try
            {
                DataSet objoptName_all;
                bool isfirst = false;
                // first Attempt
                _tempstring = "";
                objoptName_all = GetOptionValuesAll_withoutsession(false);
                if (objoptName_all != null)
                {
                    DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                    if (dr.Length > 0)
                    {
                        _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                        isfirst = true;
                    }
                }
              

                //_tempstring = (string)objHelper.GetGenericDataDB(oName, "OPTION_NAME", HelperDB.ReturnType.RTString);

                return _tempstring;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        public string GetOptionValues(string oName)
         {
            try
            {   DataSet objoptName_all;
                bool isfirst=false;
                // first Attempt
                _tempstring = "";
                objoptName_all = GetOptionValuesAll(false);   
                if (objoptName_all!=null)
                {
                    DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                    if (dr.Length > 0)
                    {
                        _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                        isfirst = true;
                    }
                }
                // second Attempt
                if (!(isfirst))
                {
                    objoptName_all = GetOptionValuesAll(true);
                    if (objoptName_all != null)
                    {
                        DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                        if (dr.Length > 0)
                        {
                            _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                        }
                    }
                }

                //_tempstring = (string)objHelper.GetGenericDataDB(oName, "OPTION_NAME", HelperDB.ReturnType.RTString);

                return _tempstring;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET THE FOLDER PATH FOR PRODUCT IMAGES  ***/
        /********************************************************************************/
        public string  SetImageFolderPath(string SourcePath,string FindString, string ConvertTo)
        {
            try
            {
                SourcePath = SourcePath.ToLower().Replace("\\","/").Replace(@"\","/");
                FindString = FindString.ToLower();
                ConvertTo = ConvertTo.ToLower();
                string returnval = string.Empty;
                string tempstr = string.Empty;
                string tempstr1 = string.Empty;
                //string strfile = HttpContext.Current.Server.MapPath("ProdImages");
                string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                if (ConvertTo != "_images")
                {
                    if ((SourcePath.Contains(FindString)))
                    {
                        // returnval = SourcePath.Replace(FindString, ConvertTo);
                        string[] temp1 = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
                        if (temp1.Length >= 2)
                        {

                            //Modified by indu on 27-June-2016 to fix _image replace problem
                            //if ((temp1[temp1.Length - 2].Contains(FindString)))
                            if ((temp1[temp1.Length - 2].EndsWith(FindString))) 
                                temp1[temp1.Length - 2] = temp1[temp1.Length - 2].Replace(FindString, ConvertTo);                            
                            else
                                temp1[temp1.Length - 2] = temp1[temp1.Length - 2].ToString() + ConvertTo;


                            for (int inx = 0; inx <= temp1.Length - 1; inx++)
                            {
                                if (temp1[inx] == "")
                                    returnval = returnval + "";
                                else
                                    returnval = returnval + "/" + temp1[inx];

                            }
                        }
                        else
                            returnval = SourcePath;

                    }
                    else
                    {
                        string[] temp = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
                        if (temp.Length >= 2)
                        {
                            tempstr = temp[temp.Length - 2] + ConvertTo;
                            //returnval = SourcePath.Replace(temp[temp.Length - 2].ToString(), temp[temp.Length - 2].ToString() + ConvertTo);
                            temp[temp.Length - 2] = temp[temp.Length - 2].ToString() + ConvertTo;
                            for (int inx = 0; inx <= temp.Length - 1; inx++)
                            {
                                if (temp[inx] == "")
                                    returnval = returnval + "";
                                else
                                    returnval = returnval + "/" + temp[inx];

                            }
                        }
                        else
                            returnval = SourcePath;
                    }
                }
                else
                {
                    
                    tempstr = strfile.Replace("\\", "/") + SourcePath.Replace(FindString,"");
                    tempstr1 = strfile.Replace("\\", "/") + SourcePath.Replace(FindString, ConvertTo);

                    if (File.Exists(tempstr))
                    {
                        returnval = SourcePath.Replace(FindString, "");
                    }
                    else if (File.Exists(tempstr1))
                    {
                        returnval = SourcePath.Replace(FindString, ConvertTo);
                    }
                    else
                        returnval = SourcePath;

                }

                return returnval;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL THE OPTION VALUES  ***/
        /********************************************************************************/

        public DataSet GetOptionValuesAll_withoutsession(bool Reset)
        {
            try
            {
                DataSet objoptName_all;
                if ((Reset))
                {
                    objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                  //  HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                }
                else
                {
                    //if (HttpContext.Current.Session["OPTION_NAME_ALL"] == null)
                    //{
                        objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                    //    HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                    //}
                    //else
                    //    objoptName_all = (DataSet)HttpContext.Current.Session["OPTION_NAME_ALL"];
                }

                return objoptName_all;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public DataSet GetOptionValuesAll(bool Reset)
        {
            try
            {
                DataSet objoptName_all;
                if ((Reset))
                {
                    objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                    HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                }
                else
                {
                    if (HttpContext.Current.Session["OPTION_NAME_ALL"] == null)
                    {
                        objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                        HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                    }
                    else
                        objoptName_all = (DataSet)HttpContext.Current.Session["OPTION_NAME_ALL"];
                }

                return objoptName_all;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public bool GetIsEcomEnabled(string userid)
        //{
        //    bool retvalue = false;
        //    DataTable objDt = new DataTable();
        //    try
        //    {

        //        //string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        if (!string.IsNullOrEmpty(userid))
        //        {
        //            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //            //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //            //objHelperService.SQLString = sSQL;
        //            //int iROLE = objHelperService.CI(objHelperService.GetValue("USER_ROLE"));
        //            int iROLE = 0;
        //            if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
        //            {
        //                objDt = (DataTable)objHelper.GetGenericDataDB(WesCatalogId, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
        //                if (objDt != null && objDt.Rows.Count > 0)
        //                {
        //                    HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
        //                    iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
        //                }
        //            }
        //            else
        //            {
        //                objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
        //                if (objDt != null && objDt.Rows.Count > 0)
        //                {
        //                    iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
        //                }
        //            }
        //            if (iROLE <= 3)
        //                retvalue = true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //        objDt.Dispose();
        //        objDt = null;

        //    }
        //    return retvalue;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE ECOM OPTION IS ENABLED OR NOT  ***/
        /********************************************************************************/
        public bool GetIsEcomEnabled(string userid)
        {
            bool retvalue = false;
            DataTable objDt = new DataTable();
            try
            {
                if (userid == "")
                    userid = "0";

                //string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid) && Convert.ToInt32(userid) != 0)
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                    //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //int iROLE = objHelperService.CI(objHelperService.GetValue("USER_ROLE"));
                    int iROLE = 0;
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelper.GetGenericDataDB(WesCatalogId, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                            iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
                        }
                    }
                    else
                    {
                        objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
                        }
                    }
                    if (iROLE <= 3)
                        retvalue = true;
                    else
                    {
                        if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                            retvalue = true;
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
            }
            finally
            {
                if (objDt != null)
                objDt.Dispose(); 
                objDt = null;

            }
            return retvalue;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH PRODUCT DETAILS  ***/
        /********************************************************************************/
        public DataSet GetPowerSearchProducts(string Searchtxt, int price_code, int user_id)
        {
            return objHelper.GetPowerSearchProducts(Searchtxt, price_code, user_id);
        }


        /*********************************** OLD CODE ***********************************/      
        //public string connection()
        //{
        //    try
        //    {
        //        ConnectionDB objConnection = new ConnectionDB();

        //        return objConnection.ConnectionString.Replace("\\", @"\");
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //        return "";
        //    }
        //}
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO TRIM  ***/
        /********************************************************************************/
        public string StringTrim(string sValue, int Trimlen, bool trimspace)
        {
            try
            {
                string sReturnValue = string.Empty;
                sValue = sValue.Replace(" ", "");

                if (sValue.Length > Trimlen)
                    sReturnValue = sValue.Substring(0, Trimlen) + "...";
                else
                    sReturnValue = sValue;

                return sReturnValue;
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
        /*** PURPOSE      : TO RETRIVE THE UPDATED VALUES  ***/
        /********************************************************************************/
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
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CONVERT THE OBJECT INTO STRING  ***/
        /********************************************************************************/
        public string CS(object obj)
        {
            try
            {
                string sRetValue = string.Empty;
                if (obj != null && obj != DBNull.Value)
                {
                    sRetValue = Convert.ToString(obj);
                    sRetValue = sRetValue.Replace("'", "''");
                }

                return sRetValue;
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
        /*** PURPOSE      : TO SET OBJECT VALUE TO STRING ***/
        /********************************************************************************/
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
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO INTEGER VALUE  ***/
        /********************************************************************************/
        public int CI(object obj)
        {
            try
            {
                CultureInfo usCulture = new CultureInfo("en-US"); 
                int retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt32(obj, usCulture);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO LONG INTEGER VALUE  ***/
        /********************************************************************************/
        public Int64 CLI(object obj)
        {
            try
            {
                Int64 retValue = 0;
                CultureInfo usCulture = new CultureInfo("en-US"); 
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt64(obj, usCulture);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO DOUBLE VALUE  ***/
        /********************************************************************************/
        public double CD(object obj)
        {
            try
            {

                CultureInfo usCulture = new CultureInfo("en-US"); 
                double retValue = 0.0;
                if (obj != null && obj != DBNull.Value)
                {
                    retValue = Convert.ToDouble(obj, usCulture);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO DECIMAL VALUE  ***/
        /********************************************************************************/
        public decimal CDEC(object obj)
        {
            try
            {
                decimal retValue = 0;
                CultureInfo usCulture = new CultureInfo("en-US");
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {

                    retValue = Convert.ToDecimal(obj, usCulture);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO BOOLEAN VALUE  ***/
        /********************************************************************************/
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
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        public  string CheckPriceValueDecimal(string cost)
        {
            string[] price = null;
            try
            {
                price = cost.ToString().Split('.');
                if (price[1].Length >= 4)
                {
                    if ((price.Length > 0 && Convert.ToInt32(price[1].Substring(2, 2)) > 1))
                    {
                        cost = cost.ToString();
                    }
                    else
                    {
                        cost = Convert.ToDouble(cost).ToString("#0.00");
                    }

                }
                else
                    cost = Convert.ToDouble(cost).ToString("#0.00");
            }
            catch (Exception ex)
            {
            }
            return cost;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK THE EMAIL ID IS VALID OR NOT  ***/
        /********************************************************************************/
        public bool ValidateEmail(string emailAddress)
        {

            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Match match = regex.Match(emailAddress);
            if (match.Success)
                return true;
            else
                return false;
        }

        public void writelog(string newurl)
        {
            try
            {
               // System.IO.StreamWriter log;


               // //log = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("logfile_2.txt"));
               //log = System.IO.File.AppendText("C:\\WES WEBSITE\\WESR\\logfile_2.txt"); 
               // log.WriteLine(newurl);
               // log.Close();'


                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "Lognew/TimeRecord" + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");
                if (!(File.Exists(Path)))
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }             
               

                StreamWriter sw;
               
                    sw = new StreamWriter(Path, true);
                    sw.WriteLine(newurl);                  
                    sw.Flush();
                    sw.Close();
             
               


            }
            catch(Exception ex)
            {
            
            }
        }

        public string viewebook(string ebookpath)
        {
            try
            {
                //Modified By:indu
                 string Ebook_pdf_FileRef=System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();
                 if (!(ebookpath.Contains(Ebook_pdf_FileRef)))
                 {
                     ebookpath = ebookpath.Replace("/media/", "/media/wes_secure_files/").Replace("\\media\\ebook\\", "\\media\\wes_secure_files\\ebook\\");  
                 }
                string ebookpath_updated = ebookpath ;

                if (ebookpath.Contains("\\attachments"))
                {
                    ebookpath_updated = ebookpath.Replace("\\attachments", "");
                }
               
                else
                {
                ebookpath = "\\attachments" + ebookpath;
                }
                if (ebookpath.Contains(".htm") || ebookpath.Contains(".html"))
                {
                    return ebookpath_updated;
                }
                else
                {

                    string folderPath = HttpContext.Current.Server.MapPath(ebookpath.Replace("mediapub","media/wes_public_files"));   // or whatever folder you want to load..



                    var htmlFiles = new DirectoryInfo(folderPath).GetFiles("*.html");
                   objErrorHandler.CreateLog("html" + folderPath);

                    if (htmlFiles.Length > 0)
                    {
                     
                        var firsthtmlFilename = htmlFiles[0].Name;
                        ebookpath_updated = ebookpath_updated.Replace("\\", "/") + "/" + firsthtmlFilename;
                        //objErrorHandler.CreateLog("htm"+ebookpath_updated);
                        return ebookpath_updated;

                    }

                    else
                    {
                       // objErrorHandler.CreateLog("/htmFiles");
                        var htmFiles = new DirectoryInfo(folderPath).GetFiles("*.htm");
                        if (htmFiles.Length > 0)
                        {
                            var firsthtmFilename = htmFiles[0].Name;
                            ebookpath_updated = ebookpath_updated.Replace("\\", "/") + "/" + firsthtmFilename;
                           objErrorHandler.CreateLog(ebookpath_updated);
                            return ebookpath_updated;
                        }
                    }

                }
              // objErrorHandler.CreateLog("ebookpath" + ebookpath);
                //Modified by Indu on 21 Nov 2018
                return ebookpath.Replace("\\attachments", ""); 
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString()); 
                return ebookpath.Replace("\\attachments", ""); 
            }
        }
        public  string StripWhitespace(string body)
        {
            string html = body;
            //string[] lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            //StringBuilder emptyLines = new StringBuilder();
            //foreach (string line in lines)
            //{
            //    string s = line.Trim();
            //    if (s.Length > 0 && !s.StartsWith("//"))
            //        emptyLines.AppendLine(s.Trim());
            //}

            //body = emptyLines.ToString();

            //// remove C styles comments
            //body = Regex.Replace(body, "/\\*.*?\\*/", String.Empty, RegexOptions.Compiled | RegexOptions.Singleline);
            ////// trim left
            //body = Regex.Replace(body, "^\\s*", String.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            ////// trim right
            //body = Regex.Replace(body, "\\s*[\\r\\n]", "\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of left curly braced
            //body = Regex.Replace(body, "\\s*{\\s*", "{", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of coma
            //body = Regex.Replace(body, "\\s*,\\s*", ",", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of semicolon
            //body = Regex.Replace(body, "\\s*;\\s*", ";", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove newline after keywords
            //body = Regex.Replace(body, "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)", " ", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //body = Regex.Replace(body, "[\\r\\n]", "", RegexOptions.Compiled | RegexOptions.ECMAScript);

            /// Solution A
            //html = Regex.Replace(html, @"\n|\t", " ");
            //html = Regex.Replace(html, @">\s+<", "><").Trim();
            //html = Regex.Replace(html, @"\s{2,}", " ");

            ///// Solution B
            //html = Regex.Replace(html, @"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}", "");
            //html = Regex.Replace(html, @"[ \f\r\t\v]?([\n\xFE\xFF/{}[\];,<>*%&|^!~?:=])[\f\r\t\v]?", "$1");
            //html = html.Replace(";\n", ";");

            ///// Solution C
            //html = Regex.Replace(html, @"[a-zA-Z]+#", "#");
            //html = Regex.Replace(html, @"[\n\r]+\s*", string.Empty);
            //html = Regex.Replace(html, @"\s+", " ");
            //html = Regex.Replace(html, @"\s?([:,;{}])\s?", "$1");
            //html = html.Replace(";}", "}");
            //html = Regex.Replace(html, @"([\s:]0)(px|pt|%|em)", "$1");

            ///// Remove comments
            //html = Regex.Replace(html, @"/\*[\d\D]*?\*/", string.Empty);
            
             // Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}|\<\!--[^\[].*?--\>", RegexOptions.Compiled);
           // html = reg.Replace(html, string.Empty);
            return html;
        }
        public bool CheckCredential()
        {
            bool returnvalue = false;
            bool validUser;
            string username;
            string password;
            string login;

            int UserID;
            TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
            UserServices objUserServices = new UserServices();
            //TradingBell.WebCat.Helpers.ErrorHandler objErrorHandler = new TradingBell.WebCat.Helpers.ErrorHandler();
            CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();

            if (HttpContext.Current.Request.Cookies["LoginInfo"] != null && HttpContext.Current.Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
            {
                try
                {
                    login = "False";
                    HttpCookie LoginInfoCookie = HttpContext.Current.Request.Cookies["LoginInfo"];
                    username = objSecurity.StringDeCrypt(LoginInfoCookie["UserName"].ToString());
                    password = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());
                    if (LoginInfoCookie["Login"] != null)
                        login = objSecurity.StringDeCrypt(LoginInfoCookie["Login"].ToString());
                    validUser = objUserServices.CheckUserName(username);
                    UserID = objUserServices.GetUserID(username);
                    if (UserID != -1 && username != string.Empty && login == "True")
                    {
                        if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                        {
                            if ((validUser))
                            {
                                bool HasAdminUser = objUserServices.HasAdmin(UserID);
                                if (objUserServices.IsUserActive(UserID.ToString()))
                                {
                                    password = objSecurity.StringEnCrypt(password);
                                    if (objUserServices.CheckUser(username, password))
                                    {
                                        string Role;
                                        Role = objUserServices.GetRole(UserID);

                                        if (Role != null)
                                        {
                                            returnvalue = true;
                                            objUserServices.OnLineFlag(true, UserID);
                                            HttpContext.Current.Session["USER_NAME"] = username;
                                            HttpContext.Current.Session["USER_ID"] = UserID;
                                            HttpContext.Current.Session["USER_ROLE"] = Role;
                                            HttpContext.Current.Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                            HttpContext.Current.Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    // objErrorHandler.ErrorMsg = ex;
                    // objErrorHandler.CreateLog();
                }

            }
            else
                returnvalue = false;



            return returnvalue;

        }

        //Added by indu

        public string MetaTagProductkeyword(DataTable dt)
        
        {
            string strkeyword = string.Empty;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (i == 0)
                {
                    strkeyword = dt.Rows[i][0].ToString();
                }
                else
                {
                    strkeyword = strkeyword + "," + dt.Rows[i]["Product Tags"].ToString();
                }
            }
            GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
            strkeyword = objgetmetadata.Replace_SpecialChar(strkeyword);
            return strkeyword;

        }


  
        public string sendmail(string strsubject,string sHTML, string emailid, string filename)
        {
            try
            {
                HelperServices objHelperServices = new HelperServices();
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());


                MessageObj.To.Add(emailid);
                MessageObj.Subject = strsubject;
                MessageObj.Body = sHTML;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(filename);
                MessageObj.Attachments.Add(attachment);

                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());

                smtpclient.Send(MessageObj);
                attachment.Dispose();
                MessageObj.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
            return "mail Send";
            //MessageBox.Show("mail Send");
        }

        public int Mail_Error_Log(string mail_type, int order_id, string mailto, string mail_error, int resend, int user_id, int user_role, int is_site)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];
                string sSQL;
                sSQL = "exec STP_TBWC_POP_MAIL_ERROR_LOG ";
                sSQL = sSQL + websiteid + ",'" + mail_type + "'," + order_id + ",'" + mailto + "','" + mail_error.Replace("'", "") + "'," + resend + "," + user_id + "," + user_role + "," + is_site + "";
                return objHelper.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }


        public int Mail_Log(string mail_type, int order_id, string mailto, string mail_error)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_ORDER(USER_ID,ORDER_STATUS,IS_SHIPPED,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT,CREATED_USER,MODIFIED_USER)";
                //sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1,0,0,0," + oInfo.UserID + "," + oInfo.UserID + " )";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];

                string sSQL;
                sSQL = "exec STP_TBWC_POP_MAIL_LOG ";
                sSQL = sSQL + websiteid + ",'" + mail_type + "'," + order_id + ",'" + mailto + "','" + mail_error + "'";
                return objHelper.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }


    }
    /*********************************** J TECH CODE ***********************************/
}