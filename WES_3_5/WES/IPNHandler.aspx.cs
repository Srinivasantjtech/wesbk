using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Net;
using System.Configuration;
using System.Text;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.Threading;
    public partial class IPNHandler : System.Web.UI.Page
    {
        string postUrl = ConfigurationManager.AppSettings["PayPalSubmitUrl"];
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        public string business_Mail_id = System.Configuration.ConfigurationManager.AppSettings["P_ID2"].ToString();
        public string Pay_Url = System.Configuration.ConfigurationManager.AppSettings["P_ID1"].ToString();

        PayPalService objPayPalService = new PayPalService();
        Security objSecurity =new Security();
        PaymentServices objPaymentServices = new PaymentServices();
        PayPalService.PaymentIPNInfo objIPNinfo = new PayPalService.PaymentIPNInfo();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
        OrderServices objOrderServices = new OrderServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        UserServices objUserServices = new UserServices();
        int ipn_mail = Convert.ToInt32(ConfigurationManager.AppSettings["IPN_MAIL_SEND"].ToString());
       // public int OrderID = 0;
      //  public string Txn_id = "";
       // public int PaymentID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
           objErrorHandler.CreatePayLog(  DecryptSP(business_Mail_id));
            try
            {
                //  objErrorHandler.CreatePayLog_Final(DateTime.Now.ToLongTimeString()+"before");
                int ms = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["TimerInt"].ToString());
                Thread.Sleep(ms);


            }
            catch (Exception ex)
            {

            }
            //objErrorHandler.CreatePayLog_Final(DateTime.Now.ToLongTimeString()+"after");  
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(postUrl);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            string ipnPost = strRequest;
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            // logging ipn messages... be sure that you give write permission to process executing this code
            string logPathDir = ResolveUrl("Messages");
            string logPath = string.Format("{0}\\{1}.txt", Server.MapPath(logPathDir), DateTime.Now.Ticks);
            File.WriteAllText(logPath, ipnPost);
            string req_id = "";
            NameValueCollection Res = objPayPalService.GetResponseStrToNameValue(ipnPost, false);
            // objErrorHandler.CreateLog(strResponse+DateTime.Now.ToString()  );
            // objErrorHandler.CreateLog("inside handler");
            try
            {

                if (strResponse == "VERIFIED")
                {




                    if (objPayPalService.VerifyResponse(Res) == 0)
                    {
                        //objErrorHandler.CreateLog(ipnPost + DateTime.Now.ToString());
                        objIPNinfo.Response_info = ipnPost;
                        objPayPalService.UpdateIPN(objIPNinfo);

                        return;

                    }



                    if (Res.Keys.Count > 0)
                    {

                        if (Res["custom"] != null)
                        {

                            req_id = Res["custom"].ToString();

                            objIPNinfo.Request_id = req_id;

                            String[] StringArray = req_id.Split('-');

                            if (StringArray[1] != null)
                                objIPNinfo.Order_id = Convert.ToInt32(StringArray[1].ToString());
                            if (StringArray[2] != null)
                                objIPNinfo.Payment_id = Convert.ToInt32(StringArray[2].ToString());
                            if (StringArray[3] != null)
                                objIPNinfo.Website_Id = Convert.ToInt32(StringArray[3].ToString());


                        }
                        if (Res["txn_id"] != null)
                            objIPNinfo.Response_Txn_ID = Res["txn_id"].ToString();



                        if (Res["mc_gross"] != null)
                            objIPNinfo.Amount = Convert.ToDecimal(Res["mc_gross"].ToString());




                        string rtnstring = objPayPalService.GetPymentStatus(objIPNinfo.Response_Txn_ID);
                        NameValueCollection conformres = objPayPalService.GetResponseStrToNameValue(rtnstring, true);
                        if (conformres["Status"] == "SUCCESS")
                        {
                            objIPNinfo.Response_Approved = "YES";

                        }
                        else
                        {
                            objIPNinfo.Response_Approved = "NO";
                        }

                        objIPNinfo.Response_info = ipnPost;
                        objPayPalService.UpdateIPN(objIPNinfo);

                        DataTable dt = objPayPalService.GETIPN(objIPNinfo.Response_Txn_ID, req_id);
                        if (dt != null && dt.Rows.Count > 1)
                        {
                            DataRow[] drs = dt.Select("APPROVED='YES' ");
                            if (drs.Length > 1)
                                return;
                        }
                        var response = new NameValueCollection();
                        response.Add("tx", objIPNinfo.Response_Txn_ID);

                        string rtn = objPayPalService.SetPayPalStatus(response, req_id);

                        // LoadIds();
                        if (conformres["Status"] == "SUCCESS")
                        {
                            // objErrorHandler.CreateLog(conformres["Status"].ToString() + "-"+ Session["USER_ROLE"].ToString());
                            objErrorHandler.CreatePayLog("before IPNHandler " + conformres["Status"]);

                            if (objOrderServices.IsNativeCountry(objIPNinfo.Order_id) == 0) // is other then au
                            {
                                // objErrorHandler.CreateLog("is other then au");
                                if (objOrderServices.GetOrderStatus(objIPNinfo.Order_id) != OrderServices.OrderStatus.COMPLETED.ToString() && objOrderServices.GetOrderStatus(objIPNinfo.Order_id) != OrderServices.OrderStatus.Proforma_Payment_Received.ToString())
                                {
                                    //if (objIPNinfo.Website_Id == 3 || objIPNinfo.Website_Id==4)
                                    //{
                                    //    objOrderServices.UpdateOrderStatus(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                                    //}
                                   if (objIPNinfo.Website_Id == 1)
                                    {
                                        objOrderServices.UpdateOrderStatus(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Online_Payment);
                                    }
                                    int Rtn = objOrderServices.SentSignal(objIPNinfo.Payment_id.ToString(), objIPNinfo.Order_id.ToString(), "190");
                                }
                            }
                            else
                            {

                                if (objOrderServices.GetOrderStatus(objIPNinfo.Order_id) != OrderServices.OrderStatus.COMPLETED.ToString() && objOrderServices.GetOrderStatus(objIPNinfo.Order_id) != OrderServices.OrderStatus.Payment_Successful.ToString() && objOrderServices.GetOrderStatus(objIPNinfo.Order_id) != OrderServices.OrderStatus.Order_Received.ToString())
                                {

                                    if (objIPNinfo.Website_Id == 1)
                                    {
                                        objOrderServices.UpdateOrderStatus(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Online_Payment);
                                    }
                                    //  SendMail(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                    int Rtn = objOrderServices.SentSignal(objIPNinfo.Payment_id.ToString(), objIPNinfo.Order_id.ToString(), "150");
                                    objErrorHandler.CreatePayLog("before IPNHandler website id" + objIPNinfo.Website_Id);
                                    if ((ipn_mail == 1) && (objIPNinfo.Website_Id == 3))
                                    {
                                        objErrorHandler.CreatePayLog("before IPNHandler SendMail_AfterPaymentPP");
                                        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                        OrderServices objorderservice = new OrderServices();
                                        oOrderInfo = objOrderServices.GetOrder(objIPNinfo.Order_id);
                                        if (oOrderInfo.Websiteid == 3)
                                        {
                                            //objErrorHandler.CreatePayLog("before SendMail_AfterPaymentPP CheckMailLog ipnhadler Orderid=" + objIPNinfo.Order_id);
                                            //DataTable dtcheckmailsend = objOrderServices.CheckMailLog(objIPNinfo.Order_id);
                                            //if (dtcheckmailsend == null || dtcheckmailsend.Rows.Count == 0)
                                            //{
                                            //    objErrorHandler.CreatePayLog("after SendMail_AfterPaymentPP CheckMailLog Orderid=" + objIPNinfo.Order_id);

                                            //    SendMail_AfterPaymentPP(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                            //    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine();
                                            //    tbwtEngine.SendMail_Review(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                            //}
                                        }
                                        else if (oOrderInfo.Websiteid == 1)
                                        {

                                            SendMail_AfterPaymentPP_WES(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                        }
                                        objErrorHandler.CreatePayLog("after IPNHandler SendMail_AfterPaymentPP");
                                    }
                                }
                                //if(websiteid != 3)
                                // SendMail(objIPNinfo.Order_id, (int)OrderServices      if (websiteid == 3).OrderStatus.Payment_Successful, false);

                                //if (ipn_mail == 1)
                                //{
                                //    objErrorHandler.CreatePayLog("before IPNHandler SendMail_AfterPaymentPP");
                                //    if (websiteid == 3)
                                //        SendMail_AfterPaymentPP(objIPNinfo.Order_id, (int)OrderServices.OrderStatus.Payment_Successful, false);
                                //    objErrorHandler.CreatePayLog("after IPNHandler SendMail_AfterPaymentPP");
                                //}

                            }
                            objErrorHandler.CreatePayLog("after IPNHandler " + conformres["Status"]);

                        }

                    }
                    else
                    {
                        objIPNinfo.Response_info = ipnPost;
                        objPayPalService.UpdateIPN(objIPNinfo);
                    }






                    // objErrorHandler.CreateLog(strResponse);

                    //check the payment_status is Completed
                    //check that txn_id has not been previously processed
                    //check that receiver_email is your Primary PayPal email
                    //check that payment_amount/payment_currency are correct
                    //process payment
                }
                else if (strResponse == "INVALID")
                {
                    //log for manual investigation
                    //objErrorHandler.CreateLog(strResponse);
                    objIPNinfo.Response_info = ipnPost;
                    objPayPalService.UpdateIPN(objIPNinfo);
                }
                else
                {
                    objIPNinfo.Response_info = ipnPost;
                    objPayPalService.UpdateIPN(objIPNinfo);
                    //log response/ipn data for manual investigation
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
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


        private void SendMail_AfterPaymentPP_WES(int OrderId, int OrderStatus, bool isau)
        {
            string toemail = "";
            try
            {
                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner1 OrderId=" + OrderId);

                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;


                if (oOrderInfo.CreatedUser != 999)
                {

                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                }
                else
                {
                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                }

                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;
                toemail = oUserInfo.AlternateEmail;

                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

                string SubmittedBy = "";
                switch (oOrderInfo.OrderStatus)
                {
                    case 6:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    case 12:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    default:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                }

                //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sHTML = "";
                try
                {
                    StringTemplateGroup _stg_container = null;
                    StringTemplateGroup _stg_records = null;
                    StringTemplate _stmpl_container = null;
                    StringTemplate _stmpl_records = null;
                    //StringTemplate _stmpl_records1 = null;
                    // StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    // StringTemplateGroup _stg_container1 = null;
                    //  StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    // int ictrows = 0;

                    DataSet dscat = new DataSet();
                    // DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                    int ictrecords = 0;

                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {
                        //if (websiteid == 3)
                        //   _stmpl_records = _stg_records.GetInstanceOf("mail-wagner" + "\\" + "row");
                        //   else
                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");

                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                    //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    //if( websiteid == 3)
                    //   _stmpl_container = _stg_container.GetInstanceOf("mail-wagner" + "\\" + "OrderSubmitted");
                    //    else
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay_wes");
                    //else
                    //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");

                    _stmpl_container.SetAttribute("PAY_METHOD", "PayPal");
                    _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                    _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
                    //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                    //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                    //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    // if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                    // else
                    //    _stmpl_container.SetAttribute("SubmittedBy", "");



                    _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                    _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                    _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                    _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                    if (shippingnotes != "")
                        _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                    else
                        _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString();
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner2");
                    objErrorHandler.CreatePayLog(sHTML);
                }
                catch (Exception ex)
                {
                    objHelperServices.Mail_Error_Log("PP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, "", ex.Message);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";

                }
                if (sHTML != "")
                {

                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner3 OrderId=" + OrderId);
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        MessageObj.Bcc.Add(webadminmail);
                        //if (isau == false)
                        //{
                        //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                        //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                        //    else
                        //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                        //}

                        // Get_ADMIN_UserEmils();
                        //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                        //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                    }
                    else
                    {
                        emails = objUserServices.Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());


                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "Wagner Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;

                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner4 OrderId=" + OrderId);
                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner5 OrderId=" + OrderId);

                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            smtpclient.Send(MessageObj);
                            objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                        }
                        if (Adminemails != "")
                        {

                            string[] emailid = Adminemails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.Subject = "Wagner International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    }
                                }
                            }
                            else
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(Adminemails.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                }
                                //MessageObj.CC.Add(emails.ToString());
                            }

                        }
                    }
                    else
                    {
                        if (emails != "")
                        {

                            string[] emailid = emails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                        objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    }
                                }
                            }
                            else
                            {
                                if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(emails.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                    //MessageObj.CC.Add(emails.ToString());
                                }
                            }

                        }


                    }


                }
            }
            catch (Exception ex)
            {
                objHelperServices.Mail_Error_Log("PP", OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                objHelperServices.Mail_Log("PP", OrderId, "", ex.Message);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();


            }
            objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP IPNHandler inner6 OrderId=" + OrderId);
        }

        private void SendMail(int OrderId, int OrderStatus, bool isau)
        {
            try
            {


                string BillAdd;
                string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId);
                ShippAdd = GetShippingAddress(OrderId);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

                string SubmittedBy = "";
                switch (oOrderInfo.OrderStatus)
                {
                    case 6:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    case 12:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                    default:
                        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                        break;
                }


                string sHTML = "";
                try
                {
                    StringTemplateGroup _stg_container = null;
                    StringTemplateGroup _stg_records = null;
                    StringTemplate _stmpl_container = null;
                    StringTemplate _stmpl_records = null;
                    StringTemplate _stmpl_records1 = null;
                    StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    StringTemplateGroup _stg_container1 = null;
                    StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    int ictrows = 0;

                    DataSet dscat = new DataSet();
                    DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                    int ictrecords = 0;

                    foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                    {

                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");
                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                    //if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                    //else
                    //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");


                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                   // if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                        _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                   // else
                    //    _stmpl_container.SetAttribute("SubmittedBy", "");



                    _stmpl_container.SetAttribute("ShippingMethod", ShippingMethod);
                    _stmpl_container.SetAttribute("BillingAddress", BillAdd);
                    _stmpl_container.SetAttribute("ShippingAddress", ShippAdd);
                    _stmpl_container.SetAttribute("shippingnotes", shippingnotes);

                    if (shippingnotes != "")
                        _stmpl_container.SetAttribute("TBT_shippingnotes", true);
                    else
                        _stmpl_container.SetAttribute("TBT_shippingnotes", false);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString();
                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    sHTML = "";
                }
                if (sHTML != "")
                {
                    //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                    //System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

                    //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    //MessageObj.To.Add(Emailadd.ToString());
                    ////MessageObj.To.Add("jtechalert@gmail.com");
                    ////MessageObj.To.Add("mohanarangam.e.r@jtechindia.com");
                    //MessageObj.Subject = "Pending Order - WES Australasia";
                    //MessageObj.IsBodyHtml = true;
                    //MessageObj.Body = sHTML;
                    //System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    ////System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    //smtpclient.UseDefaultCredentials = false;
                    //smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    ////smtpclient.Port = 587;
                    ////smtpclient.Credentials = new System.Net.NetworkCredential("jtechalert@gmail.com", "jtech@#$123");
                    //smtpclient.Send(MessageObj);

                    //objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                    ////ArrayList CCList = new ArrayList();
                    ////CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    ////objNotificationServices.NotifyCC = CCList;
                    //objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    //objNotificationServices.NotifyTo.Add(Emailadd.ToString());

                   // string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                    //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                    //objNotificationServices.NotifySubject = EmailSubject;
                    //objNotificationServices.NotifyMessage = sHTML;
                    //objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                    //objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                    //objNotificationServices.NotifyIsHTML = true;
                    //objNotificationServices.SendMessage();


                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

                    string emails = "";
                    string Adminemails = "";
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                        //if (isau == false)
                        //{
                        //    if (System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"].ToString() == "9200")
                        //        Adminemails = System.Configuration.ConfigurationManager.AppSettings["ToMail"].ToString();
                        //    else
                        //        Adminemails = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                        //}

                        // Get_ADMIN_UserEmils();
                        //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                        //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                    }
                    else
                    {
                        emails = Get_ADMIN_APPROVED_UserEmils(UserID.ToString());

                        MessageObj.To.Add(Emailadd.ToString());


                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                   // {
                        MessageObj.Subject = "Wes Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                   // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);




                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                            //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                            MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());
                            smtpclient.Send(MessageObj);
                        }
                        if (Adminemails != "")
                        {

                            string[] emailid = Adminemails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (ApprovedUserEmailadd.ToUpper().ToString() != id.ToUpper().ToString() && Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.Subject = "Wes International Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                    }
                                }
                            }
                            else
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(Adminemails.ToString());
                                    smtpclient.Send(MessageObj);
                                }
                                //MessageObj.CC.Add(emails.ToString());
                            }

                        }
                    }
                    else
                    {
                        if (emails != "")
                        {

                            string[] emailid = emails.ToString().Split(',');
                            if (emailid.Length > 0)
                            {
                                foreach (string id in emailid)
                                {
                                    if (Emailadd.ToUpper().ToString() != id.ToUpper().ToString())
                                    {
                                        //MessageObj.CC.Add(id.ToString());
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                    }
                                }
                            }
                            else
                            {
                                if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                                {
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(emails.ToString());
                                    smtpclient.Send(MessageObj);
                                    //MessageObj.CC.Add(emails.ToString());
                                }
                            }

                        }


                    }


                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
        }
        public string GetShippingAddress(int OrderID)
        {
            string sShippingAddress = "";
            OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
            oOI = objOrderServices.GetOrder(OrderID);

            if (oOI.ShipCompName.Trim().Length > 0)
                sShippingAddress = oOI.ShipCompName + "<BR>";
            else
                sShippingAddress = "";

            sShippingAddress = sShippingAddress + oOI.ShipFName + oOI.ShipLName + "<BR>";
            if (oOI.ShipAdd1.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd1.Trim() + "<BR>";
            }
            if (oOI.ShipAdd2.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd2.Trim() + "<BR>";
            }
            if (oOI.ShipAdd3.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + oOI.ShipAdd3.Trim() + "<BR>";
            }
            if (oOI.ShipCity.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipCity + "<BR>";
            if (oOI.ShipState.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipState + "<BR>";
            if (oOI.ShipZip.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipZip + "<BR>";
            if (oOI.ShipCountry.Trim().Length > 0)
                sShippingAddress = sShippingAddress + oOI.ShipCountry + "<BR>";
            //if (oOI.ReceiverContact.Trim().Length > 0)
            //{
            //    sShippingAddress = sShippingAddress + "<BR>" + oOI.ReceiverContact + "<BR>";
            //}
            sShippingAddress = sShippingAddress + oOI.ShipPhone + "<BR>";
            if (oOI.DeliveryInstr.Trim().Length > 0)
            {
                sShippingAddress = sShippingAddress + "<BR>" + oOI.DeliveryInstr + "<BR>";
            }

            return sShippingAddress;
        }
        public string GetBillingAddress(int OrderID)
        {
            string sBillingAddress = "";
            OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
            oBI = objOrderServices.GetOrder(OrderID);
            if (oBI.BillcompanyName.Trim().Length > 0)
                sBillingAddress = oBI.BillcompanyName + "<BR>";
            else
                sBillingAddress = "";

            //sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
            if (oBI.BillAdd1.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd1.Trim() + "<BR>";
            }
            if (oBI.BillAdd2.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd2.Trim() + "<BR>";
            }
            if (oBI.BillAdd3.Trim().Length > 0)
            {
                sBillingAddress = sBillingAddress + oBI.BillAdd3.Trim() + "<BR>";
            }
            if (oBI.BillCity.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillCity + "<BR>";
            if (oBI.BillState.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillState + "<BR>";
            if (oBI.BillZip.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillZip + "<BR>";
            if (oBI.BillCountry.Trim().Length > 0)
                sBillingAddress = sBillingAddress + oBI.BillCountry + "<BR>";

            sBillingAddress = sBillingAddress + oBI.BillPhone;





            return sBillingAddress;
        }
       
        private string Get_ADMIN_APPROVED_UserEmils(string user_id)
        {
            DataSet oDs = new DataSet();
            string emails = "";

            string userid = user_id;//Session["USER_ID"].ToString();
            if (userid == "")
                userid = "0";


            try
            {

                oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
                if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
                {

                    oDs.Tables[0].TableName = "Users";

                    foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                    {
                        if (rItem["EMAILADDR"].ToString() != "")
                            emails = emails + rItem["EMAILADDR"].ToString() + ",";

                    }
                    if (emails != "")
                        emails = emails.Substring(0, emails.Length - 1) + "";
                }
            }
            catch (Exception ex)
            {

            }
            return emails;
        }
    }

