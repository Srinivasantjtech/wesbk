using System;
using System.Collections.Generic;
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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CommonServices
{
  public  class SecurePayService
    {
        CatalogDB.HelperDB objHelper = new CatalogDB.HelperDB();
        HelperServices objHelperService = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        Security objSecurity = new Security();
        CountryDB objCountryDB = new CountryDB();
        public const string API_VERSION = "xml-4.2";
        public const string TIME_OUT = "60";
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        int websiteid =Convert.ToInt32 (  ConfigurationManager.AppSettings["WEBSITEID"].ToString());
        public string SP_id = System.Configuration.ConfigurationManager.AppSettings["SP_ID1"].ToString();
        public string SP_p = System.Configuration.ConfigurationManager.AppSettings["SP_ID2"].ToString();
        public string SP_URL = System.Configuration.ConfigurationManager.AppSettings["SP_ID3"].ToString();
        //Testing

         //string strURL = "http://test.securepay.com.au/xmlapi/payment";        
        string strURL = "https://www.securepay.com.au/test/payment";

        //string strURL = "http://test.securepay.com.au/xmlapi/payment";        
         //string strURL = "https://www.securepay.com.au/test/payment";


        //live
        //string strURLlive = "https://www.securepay.com.au/xmlapi/payment";

        

        public enum TransactionTypes
        {
            Standard_Payment = 0,
            Mobile_Payment = 1,
            Batch_Payment = 2,
            Periodic_Payment = 3,
            Refund = 4,
            Error_Reversal = 5,
            Client_Reversal = 6,
            Preauthorise = 10,
            Preauth_Complete_Advice = 11,
            Recurring_Payment = 14,
            Direct_Entry_Debit = 15,
            Direct_Entry_Credit = 17,
            Card_Present_Payment = 19,
            IVR_Payment = 20
        }
         public struct PaymentRequestInfo
        {
             public string  Request_id;
             public int request_type_id;
             public string  request_Timestamp;
             public int Payment_id;
             public int Order_id;             
             public decimal  Amount;
             public int Card_Type_id;
             public string Card_No;
             public string Card_Name;
             public string Card_CVV;
             public string Card_ExpiryDate;
             public string Request_xml;
             public string Request_xml_db;
             public string Response_xml;
             public string Response_xml_db; 
             public string Response_Approved;
             public string Response_Status_Code;
             public string Response_Status_desc;
             public string Response_Txn_ID;
             public string Response_Code;
             public string Response_Text;
             public string Error_Text;             
             public int Website_Id;             
             public string Payment_Request_id;


         }
        
        public enum TransactionSources
        {
            XML = 23
        }
        PaymentRequestInfo objPRInfo=new PaymentRequestInfo();
        OrderServices.OrderInfo objOrderInfo= new OrderServices.OrderInfo();
        public string InsertRequest_BT()
        {
            string rtnstring = "";
            string sSQL = "";
            try
            {
                int rtnvalue = 0;

                string cardno = "";
                //if (objPRInfo.Card_No.Length > 10)
                //    cardno = objPRInfo.Card_No.Substring(6, objPRInfo.Card_No.Length - 9);
                //else
                //    cardno = objPRInfo.Card_No;
                //if (cardno != "")
                //{
                //    cardno = objPRInfo.Card_No.Replace(cardno, string.Concat(Enumerable.Repeat("X", cardno.Length)));
                //}
                sSQL = "Exec STP_TBWC_POP_PAYMENT_REQUEST_RESPONSE_BR '" + objPRInfo.Request_id + "'";
                sSQL = sSQL + ",'" + objPRInfo.request_type_id + "','" + objPRInfo.request_Timestamp + "'," + objPRInfo.Payment_id + "," + objPRInfo.Order_id + "," + objPRInfo.Card_Type_id + ",'" + objPRInfo.Card_Name + "',";
                sSQL = sSQL + "'" + objPRInfo.Card_No + "','',";
                sSQL = sSQL + "''," + objPRInfo.Website_Id;
                
                rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                if (rtnvalue <= 0)
                {
                    rtnstring = "Unable to Create Request Data";
                }

                return rtnstring;
            }
            catch (Exception e)
            {
                objErrorHandler.CreateLog(sSQL);
                HelperServices objHelperServices = new HelperServices();
                objHelperServices.Mail_Error_Log("", objPRInfo.Order_id, "", e.ToString(), 0, 0, 0, 1);
                return "Unable to Create Request Data";
            }
        }
        public string GetUpdateResponse_braintree(int paiduserid)
        {


            //string ElementName = "";
            try
            {
                //    //XmlReaderSettings settings = new XmlReaderSettings { Encoding = Encoding.UTF8};

                //    XmlReader reader = XmlReader.Create(new StringReader(objPRInfo.Response_xml));

                //    while (reader.Read())
                //    {
                //        if (reader.MoveToContent() == XmlNodeType.Element)
                //        {
                //            ElementName = reader.Name;
                //            switch (ElementName)
                //            {
                //                case "responseCode":
                //                    objPRInfo.Response_Code = reader.ReadString();
                //                    break;
                //                case "responseText":
                //                    objPRInfo.Response_Text = reader.ReadString();
                //                    break;
                //                case "approved":
                //                    objPRInfo.Response_Approved = reader.ReadString();
                //                    break;
                //                case "statusCode":
                //                    objPRInfo.Response_Status_Code = reader.ReadString();
                //                    break;
                //                case "statusDescription":
                //                    objPRInfo.Response_Status_desc = reader.ReadString();
                //                    break;
                //                case "txnID":
                //                    objPRInfo.Response_Txn_ID = reader.ReadString();
                //                    break;
                //            }
                //        }

                //    }
                //    reader.Close();

                UpdateRequest(paiduserid);
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_xml);
               
                return "Update Error";
            }
            return "";


        }
        public int GetCardTypeID(string Card_type_id)
        {

            switch (Card_type_id)
            {
                case "American Express":
                    return 0;

                case "Visa":
                    return 13;
                case "MasterCard":
                    return 10;


                default:
                    return -1;

            }


        }

        public delegate PaymentRequestInfo SyncDelegate(int order_id, int payment_id, string Card_type_id, string Card_Name, string Card_No, string Card_Cvv, string Card_Expiry_Date, string ResponseApproved, string Response_Status_Code, string ProcessorResponseText, string Id, string NetworkResponseCode, string NetworkResponseText, string Amount, string paiduserid);
        public void call_GetPaymentRequest_braintree(int order_id, int payment_id, string Card_type_id, string Card_Name, string Card_No, string Card_Cvv, string Card_Expiry_Date, string ResponseApproved, string Response_Status_Code, string ProcessorResponseText, string Id, string NetworkResponseCode, string NetworkResponseText, string Amount, string paiduserid)
        {
           
            
            SyncDelegate syncDelegate = new SyncDelegate(GetPaymentRequest_braintree);
            IAsyncResult asyncResult = syncDelegate.BeginInvoke(order_id, payment_id, Card_type_id, Card_Name, Card_No, Card_Cvv, Card_Expiry_Date, ResponseApproved, Response_Status_Code, Id, ProcessorResponseText, NetworkResponseCode, NetworkResponseText, Amount, paiduserid,  null,null);
            syncDelegate.EndInvoke(asyncResult);
        }

            public PaymentRequestInfo GetPaymentRequest_braintree(int order_id, int payment_id, string Card_type_id, string Card_Name, string Card_No, string Card_Cvv, string Card_Expiry_Date, string ResponseApproved, string Response_Status_Code, string ProcessorResponseText, string Id, string NetworkResponseCode, string NetworkResponseText, string Amount, string paiduserid)
        {
            try

            {
                string approved = "NO";
                string returnstr = "";
                //  objOrderInfo = objOrderServices.GetOrder(order_id);
                objPRInfo.Order_id = order_id;
                objPRInfo.Payment_id = payment_id;
                objPRInfo.request_type_id = 0;
                objPRInfo.Card_Type_id = GetCardTypeID(Card_type_id);
                objPRInfo.Card_Name = Card_Name;
                objPRInfo.Card_No = Card_No;
                objPRInfo.Card_CVV = Card_Cvv;
                objPRInfo.Card_ExpiryDate = Card_Expiry_Date;
                decimal amt = Convert.ToDecimal(Amount);
                objPRInfo.Amount = amt;
                objPRInfo.Website_Id = websiteid;
                objPRInfo.Payment_Request_id = "";


                returnstr = GetCreateRequest_BR(TransactionTypes.Standard_Payment);
                if (returnstr != "")
                {
                    objPRInfo.Error_Text = returnstr;
                    return objPRInfo;
                }

                returnstr = InsertRequest_BT();
                if (returnstr != "")
                {
                    objPRInfo.Error_Text = returnstr;
                    return objPRInfo;
                }
                // returnstr = GetSendRequest();
                objPRInfo.Response_Approved = ResponseApproved;
                objPRInfo.Response_Status_Code = Response_Status_Code;
                objPRInfo.Response_Txn_ID = Id;
                objPRInfo.Response_Status_desc = ProcessorResponseText;
                objPRInfo.Response_Text = NetworkResponseText;
                objPRInfo.Response_Code = NetworkResponseCode;
                objPRInfo.Error_Text = NetworkResponseText;
                if (returnstr != "")
                {
                    objPRInfo.Error_Text = returnstr;
                    return objPRInfo;
                }
                returnstr = GetUpdateResponse_braintree(Convert.ToInt32(paiduserid));
                if (returnstr != "")
                {
                    objPRInfo.Error_Text = ProcessorResponseText;
                    return objPRInfo;
                }
                if (objPRInfo.Response_Approved != null)
                    approved = objPRInfo.Response_Approved.ToUpper();

                if (objPRInfo.Response_Code == null)
                    objPRInfo.Response_Code = objPRInfo.Response_Status_Code;
                if (objPRInfo.Response_Text == null)
                    objPRInfo.Response_Text = objPRInfo.Response_Status_desc;

                if (approved == "NO")
                {
                    objPRInfo.Error_Text = objPRInfo.Response_Text;
                    return objPRInfo;
                }

            }
            catch (Exception ex)
            { 
                HelperServices objHelperServices = new HelperServices();
                objHelperServices.Mail_Error_Log("", order_id, "", ex.ToString(), 0, 0, 0, 1);
            }
            return objPRInfo;
        }

        public PaymentRequestInfo GetPaymentRequest(int order_id, int payment_id, string Card_type_id, string Card_Name, string Card_No, string Card_Cvv, string Card_Expiry_Date,string paiduser)
        {
            objErrorHandler.CreateLog(order_id + Card_Expiry_Date);
            string approved = "NO";
            string returnstr = "";
            objOrderInfo = objOrderServices.GetOrder(order_id);
            objPRInfo.Order_id = order_id;
            objPRInfo.Payment_id = payment_id;
            objPRInfo.request_type_id = 0;
            objPRInfo.Card_Type_id = objHelperService.CI(Card_type_id);
            objPRInfo.Card_Name = Card_Name;
            objPRInfo.Card_No = Card_No;
            objPRInfo.Card_CVV = Card_Cvv;
            
            objPRInfo.Card_ExpiryDate = Card_Expiry_Date;
            objPRInfo.Amount = objOrderInfo.TotalAmount;
            objPRInfo.Website_Id = websiteid;
            objPRInfo.Payment_Request_id = "";


            objPRInfo.Error_Text = "";

            returnstr = GetCreateRequest(TransactionTypes.Standard_Payment);
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = InsertRequest();
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = GetSendRequest();
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = GetUpdateResponse(Convert.ToInt32(paiduser));
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            if (objPRInfo.Response_Approved != null)
                approved = objPRInfo.Response_Approved.ToUpper();

            if (objPRInfo.Response_Code == null)
                objPRInfo.Response_Code = objPRInfo.Response_Status_Code;
            if (objPRInfo.Response_Text == null)
                objPRInfo.Response_Text = objPRInfo.Response_Status_desc;

            if (approved == "NO")
            {
                objPRInfo.Error_Text = objPRInfo.Response_Text;
                return objPRInfo;
            }
            return objPRInfo;
        }


       
        public PaymentRequestInfo GetRefundRequest(int order_id, int payment_id, string Txn_id,DataTable tblPayInfo )
        {
            string approved = "NO";
            string returnstr = "";
            objOrderInfo = objOrderServices.GetOrder(order_id);
            objPRInfo.Order_id = order_id;
            objPRInfo.Payment_id = payment_id;
            objPRInfo.request_type_id = 4;
            objPRInfo.Card_Type_id = objHelperService.CI(tblPayInfo.Rows[0]["Card_type_id"].ToString());
            objPRInfo.Card_Name = tblPayInfo.Rows[0]["Card_Name"].ToString();
            objPRInfo.Card_No = tblPayInfo.Rows[0]["Card_No"].ToString();
            objPRInfo.Card_CVV = tblPayInfo.Rows[0]["Card_Cvv"].ToString();
            objPRInfo.Card_ExpiryDate = tblPayInfo.Rows[0]["CARD_EXPIRYDATE"].ToString();
            objPRInfo.Response_Txn_ID = tblPayInfo.Rows[0]["Response_Txn_ID"].ToString();

            objPRInfo.Amount = objOrderInfo.TotalAmount;
            objPRInfo.Website_Id = websiteid;
            objPRInfo.Payment_Request_id = tblPayInfo.Rows[0]["Request_id"].ToString();
            objPRInfo.Error_Text = "";

            returnstr = GetCreateRequest(TransactionTypes.Refund );
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = InsertRequest();
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = GetSendRequest();
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            returnstr = GetUpdateResponse(0);
            if (returnstr != "")
            {
                objPRInfo.Error_Text = returnstr;
                return objPRInfo;
            }
            if (objPRInfo.Response_Approved != null)
                approved = objPRInfo.Response_Approved.ToUpper();

            if (objPRInfo.Response_Code == null)
                objPRInfo.Response_Code = objPRInfo.Response_Status_Code;
            if (objPRInfo.Response_Text == null)
                objPRInfo.Response_Text = objPRInfo.Response_Status_desc;

            if (approved == "NO")
            {
                objPRInfo.Error_Text = objPRInfo.Response_Text;
                return objPRInfo;
            }
            return objPRInfo;
        }
      

        public string GetUpdateResponse (int paiduserid)
        {
            

            string ElementName = "";
            try
            {
                //XmlReaderSettings settings = new XmlReaderSettings { Encoding = Encoding.UTF8};

                XmlReader reader = XmlReader.Create(new StringReader(objPRInfo.Response_xml));

                while (reader.Read())
                {
                    if (reader.MoveToContent() == XmlNodeType.Element)
                    {
                        ElementName = reader.Name;
                        switch (ElementName)
                        {
                            case "responseCode":
                                objPRInfo.Response_Code = reader.ReadString();
                                break;
                            case "responseText":
                                objPRInfo.Response_Text = reader.ReadString();
                                break;
                            case "approved":
                                objPRInfo.Response_Approved = reader.ReadString();
                                break;
                            case "statusCode":
                                objPRInfo.Response_Status_Code = reader.ReadString();
                                break;
                            case "statusDescription":
                                objPRInfo.Response_Status_desc = reader.ReadString();
                                break;
                            case "txnID":
                                objPRInfo.Response_Txn_ID = reader.ReadString();
                                break;
                        }
                    }

                }
                reader.Close();

                UpdateRequest(paiduserid);
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(objPRInfo.Request_id, objPRInfo.Response_xml);   
                return "Update Error";
            }
            return "";

           
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : SEND XML REQUEST To Secury Pay  ***/
        /********************************************************************************/
        public string  GetSendRequest()
        {
            try
            {
                strURL = objSecurity.StringDeCrypt(SP_URL, EnDekey);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                objErrorHandler.CreatePayLog(strURL);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strURL);
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(objPRInfo.Request_xml);
                objErrorHandler.CreatePayLog(objPRInfo.Request_xml);
                req.Method = "POST";
                req.ContentType = "text/xml;charset=utf-8";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                if (sr.ToString() != "")
                    objPRInfo.Response_xml = sr.ReadToEnd();
                else
                    return "Response Error";
            }
            catch (Exception ex)
            {
                objErrorHandler.CreatePayLog(ex.ToString());
                return "Response Error";

            }
            return "";

            //XmlTextReader reader = new XmlTextReader(sr);

        
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : CREATE XML REQUEST ***/
        /********************************************************************************/



        public string GetCreateRequest_BR(TransactionTypes trantype)
        {
            try
            {
                DataTable Dtbl = new DataTable();
                StringBuilder request = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.UTF8 };
                XmlWriter xml = XmlWriter.Create(request, settings);

                string messageID = "", timestamp = "", merchantID = "", merchant_pass = "";


                //Dtbl = objHelper.GetDataTableDB("select NEWID()");
                //if (Dtbl != null && Dtbl.Rows.Count > 0)
                //{
                //    messageID = Dtbl.Rows[0][0].ToString();
                //}
                messageID = GetNewId(objPRInfo.Order_id.ToString(), objPRInfo.Payment_id.ToString());
                merchantID = objSecurity.StringDeCrypt(SP_id, EnDekey);
                merchant_pass = objSecurity.StringDeCrypt(SP_p, EnDekey);
                strURL = objSecurity.StringDeCrypt(SP_URL, EnDekey);



                timestamp = this.getTimeStamp();
                objPRInfo.request_Timestamp = timestamp;
                objPRInfo.Request_id = messageID;
                xml.WriteStartDocument(true);


                xml.WriteStartElement("SecurePayMessage");

                xml.WriteStartElement("MessageInfo");

                xml.WriteElementString("messageID", messageID);
                xml.WriteElementString("messageTimestamp", timestamp);
                xml.WriteElementString("timeoutValue", TIME_OUT);
                xml.WriteElementString("apiVersion", API_VERSION);
                xml.WriteEndElement();

                xml.WriteStartElement("MerchantInfo");
                xml.WriteElementString("merchantID", merchantID);
                xml.WriteElementString("password", merchant_pass);
                xml.WriteEndElement();

                xml.WriteElementString("RequestType", "Payment");
                xml.WriteStartElement("Payment");
                xml.WriteStartElement("TxnList");
                xml.WriteAttributeString("count", "1");
                xml.WriteStartElement("Txn");
                xml.WriteAttributeString("ID", "1");
                if (trantype == TransactionTypes.Standard_Payment)
                {
                    xml.WriteElementString("txnType", Convert.ToString((int)TransactionTypes.Standard_Payment));
                }
                else if (trantype == TransactionTypes.Refund)
                {
                    xml.WriteElementString("txnType", Convert.ToString((int)TransactionTypes.Refund));
                }
                xml.WriteElementString("txnSource", "23"); //Source 23: XML API

                if (strURL.ToUpper().Contains("TEST") == true)
                {
                    if (objPRInfo.Amount.ToString().Contains(".") == true)
                    {
                        //string[] amt = objPRInfo.Amount.ToString().Split(new string[] { "." }, StringSplitOptions.None);
                        //xml.WriteElementString("amount", amt[0] + amt[1]);
                        xml.WriteElementString("amount", "100");
                    }
                    else
                    {
                        //xml.WriteElementString("amount", objPRInfo.Amount.ToString());
                        xml.WriteElementString("amount", "100");
                    }
                }
                else
                {
                    if (objPRInfo.Amount.ToString().Contains(".") == true)
                    {
                        string[] amt = objPRInfo.Amount.ToString().Split(new string[] { "." }, StringSplitOptions.None);
                        xml.WriteElementString("amount", amt[0] + amt[1]);
                        //xml.WriteElementString("amount","100");
                    }
                    else
                    {
                        xml.WriteElementString("amount", objPRInfo.Amount.ToString());
                        //xml.WriteElementString("amount", "100");
                    }
                }



                xml.WriteElementString("purchaseOrderNo", objPRInfo.Order_id.ToString());

                if (trantype == TransactionTypes.Refund)
                {
                    xml.WriteElementString("txnID", objPRInfo.Response_Txn_ID);
                }


                xml.WriteStartElement("CreditCardInfo");
                xml.WriteElementString("cardNumber", objPRInfo.Card_No);

                string[] Ed = objPRInfo.Card_ExpiryDate.Split(new string[] { "/" }, StringSplitOptions.None);

                xml.WriteElementString("expiryDate", Ed[0] + "/" + Ed[1].Substring(2));

                if (trantype == TransactionTypes.Standard_Payment)
                {
                    if (objPRInfo.Card_CVV != "")
                    {
                        xml.WriteElementString("cvv", objPRInfo.Card_CVV);
                    }
                }

                xml.WriteEndElement();




                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndElement();



                xml.WriteEndElement();

                xml.Flush();
                xml.Close();
                objPRInfo.Request_xml = request.ToString().Replace(@"\", "");

              
                return "";
            }
            catch (Exception ex)
            {
                    HelperServices objHelperServices = new HelperServices();
                    objHelperServices.Mail_Error_Log("", objPRInfo.Order_id, "", ex.ToString(), 0, 0, 0, 1);
                    return "";
            }

        }
        public string  GetCreateRequest( TransactionTypes trantype   )
        {
            try
            {
                DataTable Dtbl = new DataTable();
                StringBuilder request = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.UTF8};
                XmlWriter xml = XmlWriter.Create(request, settings);
                
                string messageID = "", timestamp = "", merchantID = "", merchant_pass = "";


                //Dtbl = objHelper.GetDataTableDB("select NEWID()");
                //if (Dtbl != null && Dtbl.Rows.Count > 0)
                //{
                //    messageID = Dtbl.Rows[0][0].ToString();
                //}
                messageID = GetNewId(objPRInfo.Order_id.ToString(), objPRInfo.Payment_id.ToString());
                merchantID = objSecurity.StringDeCrypt(SP_id, EnDekey);
                merchant_pass = objSecurity.StringDeCrypt(SP_p, EnDekey);
                strURL = objSecurity.StringDeCrypt(SP_URL, EnDekey);



                timestamp = this.getTimeStamp();
                objPRInfo.request_Timestamp = timestamp;
                objPRInfo.Request_id = messageID;
                xml.WriteStartDocument(true);
                

                xml.WriteStartElement("SecurePayMessage");

                xml.WriteStartElement("MessageInfo");
                 
                xml.WriteElementString("messageID", messageID);
                xml.WriteElementString("messageTimestamp", timestamp);
                xml.WriteElementString("timeoutValue", TIME_OUT);
                xml.WriteElementString("apiVersion", API_VERSION);
                xml.WriteEndElement();

                xml.WriteStartElement("MerchantInfo");
                xml.WriteElementString("merchantID", merchantID);
                xml.WriteElementString("password", merchant_pass);
                xml.WriteEndElement();

                xml.WriteElementString("RequestType", "Payment");
                xml.WriteStartElement("Payment");
                xml.WriteStartElement("TxnList");
                xml.WriteAttributeString("count", "1");
                xml.WriteStartElement("Txn");
                xml.WriteAttributeString("ID", "1");
                if (trantype == TransactionTypes.Standard_Payment)
                {
                    xml.WriteElementString("txnType", Convert.ToString((int)TransactionTypes.Standard_Payment));
                }
                else if (trantype == TransactionTypes.Refund)
                {
                    xml.WriteElementString("txnType", Convert.ToString((int)TransactionTypes.Refund));
                }
                xml.WriteElementString("txnSource", "23"); //Source 23: XML API

                if (strURL.ToUpper().Contains("TEST")==true)
                {
                    if (objPRInfo.Amount.ToString().Contains(".") == true)
                    {
                        //string[] amt = objPRInfo.Amount.ToString().Split(new string[] { "." }, StringSplitOptions.None);
                        //xml.WriteElementString("amount", amt[0] + amt[1]);
                        xml.WriteElementString("amount","100");
                    }
                    else
                    {
                        //xml.WriteElementString("amount", objPRInfo.Amount.ToString());
                        xml.WriteElementString("amount", "100");
                    }
                }
                else
                {
                    if (objPRInfo.Amount.ToString().Contains(".") == true)
                    {
                        string[] amt = objPRInfo.Amount.ToString().Split(new string[] { "." }, StringSplitOptions.None);
                        xml.WriteElementString("amount", amt[0] + amt[1]);
                        //xml.WriteElementString("amount","100");
                    }
                    else
                    {
                        xml.WriteElementString("amount", objPRInfo.Amount.ToString());
                        //xml.WriteElementString("amount", "100");
                    }
                }
                


                xml.WriteElementString("purchaseOrderNo", objPRInfo.Order_id.ToString());

                if (trantype == TransactionTypes.Refund)
                {
                    xml.WriteElementString("txnID", objPRInfo.Response_Txn_ID);
                }


                xml.WriteStartElement("CreditCardInfo");
                xml.WriteElementString("cardNumber", objPRInfo.Card_No);

                string[] Ed = objPRInfo.Card_ExpiryDate.Split(new string[] { "/" }, StringSplitOptions.None);

                xml.WriteElementString("expiryDate", Ed[0] + "/" + Ed[1].Substring(2));

                if (trantype == TransactionTypes.Standard_Payment)
                {
                    if (objPRInfo.Card_CVV != "")
                    {
                        xml.WriteElementString("cvv", objPRInfo.Card_CVV);
                    }
                }

                xml.WriteEndElement();


              

                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndElement();

             

                xml.WriteEndElement();

                xml.Flush();
                xml.Close();
                objPRInfo.Request_xml = request.ToString().Replace(@"\","");

                //-------------------------- For DB--------------------------------
                //string cardno="";
                //if (objPRInfo.Card_No.Length>10 )
                //    cardno = objPRInfo.Card_No.Substring(6, objPRInfo.Card_No.Length - 9);
                //else
                //    cardno =objPRInfo.Card_No;    

                //cardno = objPRInfo.Card_No.Replace(cardno, string.Concat(Enumerable.Repeat("X", cardno.Length)));

                //objPRInfo.Request_xml_db=objPRInfo.Request_xml;

                //objPRInfo.Request_xml_db=objPRInfo.Request_xml_db.Replace(objPRInfo.Card_No, cardno);
                //objPRInfo.Request_xml_db=objPRInfo.Request_xml_db.Replace(merchantID, "");
                //objPRInfo.Request_xml_db=objPRInfo.Request_xml_db.Replace(merchant_pass, "");
                //objPRInfo.Request_xml_db = objPRInfo.Request_xml_db.Replace(objPRInfo.Card_ExpiryDate, "");
           
               //-------------------------- For DB--------------------------------

                //string tranMessage = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                //tranMessage += "<SecurePayMessage>";
                //tranMessage += "<MessageInfo>";
                //tranMessage += "<messageID>" + objPRInfo.Request_id + "</messageID>";
                //tranMessage += "<messageTimestamp>" + objPRInfo.request_Timestamp + "</messageTimestamp>";
                //tranMessage += "<timeoutValue>60</timeoutValue>";
                //tranMessage += "<apiVersion>xml-4.2</apiVersion>";
                //tranMessage += "</MessageInfo>";
                //tranMessage += "<MerchantInfo>";
                //tranMessage += "<merchantID>" + merchantID  + "</merchantID>";
                //tranMessage += "<password>"+ merchant_pass +"</password>";
                //tranMessage += "</MerchantInfo>";
                //tranMessage += "<RequestType>Payment</RequestType>";
                //tranMessage += "<Payment>";
                //tranMessage += "<TxnList count=\"1\">";
                //tranMessage += "<Txn ID=\"1\">";
                //tranMessage += "<txnType>0</txnType>";
                //tranMessage += "<txnSource>23</txnSource>";
                //tranMessage += "<amount>"+ Convert.ToDouble(objPRInfo.Amount)+ "</amount>";
                //tranMessage += "<purchaseOrderNo>" + objPRInfo.Order_id  + "</purchaseOrderNo>";
                //tranMessage += "<CreditCardInfo>";
                //tranMessage += "<cardNumber>" + objPRInfo.Card_No  + "</cardNumber>";
                //tranMessage += "<cvv>" + objPRInfo.Card_CVV  + "</cvv>";
                //tranMessage += "<expiryDate>" + objPRInfo.Card_ExpiryDate  + "</expiryDate>";
                //tranMessage += "</CreditCardInfo>";
                //tranMessage += "</Txn>";
                //tranMessage += "</TxnList>";
                //tranMessage += "</Payment>";
                //tranMessage += "</SecurePayMessage>";
                //objPRInfo.Request_xml = tranMessage;
                return "";
            }
            catch (Exception ex)
            {
               
                return "unable to Create request";
            }
            
        }
        protected string GetNewId(string Order_id, string Payment_id)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 16)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return "Wes-" + Order_id + "-" + Payment_id + '-' + websiteid.ToString() + '-' + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : INSERT REQUEST ***/
        /********************************************************************************/
        public string  InsertRequest()
        {
            string rtnstring = "";
            try
            {
                int rtnvalue = 0;

                string cardno = "";
                if (objPRInfo.Card_No.Length > 10)
                    cardno = objPRInfo.Card_No.Substring(6, objPRInfo.Card_No.Length - 9);
                else
                    cardno = objPRInfo.Card_No;

                cardno = objPRInfo.Card_No.Replace(cardno, string.Concat(Enumerable.Repeat("X", cardno.Length)));

                string sSQL = "Exec STP_TBWC_POP_PAYMENT_REQUEST_RESPONSE_SP '" + objPRInfo.Request_id + "'";
                sSQL = sSQL + ",'" + objPRInfo.request_type_id + "','" + objPRInfo.request_Timestamp + "'," + objPRInfo.Payment_id + "," + objPRInfo.Order_id + "," + objPRInfo.Card_Type_id + ",'" + objPRInfo.Card_Name + "',";
                sSQL = sSQL + "'" + cardno + "','',";
                sSQL = sSQL + "'',"+ objPRInfo.Website_Id ;
                rtnvalue= objHelper.ExecuteSQLQueryDB(sSQL);
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
        public string UpdateRequest(int userid)
        {
            string rtnstring = "";
            try
            {
                int rtnvalue = 0;

                string sSQL = "Exec STP_TBWC_RENEW_PAYMENT_REQUEST_RESPONSE_SP '" + objPRInfo.Request_id + "'";
                sSQL = sSQL + ",'','" + objPRInfo.Response_Approved + "','" + objPRInfo.Response_Status_Code + "','" + objPRInfo.Response_Status_desc + "','" + objPRInfo.Response_Txn_ID + "','" + objPRInfo.Response_Code + "','" + objPRInfo.Response_Text + "'";
                sSQL = sSQL + ",'" + userid +"'";
                rtnvalue =objHelper.ExecuteSQLQueryDB(sSQL);
                objErrorHandler.CreateLog(sSQL); 
                if (rtnvalue <= 0)
                {
                    objErrorHandler.CreateLog(objPRInfo.Request_id, objSecurity.StringDeCrypt(objPRInfo.Response_xml,EnDekey));
                    rtnstring = "Unable to Update Response Data";
                }
                return rtnstring;

            }
            catch (Exception e)
            {
                objErrorHandler.CreateLog(objPRInfo.Request_id, objSecurity.StringDeCrypt(objPRInfo.Response_xml,EnDekey));
                return "Unable to Update Response Data";
            }
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


