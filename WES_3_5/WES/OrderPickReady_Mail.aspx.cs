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
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Net.Mime;
using System.Net.Mail;

namespace WES
{
    public partial class OrderPickReady_Mail : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        // OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        //  UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        PayOnlineService objPayOnlineService = new PayOnlineService();

        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        // string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();
        PayPalService objPayPalService = new PayPalService();
        //string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        // string strPaymentLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";

        //  string strBackLink = "";
        string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
        //string key = "";
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

       
          

            try
            {
                int rtnmail = 0;
              //  SendInvoiceSignal("328562|375272");
                DataTable temptbl = objOrderServices.GetOrder_PICKUP_ORDER();
                if (temptbl != null && temptbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in temptbl.Rows)//For Records
                    {
                        try
                        {
                            lblorderid.Text = lblorderid.Text + "---" + dr["ORDER_ID"].ToString();
                           // objErrorHandler.CreateLog("before OrderPickReady mail");
                            rtnmail = SendMail(Convert.ToInt32(dr["ORDER_ID"].ToString()), (int)OrderServices.OrderStatus.Order_Ready_For_Pick_Up, true);
                           // objErrorHandler.CreateLog("after OrderPickReady mail");
                            if (rtnmail == 1)
                            {
                                int flgsentmail = 1;

                           
                                objOrderServices.UpdateMake_Payment_mail_status(Convert.ToInt32(dr["ORDER_ID"].ToString()), flgsentmail);
                            }
                         
                        }
                        catch (Exception er)
                        {
                            objErrorHandler.ErrorMsg = er;
                            objErrorHandler.CreateLog();
                        }
                    }
                }





            }
            catch (Exception ex)
            {



            }


        }
        //human-readable representation of an NVC



        public  string SendInvoiceSignal(string strvalue)
        {
            try
            {

                string DecryptedValueString = strvalue;
                //objErrorHandler.CreateLog("SendInvoiceSignal" + strvalue);
                string invno = "-1";
                OrderServices objOrderServices = new OrderServices();
                HelperServices objHelperServices = new HelperServices();
                if (!string.IsNullOrEmpty(DecryptedValueString))
                {
                    string[] PdfFileName = DecryptedValueString.Split('|');
                    string pdffile = HttpContext.Current.Server.MapPath(string.Format("~/Invoices/In{0}", PdfFileName[1] + ".pdf"));
                    invno = PdfFileName[1].ToString();
                    HttpContext.Current.Session["PdfFileName"] = PdfFileName[0].ToString();
                    HttpContext.Current.Session["pdffile"] = pdffile;
                    HttpContext.Current.Session["invno"] = PdfFileName[0].ToString();


                    string INVOICE_NO_OF_TIME_TRY = System.Configuration.ConfigurationManager.AppSettings["INVOICE_NO_OF_TIME_TRY"].ToString();
                    string INVOICE_WAIT_TIME = System.Configuration.ConfigurationManager.AppSettings["INVOICE_WAIT_TIME"].ToString();
                //    objErrorHandler.CreateLog("B4 File Exist SendInvoiceSignal" + pdffile);
                    //System.Threading.Thread.Sleep(100000);
                    if (System.IO.File.Exists(pdffile))
                    {
                        return "inv" + invno;
                    }
                    else
                    {
                        if (PdfFileName[0] != null && PdfFileName[0].ToString().Trim() != string.Empty)
                        {
                            int cStatus = 0;
                            int invTry = objHelperServices.CI(INVOICE_NO_OF_TIME_TRY);
                            int invWaitTime = objHelperServices.CI(INVOICE_WAIT_TIME);
                            string cOrderNo = PdfFileName[0].ToString();
                            //cStatus = objOrderServices.SentSignalInvoiceNotification(cOrderNo);
                            if (objOrderServices.GetOrderStatus(objHelperServices.CI(cOrderNo)).ToLower() == (Enum.GetName(typeof(OrderServices.OrderStatus), OrderServices.OrderStatus.Proforma_Payment_Required)).ToLower())
                            {
                              //  objErrorHandler.CreateLog("B4 SentSignal to db" + cOrderNo+ "201");
                                cStatus = objOrderServices.SentSignal("0", cOrderNo, "201");
                            }
                            else
                            {
                               // objErrorHandler.CreateLog("B4 SentSignal to db" + cOrderNo + "200");
                                cStatus = objOrderServices.SentSignal("0", cOrderNo, "200");
                            }


                            if (cStatus >= 0)
                            {

                                for (int i = 0; i < invTry; i++)
                                {
                                    objErrorHandler.CreateLog("invTry:" + i + HttpContext.Current.Session["pdffile"].ToString());
                                    if (System.IO.File.Exists(HttpContext.Current.Session["pdffile"].ToString()))
                                    {



                                        return "inv" + invno;
                                    }
                                    System.Threading.Thread.Sleep(invWaitTime);
                                }
                            }
                            else
                            {
                                HttpContext.Current.Session["pdffile"] = "";
                                HttpContext.Current.Session["PdfFileName"] = "";

                                return invno;
                            }

                        }
                    }
                }
                else
                {
                    HttpContext.Current.Session["pdffile"] = "";
                    HttpContext.Current.Session["PdfFileName"] = "";

                    return invno;
                }
                HttpContext.Current.Session["pdffile"] = "";
                HttpContext.Current.Session["PdfFileName"] = "";
                return invno;
            }
            catch(Exception ex)
            {
                return "";
            }

        }

        private int SendMail(int OrderId, int OrderStatus, bool isau)
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
                // objErrorHandler.CreateLog(oOrderInfo.CreatedUser.ToString()+"Createduser");
                // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                //   objErrorHandler.CreateLog(oOrderInfo.UserID.ToString() + "UserID");
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
               // dsOItem = objOrderServices.GetOrderItems(OrderId);
                //    objErrorHandler.CreateLog(OrderId.ToString() + "OrderId");
                //   objErrorHandler.CreateLog(dsOItem.Tables[0].Rows.Count.ToString() );
             

                //string ShippingMethod = oOrderInfo.ShipMethod;
                //string CustomerOrderNo = oPayInfo.PORelease;
                //string shippingnotes = oOrderInfo.ShipNotes;




                //  oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                //string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                //string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

                string SubmittedBy = "";
                //switch (oOrderInfo.OrderStatus)
                //{
                //    case 6:
                //        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                //        break;
                //    case 12:
                //        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                //        break;
                //    default:
                //        SubmittedBy = oUserInfo.Contact + "&nbsp;&nbsp;" + String.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.ModifiedDate);
                //        break;
                //}

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
               
                    _stg_container = new StringTemplateGroup("main", stemplatepath);

                   
                    
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderReady");


                 
                    _stmpl_container.SetAttribute("Invoicedate", oOrderInfo.InvoiceDate.ToString("dd-MM-yyyy") );
                    _stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                 
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    if ((oOrderInfo.ShipPhone != "")&& (oOrderInfo.ShipMethod=="Counter Pickup"))

                    {
                        if (oOrderInfo.ShipPhone.StartsWith("04") && oOrderInfo.ShipPhone.Length==10)
                        {
                            string output = SendSMS(oOrderInfo.InvoiceNo, oOrderInfo.ShipPhone);
                         //   objErrorHandler.CreateLog(output);
                        }
                    }
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
                    string url1 = HttpContext.Current.Request.Url.Authority.ToString();

                 
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());




                    //string emails = "";
                    //string Adminemails = "";
                    string webadminmail = "";
                    webadminmail = objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    //objErrorHandler.CreateLog("Email Id Make Payment Send Mail" + Emailadd.ToString() + "inside if");
                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {

                        MessageObj.To.Add(Emailadd.ToString());
                        //   MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());
                    }
                    else
                    {


                        MessageObj.To.Add(Emailadd.ToString());

                        // MessageObj.Bcc.Add(objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString());
                    }
                    MessageObj.Bcc.Add("nathan@wes.net.au");
                    //MessageObj.Bcc.Add("indumathi@jtechindia.com");
                    
                    //objHelperServices.GetOptionValues("WEB ADMIN EMAIL").ToString();
                    // MessageObj.Bcc.Add(webadminmail);

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "WES - Order Ready For Pick Up - Invoice#" + oOrderInfo.InvoiceNo;
                    SendInvoiceSignal(oOrderInfo.OrderID.ToString().Trim() + "|" + oOrderInfo.InvoiceNo.ToString().Trim());

                    try
                    {
                        objErrorHandler.CreateLog("b4 attachment" + HttpContext.Current.Server.MapPath("Invoices\\In" + oOrderInfo.InvoiceNo + ".pdf"));
                        System.Net.Mail.Attachment attachment = new Attachment(HttpContext.Current.Server.MapPath("Invoices\\In" + oOrderInfo.InvoiceNo + ".pdf"), MediaTypeNames.Application.Pdf);
                        MessageObj.Attachments.Add(attachment);
                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString());   
                    }
                   

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
                    objHelperServices.Mail_Log("OP", OrderId, Emailadd,"WES - Order Ready For Pick Up");
                  
                    return 1;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
            return -1;
        }

        protected string DecryptSP(string ordid)
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




        private string SendSMS(string InvoiceNo,string MobileNo)
        {

            try
            {
                string message = "Order Ready for Pick Up!\n";

                message = message + "Invoice No: " + InvoiceNo + "\n";

                message = message + "\n";

                message = message + "Important\n";


                message = message += "When coming into store please bring ID and show this message\n";

                message = message += "\n";

                message = message += "Store Pick Up Location\n";

                message = message += "WES Components\n";

                message = message += "84-90 Parramatta Road,\n";

                message = message += "Summer Hill, NSW 2130\n";

                message = message += "Basement parking available.\n";

                message = message += "Map: https://goo.gl/maps/rVvQC7LeRRt";
                message = message += "\n";
                message = message += "\n";
                message = message += "Business Hours\n";

                message = message += "8.30AM - 5PM Monday to Friday\n";

                message = message += "9.00AM - 4PM Saturday\n";

                message = message += "Excluding Public Holidays\n";

                message = message += "\n";

                message = message += "Regards\n";

                message = message += "Wes Team";

                //            "method":"sms.send_sms",

                //"params":{

                //"login":"web",

                //"pass":"w35@SMS@w3b",

                //"modem_no":"1",

                //"to":"0402609818,0414232633",

                string baseUrl = string.Empty;
                //if ((MobileNo != "")&& (MobileNo!=null))
                //{
                //    if (MobileNo.Substring(0, 2) != "04" || MobileNo.Length != 10)
                //    {
                        baseUrl = "http://10.10.10.32/index.php/http_api/send_sms?login=web&pass=w35@SMS@w3b&to=0414232633," + MobileNo + "&message=" + HttpUtility.UrlEncode(message) + "";
                //    }
                //    else
                //    {
                //        baseUrl = "http://10.10.10.32/index.php/http_api/send_sms?login=web&pass=w35@SMS@w3b&to=0414232633&message=" + HttpUtility.UrlEncode(message) + "";

                //    }
                //}
                //else
                //{
                //    baseUrl = "http://10.10.10.32/index.php/http_api/send_sms?login=web&pass=w35@SMS@w3b&to=0414232633&message=" + HttpUtility.UrlEncode(message) + "";
                //}
                WebClient client = new WebClient();
               // objErrorHandler.CreateLog(message);
                //  client.QueryString.Add("method", "send_sms");
                //    client.QueryString.Add("login", "web");
                //    client.QueryString.Add("pass", "w35@SMS@w3b");
                //client.QueryString.Add("modem_no", "1");
                //client.QueryString.Add("to", "0414232633");

                //    client.QueryString.Add("message", HttpUtility.UrlEncode(message));
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/16.0");
               // objErrorHandler.CreateLog(baseUrl);
                Stream receivedStream = client.OpenRead(baseUrl);

                StreamReader reader = new StreamReader(receivedStream);
                string result = reader.ReadToEnd();
                receivedStream.Close();
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return ex.ToString();
            }
        }

        // merges two NVCs

        #region "Function.."

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
}
