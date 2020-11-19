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



    public partial class PayOnlineCC : System.Web.UI.Page
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

        PayPalService objPayPalService = new PayPalService();
     
        Security objSecurity = new Security();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
       

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
        string renUrl =HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
        
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
       
        protected void Page_Load(object sender, EventArgs e)
        {
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
                LoadIds();

                oUserInfo = objUserServices.GetUserInfo(UserID);

                //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower()=="au" )
                //{
                //    div1.InnerHtml = "";
                //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                //    return;
                //}


                if (IsPostBack)
                {

                    btnPay.Style.Add("display", "none");
                    ImgBtnEditShipping.Style.Add("display", "none");
                    BtnProgress.Style.Add("display", "block");

                    return;
                }




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
                    //lbltotalamt.Text = oOrderInfo.TotalAmount.ToString();

                    // lblorderid.Text = OrderID.ToString();
                    //lblOrderNo.Text = " : " + oPayInfo.PORelease;
                    LoadOrderItem();
                    //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                    //renUrl = renUrl + "?key=" + EncryptSP(OrderID.ToString()) + "&";
                }

                if (Session["ispaypal"] != null)
                {
                    Session["ispaypal"] = null;
                    btnPay_Click(sender, e);
                }
                if (oPayInfo.PayResponse.ToLower() == "yes")
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                    return;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
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
          //  Response.Redirect("PayOnlineCC.aspx?" + EncryptSP(OrderID.ToString()), false);
        }
        protected void btnBankTrasfer_Click(object sender, EventArgs e)
        {
         //   Response.Redirect("PayOnline_BankTrasfer.aspx?" + OrderID.ToString() + "#####" + "PaySP", false);
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
         
        protected void btnPay_Click(object sender, EventArgs e)
        {

            try
            {

                LoadIds();
                //renUrl = renUrl.Replace("PayOnlineCC", "BillInfo");
                //renUrl = renUrl + "?Paytype=submitorder&key=" + EncryptSP(OrderID.ToString());


                string returnurl = "/Billinfo.aspx?Paytype=submitorder&key=" + EncryptSP(OrderID.ToString() + "#####" + "PayPP" + "#####" + "Paid");
                renUrl = renUrl.Replace(Request.Url.AbsolutePath, returnurl);
                objErrorHandler.CreatePayLog(renUrl);
                oOrderInfo = objOrderServices.GetOrder(OrderID);
                oUserInfo = objUserServices.GetUserInfo(UserID);

                //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
                //{
                //    div1.InnerHtml = "";
                //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
                //    return;
                //}
                if (oPayInfo.PayResponse.ToLower() == "yes")
                {
                    divContent.InnerHtml = "Already payment has been made , Ref. Payment History" + strPaymentLink;
                    return;
                }

                string Requeststr = objPayPalService.PayPalInitRequest(OrderID, PaymentID, oOrderInfo, renUrl);

                if (Requeststr.Contains("Form") == false)
                    divContent.InnerHtml = Requeststr;
                else
                    this.Page.Controls.Add(new LiteralControl(Requeststr));

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
       

        private void LoadIds()
        {
            try
            {
                if (Request.Url.Query != null && Request.Url.Query != "")
                {

                    string id = Request.Url.Query.Replace("?", "");
                    id = DecryptSP(id);
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
                    div1.InnerHtml = "";
                    div2.InnerHtml = "Invalid Data" + strPaymentLink;
                    return;

                }
                // objErrorHandler.CreateLog("orderid:" + OrderID);
                oPayInfo = objPaymentServices.GetPayment(OrderID);
                PaymentID = oPayInfo.PaymentID;
            }
            catch(Exception ex)
            {}
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
                lblAmount.Text = oPayInfo.Amount.ToString();
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
                    sBillAdd = sBillAdd +"<br>" +WrapText(oOrderInfo.BillAdd1) + "<br>";
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
                    sShippAdd = sShippAdd + WrapText(oOrderInfo.ShipLName) + "<br>";
                }
                if (oOrderInfo.ShipAdd1 != null)
                {
                    sShippAdd = sShippAdd + "<br>" + WrapText(oOrderInfo.ShipAdd1) + "<br>";
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


