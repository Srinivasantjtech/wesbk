using Antlr3.ST;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;

public partial class BillInfoSP : System.Web.UI.Page
    {
   // public string ClientToken = string.Empty;
    HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        OrderServices objOrderServices = new OrderServices();

        PaymentServices objPaymentServices = new PaymentServices();
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();

        OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

        UserServices objUserServices = new UserServices();
       // UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

       // PayOnlineService objPayOnlineService = new PayOnlineService();

      //  Security objSecurity = new Security();
      //  const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";

       // string environment = "test"; // Change to "live" to process real transactions.
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
  

    //protected void Page_PreInit()
    //{
    //    if (httpRequestVariables()["tx"] != null)
    //    {
    //        Page.MasterPageFile = "Blank.Master";
    //    }
    //}
    //protected NameValueCollection httpRequestVariables()
    //{
    //    var post = Request.Form;       // $_POST
    //    var get = Request.QueryString; // $_GET
    //    return Merge(post, get);
    //}

    //protected string EncryptSP(string ordid)
    //{
    //    string enc = "";
    //    enc = objSecurity.StringEnCrypt(ordid, EnDekey);
    //    enc = objSecurity.StringEnCrypt(enc, EnDekey);
    //    enc = objSecurity.StringEnCrypt(enc, EnDekey);
    //    enc = objSecurity.StringEnCrypt(enc, EnDekey);
    //    enc = objSecurity.StringEnCrypt(enc, EnDekey);
    //    return HttpUtility.UrlEncode(enc);
    //}
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
                //  key = DecryptSP(Request["key"].ToString());
                // key =(Request["key"].ToString());
                OrderID = Convert.ToInt32(Request["key"].ToString());
            }
            //if (key == "")
            //{
            //    key = Request["key"].ToString();
            //}
            //if (httpRequestVariables()["tx"] != null)
            //{
            //    HttpContext.Current.Session["payflowresponse"] = httpRequestVariables();
            //    var response = HttpContext.Current.Session["payflowresponse"] as NameValueCollection;
            //    if (response["cm"] != null)
            //        objPayPalService.SetPayPalStatus(response, response["cm"].ToString());
            //    //string rtn = 
            //    //output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "?key=" + EncryptSP(key) +"\";</script>";
            //    output += "<script type=\"text/javascript\">window.top.location.href = \"" + renUrl + "\";</script>";
            //    BodyContentDiv.InnerHtml = output;
            //    return;
            //}

            //if (IsPostBack)
            //    return;
            // LoadIds();




            //if (!IsPostBack)
            //{
            string BillAdd;
            string ShippAdd;


            if (Session["hfisordercompleted"] != null && Session["hfisordercompleted"].ToString() == OrderID.ToString())
            {
                Response.Redirect("OrderHistory.aspx");



            }


            oPayInfo = objPaymentServices.GetPayment(OrderID);
            PaymentID = oPayInfo.PaymentID;
            
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
                    if (HttpContext.Current.Session["paySPresponse"] != null && HttpContext.Current.Session["paySPresponse"] != "")
                    {

                        if (HttpContext.Current.Session["paySPresponse"] == "SUCCESS")
                        {

                            //// string ordstatus = objOrderServices.GetOrderStatus(OrderID);
                            //string[] ipn = null;

                            //if (HttpContext.Current.Session["IPN"] != null && HttpContext.Current.Session["IPN"] != "")
                            //{
                            //    ipn = HttpContext.Current.Session["IPN"].ToString().Split('#');

                            //    if (ipn.Length >= 1)
                            //    {
                            //        DataTable dt = objPayPalService.GETIPN(ipn[1].ToString(), ipn[0].ToString());
                            //        if (dt != null && dt.Rows.Count > 0)
                            //        {
                            //            DataRow[] drs = dt.Select("APPROVED='YES'");
                            //            if (drs.Length > 0)
                            //                isipn = true;
                            //        }
                            //    }
                            //}




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
                          //  objOrderServices.UpdatePaymentOrderStatus_Onlinepayment(OrderID, PaymentID, false);

                            //if (objOrderServices.IsNativeCountry(OrderID) == 0)
                            //{

                            //    objOrderServices.UpdatePaymentOrderStatus_Onlinepayment(OrderID, PaymentID, isipn);
                            //}
                            //else if (ispickuponly_zone(OrderID) == true)
                            //{
                            //    objOrderServices.UpdatePaymentOrderStatus_Remotezone(OrderID, PaymentID, isipn);
                            //}
                            //else
                            //{
                            objErrorHandler.CreatePayLog("paytype-" + "securepay" + Request.QueryString["Paytype"] + OrderID.ToString());
                            if (Request.QueryString["Paytype"] == "submitorder")
                            {
                                objOrderServices.UpdatePaymentOrderStatus_Onlinepayment(OrderID, PaymentID, isipn);
                        if (Request.QueryString["paytype"] != null && Request.QueryString["ptype"].ToString() == "SP")
                        {
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "SP");
                        }
                        else if (Request.QueryString["paytype"] != null && Request.QueryString["ptype"].ToString() == "BR")
                        {
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "BR");
                        }
                            call_SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success, false,oPayInfo,oOrderInfo,"BR");
                            }
                            else
                            {
                                objOrderServices.UpdatePaymentOrderStatus_DirectOnlinepayment(OrderID, PaymentID, isipn);
                        if (Request.QueryString["paytype"]!=null && Request.QueryString["ptype"].ToString() == "SP")
                        {
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "SP");
                        }
                        else if (Request.QueryString["paytype"] != null && Request.QueryString["ptype"].ToString() == "BR")
                        {
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, "BR");
                        }
                        Session["hfisordercompleted"] = OrderID;
                              call_SendMail_AfterPaymentSP(OrderID, (int)OrderServices.OrderStatus.Online_Payment, false,oPayInfo,oOrderInfo,"BR");
                            }
                           
                            //}
                          
                           
                            //objOrderServices.UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Payment_Successful );


                            divError.InnerHtml = "";
                            divOk.InnerHtml = "Transaction approved! Thank you for your order.";
                           
                            //divlink.InnerHtml="<br/><a href=\"home.aspx\" class=\"toplinkatest\" >Home</a>";
                            string cardno = string.Empty; 
                            if (Request.QueryString["cn"] != null)
                            { 
                            
                            cardno=Request.QueryString["cn"].ToString();
                            }
                            divlink.InnerHtml = "<table><tr><td>Payment Method: Credit Card <td></tr>";
                            divlink.InnerHtml = divlink.InnerHtml + "<tr><td>" +"Card No: xxxx xxxx xxxx " + cardno +" </td></tr></table>";
                            objErrorHandler.CreatePayLog(divlink.InnerHtml + OrderID.ToString());
                            Session["ORDER_ID"] = "0";
                            //if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                            //{
                            //    Sess ion["PrevOrderID"] = "0";
                            //}
                        }
                        else
                        {
                            divOk.InnerHtml = "";
                            divOk.Visible = false;
                            divError.InnerHtml = "Transaction failed! Please try again.<br/>";
                            objErrorHandler.CreatePayLog(divError.InnerHtml + OrderID.ToString());
                            divlink.InnerHtml = "<a href=\"PayOnlineCC.aspx?" + OrderID.ToString() + "\" class=\"toplinkatest\" >Back</a>";

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





                //}
            }            
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString() + "page load" + OrderID);  
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
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();

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
    public delegate void SyncDelegate(int OrderId, int OrderStatus, bool isau, PaymentServices.PayInfo oPayInfo, OrderServices.OrderInfo oOrderInfo, string paytype, string stemplatepath);
    public void call_SendMail_AfterPaymentSP(int OrderId, int OrderStatus, bool isau, PaymentServices.PayInfo oPayInfo, OrderServices.OrderInfo oOrderInfo, string paytype)
    {
      string  stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
       
        SyncDelegate syncDelegate = new SyncDelegate(SendMail_AfterPaymentSP);
            IAsyncResult asyncResult = syncDelegate.BeginInvoke(OrderId,OrderStatus,isau, oPayInfo, oOrderInfo,paytype, stemplatepath, null, null);
        syncDelegate.EndInvoke(asyncResult);
    }
    public   void SendMail_AfterPaymentSP(int OrderId, int OrderStatus, bool isau, PaymentServices.PayInfo oPayInfo, OrderServices.OrderInfo oOrderInfo,string paytype,string stemplatepath)
        {
        
        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP start OrderId=" + OrderId);
            string toemail = "";
            try
            {

              //  objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner1 OrderId=" + OrderId);
                string BillAdd;
                string ShippAdd;
               
                DataSet dsOItem = new DataSet();
              //  OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
            if(oPayInfo.PaymentID==null)
                { 
            oPayInfo = objPaymentServices.GetPayment(OrderId);
            }
            if (oOrderInfo.OrderID == null)
            {
                oOrderInfo = objOrderServices.GetOrder(OrderId);
            }
            if (oOrderInfo.ShipMethod == "" || oOrderInfo.ShipMethod == null)
            {
                oOrderInfo = objOrderServices.GetOrder(OrderId);
            }

            int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

            //oUserInfo = objUserServices.GetUserInfo(UserID);
            //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            
                dsOItem = objOrderServices.GetOrderItems(OrderId);
                BillAdd = GetBillingAddress(OrderId,oOrderInfo);
                ShippAdd = GetShippingAddress(OrderId,oOrderInfo);

                string ShippingMethod = oOrderInfo.ShipMethod;
                string CustomerOrderNo = oPayInfo.PORelease;
                string shippingnotes = oOrderInfo.ShipNotes;


              //  objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner2 OrderId=" + OrderId);
                if (oOrderInfo.CreatedUser != 999)
                {

                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                }
                else
                {
                    oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
                }
                // oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
                string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail ;
                toemail = oUserInfo.AlternateEmail;
                if (oUserInfo.AlternateEmail != "")
                {
                 Emailadd = oUserInfo.AlternateEmail;
                 toemail = oUserInfo.AlternateEmail;
            }
               

              //url = HttpContext.Current.Request.Url.Authority.ToString();
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
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner3 OrderId=" + OrderId);
                //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
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

                    
                    int ictrows = 0;

                    DataSet dscat = new DataSet();
                    DataTable dt = null;
                    _stg_records = new StringTemplateGroup("row", stemplatepath);
                    _stg_container = new StringTemplateGroup("main", stemplatepath);


                    lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];

                    int ictrecords = 0;
                   // objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner4 OrderId=" + OrderId);
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


                    //_stmpl_container.SetAttribute("CONNOTNO", oOrderInfo.TrackingNo);  
                    //_stmpl_container.SetAttribute("INVOICENO", oOrderInfo.InvoiceNo);
                    //_stmpl_container.SetAttribute("SHIPPEDBY", oOrderInfo.ShipCompany);
                    _stmpl_container.SetAttribute("PAY_METHOD", "Credit Card SP");
                    _stmpl_container.SetAttribute("AMOUNT", oOrderInfo.TotalAmount);
                    _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID);
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

                    //Added by :Indu For View order button 
                    _stmpl_container.SetAttribute("OrderNo", oOrderInfo.OrderID);
                    string orderurl = "https://www.wes.com.au/OrderReport.aspx?OrdId=" + oOrderInfo.OrderID;
                    _stmpl_container.SetAttribute("OrderURL", orderurl);
                    _stmpl_container.SetAttribute("pricecurrency", "$");

                    _stmpl_container.SetAttribute("pname", oOrderInfo.BillcompanyName);
                    _stmpl_container.SetAttribute("pstreet", oOrderInfo.BillAdd1);
                    _stmpl_container.SetAttribute("locality", oOrderInfo.BillCity);
                    _stmpl_container.SetAttribute("region", oOrderInfo.BillState);
                    _stmpl_container.SetAttribute("country", oOrderInfo.BillCountry);
                    //****************************//


                    sHTML = _stmpl_container.ToString();
                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP after sHTML");
                   // objErrorHandler.CreatePayLog(sHTML);

                }
                catch (Exception ex)
                {
                
                    objHelperServices.Mail_Error_Log(paytype, oOrderInfo.OrderID, "", ex.Message, 0, objHelperServices.CI(UserID), Convert.ToInt16(0), 1);
                  //  objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, "", ex.Message);
               
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

                   // objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner6 OrderId=" + OrderId);
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                string frommail = objHelperServices.GetOptionValues_withoutsession("ADMIN EMAIL").ToString();
                if (frommail == "")
                {
                    frommail = "sales@wes.com.au";
                }
             //   objErrorHandler.CreateLog(frommail);
                    MessageObj.From = new System.Net.Mail.MailAddress(frommail);



               objErrorHandler.CreateLog("SP SendMail_AfterPaymentSPb4 webadminmail");

                    string emails = "";
                    string Adminemails = "";
                    string webadminmail = "";
                
                    webadminmail = objHelperServices.GetOptionValues_withoutsession("WEB ADMIN EMAIL").ToString();
                if (webadminmail == "")
                {
                    webadminmail = frommail;
                }
                if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        MessageObj.To.Add(Emailadd.ToString());
                      //  MessageObj.Bcc.Add(webadminmail);
                    MessageObj.Bcc.Add("indumathi@jtechindia.com");
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
                    //string addgoogleurl = System.Configuration.ConfigurationManager.AppSettings["addgoogleurl"].ToString();
                    //if (addgoogleurl == "true")
                    //{
                    //    MessageObj.To.Add("schema.whitelisting+sample@gmail.com");
                    //    //MessageObj.To.Add("indumathi@jtechindia.com");
                    //}
                    //MessageObj.Subject = "Your Order No :" +OrderID.ToString();

                    //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    // {
                    MessageObj.Subject = "WES Order Payment Successful - Order No : " + CustomerOrderNo;
                    //}
                    //else
                    //{
                    //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                    // }

                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP before mail send");

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues_withoutsession("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues_withoutsession("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues_withoutsession("MAIL SERVER PASSWORD").ToString());
              //  objErrorHandler.CreateLog("sp1");
                smtpclient.Send(MessageObj);
                    objHelperServices.Mail_Log("SP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner9 OrderId=" + OrderId);

                    if (Convert.ToInt16(oUserInfo.USERROLE) == 1 || Convert.ToInt16(oUserInfo.USERROLE) == 2)
                    {
                        if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                        {
                        //MessageObj.CC.Add(ApprovedUserEmailadd.ToString());
                      //  objErrorHandler.CreateLog("sp2");
                        MessageObj.To.Clear();
                            MessageObj.To.Add(ApprovedUserEmailadd.ToString());

                            smtpclient.Send(MessageObj);

                        
                       // objErrorHandler.CreateLog(MessageObj.To.ToString() + "sp2");
                        objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                       
                            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP ApprovedUserEmailadd");
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
                                    //objErrorHandler.CreateLog("sp3");
                                    //MessageObj.CC.Add(id.ToString());
                                    MessageObj.Subject = "WES Order Alert - Order No : " + CustomerOrderNo.ToString();
                                        MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);
                                  
                                   // objErrorHandler.CreateLog(MessageObj.To.ToString() + "sp3");
                                    objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                                   
                                   
                                        objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner11 OrderId=" + OrderId);
                                    }
                                }
                            }
                            else
                            {
                                if (ApprovedUserEmailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString() && Emailadd.ToUpper().ToString() != Adminemails.ToUpper().ToString())
                                {
                               // objErrorHandler.CreateLog("sp4");
                                MessageObj.To.Clear();
                                    MessageObj.To.Add(Adminemails.ToString());
                                    smtpclient.Send(MessageObj);


                                //objErrorHandler.CreateLog("sp4");
                                //objErrorHandler.CreateLog(MessageObj.To.ToString() + "sp4");


                                objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                               
                                
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner12 OrderId=" + OrderId);
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
                                   // objErrorHandler.CreateLog("sp6");
                                    MessageObj.To.Clear();
                                        MessageObj.To.Add(id.ToString());
                                        smtpclient.Send(MessageObj);


                                  
                                  //  objErrorHandler.CreateLog(MessageObj.To.ToString() + "sp6");

                                    objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                                   
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner13 OrderId=" + OrderId);
                                    }
                                }
                            }
                            else
                            {
                                if (Emailadd.ToUpper().ToString() != emails.ToUpper().ToString())
                                {
                               // objErrorHandler.CreateLog("sp7");
                                MessageObj.To.Clear();
                                    MessageObj.To.Add(emails.ToString());
                                    smtpclient.Send(MessageObj);
                              
                               // objErrorHandler.CreateLog(MessageObj.To.ToString() + "sp7");

                                objHelperServices.Mail_Log(paytype, oOrderInfo.OrderID, MessageObj.To.ToString(), "");

                               
                                    objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);
                                    //MessageObj.CC.Add(emails.ToString());
                                }
                            }

                        }


                    }


                }
            }
            catch (Exception ex)
            {


           

                objHelperServices.Mail_Error_Log(paytype, OrderId, toemail.ToString(), ex.Message, 0, objHelperServices.CI(0), Convert.ToInt16(0), 1);
               // objHelperServices.Mail_Log(paytype, OrderId, toemail.ToString(), ex.Message);

           
           
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP inner14 OrderId=" + OrderId);

            }
            objErrorHandler.CreatePayLog("SP SendMail_AfterPaymentSP end OrderId=" + OrderId);
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
        //protected string DecryptSP(string ordid)
        //{

        //    try
        //    {
        //        string enc = "";
        //        enc = HttpUtility.UrlDecode(ordid);
        //        enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //        enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //        enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //        enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //        enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //        return enc;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            Response.Redirect("OrderHistory.aspx");
        }
     
        //private void LoadIds()
        //{

        //    string id = "";
        //    //if (Request["key"] != null)
        //    //{
        //    //    id = Request["key"].ToString();
        //    //    id = DecryptSP(id);
        //    //}
        //    if (HttpContext.Current.Session["P_Oid"] != null)
        //        id = HttpContext.Current.Session["P_Oid"].ToString();

        //    if (id != null)
        //        OrderID = Convert.ToInt32(id);
        //    else
        //    {                
        //        div1.InnerHtml = "Invalid Data" + strPaymentLink;
        //        return;

        //    }
        //    //oPayInfo = objPaymentServices.GetPayment(OrderID);
        //    //PaymentID = oPayInfo.PaymentID;
        //}
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
        public string  BuildBillAddress()
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

        public string GetShippingAddress(int OrderID, OrderServices.OrderInfo oOrderInfo)
        {
            string sShippingAddress = "";
            OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
        //oOI = objOrderServices.GetOrder(OrderID);
        oOI = oOrderInfo;

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

        public string GetBillingAddress(int OrderID, OrderServices.OrderInfo oOrderInfo)
        {
            string sBillingAddress = "";
            OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();

        //oBI = objOrderServices.GetOrder(OrderID);
        oBI = oOrderInfo;
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
        #endregion
    }
