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
public partial class MakeOpenOrderMail : System.Web.UI.Page
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
        

    //     protected string EncryptSP(string ordid)
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
            
            try
            {
                int rtnmail = 0;
                DataTable temptbl = objOrderServices.Get_Order_Open_Status_Details();
                if (temptbl != null && temptbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in temptbl.Rows)//For Records
                    {
                        try
                        {
                            rtnmail = SendMail(Convert.ToInt32(dr["ORDER_ID"].ToString()), Convert.ToInt32(dr["ORDER_STATUS"].ToString()));
                           
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

        private int SendMail(int OrderId, int OrderStatus)
        {
            try
            {

              //  string BillAdd;
               // string ShippAdd;
                string stemplatepath;
                DataSet dsOItem = new DataSet();
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();

               // oPayInfo = objPaymentServices.GetPayment(OrderId);
                oOrderInfo = objOrderServices.GetOrder(OrderId);

                int UserID = oOrderInfo.UserID; //objHelperServices.CI(Session["USER_ID"].ToString());

                //oUserInfo = objUserServices.GetUserInfo(UserID);
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
                dsOItem = objOrderServices.GetOrderItems(OrderId);
               // BillAdd = GetBillingAddress(OrderId);
              //  ShippAdd = GetShippingAddress(OrderId);

               // string ShippingMethod = oOrderInfo.ShipMethod;
               // string CustomerOrderNo = oPayInfo.PORelease;
              //  string shippingnotes = oOrderInfo.ShipNotes;

                if (dsOItem == null || dsOItem.Tables.Count <= 0)
                    return 0;


                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
              //  string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
              //  string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
                string Emailadd = oUserInfo.AlternateEmail;


                string url = HttpContext.Current.Request.Url.Authority.ToString();
               // string PendingorderURL = "";// string.Format("http://" + url + "/PendingOrder.aspx");

                int ModifiedUser = objHelperServices.CI(oOrderInfo.ModifiedUser);
                oUserInfo = objUserServices.GetUserInfo(ModifiedUser);
                //string ApprovedUserEmailadd = oUserInfo.AlternateEmail;

               

               
                string sHTML = "";
                
                try
                {
                    StringTemplateGroup _stg_container = null;
                    StringTemplateGroup _stg_records = null;
                    StringTemplate _stmpl_container = null;
                    StringTemplate _stmpl_records = null;
                   
                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];

                   
                    TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                    TBWDataList1[] lstrows1 = new TBWDataList1[0];
                   
                    stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                  

                    DataSet dscat = new DataSet();
                   // DataTable dt = null;
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


                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderOpenStatus");

                   _stmpl_container.SetAttribute("ORDER_ID", oOrderInfo.OrderID );

                   _stmpl_container.SetAttribute("CONTACT", oUserInfo.Contact);
              
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);                                   

                     string url1 = HttpContext.Current.Request.Url.Authority.ToString();
                     url1 = url1 + "/shipping.aspx?OrderID=" + oOrderInfo.OrderID.ToString() + "&ApproveOrder=Approve";
                    // url1 = url1+ "/checkout.aspx?" +EncryptSP(oOrderInfo.OrderID.ToString());

                   _stmpl_container.SetAttribute("TBT_CHKOUT_URL", url1);  

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
                   

                    //sHTML = sHTML.Replace("PAY_METHOD", "http://" + url1 + "/checkout.aspx?" + oOrderInfo.OrderID.ToString()+ "#####" + "PaySP" ); 
                    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                    //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());
                    MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    MessageObj.Subject = "Your Items in Cart - WES Australasia";
                    MessageObj.IsBodyHtml = true;
                    MessageObj.Body = sHTML;
                    MessageObj.To.Add(Emailadd.ToString());

                    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                    smtpclient.UseDefaultCredentials = false;
                    smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                    smtpclient.Send(MessageObj);
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
       
        //protected string DecryptSP(string ordid)
        //{
        //    string enc = "";
        //    enc = HttpUtility.UrlDecode(ordid);
        //    enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //    enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //    enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //    enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //    enc = objSecurity.StringDeCrypt(enc, EnDekey);
        //    return enc;
        //}

    


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
    }
