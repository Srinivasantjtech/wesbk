using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Data;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using Antlr3.ST;
    public partial class BillInfo : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        PayOnlineService objPayOnlineService = new PayOnlineService();

        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();
        PayPalService objPayPalService = new PayPalService();
        string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strPaymentLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        
        string strBackLink = "";
        string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
        string key = "";
        protected override void OnPreRender(EventArgs e)
        {
            //base.OnPreRender(e);
            //string sb;
            //sb = "<script language=javascript>\n";
            //sb += "window.history.forward(1);\n";
            //sb += "\n</script>";
            //ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


            //sb = "";
            //sb = "<script type=javascript>\n";
            //sb += "window.onload = function () { Clear(); }\n";
            //sb += "function Clear() { \n";
            //sb += " var Backlen=history.length; \n";
            //sb += " if (Backlen > 0) history.go(-Backlen); \n";
            //sb += "\n}</script>";
            //ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



        }
        protected void Page_PreInit()
        {
            if (httpRequestVariables()["tx"] != null)
            {
                Page.MasterPageFile = "Blank.Master";
            }
        }
        protected NameValueCollection httpRequestVariables()
        {
            var post = Request.Form;       // $_POST
            var get = Request.QueryString; // $_GET
            return Merge(post, get);
        }

         protected string EncryptSP(string ordid)
    {
        string enc = "";
        enc = objSecurity.StringEnCrypt(ordid, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        enc = objSecurity.StringEnCrypt(enc, EnDekey);
        return HttpUtility.UrlEncode(enc);
    }
        protected void Page_Load(object sender, EventArgs e)
        {
            

            
            string output = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.ClearHeaders();
            //Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
            //Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1
            //Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1
            //Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1
            //Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1
            //Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); // HTTP 1.1

            try
            {
                if (Request["key"] != null)
                {
                    key = DecryptSP(Request["key"].ToString());
                }
                //objErrorHandler.CreateLog("inside billinfo");
                if (httpRequestVariables()["tx"] != null)
                {
                    //objErrorHandler.CreateLog("inside trasaction"); 
                    HttpContext.Current.Session["payflowresponse"] = httpRequestVariables();
                
                    var response = HttpContext.Current.Session["payflowresponse"] as NameValueCollection;
                    if (response["cm"] != null)
                        objErrorHandler.CreatePayLog(response["cm"].ToString());
                        objPayPalService.SetPayPalStatus(response, response["cm"].ToString());
                    //string rtn = 
                    //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
                        output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "\";</script>";
                        BodyContentDiv.InnerHtml = output;
                        return;
                }

                if (IsPostBack)
                    return;
                LoadIds();




                if (!IsPostBack)
                {
                    string BillAdd;
                    string ShippAdd;



                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    BillAdd = BuildBillAddress();
                    ShippAdd = BuildShippAddress();
                    lblDeliveryTo.Text = BillAdd;
                    lblShipTo.Text = ShippAdd;
                    lblOrderNo.Text = oPayInfo.PORelease;
                    lblShippingMethod.Text = oOrderInfo.ShipMethod;
                    LoadOrderItem();

                    //////Check if we just returned inside the iframe.  If so, store payflow response and redirect parent window with javascript.


                    //Check whether we stored a server response.  If so, print it out.


                    bool isipn = false;
                    objErrorHandler.CreatePayLog("Inside BillInfo Paypal:"+OrderID.ToString());
                    if (HttpContext.Current.Session["payflowresponse"] != null && HttpContext.Current.Session["payflowresponse"] != "")
                    {

                        if (HttpContext.Current.Session["payflowresponse"] == "SUCCESS")
                        {

                             string ordstatus = objOrderServices.GetOrderStatus(OrderID);
                            string[] ipn = null;

                            if (HttpContext.Current.Session["IPN"] != null && HttpContext.Current.Session["IPN"] != "")
                            {
                                ipn = HttpContext.Current.Session["IPN"].ToString().Split('#');

                                if (ipn.Length >= 1)
                                {
                                    DataTable dt = objPayPalService.GETIPN(ipn[1].ToString(), ipn[0].ToString());
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        DataRow[] drs = dt.Select("APPROVED='YES'");
                                        if (drs.Length > 0)
                                            isipn = true;
                                    }
                                }
                            }




                            //if (objOrderServices.IsNativeCountry(OrderID) == 0) // is other then au
                            //{
                            //    if (isipn == false)
                            //    {
                            //        objOrderServices.UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                            //        int Rtn = objOrderServices.SentSignal(PaymentID.ToString(), OrderID.ToString(), "190");
                            //    }
                            //}
                            //else
                            //{
                            //    if (isipn == false)
                            //    {
                            //        objOrderServices.UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Payment_Successful);
                            //        int Rtn = objOrderServices.SentSignal(PaymentID.ToString(), OrderID.ToString(), "150");
                            //    }
                            //}
 //                                if (objOrderServices.IsNativeCountry(OrderID) == 0)
 //               {
 //                   objErrorHandler.CreateLog("inside native country" + OrderID + "UpdatePaymentOrderStatus_Onlinepayment");
 //objOrderServices.UpdatePaymentOrderStatus_Onlinepayment(OrderID, PaymentID, isipn);
 //                                }
 //                                else if (ispickuponly_zone(OrderID) == true )
 //                                {
 //                                    objErrorHandler.CreateLog("ispickuponly_zone" + OrderID + "UpdatePaymentOrderStatus_Remotezone");
 //                                    objOrderServices.UpdatePaymentOrderStatus_Remotezone(OrderID, PaymentID, isipn);
 //                                }
 //                                else

 //                                {
                                   //  objErrorHandler.CreateLog("inside " + OrderID + "UpdatePaymentOrderStatus_Onlinepayment");
                            objErrorHandler.CreatePayLog("paytype-" + "paypal" + ordstatus);
                            //if (Request.QueryString["Paytype"] == "submitorder")
                            //{
                            if(ordstatus==OrderServices.OrderStatus.OPEN.ToString())
                            {
                                objOrderServices.UpdatePaymentOrderStatus_DirectOnlinepayment(OrderID, PaymentID, isipn);
                                int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "PP");
                                SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Online_Payment, false);
                               
                            }
                            else
                            {
                                objOrderServices.UpdatePaymentOrderStatus_Onlinepayment(OrderID, PaymentID, isipn);
                                int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "PP");
                                SendMail_AfterPaymentPP(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success, false);
                            }
                           
                                
                            divError.InnerHtml = "";
                            divOk.InnerHtml = "Transaction approved! Thank you for your order.";
                            //divlink.InnerHtml="<br/><a href=\"home.aspx\" class=\"toplinkatest\" >Home</a>";
                            divlink.InnerHtml = "Payment Method: Paypal";
                            objErrorHandler.CreatePayLog("Transaction approved! Thank you for your order." + OrderID.ToString());
                            //if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                            //{
                            //    Session["PrevOrderID"] = "0";
                            //}
                            Session["ORDER_ID"] = "0";
                        }
                        else
                        {
                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                            objErrorHandler.CreatePayLog("Transaction failed! Please try again.<br/>"+ OrderID.ToString());
                            divlink.InnerHtml = "<a href=\"PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"toplinkatest\" >Back</a>";

                        }
                        HttpContext.Current.Session["payflowresponse"] = "";
                        HttpContext.Current.Session["IPN"] = "";
                        //output += "<p>(server response follows)</p>\n";
                        //output += print_r(payflowresponse);
                        //divContent.InnerHtml = output;
                        return;
                    }
                    else
                    {
                        Response.Redirect("home.aspx");
                    }





                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Session["payflowresponse"] = "";
                //HttpContext.Current.Session["IPN"] = "";
                //if (HttpContext.Current.Session["payflowresponse"] != null && HttpContext.Current.Session["payflowresponse"] != "")
                //{

                //    if (HttpContext.Current.Session["payflowresponse"] == "SUCCESS")
                //    {
                //        divError.InnerHtml = "";
                //        divOk.InnerHtml = "Pay Pal Transaction approved! but Order Updation failed ,Please Contact Administrator!";
                //        //divlink.InnerHtml="<br/><a href=\"home.aspx\" class=\"toplinkatest\" >Home</a>";
                //        divlink.InnerHtml = "Payment Method: Paypal";
                //    }
                //}
                //else
                //{
                //    divOk.InnerHtml = "";
                //    divOk.Visible = false;
                //    divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                //    divlink.InnerHtml = "<a href=\"PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"toplinkatest\" >Back</a>";
                //}


            }

        }


        private bool ispickuponly_zone(int orderid)
        {


            try
            {
                string zipcode = objOrderServices.GetZipCode(orderid);
                if (zipcode != null)
                {
                    DataSet ds = objUserServices.GetZONE(Convert.ToInt32(zipcode));
                    if ((ds != null) && (ds.Tables[0].Rows.Count > 0))
                    {
                        if (ds.Tables[0].Rows[0]["zone"].ToString().ToUpper() == "REMOTE")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {

                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return false;
            }

        }

        private bool ispickuponly_product(int orderid)
        {
            try
            {

                DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                    {
                        DataSet ds = objUserServices.GetIsPickUpOnly(dsOD.Tables[0].Rows[i]["product_id"].ToString());
                        if ((ds != null) && (ds.Tables[0].Rows.Count > 0))
                        {
                            return true;
                        }

                    }


                }
                return false;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return false;
            }
        }
        private void SendMail_AfterPaymentPP(int OrderId, int OrderStatus, bool isau)
        {
            string toemail = "";
            try
            {
                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner1 OrderId=" + OrderId);

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


                //if (oOrderInfo.CreatedUser != 999)
                //{

                //    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                //}
                //else
                //{
                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                //}

                //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.Email;
                toemail = oUserInfo.Email;
                if (oUserInfo.AlternateEmail != "")
                {
                    Emailadd = oUserInfo.AlternateEmail;
                    toemail = oUserInfo.AlternateEmail;
                }
               

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
                    //StringTemplate _stmpl_recordsrows = null;
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                    //StringTemplateGroup _stg_container1 = null;
                    //StringTemplateGroup _stg_records1 = null;
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];

                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    //int ictrows = 0;

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
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmittedAfterPay");
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
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner2");
                    objErrorHandler.CreatePayLog(sHTML);
                }
                catch (Exception ex)
                {
                    objHelperServices.Mail_Error_Log("PP", oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(Session["USER_ID"].ToString()), Convert.ToInt16(Session["USER_ROLE"]), 1);
                   // objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, "", ex.Message);
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    //sHTML = "";

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

                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner3 OrderId=" + OrderId);
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP messagefrom " + MessageObj.From);
                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP WEB ADMIN EMAIL " + MessageObj.From.ToString());
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
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP b4 subject OrderId=" + OrderId + Emailadd);
                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "WES Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                   // MessageObj.Bcc.Add("indumathi@jtechindia.com");
                    //if (isau == false)
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner4 OrderId=" + OrderId);
                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);

                  objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                    objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner5 OrderId=" + OrderId + MessageObj.To.ToString());

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
                                        MessageObj.Subject = "WES International Order Alert - Order No : " + CustomerOrderNo.ToString();
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

                objErrorHandler.CreateLog(ex.ToString());

            }
            objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner6 OrderId=" + OrderId);
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

            sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
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

        //human-readable representation of an NVC
        public string print_r(Object obj)
        {
            string output = "<pre>\n";
            if (obj is NameValueCollection)
            {
                NameValueCollection nvc = obj as NameValueCollection;
                foreach (var key in nvc)
                {
                    if(key!=null)
                    output += key + "=" + nvc[key.ToString()] + "\n";
                };
            }
            else
            {
                output += "UNKNOWN TYPE";
            }
            output += "</pre>";
            return output;
        }
        //protected NameValueCollection run_payflow_call(NameValueCollection requestArray)
        //{
        //    String nvpstring = "";
        //    foreach (string key in requestArray)
        //    {
        //        //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
        //        var val = requestArray[key];
        //        nvpstring += key + "[ " + val.Length + "]=" + val + "&";
        //    }

        //    string urlEndpoint;
        //    if (environment == "pilot" || environment == "test" || environment == "sandbox")
        //    {
        //        urlEndpoint = "https://pilot-payflowpro.paypal.com/";
        //    }
        //    else
        //    {
        //        urlEndpoint = "https://payflowpro.paypal.com";
        //    }

        //    //send request to Payflow
        //    HttpWebRequest payReq = (HttpWebRequest)WebRequest.Create(urlEndpoint);
        //    payReq.Method = "POST";
        //    payReq.ContentLength = nvpstring.Length;
        //    payReq.ContentType = "application/x-www-form-urlencoded";

        //    StreamWriter sw = new StreamWriter(payReq.GetRequestStream());
        //    sw.Write(nvpstring);
        //    sw.Close();

        //    //get Payflow response
        //    HttpWebResponse payResp = (HttpWebResponse)payReq.GetResponse();
        //    StreamReader sr = new StreamReader(payResp.GetResponseStream());
        //    string response = sr.ReadToEnd();
        //    sr.Close();

        //    //parse string into array and return
        //    NameValueCollection dict = new NameValueCollection();
        //    foreach (string nvp in response.Split('&'))
        //    {
        //        string[] keys = nvp.Split('=');
        //        dict.Add(keys[0], keys[1]);
        //    }
        //    return dict;
        //}
        //protected string GetNewId(string Order_id)
        //{
        //    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + Order_id;
        //    var random = new Random();
        //    var result = new string(
        //        Enumerable.Repeat(chars, 16)
        //                  .Select(s => s[random.Next(s.Length)])
        //                  .ToArray());
        //    return "Wagner"+Order_id+"-" + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        //}
        protected string DecryptSP(string ordid)
        {

            try
            {
                string enc = "";
                enc = HttpUtility.UrlDecode(ordid);
                enc = objSecurity.StringDeCrypt(enc, EnDekey);
                enc = objSecurity.StringDeCrypt(enc, EnDekey);
                enc = objSecurity.StringDeCrypt(enc, EnDekey);
                enc = objSecurity.StringDeCrypt(enc, EnDekey);
                enc = objSecurity.StringDeCrypt(enc, EnDekey);
                return enc;
            }
            catch (Exception ex)
            {
            
            return "";
            }
        }

        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistory.aspx");
        }
     
        private void LoadIds()
        {

            string id = "";
            //if (Request["key"] != null)
            //{
            //    id = Request["key"].ToString();
            //    id = DecryptSP(id);
            //}
            if (HttpContext.Current.Session["P_Oid"] != null)
                id = HttpContext.Current.Session["P_Oid"].ToString();

            if (id != null)
                OrderID = Convert.ToInt32(id);
            else
            {                
                div1.InnerHtml = "Invalid Data" + strPaymentLink;
                return;

            }
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;
        }
        private void LoadOrderItem()
        {
            try
            {
                if (OrderID > 0)
                {
                    dsOItem = objOrderServices.GetOrderItems(OrderID);


                    OrderitemdetailRepeater.DataSource = dsOItem;
                    OrderitemdetailRepeater.DataBind();

                    Product_Total_price.Text = oOrderInfo.ProdTotalPrice.ToString();
                    Tax_amount.Text = oOrderInfo.TaxAmount.ToString();
                    Total_Amount.Text = oOrderInfo.TotalAmount.ToString();
                    lblCourier.Text = oOrderInfo.ShipCost.ToString();
                }
            }
            catch (Exception ex)
            { 
            }

        }

        private void GetPaymentDetails()
        {
            tblPaymentInfo = objOrderServices.GetPaymentDetails(OrderID.ToString(), PaymentID.ToString(), Txn_id);
        }
       
        private string Setdrpdownlistvalue(DropDownList d, string val)
        {
            ListItem li;
            string returnselected = "";
            for (int i = 0; i < d.Items.Count; i++)
            {
                li = d.Items[i];
                if (li.Value.ToUpper() == val.ToUpper())
                {
                    d.SelectedIndex = i;
                    returnselected = li.Text.ToUpper();
                    break;
                }
            }
            return returnselected;
        }


        // merges two NVCs
        public static NameValueCollection Merge(NameValueCollection first, NameValueCollection second)
        {
            if (first == null && second == null)
                return null;
            else if (first != null && second == null)
                return new NameValueCollection(first);
            else if (first == null && second != null)
                return new NameValueCollection(second);

            NameValueCollection result = new NameValueCollection(first);
            for (int i = 0; i < second.Count; i++)
                result.Set(second.GetKey(i), second.Get(i));
            return result;
        }
        #region "Function.."
        public string BuildBillAddress()
        {
            try
            {
                string sBillAdd = "";

                if (oOrderInfo.BillAdd1 != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd1) + "<br>";
                }
                if (oOrderInfo.BillAdd2 != "")
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2) + "<br>";
                }
                else
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd2);
                if (oOrderInfo.BillAdd3 != "")
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3) + "<br>";
                }
                else
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillAdd3);
                }
                if (oOrderInfo.BillCity != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCity) + "<br>";
                }
                if (oOrderInfo.BillState != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillState) + "-";
                }
                if (oOrderInfo.BillZip != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillZip) + "<br>";
                }
                if (oOrderInfo.BillCountry != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillCountry) + "<br>";
                }
                if (oOrderInfo.BillPhone != null)
                {
                    sBillAdd = sBillAdd + "Phone No:" + WrapText(oOrderInfo.BillPhone) + "<br>";
                }
                return sBillAdd;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public string BuildShippAddress()
        {
            try
            {
                string sShippAdd = "";

                if (oOrderInfo.ShipFName != null)
                {
                    sShippAdd = WrapText(oOrderInfo.ShipFName) + " ";
                }
                if (oOrderInfo.ShipMName != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipMName) + " ";
                }
                if (oOrderInfo.ShipLName != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipLName) + "<br>";
                }
                if (oOrderInfo.ShipAdd1 != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd1) + "<br>";
                }
                if (oOrderInfo.ShipAdd2 != "")
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2) + "<br>";
                }
                else
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd2);
                if (oOrderInfo.ShipAdd3 != "")
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3) + "<br>";
                }
                else
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipAdd3);
                if (oOrderInfo.ShipCity != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCity) + "<br>";
                }
                if (oOrderInfo.ShipState != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipState) + "-";
                }
                if (oOrderInfo.ShipZip != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipZip) + "<br>";
                }
                if (oOrderInfo.ShipCountry != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipCountry) + "<br>";
                }
                if (oOrderInfo.ShipPhone != null)
                {
                    sShippAdd = sShippAdd + "Phone No:" + WrapText(oOrderInfo.ShipPhone) + "<br>";
                }
                return sShippAdd;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public string WrapText(string BillAdd)
        {
            string newline = " \n ";
            if (BillAdd.Length > 50 & BillAdd.Length <= 100)
                BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51) + newline;
            else if (BillAdd.Length > 100 & BillAdd.Length <= 150)
                BillAdd = BillAdd.Substring(0, 50) + newline + BillAdd.Substring(51, 49) + newline + BillAdd.Substring(101);
            return BillAdd;
        }

        #endregion
    }
