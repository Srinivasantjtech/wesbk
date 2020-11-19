using System;
using System.Collections.Generic;
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


    public partial class payonline : System.Web.UI.Page
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
        Security  objSecurity = new Security();
        const string EnDekey = "WES@SecuryPAY@dm1n@123";

        public int OrderID = 0;
        public int PaymentID = 0;
        DataSet dsOItem = new DataSet();
        DataSet dsOItem1 = new DataSet();
     
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                Response.Buffer = true;
                //Response.ExpiresAbsolute = DateTime.Now;
                Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);


                Response.Expires = 0;

                Response.AddHeader("pragma", "no-cache");
                Response.AddHeader("cache-control", "private");
                Response.CacheControl = "no-cache";

                if (IsPostBack)
                    return;

                if (Session["XpayC"] != null)
                {
                    if (Convert.ToInt32(Session["XpayC"]) > 3)
                    {
                        div1.InnerHtml = "";
                        div2.InnerHtml = "More than 3 att.";
                    }
                    else
                        Session["XpayC"] = Convert.ToInt32(Session["XpayC"]) + 1;
                }
                else
                    Session["XpayC"] = 0;


                if (Request["OrdId"] != null)
                {
                    OrderID = objHelperServices.CI(Request["OrdId"].ToString());
                }
                oPayInfo = objPaymentServices.GetPayment(OrderID);
                PaymentID = oPayInfo.PaymentID;
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
                    lblOrderNo.Text = " : " + oPayInfo.PORelease;
                    LoadOrderItem();


                    if (Session["Pay"] != null)
                    {
                        if (Session["Pay"].ToString() == "End")
                        {
                            initcontrol();
                            Session["Pay"] = "Start";
                        }
                        else
                        {

                            div1.InnerHtml = "";
                            div2.InnerHtml = "Session Already stared ,Pls close the popup window";

                        }

                    }
                    else
                    {
                        initcontrol();
                        Session["Pay"] = "Start";
                    }


                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            
         }
        private void initcontrol()
        {
            txtCardNumber.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
            txtCardCVVNumber.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
            LoadCardList();
            for (int y = DateTime.Now.Year; y <= DateTime.Now.Year + 15; y++)
            {
                drpExpyear.Items.Add(y.ToString());
            }
        }
        protected void OnClick_Pay(object sender, EventArgs e)
        {
            string rtnstr = "";

            PayOnlineService.PaymentRequestInfo objPRInfo = new PayOnlineService.PaymentRequestInfo();
            try
            {
                if (Convert.ToInt32(Session["XpayC"]) > 3)
                {
                    div1.InnerHtml = "";
                    div2.InnerHtml = "More than 3 att. try again";
                }
                else
                {
                   // objPRInfo = objPayOnlineService.GetPaymentRequest(OrderID, PaymentID, txtCardName.Text, txtCardNumber.Text, txtCardCVVNumber.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
                   
                   if (objPRInfo.Error_Text != "")
                   {                       
                       div2.InnerHtml = rtnstr;
                   }
                   else
                   {
                       Session["Pay"] = "End";
                       div1.InnerHtml = "";
                       div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX "+OrderID.ToString() + " Payment successfully" ;
                       
                   }

                }
                
            }
            catch(Exception ex)
            {
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
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
        public void LoadCardList()
        {
            try
            {
                DataSet oDs = new DataSet();
                oDs = objPayOnlineService.GetCardList();
                drppaymentmethod.Items.Clear();
                drppaymentmethod.DataSource = oDs;
                drppaymentmethod.DataValueField = oDs.Tables[0].Columns["CARD_ID"].ToString();
                drppaymentmethod.DataTextField = oDs.Tables[0].Columns["CARD_TYPE"].ToString();
                drppaymentmethod.DataBind();

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
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
