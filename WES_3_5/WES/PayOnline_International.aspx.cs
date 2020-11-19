using System;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using Antlr3.ST;
using Braintree;

namespace WES
{
    public partial class PayOnline_International : System.Web.UI.Page
    {
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();
        NotificationServices objNotificationServices = new NotificationServices();
        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        PayPalService objPayPalService = new PayPalService();

        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        public string ClientToken = string.Empty;

        public int UserID = 0;

        string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();

        string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strPaymentLink = "<br/><a href=\"PaymentHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strBackLink = "";
        string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
        string Userid;
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    string sb;
        //    sb = "<script language=javascript>\n";
        //    sb += "window.history.forward(1);\n";
        //    sb += "\n</script>";
        //    ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


        //    sb = "" ;
        //    sb = "<script type=javascript>\n";
        //    sb += "window.onload = function () { Clear(); }\n";
        //    sb += "function Clear() { \n";
        //    sb += " var Backlen=history.length; \n";
        //    sb += " if (Backlen > 0) history.go(-Backlen); \n";
        //    sb += "\n}</script>";
        //    ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);
        //}
        protected void GenerateClientToken()
        {
            try
            {
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantId = "mjff7p7mgb4qmp77",
                    PublicKey = "h673fc8hc4v7pqh4",

                    PrivateKey = "92c877d009ac2dc337a38fd5737301e3",

                };

                //var gateway = new BraintreeGateway
                //{
                //    Environment = Braintree.Environment.PRODUCTION,
                //    MerchantId = "wrv3fq8x3r269ycd",
                //    PublicKey = "nm7v4wm8dmw7b6rq",
                //    PrivateKey = "a3d333f589d80552db255c34c1407c40"
                //};



                this.ClientToken = gateway.ClientToken.Generate();


               // objErrorHandler.CreateLog("clientToken paysp:" + this.ClientToken);
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            GenerateClientToken();
            if (RBPaypal.Checked == true)
            {
                creditflag2.Style.Add("display", "none");
                lblcreditcard.Visible = false;
            }
            //Response.Buffer = true;
            ////Response.ExpiresAbsolute = DateTime.Now;
            //Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);


            //Response.Expires = 0;
            //Response.AddHeader("Expires", "-1");
            //Response.AddHeader("pragma", "no-cache");
            //Response.AddHeader("cache-control", "private");
            //Response.CacheControl = "no-cache";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();


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
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                {
                    UserID = Convert.ToInt32(Session["USER_ID"].ToString());
                }
                else
                {
                    // Response.Redirect("login.aspx");
                    divTimeout.Visible = true;
                    divCC.Visible = false;
                    return;
                }
                int valid = GetParams_orderid(UserID.ToString());
                if (valid == 0)
                {
                    divonlinesubmitordererror.Visible = true;

                    this.Title = "WES Alert";
                    divmaincontent.Visible = false;
                    return;
                }
                else
                {
                    divonlinesubmitordererror.Visible = false;

                }
                LoadIds();
                //GetPaymentTerm(UserID.ToString());
                if (!IsPostBack)
                {
                    drpExpyear.Items.Clear();
                    for (int y = DateTime.Now.Year; y <= DateTime.Now.Year + 20; y++)
                    {
                        drpExpyear.Items.Add(y.ToString());
                    }
                }
                oUserInfo = objUserServices.GetUserInfo(UserID);

                //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower()=="au" )
                //{
                //    div1.InnerHtml = "";
                //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                //    return;
                //}

                if (oPayInfo.PayResponse == "yes")
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                    return;
                }
                if (IsPostBack)
                {

                    // btnPay.Style.Add("display", "none");
                    // ImgBtnEditShipping.Style.Add("display", "none");
                    // BtnProgress.Style.Add("display", "block");

                    return;
                }
                int tmpOrdStatus = (int)OrderServices.OrderStatus.Proforma_Payment_Required;

                string orderdstatus = objOrderServices.GetOrderStatus(OrderID);

                if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
                {

                    Session["ORDER_ID"] = 0;
                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    return;
                }
                
                if (!IsPostBack)
                {
                    string BillAdd;
                    string ShippAdd;
                   
                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    lblShippingMethod.Text = oOrderInfo.ShipMethod;
                    lbltotalamt.Text = oOrderInfo.TotalAmount.ToString();
                    lblbttotalamount.Text = oOrderInfo.TotalAmount.ToString();
                    lblorderid.Text = OrderID.ToString();
                    BillAdd = BuildBillAddress();
                    ShippAdd = BuildShippAddress();
                    lblDeliveryTo.Text = BillAdd;
                    lblShipTo.Text = ShippAdd;
                    lblOrderNo.Text = "WES" + OrderID;
                    HttpContext.Current.Session["Pay_ORDER_ID"] = OrderID;
                    Session["Pay_User_id"] = oOrderInfo.UserID;
                    if (oOrderInfo.Payment_Selection == "BT")
                    {
                        divdirectdeposit.Style.Add("display", "block");
                        //divdirectdeposit.d = true;
                        lbldefaultpayment.Visible = true;
                        divpaypal.Style.Add("display", "none");
                        divpayonacccontent.Visible = true;
                        lblpaypalcard.Visible = false;
                        RBdefautpaymenttype.Checked = true;
                        RBdefautpaymenttype.Enabled = false;
                        lblcreditcard.Visible = false;
                        divcreditcard.Style.Add("display", "none");
                        creditflag2.Style.Add("display", "none");
                        //try
                        //{
                        //    ProceedFunction();
                        //}
                        //catch
                        //{ }
                    }
                    //else if (oOrderInfo.Payment_Selection == "PP")
                    //else if (oOrderInfo.ShipCountry.ToLower() != "new zealand")
                    //{
                    //    RBPaypal.Checked = true;
                    //    divpaypal.Style.Add("display", "block");
                    //    divpayonacccontent.Visible = false;
                    //    lblpaypalcard.Visible = true;
                    //    divdirectdeposit.Visible = false;
                    //    lbldefaultpayment.Visible = false;
                    //    RBPaypal.Enabled = false;
                    //    lblcreditcard.Visible = false;
                    //    divcreditcard.Style.Add("display", "none");
                    //    divdedault.Style.Add("display", "none");
                    //    creditflag2.Style.Add("display", "none"); 
                    //}
                    //else if (oOrderInfo.ShipCountry.ToLower() == "new zealand")
                    //{
                    //    RBCreditCard.Checked = true;
                    //    divpaypal.Style.Add("display", "none");
                    //    divpayonacccontent.Visible = false;
                    //    divdedault.Style.Add("display", "none");
                    //    lblpaypalcard.Visible = false;
                    //    divdirectdeposit.Visible = false;
                    //    lbldefaultpayment.Visible = false;
                    //    RBPaypal.Enabled = false;
                    //    lblcreditcard.Visible = true;
                        
                    //    //if (System.Configuration.ConfigurationManager.AppSettings["creditflag"].ToString() == "1")
                    //    //{
                    //    //    divcreditcard.Style.Add("display", "block");
                    //    //    creditflag1.Visible = true;
                    //    //    creditflag2.Visible = false;
                    //    //    creditflag3.Visible = false;
                    //    //    imgsecurepay.Src = "../images/cards_sm.png";
                    //    //}
                    //    //else if (System.Configuration.ConfigurationManager.AppSettings["creditflag"].ToString() == "2")
                    //    //{
                    //    //    creditflag1.Visible = false;
                    //    //    creditflag2.Visible = true;
                    //    //    creditflag3.Visible = false;
                    //    //    imgsecurepay.Src = "../images/cards_sm.png";
                    //    //    GenerateClientToken();
                    //    //}
                    //    //else
                    //    //{
                    //    //    creditflag1.Visible = false;
                    //    //    creditflag2.Visible = false;
                    //    //    creditflag3.Visible = true;

                    //    //}

                    //    string userid = string.Empty;
                    //    string userid1 ="";
                    //    if (Session["Pay_User_id"]!=null)
                    //    {
                    //        userid1 = Session["Pay_User_id"].ToString();
                    //    }
                        

                    //        userid =Session["User_id"].ToString();
                       
                    //    //int c = objOrderServices.checkcardnoexsist(userid,userid1, "","");
                    //    //objErrorHandler.CreateLog(c + "checkcardnoexsist");

                    //    //if (c == 1)
                    //    //{
                    //    //    creditflag1.Style.Add("display", "block");
                    //    //    creditflag2.Style.Add("display", "none");
                    //    //    creditflag3.Visible = false;
                    //    //    imgsecurepay.Src = "../images/cards_sm.png";
                    //    //}
                    //    //else
                    //    //{
                    //        //creditflag1.Visible = false;
                    //        //creditflag2.Visible = true;
                    //        creditflag1.Style.Add("display", "none");
                    //        creditflag2.Style.Add("display", "block");
                    //        creditflag3.Visible = false;
                    //        imgsecurepay.Src = "../images/cards_sm.png";
                    //   // }


                    //}
                    else
                    {
                        //divdedault.Style.Add("display", "block");
                        //lbldefaultpayment.Visible = true;
                        //divpaypal.Visible = true;
                        //divpayonacccontent.Visible = true;
                        //lblpaypalcard.Visible = true;
                        //RBdefautpaymenttype.Checked = true;
                        //RBdefautpaymenttype.Enabled = true;
                        //lblcreditcard.Visible = true;
                        //divcreditcard.Style.Add("display", "none");
                        //divpaypal.Style.Add("display", "none");

                       // RBPaypal.Checked = true;
                        divpaypal.Style.Add("display", "none");
                        RBCreditCard.Checked = true;
                        divpayonacccontent.Visible = false;
                        lblpaypalcard.Visible = true;
                        divdirectdeposit.Visible = false;
                        lbldefaultpayment.Visible = false;
                        RBPaypal.Enabled = true;
                        lblcreditcard.Visible = true; 
                        divcreditcard.Style.Add("display", "none");
                        divdedault.Style.Add("display", "none");
                        creditflag2.Style.Add("display", "block");
                        imgsecurepay.Src = "../images/cards_sm.png";
                    }
                    //lblOrderNo.Text = " : " + oPayInfo.PORelease;
                    LoadOrderItem();
                    //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                    //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) + "&";
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
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
        //public void GetPaymentTerm(string userid)
        //{
        //    int i = 0;
        //    DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(userid));
        //    if (ds != null)
        //    {
        //        i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

        //    }
        //    divdirectdeposit.Style.Add("display", "block");

        //    //Cash
        //    if (i == 1 || i == 3 || i == 8)
        //    {
        //        RBdefautpaymenttype.Text = "Direct Deposit";
        //        RBdefautpaymenttype.Checked = true;
        //        divpayoaccount.Style.Add("display", "none");
        //        divdirectdeposit.Style.Add("display", "block");
        //        divmastercard.Style.Add("display", "none");
        //        divD1.Visible = true;
        //        divD2.Visible = false;
        //        divD3.Visible = false;
        //    }
        //    //30 days
        //    else if (i == 2 || i == 6)
        //    {
        //        RBdefautpaymenttype.Text = "Pay on Account";
        //        RBdefautpaymenttype.Checked = true;
        //        divpayoaccount.Style.Add("display", "block");
        //        divdirectdeposit.Style.Add("display", "none");
        //        divmastercard.Style.Add("display", "none");
        //        divD1.Visible = false;
        //        divD2.Visible = true;
        //        divD3.Visible = false;
        //    }
        //    //Credit Card
        //    else if (i == 4)
        //    {
        //        RBdefautpaymenttype.Text = "Master Card";
        //        RBdefautpaymenttype.Checked = true;
        //        divmastercard.Style.Add("display", "block");
        //        divdirectdeposit.Style.Add("display", "none");
        //        divpayoaccount.Style.Add("display", "none");
        //        lblmastercardno.Text = ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().Substring(0, 4) + " " + "xxxx" + " " + "xxxx" + " " + "xxxx";
        //        lblmasterexpirydate.Text = "Exp:" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(0, 2) + "/" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(1, 2);
        //        divD1.Visible = false;
        //        divD2.Visible = false;
        //        divD3.Visible = true;
        //    }
        //    //Cash on pickup
        //    else if (i == 7)
        //    {
        //        RBdefautpaymenttype.Text = "Bank Deposit";
        //        RBdefautpaymenttype.Checked = true;
        //        divD1.Visible = false;
        //        divD2.Visible = true;
        //        divD3.Visible = false;
        //    }
        //    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
        //    oOrderInfo = objOrderServices.GetOrder(OrderID);
        //    lbltotalamt.Text = oOrderInfo.TotalAmount.ToString();
        //}
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
        protected void btndeposit_Click(object sender, EventArgs e)
        {

        }
        protected void ProceedFunction()
        {
            try
            {
               // btndirectdeposit.Visible = false;
                //color.BgColor = "FFFFFF";
                //  color1.BgColor = "FFFFFF";
             // colo2.BgColor = "FFFFFF";
                bool isau = false;
                // color5.BgColor = "FFFFFF";
                QuoteServices objQuoteServices = new QuoteServices();
                //int OrdStatusID = (int)OrderServices.OrderStatus.ORDERPLACED;
                //if (objOrderServices.GetOrderStatus(OrderID) != "ORDERPLACED")
                if (objOrderServices.GetOrderStatus(OrderID) != "")
                {
                    int OrdStatusVerify = (int)OrderServices.OrderStatus.MANUALPROCESS;
                    DataSet oDs = new DataSet();
                    oDs = objOrderServices.GetOrderItems(OrderID);
                    int ChkOrderExist = 0;

                    int UptOrderStatus = -1;
                    //int OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                    int OrdStatus = 0;
                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                    {
                        isau = false;
                        //OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                        OrdStatus = (int)OrderServices.OrderStatus.Proforma_Payment_Success;
                    }
                    else  if (ispickuponly_zone(OrderID) == false)
                    {

                        OrdStatus = (int)OrderServices.OrderStatus.Proforma_Payment_Success;
                    }
                    else
                    {
                        isau = true;
                        switch (Convert.ToInt16(Session["USER_ROLE"]))
                        {
                            case 1:
                                //OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Proforma_Payment_Success;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }

                                break;
                            case 2:

                                //OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Proforma_Payment_Success;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                                break;
                            case 3:
                                OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                                break;
                        }
                    }

                   


                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    if (oPayInfo.OrderID == OrderID && (oPayInfo.PaymentType == PaymentServices.PaymentType.CCPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CCPaymentDeclined || oPayInfo.PaymentType == PaymentServices.PaymentType.CHEPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CODPayment))
                    {
                        ChkOrderExist = 1;
                    }
                    //if (Session["PAYMENTINFO"] != null || Session["PAYMENTINFO"].ToString() != null)
                    {
                        Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
                        decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                        oPayInfo.PayResponse = "";
                        oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                        oPayInfo.OrderID = OrderID;
                        oPayInfo.PONumber = objHelperServices.Prepare("");
                      //  oPayInfo.PORelease = refid;
                        oPayInfo.Amount = TotCost;
                        oPayInfo.UserId = OrderID;

                    }
                    if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 1)
                    {
                        if (ChkOrderExist == 0)
                        {
                            ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            if (isau == false)
                            {
                                cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                            }
                            else
                            {
                                cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                            }

                        }
                        else if (ChkOrderExist == 1)
                        {
                            ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);

                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            if (isau == false)
                                cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                            else
                                cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                        }

                        if (UptOrderStatus != -1)
                        {
                            int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                            objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                            //SendNotification(OrderID);
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "BT");
                            SendMail(OrderID, OrdStatus);
                            if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                            {
                                Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                            }
                            else
                            {
                                //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                            }
                            //greenalert.Visible = true;
                          //  divpaymentoption.Visible = false;
                        }

                    }
                    else if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 4)
                    {
                        if (Session["PAYMENTINFO"] != null)
                        {
                            oPayInfo = (PaymentServices.PayInfo)Session["PAYMENTINFO"];
                        }
                        if (ChkOrderExist == 0)
                        {
                            ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatusVerify);
                        }
                        else if (ChkOrderExist == 1)
                        {
                            ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatusVerify);
                        }
                        if (UptOrderStatus != -1)
                        {
                            int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                            objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                            if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                            {
                                Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                            }
                            else
                            {
                                //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
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
        private void SendMail(int OrderId, int OrderStatus)
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

                int UserID = objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderID);
                BillAdd = GetBillingAddress(OrderID);
                ShippAdd = GetShippingAddress(OrderID);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;




                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


                string url = HttpContext.Current.Request.Url.Authority.ToString();
                string PendingorderURL = string.Format("http://" + url + "/PendingOrder.aspx");

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

                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "Row_BankTrasfer");
                        _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                        _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        if (oOrderInfo.Payment_Selection != "BT")
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                        }
                        else
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_BankTrasfer");
                        }
                    }
                    else
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");

                    _stmpl_container.SetAttribute("OrderTotalAmount", oOrderInfo.TotalAmount);
                    _stmpl_container.SetAttribute("OrderDate", Createdon);
                    _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                    _stmpl_container.SetAttribute("PayOrderNo", oPayInfo.OrderID);
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
                    _stmpl_container.SetAttribute("CreatedBy", Createdby);
                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                        _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                    else
                        _stmpl_container.SetAttribute("SubmittedBy", "");



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

                    string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
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
                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());

                        Adminemails = Get_ADMIN_UserEmils();
                        //if (ApprovedUserEmailadd.Trim() != "" && Emailadd.ToString() != ApprovedUserEmailadd.ToString())
                        //   MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                    }
                    else
                    {
                        emails = Get_ADMIN_APPROVED_UserEmils();

                        MessageObj.To.Add(Emailadd.ToString());


                    }

                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        MessageObj.Subject = "WES Australasia Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                    }
                    else
                    {
                        MessageObj.Subject = "WES Australasia Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;


                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);




                    if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
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
        private string Get_ADMIN_APPROVED_UserEmils()
        {
            DataSet oDs = new DataSet();
            string emails = "";

            string userid = Session["USER_ID"].ToString();
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
        private string Get_ADMIN_UserEmils()
        {
            DataSet oDs = new DataSet();
            string emails = "";

            string userid = Session["USER_ID"].ToString();
            if (userid == "")
                userid = "0";


            try
            {

                oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
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

           // objErrorHandler.CreateLog(sBillingAddress);

          sBillingAddress = sBillingAddress + oBI.BillFName + oBI.BillLName + "<BR>";
         // objErrorHandler.CreateLog(sBillingAddress);
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
        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistory.aspx");
        }
        protected void btnSecurePayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("paysp.aspx?" + EncryptSP(OrderID.ToString()), false);
        }

        protected void btnPayPalPayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
        }
        protected void ImgBtnEditShipping_Click(object sender, EventArgs e)
        {
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
            {
                if (OrderID > 0)
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + OrderID.ToString(), false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
                }
            }

            //   Response.Redirect("Shipping.aspx?shipping.aspx?OrderID="+ OrderID +"&ApproveOrder=Approve", false);           
        }

        //protected void btnPay_Click(object sender, EventArgs e)
        //{


        //    LoadIds();
        //    renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
        //    renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString());

        //    oOrderInfo = objOrderServices.GetOrder(OrderID);
        //    oUserInfo = objUserServices.GetUserInfo(UserID);

        //    //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
        //    //{
        //    //    div1.InnerHtml = "";
        //    //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
        //    //    return;
        //    //}
        //    if (oPayInfo.PayResponse.ToLower() == "yes")
        //    {
        //        divContent.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
        //        return;
        //    }

        //    string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

        //    if (Requeststr.Contains("Form") == false)
        //        divContent.InnerHtml = Requeststr;
        //    else
        //        this.Page.Controls.Add(new LiteralControl(Requeststr));

        //}
        private int GetParams_orderid(string orderid)
        {
            try
            {
                if (Request.Url.Query != null && Request.Url.Query != "")
                {
                    string Userid = Session["USER_ID"].ToString();
                    string id;
                    if (Request.Url.Query.Contains("key=") == false)
                    {

                        id = Request.Url.Query.Replace("?", "");
                        id = id.Replace("#####" + "PaySP", "");
                        int n;
                        bool isNumeric = int.TryParse(id, out n);

                        if (isNumeric == true)
                        {
                            OrderDB objOrderDB = new OrderDB();
                            //  objErrorHandler.CreateLog("before userid");
                            string orderuserid = (string)objOrderDB.GetGenericDataDB("", id.ToString(), "GETUSERID_ORDER", OrderDB.ReturnType.RTString);
                            //  objErrorHandler.CreateLog("orderuserid--" + orderuserid);
                            //  objErrorHandler.CreateLog("Userid--" + Userid.ToString());
                        
                            if (orderuserid != Userid.ToString()) 
                            {

                                return 0;
                            }
                        }
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return 1;

            }

        }


        private void LoadIds()
        {
            try
            {
                if (Request.Url.Query != null && Request.Url.Query != "")
                {

                    string id = Request.Url.Query.Replace("?", "");
                    //id = DecryptSP(id);
                    //string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);


                    OrderID = objHelperServices.CI(id);

                    if (id.Length > 1)
                    {
                        Txn_id = id;
                        strBackLink = strPaymentLink;
                    }
                    else
                        strBackLink = strOrderLink;


                }
                else
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Invalid Data" + strPaymentLink;
                    return;

                }
                oPayInfo = objPaymentServices.GetPayment(OrderID);
                PaymentID = oPayInfo.PaymentID;

                // lblTotalAmount1.Text = oPayInfo.Amount.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

        }
        private void LoadOrderItem()
        {
            try
            {
                dsOItem = objOrderServices.GetOrderItems(OrderID);

                OrderitemdetailRepeater.DataSource = dsOItem;
                OrderitemdetailRepeater.DataBind();

                Product_Total_price.Text = oOrderInfo.ProdTotalPrice.ToString();
                Tax_amount.Text = oOrderInfo.TaxAmount.ToString();
                Total_Amount.Text = oOrderInfo.TotalAmount.ToString();
                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                    lblTotalCap.Text = "Total";
                }
                else
                {
                    lblTotalCap.Text = "Total Inc GST";
                }
                lblCourier.Text = oOrderInfo.ShipCost.ToString();

                //lblSubTotal1.Text = oOrderInfo.ProdTotalPrice.ToString();
                // lblTaxAmount1.Text = oOrderInfo.TaxAmount.ToString();
                // lblCourierAmt1.Text = oOrderInfo.ShipCost.ToString();
                // lblTotalAmount1.Text = oOrderInfo.TotalAmount.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

        }
        protected void btnSecurePay_Click(object sender, EventArgs e)
        {
            string rtnstr = "";
            //try
            //{
            //    txtCardNumber.Style.Remove("border");
            //    drpExpmonth.Style.Remove("border");
            //    drpExpyear.Style.Remove("border");
            //    txtCardCVVNumber.Style.Remove("border");
            //    //drppaymentmethod.Style.Remove("border");
            //}
            //catch
            //{
            //}
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["Pay_ORDER_ID"] != null)
            {
                try
                {
                    int x = txtcreditcardno.Text.Length;
                    //string cardno1 = txtCardCVVNumber.Text.Substring(0, 6) + '* *****' + 1007;
                    //    400000 * *****1000
                    string userid = string.Empty;
                    string userid1 = "";
                    if (Session["Pay_User_id"] != null)
                    {
                        userid1 = Session["Pay_User_id"].ToString();
                    }
                   

                        userid = Session["USER_ID"].ToString();
                    


                    userid = Session["Pay_User_id"].ToString();
                   
                    int c = objOrderServices.checkcardnoexsist(userid,userid1, txtcreditcardno.Text.Substring(x - 4, 4), txtcreditcardno.Text.Substring(0, 3));
                   
                    objErrorHandler.CreateLog(c + "securepayclick");
                    if (c == 0)
                    {

                        divgobr.InnerHtml = "Please Re-enter Card Details and Try Again";
                        divgobr.Style.Add("display", "block");

                        creditflag1.Style.Add("display", "none");
                        creditflag2.Style.Add("display", "block");
                        imgsecurepay.Src = "../images/cards_sm.png";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }
            }
            SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

            try
            {

                //       GetParams();


                //       if (Session["XpayMS"] != null)
                //       {
                //           if (Convert.ToInt32(Session["XpayMS"]) > 3)
                //           {
                //               // div1.InnerHtml = "";
                //               div2.InnerHtml = "More than 3 attempt. try again" + "<br/>" + "<a href=\"checkout.aspx?" + EncryptSP(OrderID.ToString() + "#####" + "Pay") + "\" class=\"btn-lg green_bg border_none border_radius_none white_color semi_bold font_14 mar_right_30 mar_left_20 margin_top_20 \" >Pay Now</a>";
                //               div2.Visible = true;
                //               return;
                //           }
                //           else
                //               Session["XpayMS"] = Convert.ToInt32(Session["XpayMS"]) + 1;
                //       }
                //       else
                //           Session["XpayMS"] = 0;

                //       oPayInfo = objPaymentServices.GetPayment(OrderID);
                //PaymentID = oPayInfo.PaymentID;

                //       if (PaymentID == 0)
                //           Response.Redirect("orderDetails.aspx?bulkorder=1&Pid=0&ORDER_ID=" + OrderID);

                //       if (oPayInfo.PayResponse.ToLower() == "yes")
                //       {
                //           divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                //           return;
                //       }

                //objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, drppaymentmethod.SelectedValue, txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                // order_submit_process();
                if (!IsPostBack)
                {
                    drpExpyear.Items.Clear();
                    for (int y = DateTime.Now.Year; y <= DateTime.Now.Year + 20; y++)
                    {
                        drpExpyear.Items.Add(y.ToString());
                    }
                }
                SecurePayService objSecurePayService = new SecurePayService();
                objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtnamecard.Text, txtcreditcardno.Text, txtCVV.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text, HttpContext.Current.Session["USER_ID"].ToString());

                if (objPRInfo.Error_Text != "")
                {
                    btnSP.Style.Add("display", "block");
                    BtnProgressSP.Style.Add("display", "none");
                    // ImgBtnEditShipping.Style.Add("display", "block");

                    div2.InnerHtml = "Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                    div2.Visible = true;
                    //divcreditcard.Style.Add("display","block");
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:creditcardclick();", true);
                    // greenalert.Visible = false;
                    //   Page.RegisterStartupScript("page", "<script language="'javascript'">creditcardclick()</script>");
                    //   ImageButton2.Visible = false;



                    HttpContext.Current.Session["payflowresponse"] = "FAIL";

                    if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
                        txtcreditcardno.Style.Add("border", "1px solid #FF0000");

                    if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
                        txtCVV.Style.Add("border", "1px solid #FF0000");

                    if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
                    {
                        drpExpmonth.Style.Add("border", "1px solid #FF0000");
                        drpExpyear.Style.Add("border", "1px solid #FF0000");
                    }
                    if (objPRInfo.Error_Text.ToLower().Contains("card type") == true)
                    {
                        //drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
                    }
                    HttpContext.Current.Session["paySPresponse"] = "";
                }
                else
                {

                    //Session["Pay"] = "End";
                    Session["XpayMS"] = null;
                    // div1.InnerHtml = "";
                    //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                    //div2.InnerHtml = "";
                    // div2.Visible = false;
                    HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                    HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                    HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                    string cardno = txtcreditcardno.Text.Substring(txtcreditcardno.Text.Length - 4, 4);
                    Response.Redirect("BillInfoSP.aspx?Paytype=submitorder&key=" + OrderID.ToString() + "&cn=" + cardno + "&ptype=SP");

                    //Response.Redirect("BillInfoSP.aspx?key=" + OrderID.ToString() + "#####" + "&PaySP" + "#####" + "Paid");
                }



            }
            catch (Exception ex)
            {
            }



        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                objErrorHandler.CreatePayLog("btnPay_Click start Orderid=" + OrderID);
                div2.Visible = false;

                //  GetParams();
                // order_submit_process();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

                int Userid = objHelperServices.CI(Session["USER_ID"]);



                oOrderInfo = objOrderServices.GetOrder(OrderID);
                oUserInfo = objUserServices.GetUserInfo(Userid);
                oPayInfo = objPaymentServices.GetPayment(OrderID);
                PaymentID = oPayInfo.PaymentID;
                //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
                //{
                //    div1.InnerHtml = "";
                //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                //    return;
                //}

                if (oPayInfo.PayResponse != null)
                {
                    if (oPayInfo.PayResponse.ToLower() == "yes")
                    {
                        divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                        return;
                    }
                }

                string returnurl = "/Billinfo.aspx?key=" + EncryptSP(OrderID.ToString() + "#####" + "PayPP" + "#####" + "Paid");
                renUrl = renUrl.Replace(Request.Url.AbsolutePath, returnurl);
                string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);
                HttpContext.Current.Session["P_Oid"] = OrderID;
                if (Requeststr.Contains("Form") == false)
                    divContent.InnerHtml = Requeststr;
                else
                    this.Page.Controls.Add(new LiteralControl(Requeststr));

                btnPay.Visible = false;
                BtnProgress.Visible = true;

                objErrorHandler.CreatePayLog("btnPay_Click End Orderid=" + OrderID);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }

        protected void ImageButton4_Click(object sender, EventArgs e)
        {
            try
            {
                ProceedFunction();
            }
            catch
            { }
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


        protected static string Encrypt_SP(string ordid)
        {
            Security objSecurity = new Security();
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
        [System.Web.Services.WebMethod]
        public static string SaleTrans(string nounce, string Amount)
        {
            try
            {

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["Pay_ORDER_ID"] != null)
                {
                    string x = "";
                    var gateway = new BraintreeGateway
                    {
                        Environment = Braintree.Environment.SANDBOX,
                        MerchantId = "mjff7p7mgb4qmp77",
                        PublicKey = "h673fc8hc4v7pqh4",
                        PrivateKey = "92c877d009ac2dc337a38fd5737301e3"
                    };
                    //var gateway = new BraintreeGateway
                    //{
                    //    Environment = Braintree.Environment.PRODUCTION,
                    //    MerchantId = "wrv3fq8x3r269ycd",
                    //    PublicKey = "nm7v4wm8dmw7b6rq",
                    //    PrivateKey = "a3d333f589d80552db255c34c1407c40"
                    //};

                    SecurePayService objSecurePayService = new SecurePayService();
                    SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
                    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                    PaymentServices objPaymentServices = new PaymentServices();
                    ErrorHandler objErrorHandler = new ErrorHandler();

                    string OrderID = HttpContext.Current.Session["Pay_ORDER_ID"].ToString();
                    //objErrorHandler.CreateLog("saletrans" + OrderID);
                    int intOrderID = Convert.ToInt32(OrderID);

                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    OrderServices objOrderServices = new OrderServices();
                    oOrderInfo = objOrderServices.GetOrder(intOrderID);


                    objErrorHandler.CreateLog("saletrans" + oOrderInfo.OrderStatus);

                    if (oOrderInfo.OrderStatus == 18)

                    {
                        oPayInfo = objPaymentServices.GetPayment(intOrderID);

                        int PaymentID = oPayInfo.PaymentID;

                        if (nounce == "no")
                        {
                            x = "Error " + "Please Try again or use a different card / payment method.";
                            objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes", oOrderInfo.TotalAmount.ToString(),HttpContext.Current.Session["USER_ID"].ToString());

                            objErrorHandler.CreateLog(" br  Orderid=" + OrderID + "Error Processing PARes");

                            return x;
                        }


                        var request = new TransactionRequest
                        {
                            Amount = Convert.ToDecimal(Amount),
                            //MerchantAccountId = "wesallianceptyltdAUD", 
                            PaymentMethodNonce = nounce,

                            Options = new TransactionOptionsRequest
                            {
                                SubmitForSettlement = true
                            },
                            BillingAddress = new PaymentMethodAddressRequest
                            {
                                PostalCode = oPayInfo.Zip,
                                FirstName = oPayInfo.BillFName,
                                LastName = oPayInfo.BillLName,
                                StreetAddress = oPayInfo.Address1,
                                Locality = oPayInfo.City,
                                Company = oPayInfo.Country

                            },
                            OrderId = OrderID,

                        };

                        // 

                        PaymentMethodNonce paymentMethodNonce = gateway.PaymentMethodNonce.Find(nounce);
                        ThreeDSecureInfo info = paymentMethodNonce.ThreeDSecureInfo;
                        string Enrolled = "";
                        string Status = "";
                        bool? LiabilityShifted = null;
                        bool? LiabilityShiftPossible = null;
                        string TRANSACTIONID = "";
                        if (info != null)
                        {
                            Enrolled = info.Enrolled;
                            Status = info.Status;
                            LiabilityShifted = info.LiabilityShifted;
                            LiabilityShiftPossible = info.LiabilityShiftPossible;
                            TRANSACTIONID = info.DsTransactionId;
                        }
                        if (LiabilityShifted == true && (Status == "authenticate_successful" || Status == "authenticate_attempt_successful"))
                        {

                            Result<Transaction> result = gateway.Transaction.Sale(request);



                            if (result.IsSuccess() == true)
                            {


                                Transaction transaction = result.Target;
                                string ResponseId = transaction.Id;
                                string ResponseText = transaction.ProcessorResponseText;
                                string Responsecode = transaction.ProcessorResponseCode;

                                string cardtype = result.Target.CreditCard.CardType.GetType().ToString();
                                objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, result.Target.CreditCard.CardType.ToString(), result.Target.CreditCard.CardholderName, result.Target.CreditCard.MaskedNumber, result.Target.CvvResponseCode, result.Target.CreditCard.ExpirationDate, "YES", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText,Amount, HttpContext.Current.Session["USER_ID"].ToString());
                                objErrorHandler.CreatePayLog("Card No:" + result.Target.CreditCard.MaskedNumber);
                                //  ErrorHandler objErrorHandler = new ErrorHandler();
                                objErrorHandler.CreatePayLog("************************");
                                HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                                objErrorHandler.CreatePayLog("b4 order status update" + OrderID);
                                //HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                                //HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                                //HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                                HttpContext.Current.Session["XpayMS"] = null;
                                // div1.InnerHtml = "";
                                //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                                //div2.InnerHtml = "";
                                // div2.Visible = false;
                                HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                                HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                                HttpContext.Current.Session["P_Oid"] = OrderID.ToString();


                                string cardno = result.Target.CreditCard.MaskedNumber.Substring(result.Target.CreditCard.MaskedNumber.Length - 4, 4);
                                return "BillInfoSP.aspx?key = " + OrderID.ToString()  +"&cn=" + cardno + "&ptype=BR";

                                //  x = "true";
                            }
                            else
                            {
                                Transaction transaction = result.Transaction;
                                string errorMessages = "";
                                if (transaction.Status == TransactionStatus.GATEWAY_REJECTED)
                                {
                                    errorMessages = "Reason: Gateway rejected.";
                                    // errorMessages += transaction.GatewayRejectionReason;
                                    // e.g. "avs"
                                }
                                else if (transaction.Status == TransactionStatus.FAILED)
                                {
                                    errorMessages = "Reason: Transaction Failed.";
                                }
                                else if (transaction.Status == TransactionStatus.PROCESSOR_DECLINED)
                                {
                                    errorMessages = "Reason: Processor Declined.";
                                }
                                else if (transaction.Status == TransactionStatus.UNRECOGNIZED)
                                {
                                    errorMessages = "Reason: Transaction Unrecognized.";
                                }
                                else if (transaction.Status == TransactionStatus.VOIDED)
                                {
                                    errorMessages = "Reason: Transaction voided.";
                                }
                                else if (transaction.Status == TransactionStatus.AUTHORIZATION_EXPIRED)
                                {
                                    errorMessages = "Reason: Authorization Expired";
                                }
                                else
                                {

                                    foreach (ValidationError error in result.Errors.DeepAll())
                                    {
                                        errorMessages += (int)error.Code + " - " + error.Message + "\n";
                                    }
                                }
                                if (errorMessages == "")
                                {
                                    errorMessages = result.Message;
                                }
                                //    ErrorHandler objErrorHandler = new ErrorHandler();
                                objErrorHandler.CreatePayLog(" br  Orderid=" + OrderID + errorMessages);
                                objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText,Amount, HttpContext.Current.Session["USER_ID"].ToString());
                                x = "Error " + errorMessages;
                            }


                            return x;
                        }
                        else
                        {
                            //   objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                            x = "Error " + "Please Try again or use a different card / payment method.";
                            objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status,Amount,HttpContext.Current.Session["USER_ID"].ToString());

                            return x;
                        }

                    }
                    else
                    {

                        HttpContext.Current.Session["ORDER_ID"] = 0;

                        return "Error:QTEEMPTY";
                    }
                }
                else
                {

                    //HttpContext.Current.Response.Redirect("login.aspx");
                    return "Error:Session Timed out";
                }
            }

            catch (Exception ex)
            {
                ErrorHandler objErrorHandler = new ErrorHandler();
                objErrorHandler.CreatePayLog(ex.ToString());
                return "Error: " + "Please Try Again";
            }
        }

        #region "Function.."
        public string BuildBillAddress()
        {
            try
            {
                string sBillAdd = "";
           
                if (oOrderInfo.BillcompanyName != null)
                {
                    sBillAdd = WrapText(oOrderInfo.BillcompanyName) + "<br>";
                }
                if (oOrderInfo.BillFName != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillFName) + " ";
                }
                if (oOrderInfo.BillLName != null)
                {
                    sBillAdd = sBillAdd + WrapText(oOrderInfo.BillLName) + " ";
                }
                if (oOrderInfo.BillAdd1 != null)
                {
                    sBillAdd = sBillAdd +"<br>"+ WrapText(oOrderInfo.BillAdd1) + "<br>";
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

                if (oOrderInfo.ShipCompName != null)
                {
                    sShippAdd = WrapText(oOrderInfo.ShipCompName) + "<br>";
                }
                if (oOrderInfo.ShipFName != null)
                {
                    sShippAdd =sShippAdd+ WrapText(oOrderInfo.ShipFName) + " ";
                }
                if (oOrderInfo.ShipMName != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipMName) + " ";
                }
                if (oOrderInfo.ShipLName != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipLName) ;
                }
                if (oOrderInfo.ShipAdd1 != null)
                {
                    sShippAdd =sShippAdd +  "<br>" + WrapText(oOrderInfo.ShipAdd1) + "<br>";
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


}


