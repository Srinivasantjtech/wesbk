﻿using System;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data;
using System.Text;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using System.Globalization;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Net;
using System.Web;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CommonServices
{
    public class PayPalService
    {
        CatalogDB.HelperDB objHelper = new CatalogDB.HelperDB();
        HelperServices objHelperService = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        UserServices objUserServices = new UserServices();
        CountryServices objCountryServices = new CountryServices();
        Security objSecurity = new Security();
        CountryDB objCountryDB = new CountryDB();
        HelperDB objHelperDB = new HelperDB();
        public const string API_VERSION = "xml-4.2";
        public const string TIME_OUT = "60";
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        int websiteid = Convert.ToInt32(ConfigurationManager.AppSettings["WEBSITEID"].ToString());
        public string Pay_Url  = System.Configuration.ConfigurationManager.AppSettings["P_ID1"].ToString();
        public string business_Mail_id = System.Configuration.ConfigurationManager.AppSettings["P_ID2"].ToString();
        public string PDTToken = System.Configuration.ConfigurationManager.AppSettings["P_ID3"].ToString();
        public string environment = System.Configuration.ConfigurationManager.AppSettings["P_ID4"].ToString();

        public string file_name = "";
       

      
        public struct PaymentRequestInfo
        {
            public string Request_id;
            public string request_type;
            public string request_Method;
            public string request_Timestamp;
            public int Payment_id;
            public int Order_id;
            public decimal Amount;
            //public int Card_Type_id;
            //public string Card_No;
            //public string Card_Name;
            //public string Card_CVV;
            //public string Card_ExpiryDate;

            public string Request_info;
            public string Response_info;
            public string Response_Approved;
            public string Response_Status_Code;
            public string Response_Status_desc;
            public string Response_Receipt_ID;
            public string Response_Txn_ID;
            public string Response_Code;
            public string Response_Text;
            public string Error_Text;            
            public int Website_Id;
            public string Payment_Request_id;


        }
        public struct PaymentIPNInfo
        {
            public string Request_id;
            public string Response_Txn_ID;
            public int Website_Id;
            public int Payment_id;
            public string Response_Approved;
            public int Order_id;
            public decimal Amount;
            public string Response_info;
        }
        public enum TransactionSources
        {
            XML = 23
        }
        PaymentRequestInfo objPRInfo = new PaymentRequestInfo();
        

        OrderServices.OrderInfo objOrderInfo = new OrderServices.OrderInfo();

        //public NameValueCollection PayPalInitRequestPro(int order_id,int Payment_id, OrderServices.OrderInfo oOrderInfo, string rtnUrl)
        //{
        //    string req_id=GetNewId(order_id.ToString());
        //    NameValueCollection rtn = new NameValueCollection();
        //    NameValueCollection requestArray = new NameValueCollection()
        //    {
        //        {"PARTNER", "PayPal"},                         // You'll want to change these 4
        //        {"VENDOR", "palexanderpayflowtest"},           // To use your own credentials
        //        {"USER", "palexanderpayflowtestapionly"},
        //        {"PWD", "demopass123"},
        //        {"TRXTYPE", "A"},
        //        {"AMT",oOrderInfo.TotalAmount.ToString()},
        //        {"CURRENCY", "USD"},
        //        {"CREATESECURETOKEN", "Y"},
        //        {"SECURETOKENID", req_id},  //This should be generated and unique, never used before
        //        {"RETURNURL", rtnUrl},  //Note how this simple example merely returns back to itself, rather than having a seperate Return.aspx
        //        {"CANCELURL", rtnUrl},
        //        {"ERRORURL", rtnUrl},

        //       // {"ORDERID", order_id.ToString()},
        //        {"COMMENT1", order_id.ToString()  },
        //         {"COMMENT2", Payment_id.ToString()  },
        //        {"BILLTOFIRSTNAME", oOrderInfo.BillFName  },
        //        {"BILLTOLASTNAME", oOrderInfo.BillLName},
        //        {"BILLTOSTREET", oOrderInfo.BillAdd1 +oOrderInfo.BillAdd1+ oOrderInfo.BillAdd1 },
        //        {"BILLTOCITY", oOrderInfo.BillCity},
        //        {"BILLTOSTATE", oOrderInfo.BillState},
        //        {"BILLTOZIP", oOrderInfo.BillZip },
        //        {"BILLTOCOUNTRY", oOrderInfo.BillCountry},
        //        {"SHIPTOFIRSTNAME", oOrderInfo.ShipFName },
        //        {"SHIPTOLASTNAME", oOrderInfo.ShipLName},
        //        {"SHIPTOSTREET", oOrderInfo.ShipAdd1+oOrderInfo.ShipAdd2+oOrderInfo.ShipAdd3  },
        //        {"SHIPTOCITY", oOrderInfo.ShipCity },
        //        {"SHIPTOSTATE", oOrderInfo.ShipState },
        //        {"SHIPTOZIP", oOrderInfo.ShipZip },
        //        {"SHIPTOCOUNTRY", oOrderInfo.ShipCountry},
        //    };

        //    objPRInfo.Request_id = req_id;
        //    objPRInfo.request_Timestamp = getTimeStamp();
        //    objPRInfo.Order_id = order_id;
        //    objPRInfo.Payment_id  = Payment_id;
        //    objPRInfo.Amount = oOrderInfo.TotalAmount;
        //    objPRInfo.Website_Id = websiteid;
        //    objPRInfo.request_type = "A";
        //    objPRInfo.Request_info = requestArray.ToString();


        //    rtn=  payflowInitRequest_call(requestArray);

        //    try
        //    {
        //        if (rtn["RESULT"] == "0")
        //        {
        //            InsertRequest();
        //        }
               

        //    }
        //    catch
        //    {
        //        rtn["RESULT"] = "-1";
        //    }
        //    return  rtn;

        //}
        public string PayPalInitRequest(int order_id, int Payment_id, OrderServices.OrderInfo oOrderInfo, string rtnUrl)
        {
            string rtn = "";
            string insertstr = "";
            string strState = "";
            DataSet  ds = null;
            try
            {
                UserServices.UserInfo ouserinfo =new UserServices.UserInfo();
  
                ouserinfo=objUserServices.GetUserInfo(oOrderInfo.UserID);   

                string strPay_Url = DecryptSP(Pay_Url);
                if (strPay_Url == null)
                    return "Request Ulr Error";
                string strbusiness_Mail_id = DecryptSP(business_Mail_id);
                if (strbusiness_Mail_id == null)
                    return "Request mail id Error";

                string strPDTToken = DecryptSP(PDTToken);
                if (strPDTToken == null)
                    return "Request mail id Error";

                string req_id = GetNewId(order_id.ToString(), Payment_id.ToString());
                file_name = req_id;
                NameValueCollection requestArray=null;
                if (objOrderServices.IsNativeCountry(order_id) == 1)
                {
                     //
                    ds = objCountryServices.GetStateName(oOrderInfo.ShipState);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strState = ds.Tables[0].Rows[0]["STATE_NAME"].ToString();
                    }
                    if (strState == "")
                        strState = oOrderInfo.ShipState;

                    requestArray = new NameValueCollection()
                    {
                        {"cmd", "_xclick"},                         // You'll want to change these 4
                        {"business", strbusiness_Mail_id },           // To use your own credentials
                        {"item_name", "Total Amount"},
                        {"amount", oOrderInfo.TotalAmount.ToString()},
                        {"return", rtnUrl},
                        {"invoice",order_id.ToString()},
                        {"address_override", "1"},    
                        {"custom", req_id},
                        {"rm", "2"},
                        {"currency_code", "AUD"}, 

                        {"first_name", ouserinfo.FirstName },
                        {"last_name", ouserinfo.LastName},
                        {"address1", oOrderInfo.ShipAdd1 },
                        {"address2", oOrderInfo.ShipAdd2+oOrderInfo.ShipAdd3},
                        {"city", oOrderInfo.ShipCity },
                        {"state", strState },
                        {"zip", oOrderInfo.ShipZip },
                        {"country", objUserServices.GetUserCountryCode( oOrderInfo.ShipCountry) },
                        {"email", ouserinfo.AlternateEmail },
                        {"night_phone_b", oOrderInfo.ShipPhone }
                    };

                }
                else
                {
                    requestArray = new NameValueCollection()
                    {
                        {"cmd", "_xclick"},                         // You'll want to change these 4
                        {"business", strbusiness_Mail_id },           // To use your own credentials
                        {"item_name", "Total Amount"},
                        {"amount", oOrderInfo.TotalAmount.ToString()},
                        {"return", rtnUrl},
                        {"invoice",order_id.ToString()},
                        {"address_override", "1"},    
                        {"custom", req_id},
                        {"rm", "2"},
                         {"currency_code", "AUD"}, 

                        {"no_shipping", "1" }
                        //{"last_name", oOrderInfo.ShipLName},
                        //{"address1", oOrderInfo.ShipAdd1 },
                        //{"address2", oOrderInfo.ShipAdd2+oOrderInfo.ShipAdd3  },
                        //{"city", oOrderInfo.ShipCity },
                        //{"state", oOrderInfo.ShipState },
                        //{"zip", oOrderInfo.ShipZip },
                        //{"country", oOrderInfo.ShipCountry}
                    };
                }
                objPRInfo.Request_id = req_id;
                objPRInfo.request_Timestamp = getTimeStamp();
                objPRInfo.Order_id = order_id;
                objPRInfo.Payment_id = Payment_id;
                objPRInfo.Amount = oOrderInfo.TotalAmount;
                objPRInfo.Website_Id = websiteid;
                objPRInfo.Request_info = requestArray.ToString();




                HttpContext.Current.Session["X_P"] = "";
                rtn = PypalPOSTForm(strPay_Url, requestArray);

                insertstr=InsertRequest();
                if (insertstr!="")
                    return "Data insert Error";

                HttpContext.Current.Session["X_P"] = req_id;

            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["X_P"] = "";
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
           
            return rtn;

        }

        public String GetPymentStatus(string tx_id)
        {
            string strResponse = "";
            try
            {

             
                string strPay_Url = DecryptSP(Pay_Url);
                if (strPay_Url == null)
                    return "Request Error";

                string strPDTToken = DecryptSP(PDTToken);
                if (strPDTToken == null)
                    return "Request Error";



                string query = string.Format("cmd=_notify-synch&tx={0}&at={1}", tx_id, strPDTToken);

                // Create the request back

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strPay_Url);

                // Set values for the request back
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;

                // Write the request back IPN strings
                StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(query);
                stOut.Close();

                // Do the request to PayPal and get the response
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();
                stIn.Close();
            }
            catch (Exception ex)
            {                
                return "Request Payment_Status Error";
            }

            return strResponse;
        }
        public string SetPayPalStatus(NameValueCollection response,string Request_id )
        {
            string rtnstring = "";
            NameValueCollection res=new NameValueCollection();
            string rtnstring1="";
            DataTable dt = null;



            String[] StringArray = Request_id.ToString().Split('-');  //HttpContext.Current.Session["X_P"].ToString().Split('-');

            if (StringArray != null && StringArray.Length > 3)
            {
                HttpContext.Current.Session["P_Oid"] = StringArray[1].ToString();
                HttpContext.Current.Session["P_pid"] = StringArray[2].ToString();

            }
            else
            {
                HttpContext.Current.Session["P_Oid"] ="";
                HttpContext.Current.Session["P_pid"] = "";

            }
            try
            {
                if (response["tx"] != null)
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        dt = GETIPN(response["tx"].ToString(), Request_id);
                       if (dt != null)
                           break;
                        
                    }
                    if (dt != null &&  dt.Rows.Count>0 )
                    {
                        rtnstring1 = dt.Rows[0]["IPNDATA"].ToString();
                        if(dt.Rows[0]["APPROVED"].ToString().ToUpper()=="YES")                        
                          rtnstring1="SUCCESS&"+rtnstring1;
                          else
                          rtnstring1="FAIL&"+rtnstring1;

                        res = GetResponseStrToNameValue(rtnstring1, false);
                        HttpContext.Current.Session["IPN"] = Request_id + "#" + response["tx"].ToString();
                    }
                    else
                    {
                        rtnstring1 = GetPymentStatus(response["tx"].ToString());
                        res = GetResponseStrToNameValue(rtnstring1, true);
                        HttpContext.Current.Session["IPN"] = "";
                    }

                  
                    if (res["Status"] =="SUCCESS")
                    {
                        objPRInfo.Response_Approved = "YES";
                        objPRInfo.Response_Code = "0";
                        objPRInfo.Response_Text = "SUCCESS";
                        objPRInfo.request_Method = res["payment_type"].ToString();

                        if (res["receipt_id"] != null)
                        {
                            objPRInfo.Response_Receipt_ID = res["receipt_id"].ToString();
                            objErrorHandler.CreatePayLog("receipt id in paypal "+ res["receipt_id"].ToString());
                        }
                        else
                        {
                            objErrorHandler.CreatePayLog("receipt id in paypal is null");
                        
                        }

                        objPRInfo.Request_info = rtnstring1;
                        objPRInfo.Response_Txn_ID = response["tx"].ToString();
                        objPRInfo.Request_id = Request_id;//HttpContext.Current.Session["X_P"].ToString();
                        objPRInfo.request_type = res["payment_type"].ToString();
                        HttpContext.Current.Session["payflowresponse"] = "SUCCESS";

                      
                    }
                    else
                    {                  
                        objPRInfo.Response_Approved = "NO";
                        objPRInfo.Response_Code = "-1";
                        objPRInfo.Response_Text = "FAIL";

                        objPRInfo.request_Method = "";
                        objPRInfo.Request_info = getArraytToString(response)  ;
                        objPRInfo.Response_Txn_ID = response["tx"].ToString();
                        objPRInfo.Request_id = Request_id;// HttpContext.Current.Session["X_P"].ToString();
                        objPRInfo.request_type ="";
                        HttpContext.Current.Session["payflowresponse"] = "FAIL";

                    }

              
                    

                    rtnstring = UpdateRequest(objPRInfo);
                    if (rtnstring != "")
                    {
                        HttpContext.Current.Session["X_P"] = "";
                        HttpContext.Current.Session["payflowresponse"] = "FAIL";
                        return rtnstring;
                    }

                }
                else
                {
                    HttpContext.Current.Session["X_P"] = "";
                    HttpContext.Current.Session["payflowresponse"] = "FAIL";
                    objErrorHandler.CreateLog(file_name, getArraytToString(response) + "#####" + HttpContext.Current.Session["X_P"].ToString());
                    return "Invalid Response";
                }


            }
            catch (Exception e)
            {
                HttpContext.Current.Session["X_P"] = "";
                HttpContext.Current.Session["payflowresponse"] = "FAIL";
                objErrorHandler.CreateLog(file_name, objPRInfo.Response_info);
                return "Unable to Update Response Data";
            }
            HttpContext.Current.Session["X_P"] = "";
            return rtnstring;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : SIGNAL NOTIFICATION  ***/
        /********************************************************************************/
        public int SentSignalPayment(string payment_id,string order_id )
        {
            int retVal = 0;
            DataSet dsOD = new DataSet();
            try
            {

                string sSQL = "Exec STP_TBWC_SEND_SIGNAl_PAYMENT '" + payment_id + "','" + order_id + "'";
                dsOD = objHelperDB.GetDataSetDB(sSQL);
                if (dsOD != null && dsOD.Tables[0].Rows.Count >0)
                {
                    foreach (DataRow drOD in dsOD.Tables[0].Rows)
                    {
                        retVal = objHelperService.CI(drOD[0]);
                    }
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        public int VerifyResponse(NameValueCollection Res )
        {
            int rtn = 0;
            bool isVerified = false;
            int Order_id = 0;
            int Payment_id = 0;
            string strbusiness_Mail_id = DecryptSP(business_Mail_id);
            if (strbusiness_Mail_id == null)
                strbusiness_Mail_id = "";

            if (Res.Keys.Count > 0 && Res["business"] != null && Res["business"].ToString().ToLower() == strbusiness_Mail_id.ToLower())
            {
                rtn = 1;
            }
            else
            {
                rtn= 0;
            }
            
            if (rtn == 0)
                return rtn;


            if (Res.Keys.Count > 0 && Res["payment_status"] != null && Res["payment_status"].ToString().ToLower() == "completed")
            {
                rtn = 1;
            }
            else
            {
                rtn = 0;
            }

            if (rtn == 0)
                return rtn;

            if (Res.Keys.Count > 0 && Res["custom"] != null && Res["mc_gross"] != null )
            {
                string req_id = Res["custom"].ToString();



                String[] StringArray = req_id.Split('-');

                if (StringArray[1] != null)
                    Order_id = Convert.ToInt32(StringArray[1].ToString());
                if (StringArray[2] != null)
                    Payment_id = Convert.ToInt32(StringArray[2].ToString());

                oPayInfo = objPaymentServices.GetPayment(Order_id);
                if( Convert.ToDecimal(Res["mc_gross"].ToString())==Convert.ToDecimal(oPayInfo.Amount))
                    rtn = 1;
                else
                    rtn = 0;


            }

            return rtn;
          
           

        }
        protected String getArraytToString(NameValueCollection requestArray)
        {
            String nvpstring = "";
            foreach (string key in requestArray)
            {            
                var val = requestArray[key];
                nvpstring += key + "[ " + val.Length + "]=" + val + "&";
            }
            return nvpstring;
        }

        public NameValueCollection GetResponseStrToNameValue(string postData ,Boolean isnewline )
        {

            NameValueCollection rtn = new NameValueCollection(); 
            String[] StringArray=null;
            if (isnewline == true)
            {
                StringArray = postData.Split('\n');
            }
            else
                StringArray = postData.Split('&');

            if (StringArray.Length>=1)
            {
                if (isnewline == true)
                    rtn.Add("Status", StringArray[0]); 
                else
                    rtn.Add("Status", StringArray[0]); 

                // use split to split array we already have using "=" as delimiter
                int i;
                for (i = 1; i < StringArray.Length - 1; i++)
                {
                    String[] StringArray1 = StringArray[i].Split('=');              
                    rtn.Add(StringArray1[0], HttpUtility.UrlDecode(StringArray1[1]));

                }
            }
            else
                rtn.Add("Status","FAIL"); 
                return rtn;
        }
       

        //protected NameValueCollection payflowInitRequest_call(NameValueCollection requestArray)
        //{
        //    NameValueCollection dict = new NameValueCollection();
        //    try
        //    {
        //        String nvpstring = "";
        //        foreach (string key in requestArray)
        //        {
        //            //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
        //            var val = requestArray[key];
        //            nvpstring += key + "[ " + val.Length + "]=" + val + "&";
        //        }

        //        string urlEndpoint;
        //        if (environment == "pilot" || environment == "test" || environment == "sandbox")
        //        {
        //            urlEndpoint = "https://pilot-payflowpro.paypal.com/";
        //        }
        //        else
        //        {
        //            urlEndpoint = "https://payflowpro.paypal.com";
        //        }

        //        //send request to Payflow
        //        HttpWebRequest payReq = (HttpWebRequest)WebRequest.Create(urlEndpoint);
        //        payReq.Method = "POST";
        //        payReq.ContentLength = nvpstring.Length;
        //        payReq.ContentType = "application/x-www-form-urlencoded";

        //        StreamWriter sw = new StreamWriter(payReq.GetRequestStream());
        //        sw.Write(nvpstring);
        //        sw.Close();

        //        //get Payflow response
        //        HttpWebResponse payResp = (HttpWebResponse)payReq.GetResponse();
        //        StreamReader sr = new StreamReader(payResp.GetResponseStream());
        //        string response = sr.ReadToEnd();
        //        sr.Close();

        //        //parse string into array and return
               
        //        foreach (string nvp in response.Split('&'))
        //        {
        //            string[] keys = nvp.Split('=');
        //            dict.Add(keys[0], keys[1]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.Message);                
        //    }
        //    return dict;
        //}
        protected string GetNewId(string Order_id, string Payment_id)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 16)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return "Wes-" + Order_id + "-" + Payment_id + '-' + websiteid.ToString()+'-' + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        }

        private static String PypalPOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";

            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }
            strForm.Append("</form>");

            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");

            //Return the form and the script concatenated. (The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : INSERT REQUEST ***/
        /********************************************************************************/
        public string InsertRequest()
        {
            string rtnstring = "";
            try
            {
                int rtnvalue = 0;

                string sSQL = "Exec STP_TBWC_POP_PAYMENT_REQUEST_RESPONSE_PAYPAL '" + objPRInfo.Request_id + "'";
                sSQL = sSQL + ",'" + objPRInfo.request_Timestamp + "'," + objPRInfo.Payment_id + "," + objPRInfo.Order_id + ",";
                sSQL = sSQL + "" + objPRInfo.Website_Id ;
                rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                if (rtnvalue <= 0)
                {
                    rtnstring = "Unable to Create Request Data";
                }

                return rtnstring;
            }
            catch (Exception e)
            {
                return "Unable to Create Request Data";
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : UPDATE REQUEST ***/
        /********************************************************************************/
        public string UpdateRequest(PaymentRequestInfo objPRInfo)
        {
            string rtnstring = "";
            try
            {                
                int rtnvalue = 0;
                string sSQL = "Exec STP_TBWC_RENEW_PAYMENT_REQUEST_RESPONSE_PAYPAL '" + objPRInfo.Request_id + "'";
                sSQL = sSQL + ",'" + objPRInfo.request_type + "','" + objPRInfo.request_Method + "','" + objPRInfo.Request_info + "','" + objPRInfo.Response_Approved + "','" + objPRInfo.Response_Txn_ID + "','" + objPRInfo.Response_Code + "','" + objPRInfo.Response_Text + "','" + objPRInfo.Response_Receipt_ID + "'" ;
                rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                if (rtnvalue <= 0)
                {
                    objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_info );
                    rtnstring = "Unable to Update Response Data";
                }
                return rtnstring;

            }
            catch (Exception e)
            {
                objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_info);
                return "Unable to Update Response Data";
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : UPDATE IPN ***/
        /********************************************************************************/
        public string UpdateIPN(PaymentIPNInfo objIPNInfo)
        {
            string rtnstring = "";
            try
            {
                int rtnvalue = 0;
                string sSQL = "Exec STP_TBWC_POP_PAYMENT_PAYPAL_IPN '" + objIPNInfo.Request_id + "'";
                sSQL = sSQL + ",'" + objIPNInfo.Response_Txn_ID + "'," + objIPNInfo.Payment_id + "," + objIPNInfo.Order_id + "," + objIPNInfo.Amount + ",'" + objIPNInfo .Response_Approved +"','"+ objIPNInfo.Response_info + "'," + objIPNInfo.Website_Id;
                rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                if (rtnvalue <= 0)
                {
                
                    objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_info);
                    rtnstring = "Unable to Update Response Data";
                }
   
                return rtnstring;
                

            }
            catch (Exception e)
            {
             
                objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_info);
                return "Unable to Update Response Data";
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : UPDATE IPN ***/
        /********************************************************************************/
        public DataTable  GETIPN(string txn_id, string Req_id)
        {
            return objHelper.GetDataTableDB("exec STP_TBWC_PICK_PAYMENT_PAYPAL_IPN '" + txn_id + "','" + Req_id +"'");
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : GET TIME STAMP  SECURY PAY FORMAT ***/
        /********************************************************************************/
        private string getTimeStamp()
        {
            DateTime now = DateTime.Now;
            string timestamp, tz_string;
            int tz_minutes;

            tz_minutes = Int32.Parse(now.ToString("zz", DateTimeFormatInfo.InvariantInfo)) * 60;

            if (tz_minutes >= 0)
            {
                tz_string = "+" + tz_minutes.ToString();
            }
            else
            {
                tz_string = tz_minutes.ToString();
            }

            /**
              Format: YYYYDDMMHHNNSSKKK000sOOO
              YYYY is a 4-digit year
              DD is a 2-digit zero-padded day of month
              MM is a 2-digit zero-padded month of year (January = 01)
              HH is a 2-digit zero-padded hour of day in 24-hour clock format (midnight =0)
              NN is a 2-digit zero-padded minute of hour
              SS is a 2-digit zero-padded second of minute
              KKK is a 3-digit zero-padded millisecond of second
              000 is a Static 0 characters, as SecurePay does not store nanoseconds
              sOOO is a Time zone offset, where s is + or -, and OOO = minutes, from GMT.
 
             
              */
            timestamp = now.ToString("yyyyddMMHHmmss000000", DateTimeFormatInfo.InvariantInfo) + tz_string.ToString();

            return timestamp;
        }

        public string Encrypt(string vlue)
        {
            return objSecurity.StringEnCrypt(vlue, EnDekey);
        }
        public string Decrypt(string vlue)
        {
            return objSecurity.StringDeCrypt(vlue, EnDekey);
        }
        protected string DecryptSP(string strvalue)
        {
            string enc = strvalue;            
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            enc = objSecurity.StringDeCrypt(enc, EnDekey);
            return enc;
        }
        public DataSet GetCardList()
        {
            DataSet dsOD = new DataSet();
            try
            {
                dsOD = (DataSet)objCountryDB.GetGenericDataDB("", "GET_CARD_LIST", CountryDB.ReturnType.RTDataSet);


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsOD;
        }
    }
}



