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
using System.Text.RegularExpressions;
using Braintree;


public partial class PaySP : System.Web.UI.Page
    {
       
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        PayPalService objPayPalService = new PayPalService();
        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

        SecurePayService objSecurePayService = new SecurePayService();
     
        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

        public int UserID = 0; 

        string environment = "test"; // Change to "live" to process real transactions.
                                     // (For a live transaction, you must use a real, valid CC and billing address.)
    public string ClientToken = string.Empty;
    public int OrderID = 0;
        public string Txn_id = "";
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
        DataTable tblPaymentInfo = new DataTable();

        string strOrderLink = "<br/><a href=\"OrderHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strPaymentLink = "<br/><a href=\"PaymentHistory.aspx\" class=\"toplinkatest\" >Back</a>";
        string strBackLink = "";
        string renUrl =HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
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


          //  objErrorHandler.CreateLog("clientToken paysp:" + this.ClientToken);
        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string sb;
            sb = "<script language=javascript>\n";
            sb += "window.history.forward(1);\n";
            sb += "\n</script>";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


            sb = "";
            sb = "<script type=javascript>\n";
            sb += "window.onload = function () { Clear(); }\n";
            sb += "function Clear() { \n";
            sb += " var Backlen=history.length; \n";
            sb += " if (Backlen > 0) history.go(-Backlen); \n";
            sb += "\n}</script>";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



        }
       
       
        protected void ImgBtnEditShipping_Click(object sender, EventArgs e)
        {
            if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
            {
                if (OrderID>0)
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + OrderID.ToString(), false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
                }
            }

            //Response.Redirect("Shipping.aspx?shipping.aspx?OrderID=" + OrderID + "&ApproveOrder=Approve", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            GenerateClientToken();
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
            if (!IsPostBack)
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
                    div2.Visible = false;
                    if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "" && Convert.ToInt32(Session["USER_ID"].ToString()) > 0)
                    {
                        UserID = Convert.ToInt32(Session["USER_ID"].ToString());
                    }
                    else
                    {
                        divTimeout.Visible = true;
                        divCC.Visible = false;
                        return;
                        //Response.Redirect("login.aspx");
                    }
                    LoadIds();

                    oUserInfo = objUserServices.GetUserInfo(UserID);

                    //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower()=="au" )
                    //{
                    //    div1.InnerHtml = "";
                    //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                    //    return;
                    //}

                    if (oPayInfo.PayResponse.ToLower() == "yes")
                    {
                        div1.InnerHtml = "";
                        div2.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                        div2.Visible = true;
                        return;
                    }
                    if (IsPostBack)
                    {

                        return;
                    }

                }


                if (!IsPostBack)
                {

                //if (System.Configuration.ConfigurationManager.AppSettings["creditflag"].ToString() == "1")
                //{
             
                //else
                //{
                //    creditflag1.Visible = false;
                //    creditflag2.Visible = false;
                //    creditflag3.Visible = true;

                //}
                string BillAdd;
                    string ShippAdd;
                Session["Pay_ORDER_ID"] = OrderID;
                    oPayInfo = objPaymentServices.GetPayment(OrderID);
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                Session["Pay_User_id"] = oOrderInfo.UserID;



                string userid = string.Empty;
             
                string userid1 = "";
                if (Session["Pay_User_id"] != null)
                {
                    userid1 = Session["Pay_User_id"].ToString();
                }
               

                    userid = Session["User_id"].ToString();
               
                int c = objOrderServices.checkcardnoexsist(userid,userid1, "","");
                //objErrorHandler.CreateLog(c + "checkcardnoexsist");

                if (c == 1)
                {
                    creditflag1.Style.Add("display", "block");
                    creditflag2.Style.Add("display", "none");
                    creditflag3.Visible = false;
                    imgsecurepay.Src = "../images/cards_sm.png";
                }
                else
                {
                    //creditflag1.Visible = false;
                    //creditflag2.Visible = true;
                    creditflag1.Style.Add("display", "none");
                    creditflag2.Style.Add("display", "block");
                    creditflag3.Visible = false;
                    imgsecurepay.Src = "../images/cards_sm.png";
                }






                BillAdd = BuildBillAddress();
                    ShippAdd = BuildShippAddress();
                    lblDeliveryTo.Text = BillAdd;
                    lblShipTo.Text = ShippAdd;
                    lbltotalamt.Text = oOrderInfo.TotalAmount.ToString();
                    lblorderid.Text = OrderID.ToString();
                   // initcontrol();
                    //lblOrderNo.Text = " : " + oPayInfo.PORelease;
                    LoadOrderItem();
                    //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                    //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) + "&";

                }
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            
            }
        }
        protected void btnSecurePayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("paysp.aspx?" + EncryptSP(OrderID.ToString()), false);
        }

        protected void btnPayPalPayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
        }
        protected void btnBankTrasfer_Click(object sender, EventArgs e)
        {
            Response.Redirect("PayOnline_BankTrasfer.aspx?" + OrderID.ToString() + "#####" + "PaySP", false);
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
        //private void initcontrol()
        //{

        //    txtcreditcardno.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        //    txtCVV.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        //   // LoadCardList();
        //    //for (int y = DateTime.Now.Year ; y <= DateTime.Now.Year + 20; y++)
        //    //{
        //    //    drpExpyear.Items.Add(y.ToString());
        //    //}
        //}
        //public void LoadCardList()
        //{
        //    try
        //    {
        //        DataSet oDs = new DataSet();
        //        oDs = objSecurePayService.GetCardList();
        //        drppaymentmethod.Items.Clear();
        //        drppaymentmethod.DataSource = oDs;
        //        drppaymentmethod.DataValueField = oDs.Tables[0].Columns["CARD_ID"].ToString();
        //        drppaymentmethod.DataTextField = oDs.Tables[0].Columns["CARD_TYPE"].ToString();
        //        drppaymentmethod.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();

        //    }
        //}
        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistory.aspx");
        }
        //protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (isValidCreditCardNumber(drppaymentmethod.SelectedValue.ToString(), txtcreditcardno.Text) == false)
        //    {
        //        args.IsValid = false;
        //    }
        //    else
        //    {
        //        args.IsValid = true;
        //    }
        //}
        protected void btnSecurePay_Click(object sender, EventArgs e)
        {
        objErrorHandler.CreateLog("inside securepay");
        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["Pay_ORDER_ID"] != null)
        {
            try
            {
                int x = txtcreditcardno.Text.Length;
                //string cardno1 = txtcreditcardno.Text.Substring(0, 6) + '* *****' + 1007;
                //    400000 * *****1000bi
                string userid = string.Empty;
                string userid1 ="";
                if (Session["Pay_User_id"] != null)
                {
                    userid1 = Session["Pay_User_id"].ToString();
                }
                

                    userid = Session["USER_ID"].ToString();
               

                int c = objOrderServices.checkcardnoexsist(userid,userid1, txtcreditcardno.Text.Substring(x - 4, 4), txtcreditcardno.Text.Substring(0, 3));
                objErrorHandler.CreateLog(c + "securepayclick");
                if (c == 0)
                {
                    ModalPopupExtender2.Show();
                    lblMessage_br.Text= "Please Re-enter Card Details and Try Again";
                    //divgobr.InnerHtml = "Please Re-enter Card Details and Try Again";
                    //divgobr.Style.Add("display", "block");
                    p1.Style.Add("display", "block");
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


            string rtnstr = "";
            try
            {
                txtcreditcardno.Style.Remove("border");
                txtExpyear.Style.Remove("border");
                //drpExpyear.Style.Remove("border");
                //txtcreditcardno.Style.Remove("border");
                //drppaymentmethod.Style.Remove("border");
            }
            catch
            {
            }

            SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
            try
            {

                LoadIds();
                if (oPayInfo.PayResponse.ToLower() == "yes")
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Already Payment has been made" + strBackLink;
                    div2.Visible = true;
                Def.Style.Add("display", "inline-block");
                return;
                }

                //if (Session["XpayC"] != null)
                //{
                //    if (Convert.ToInt32(Session["XpayC"]) > 3)
                //    {
                //        div1.InnerHtml = "";
                //        div2.InnerHtml = "More than 3 attempt. try again" + "<br/><a href=\"PaySP.aspx?" + EncryptSP(OrderID.ToString()) + "\" class=\"toplinkatest\" >Back</a>";
                //        div2.Visible = true;
                //        return;
                //    }
                //    else
                //        Session["XpayC"] = Convert.ToInt32(Session["XpayC"]) + 1;
                //}
                //else
                //    Session["XpayC"] = 0;

  

            int currentyear = DateTime.Now.Year;
            currentyear = Convert.ToInt32(currentyear.ToString().Substring(2, 2));

            int currentmonth = DateTime.Now.Month;
            objErrorHandler.CreateLog(currentyear + "currentyear" + currentmonth + "currentmonth");
            bool isexpvalid = false;
           
            string expmonth="";
            string expyear="";
            if (txtExpyear.Text.Contains("/") == true)
            {
                if (txtExpyear.Text.Length == 5)
                {
                    expmonth = txtExpyear.Text.Substring(0, 2);
                    if (Convert.ToInt16(expmonth) < 10 && expmonth.Contains("0") == false)
                    {
                        expmonth = "0" + expmonth;

                    }
                }
                else
                {
                    expmonth = txtExpyear.Text.Substring(0, 1);
                    expmonth = "0" + expmonth;
                }
               
                expyear = txtExpyear.Text.Substring(3, 2);
                
            }
            else {
                btnSP.Style.Add("display", "none");
               
                ImgBtnEditShipping.Style.Add("display", "none");
                divfailed_sp.Style.Add("display", "block");
                Def.Style.Add("display", "inline-block");
                txtExpyear.Focus();
                HttpContext.Current.Session["payflowresponse"] = "FAIL";
                txtExpyear.Style.Add("border", "1px solid #FF0000");
              
            }



            objPRInfo.Error_Text = "";
            objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID,"", txtnamecard.Text, txtcreditcardno.Text.Replace(" ","") , txtCVV.Text, expmonth + "/" + "20"+expyear, HttpContext.Current.Session["USER_ID"].ToString());

            if (objPRInfo.Error_Text != "")
            {
                btnSP.Style.Add("display", "none");
                //BtnProgress.Style.Add("display", "none");
                //ImgBtnEditShipping.Style.Add("display", "block");
                ImgBtnEditShipping.Style.Add("display", "none");
                divfailed_sp.Style.Add("display", "block");
                Def.Style.Add("display", "inline-block"); 
                //div2.InnerHtml = objPRInfo.Error_Text;
                //div2.Visible = true;
                HttpContext.Current.Session["payflowresponse"] = "FAIL";

                if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
                {
                txtcreditcardno.Style.Add("border", "1px solid #FF0000");
                    txtcreditcardno.Focus();
                }

                    if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
                { 
                    txtCVV.Style.Add("border", "1px solid #FF0000");
                    txtCVV.Focus();
                }

                if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
                    {
                        txtExpyear.Style.Add("border", "1px solid #FF0000");
                    txtExpyear.Focus();
                    //drpExpyear.Style.Add("border", "1px solid #FF0000");
                }

                if (HttpContext.Current.Session["NoAttempt"] != null)
                {

                    int noatt = Convert.ToInt32(HttpContext.Current.Session["NoAttempt"].ToString());
                    if (noatt >= 2)
                    {
                        ModalPopupExtender2.Show();
                        lblMessage_br.Text = "Please Re-enter Card Details and Try Again";
                        //divgobr.InnerHtml = "Please Re-enter Card Details and Try Again";
                        //divgobr.Style.Add("display", "block");
                        p1.Style.Add("display", "block");
                        creditflag1.Style.Add("display", "none");
                        creditflag2.Style.Add("display", "block");
                        imgsecurepay.Src = "../images/cards_sm.png";
                        HttpContext.Current.Session["NoAttempt"] = 0;
                        return;
                    }
                    noatt = noatt + 1;
                    HttpContext.Current.Session["NoAttempt"] = noatt;
                }
                else
                {
                    HttpContext.Current.Session["NoAttempt"] = 1;
                }
                //if (objPRInfo.Error_Text.ToLower().Contains("card type") == true )
                //{
                //    drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
                //}

            }
                else
                {
                    //Session["Pay"] = "End";
                    Session["XpayC"] = null;
                    div1.InnerHtml = "";
                    //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                    //div2.InnerHtml = "";
                    div2.Visible = false;
                   
                    // div1.InnerHtml = "";
                    //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
                    //div2.InnerHtml = "";
                    // div2.Visible = false;
                    HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                    HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                    HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                HttpContext.Current.Session["NoAttempt"] = 0;

                //  Response.Redirect("BillInfoSP.aspx?Paytype=submitorder&key=" + EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid"));
                string cardno = txtcreditcardno.Text.Substring(txtcreditcardno.Text.Length - 4, 4);
                Response.Redirect("BillInfoSP.aspx?Paytype=submitorder&key=" + OrderID.ToString()+"&cn="+ cardno+"&ptype=SP");

            }



        }
            catch (Exception ex)
            {
            }

            //LoadIds();
            //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
            //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) ;
            
            //oOrderInfo = objOrderServices.GetOrder(OrderID);
            //oUserInfo = objUserServices.GetUserInfo(UserID);

            ////if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
            ////{
            ////    div1.InnerHtml = "";
            ////    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
            ////    return;
            ////}
            //if (oPayInfo.PayResponse.ToLower() == "yes")
            //{
            //    //divContent.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
            //    return;
            //}
            
            //string Requeststr = objSecurePayService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

            //if (Requeststr.Contains("Form") == false)
            //    //divContent.InnerHtml = Requeststr;
            //else
            //    this.Page.Controls.Add(new LiteralControl(Requeststr));
             
        }

        private void LoadIds()
        {
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id = Request.Url.Query.Replace("?", "");
                id = DecryptSP(id);
                if (id != null)
                {
                    string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);


                    OrderID = objHelperServices.CI(ids[0]);
                    if (ids.Length > 1)
                    {
                        Txn_id = ids[1];
                        strBackLink = strPaymentLink;
                    }
                    else
                        strBackLink = strOrderLink;

                }
                else 
                
                {
                    OrderID = objHelperServices.CI( Request.Url.Query.Replace("?", ""));
                
                }
                

            }
            else
            {
                div1.InnerHtml = "";
                div2.InnerHtml = "Invalid Data" + strPaymentLink;
                div2.Visible = true;
                return;

            }
            oPayInfo = objPaymentServices.GetPayment(OrderID);
     
            PaymentID = oPayInfo.PaymentID;
            
           // lblTotalAmount1.Text = oPayInfo.Amount.ToString();

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
                lblAmount.Text = oOrderInfo.TotalAmount.ToString();

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

        protected void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                LoadIds();
                Session["ispaypal"] = true;
                Session["OrderID"] = OrderID.ToString();
                Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
                //LoadIds();
                ////renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                ////renUrl = renUrl + "?Paytype=submitorder&key=" + EncryptSP(OrderID.ToString());


                //string returnurl = "/Billinfo.aspx?Paytype=submitorder&key=" + EncryptSP(OrderID.ToString() + "#####" + "PayPP" + "#####" + "Paid");
                //renUrl = renUrl.Replace(Request.Url.AbsolutePath, returnurl);
                //objErrorHandler.CreateLog(renUrl);
                //oOrderInfo = objOrderServices.GetOrder(OrderID);
                //oUserInfo = objUserServices.GetUserInfo(UserID);

                ////if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
                ////{
                ////    div1.InnerHtml = "";
                ////    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                ////    return;
                ////}
                //if (oPayInfo.PayResponse.ToLower() == "yes")
                //{
                //    divContent.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                //    return;
                //}

                //string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

                //if (Requeststr.Contains("Form") == false)
                //    divContent.InnerHtml = Requeststr;
                //else
                //    this.Page.Controls.Add(new LiteralControl(Requeststr));
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
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
       


        #region "Function.."
        public string BuildBillAddress()
        {
            try
            {
                string sBillAdd = "";

                if (oOrderInfo.BillcompanyName != null)
                {
                    sBillAdd = WrapText(oOrderInfo.ShipCompName) + "<br>";
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
                    sShippAdd = WrapText(oOrderInfo.ShipFName) + " ";
                }
                if (oOrderInfo.ShipMName != null)
                {
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipMName) + " ";
                }
                if (oOrderInfo.ShipLName != null)
                {
                    sShippAdd =  sShippAdd + WrapText(oOrderInfo.ShipLName) ;
                }
                if (oOrderInfo.ShipAdd1 != null)
                {
                    sShippAdd =  sShippAdd +"<br>"+ WrapText(oOrderInfo.ShipAdd1) + "<br>";
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

    #endregion'

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


                //objErrorHandler.CreateLog("saletrans" + oOrderInfo.OrderStatus);

                if (oOrderInfo.OrderStatus == 18)

                {
                    oPayInfo = objPaymentServices.GetPayment(intOrderID);

                    int PaymentID = oPayInfo.PaymentID;

                    if (nounce == "no")
                    {
                        x = "Error " + "Please Try again or use a different card / payment method.";
                        objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes",Amount, HttpContext.Current.Session["USER_ID"].ToString());

                        objErrorHandler.CreateLog(" br  Orderid=" + OrderID + "Error Processing PARes");

                        return x;
                    }


                    var request = new TransactionRequest
                    {
                        Amount = Convert.ToDecimal(Amount),
                       MerchantAccountId = "wesallianceptyltdAUD",
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
                            HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                            HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                            HttpContext.Current.Session["P_Oid"] = OrderID.ToString();



                            string cardno = result.Target.CreditCard.MaskedNumber.Substring(result.Target.CreditCard.MaskedNumber.Length - 4, 4);
                            return  "BillInfoSP.aspx?Paytype=submitorder&key=" + OrderID.ToString() +"&cn="+ cardno + "&ptype=BR";

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

    // private bool isValidCreditCardNumber(string type, string ccnum)
    // {
    //     string regExp = "";

    //    if (type == "5") {
    //   // Visa: length 16, prefix 4, dashes optional.^4\d{3}-?\d{4}-?\d{4}-?\d{4}$---->
    //    regExp   = "/^4[0-9]{12}(?:[0-9]{3})?$/";
    //} else if (type == "6") {
    //   // Mastercard: length 16, prefix 51-55, dashes optional.
    //    regExp   = "/^5[1-5][0-9]{14}$/";
    //}  else if (type == "2") 
    //    {
    //   // American Express: length 15, prefix 34 or 37.^3[47][0-9]{13}$--->^3[4,7]\d{13}$
    //   regExp   = "/^3[47][0-9]{13}$/";
    //}        

    ////    else if (type == "Diners") {
    ////   // Diners: length 14, prefix 30, 36, or 38.^3(?:0[0-5]|[68][0-9])[0-9]{11}$--->^3[0,6,8]\d{12}$
    ////   regExp   = "/^(30[0-5]|3[68][0-9]|54\d{3}|55\d{3})[0-9]{11}$/";
    ////} 
    ////    else if (type == "JCB") {
    ////   // JCB cards beginning with 2131 or 1800 have 15 digits. JCB cards beginning with 35 have 16 digits.^(?:2131|1800|35\d{3})\d{11}$--->^(?:2131|1800|35\d{3})\d{11}$
    ////   regExp   = "/^(?:2131|1800|3528\d{1}|3529\d{1}|35[3-8][0-9]\d{1})\d{11}$/;///^(?:2131|1800|3528|3529|35[3-8][0-9])\d{11}$/";
    ////}
    ////    else if (type == "Disc") {
    ////   // Discover: length 16, prefix 6011, dashes optional.^6011-?\d{4}-?\d{4}-?\d{4}$--->6011, 622126-622925, 644-649, 65
    ////    regExp   = "/^6(?:011|5[0-9]{2}|4[4-9]\d{1})[0-9]{12}|622(12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10}$/";
    ////}

    //     if (!Regex.IsMatch(ccnum, regExp))
    //         return false;

    //     string[] tempNo = ccnum.Split('-');
    //     ccnum = String.Join("", tempNo);

    //     int checksum = 0;
    //     for (int i = (2 - (ccnum.Length % 2)); i <= ccnum.Length; i += 2)
    //     {
    //         checksum += Convert.ToInt32(ccnum[i - 1].ToString());
    //     }

    //     int digit = 0;
    //     for (int i = (ccnum.Length % 2) + 1; i < ccnum.Length; i += 2)
    //     {
    //         digit = 0;
    //         digit = Convert.ToInt32(ccnum[i - 1].ToString()) * 2;
    //         if (digit < 10)
    //         { checksum += digit; }
    //         else
    //         { checksum += (digit - 9); }
    //     }
    //     if ((checksum % 10) == 0)
    //         return true;
    //     else
    //         return false;

    // }
}


