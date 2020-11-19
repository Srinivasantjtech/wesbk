using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat;
using System.Data.SqlClient;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.Net.Mail;
using System.Web.Services;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using Braintree;
public partial class shipping : System.Web.UI.Page
{
    public string ClientToken = string.Empty;
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender Moda1Popalert = new AjaxControlToolkit.ModalPopupExtender();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    QuoteServices objQuoteServices = new QuoteServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
    UserServices.UserStatus userstatusinfo = new UserServices.UserStatus();
    UserServices objUserServices = new UserServices();
    CountryServices objCountryServices = new CountryServices();
    bool rbinternationaldefault_remote = false;
    CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
    Security objSecurity = new Security();
    SecurePayService objSecurePayService = new SecurePayService();
    PayPalService objPayPalService = new PayPalService();
    PayPalAPIService objPayPalApiService = new PayPalAPIService();
   public string emailconfimation = "";
    const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
    public int PaymentID = 0;
    public string PaytabType = "";
    string key = "";
    decimal shipcost = 0;
    bool idpayseldivvisible = false;
    //ConnectionDB objConnectionDB = new ConnectionDB();
    int QuoteStatusID = (int)QuoteServices.QuoteStatus.OPEN;
    public int OrderIDaspx = 0;
    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    string renUrl = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];
    OrderServices.OrderInfo oOrdInfo1 = new OrderServices.OrderInfo();
    string remoteweight = System.Configuration.ConfigurationManager.AppSettings["remoteweight"].ToString();
    string remoteweight_drop = System.Configuration.ConfigurationManager.AppSettings["remoteweight_drop"].ToString();

    string Lweight = System.Configuration.ConfigurationManager.AppSettings["Lweight"].ToString();
    public int OrderID = 0;
    int QuoteID = 0;
    int Userid;
    bool IsZipCodeChange = false;
    ListItem oLstItem = new ListItem();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    NotificationServices objNotificationServices = new NotificationServices();
    public bool Paytab = false;
    public bool paidtab = false;
    // ConnectionDB oCon = new ConnectionDB();
    string refid = "";
    // UserServices objUserServices = new UserServices();
    double SubTotal = 0.0;
    String UserList = "";
    bool chkRSpwd = false;
    int UsrStatus = (int)UserServices.UserStatus.ACTIVE;
    bool issecurepayclicked = false;

    

    protected void Page_PreInit()
    {
        if (httpRequestVariables()["tx"] != null)
        {
            //Page.MasterPageFile = "Blank.Master";
        }
        if (httpRequestVariables()["Token"] != null && httpRequestVariables()["PayerID"] != null)
        {
            //Page.MasterPageFile = "Blank.Master";
        }
    }
    protected NameValueCollection httpRequestVariables()
    {
        var post = Request.Form;       // $_POST
        var get = Request.QueryString; // $_GET
        return Merge(post, get);
    }
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

    public class ResponseValue {
        public string Status;
        public string Message;
        public string RedirectTo;
    }

    public class ShipInfo
    {
        public string tt1;
        public string drpSM1;
        public string TextBox1;
        public string paymenttype;
        public string paymenttypechecked;
        public string txtMobileNumber;
        public string cmbProvider;
        public string cardname;
        public string cardno;
        public string cvv;
        public string expmonth;
        public string expyr;
        public string lblshipingcost;
        public string shipcode;
        public string drpShipCompName;
        public string drpAttentionTo;
        public string drpaddress1;
        public string drpaddress2;
        public string drpsuburb;
        public string drpstate;
        public string drpcountry;
        public string drppostcode;
        public string drpinstruction;
        public string drpshipphone;
        public string Amount;
        public string nounce;
    }

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


            //     objErrorHandler.CreateLogEA("clientToken:" + this.ClientToken);
        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

       // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Call_dropshippopup();", true);
        //Added By Indu for wes online payment on 13-Oct-2017
        //GetPaymentTerm(Session["USER_ID"].ToString());
        try
        {

          
            // drpship_popup.Style.Add("display", "block");
            if (drpSM1.SelectedValue == "Courier Pick Up" || drpSM1.SelectedValue == "Shop Counter Pickup")
            {
                ImageButton2.Visible = false;
            }

            if (drpSM1.SelectedValue == "Drop Shipment Order" && txtSuburb.Text != "")
            {
                GenerateClientToken();
            }

            if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "")
            {
                Response.Redirect("Login.aspx");
            }
            if (Session["USER_ID"] != null)
            {
                if (Session["USER_ID"].ToString() == "999" || Session["USER_ID"].ToString() == "0")
                {

                    Response.Redirect("Login.aspx");
                }
            }
                

           
            if (Request["RPWD"] != null)
            {
                return;
            }
            int DuplicateItem_Prod_idCount = 0;
            string LeaveDuplicateProds = GetLeaveDuplicateProducts();

            int tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN;
            Userid = objHelperServices.CI(Session["USER_ID"]);

            txtSuburb.Attributes.Add("autocomplete", "new-password"); 
            if (string.IsNullOrEmpty(Request["OrderID"]))
            {
                OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
                Context.RewritePath("shipping.aspx?OrderID=" + OrderID);
                Session["ORDER_ID"] = OrderID;
            }
            else
            {
                Session["ORDER_ID"] = System.Convert.ToInt32(Request["OrderID"]);
                OrderID = System.Convert.ToInt32(Request["OrderID"]);
            }

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);

            }
            LoadShipPhoneNo(Session["USER_ID"].ToString());
            int i = objUserServices.GetCheckOutOption(Userid);
            if (i == 1)
            {
                divordermandatory.Visible = true;
                divmanorder.Visible = true;
                hftt1.Value = "1";
               // rftt1.Enabled = true;
               // moreinfoorder.Visible = true;
            }
            else
            {
                divordermandatory.Visible = false;
                divmanorder.Visible = false;
               // moreinfoorder.Visible = false;

            }
            string orderdstatus = objOrderServices.GetOrderStatus(OrderID);
            if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
            {
                //Modified by:indu--Reason,Pending order prb

                if (Session["USER_ROLE"].ToString() == "3")
                {
                    Session["ORDER_ID"] = 0;
                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY_ORDER", true);
                    return;
                }

                int tmpOrdStatus1 = (int)OrderServices.OrderStatus.CAU_PENDING;

                if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus1))
                {

                    Session["ORDER_ID"] = 0;
                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    return;
                }
            }


            DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds);
            DataTable tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "");

            if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
            {
                DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
            }

            if (!IsPostBack)
            {
                hfuserid.Value = Session["USER_ID"].ToString();
            }
            if (Session["USER_ID"] != null && hfuserid.Value.ToString().Trim() != "0" && Session["USER_ID"].ToString() != hfuserid.Value.ToString().Trim())
            {
                Response.Redirect("home.aspx");
            }  

               
            string custtype = Session["CUSTOMER_TYPE"].ToString();
            if (Convert.ToInt16(Session["USER_ROLE"]) == 4 && custtype == "Dealer")
            {
                Response.Redirect("home.aspx");
            }
            this.ModalPanel.Visible = false;
            this.modalPop.Hide();
            if (!IsPostBack)
            {
                GenerateClientToken();
            }

            if (Convert.ToInt16(Session["USER_ROLE"]) == 4 && custtype == "Retailer")
            {

                if (!IsPostBack)
                {
                  
                    var page = (Page)HttpContext.Current.Handler;
                    page.Title = "WES Alert";
                    this.ModalPanel.Visible = true;
                    modalPop.ID = "popUp";
                    modalPop.PopupControlID = "ModalPanel";
                    modalPop.BackgroundCssClass = "modalBackgroundshi";
                    modalPop.DropShadow = false;
                    modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                    this.ModalPanel.Controls.Add(modalPop);
                    this.modalPop.Show();
                    return;
                }

            }
            this.pnlResetPassAlert.Visible = false;
            this.Moda1Popalert.Hide();
            string RSpwd = "";

            if (Request["RPWD"] != null)
            {
                RSpwd = Request["RPWD"].ToString();
            }


            if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == 4 && RSpwd != "true")
            {
               // objErrorHandler.CreatePayLog("inside userstatus 4");
                // if (!IsPostBack)
                // {
                chkRSpwd = true;
                var page = (Page)HttpContext.Current.Handler;
                page.Title = "WES Alert";
                this.pnlResetPassAlert.Visible = true;
                Moda1Popalert.ID = "popUpAlert";
                Moda1Popalert.PopupControlID = "pnlResetPassAlert";
                Moda1Popalert.BackgroundCssClass = "modalBackgroundshi";
                Moda1Popalert.DropShadow = false;
                Moda1Popalert.TargetControlID = "btnHiddenTestPopupExtender";
                this.pnlResetPassAlert.Controls.Add(Moda1Popalert);
                this.Moda1Popalert.Show();
                return;
                // }
            }


           

            if (!(Session["PageUrl"].ToString().Contains("Confirm.aspx")))
            {
                Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
                Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
                if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString().ToUpper() == "YES")
                {
                  
                    if (!IsPostBack && (tbErrorItems == null && DuplicateItem_Prod_idCount == 0))
                    {
                        try
                        {
                            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
                            {
                                int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                                Userid = objHelperServices.CI(Session["USER_ID"]);
                                if (string.IsNullOrEmpty(Request["OrderID"]))
                                {
                                    OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                                }
                                else
                                {
                                    Session["ORDER_ID"] = System.Convert.ToInt32(Request["OrderID"]);
                                    OrderID = System.Convert.ToInt32(Request["OrderID"]);
                                }

                                if (Request["QteId"] != null)
                                    QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                                else
                                    QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);

                                if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                                    lblShoppingCart.Text = "Quote Cart";

                                //   if (OrderID == 0)
                                //  OrderID = objOrderServices.GetOrderIDForQuote(QuoteID);
                                //OrderID = objOrderServices.GetOrderIDForQuote(QuoteID);
                                decimal OrdtotCost = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));
                                decimal QtetotCost = objHelperServices.CDEC(objQuoteServices.GetCurrentProductTotalCost(QuoteID));

                                //   if ((OrdtotCost > 0) || (QtetotCost>0))

                                // if (((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0) || (QuoteID != 0 && objQuoteServices.GetQuoteItemCount(QuoteID) > 0)))
                                // && OrdtotCost > 0
                                if ((OrderID != 0 && objOrderServices.GetOrderItemCount(OrderID) > 0) || Request["QteFlag"] == "1")
                                {
                                    int a = 5;
                                    /////////New requirement//////////
                                    //tt1.Text = OrderID.ToString();
                                    // tt1.Enabled = false;
                                    string txtadd = "";
                                    LoadCountryList();

                                    Ta2.Value = LoadShippingInfostr(Session["USER_ID"].ToString());
                                    Ta3.Value = LoadBillInfostr(Session["USER_ID"].ToString());
                                    Ta4.Value = LoadBillInfostr(Session["USER_ID"].ToString());

                                    /////////New requirement//////////                            
                                    LoadShippingInfo(Session["USER_ID"].ToString());
                                    LoadBillInfo(Session["USER_ID"].ToString());
                                    //Added By Indu for wes online payment on 13-Oct-2017
                                    //if (!IsPostBack)
                                    //{
                                        GetPaymentTerm(Session["USER_ID"].ToString());
                                        showselectedtab_ondropdownchange();

                                    //}
                                    lblpaypaltotamt.Text = oPayInfo.Amount.ToString();

                                  

                                    tbNoItems.Visible = false;
                                    ShippingLink.Visible = false;
                                    lblRequired.Visible = true;
                                    LblStar.Visible = true;
                                    ChkShippingAdd.Visible = true;
                                    ChkShipDefaultaddr.Visible = true;
                                    ChkbillingAdd.Visible = true;
                                    ChkBillDefaultaddr.Visible = true;

                                    //  LoadStates("US");
                                  //  Load_UserRole(Session["USER_ID"].ToString());



                                }
                                // }
                                else
                                {

                                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", false);
                                    btnShipProceed.Enabled = false;
                                    tbNoItems.Visible = true;
                                    ShippingLink.Visible = true;
                                    lblRequired.Visible = false;
                                    LblStar.Visible = false;
                                    ChkShippingAdd.Visible = false;
                                    ChkShipDefaultaddr.Visible = false;
                                    ChkbillingAdd.Visible = false;
                                    ChkBillDefaultaddr.Visible = false;
                                    txtbilladd1.Enabled = false;
                                    txtbilladd2.Enabled = false;
                                    txtbilladd3.Enabled = false;
                                    txtbillcity.Enabled = false;
                                    txtbillFName.Enabled = false;
                                    txtbillLName.Enabled = false;
                                    txtbillphone.Enabled = false;
                                    txtbillzip.Enabled = false;
                                    txtSAdd1.Enabled = false;
                                    txtSAdd2.Enabled = false;
                                    txtSAdd3.Enabled = false;
                                    txtSCity.Enabled = false;
                                    txtSFName.Enabled = false;
                                    txtSLName.Enabled = false;
                                    txtSPhone.Enabled = false;
                                    txtSZip.Enabled = false;
                                    cmbShipMethod.Enabled = false;
                                    cmbProvider.Enabled = false;
                                    drpBillCountry.Enabled = false;
                                    drpBillState.Enabled = false;
                                    drpShipCountry.Enabled = false;
                                    drpShipState.Enabled = false;
                                }
                            }

                            HtmlMeta meta = new HtmlMeta();
                            meta.Name = "keywords";
                            meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
                            this.Header.Controls.Add(meta);

                            // Render: <meta name="Description" content="noindex" />
                            meta = new HtmlMeta();
                            meta.Name = "Description";
                            meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
                            this.Header.Controls.Add(meta);

                            // Render: <meta name="Abstraction" content="Some words listed here" />
                            meta.Name = "Abstraction";
                            meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
                            this.Header.Controls.Add(meta);

                            // Render: <meta name="Distribution" content="noindex" />
                            meta = new HtmlMeta();
                            meta.Name = "Distribution";
                            meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
                            this.Header.Controls.Add(meta);

                        }
                        catch (Exception Ex)
                        {
                            btnShipProceed.Enabled = false;
                            tbNoItems.Visible = true;
                            ShippingLink.Visible = true;
                            lblRequired.Visible = false;
                            LblStar.Visible = false;
                            ChkShippingAdd.Visible = false;
                            ChkShipDefaultaddr.Visible = false;
                            ChkbillingAdd.Visible = false;
                            ChkBillDefaultaddr.Visible = false;
                            objErrorHandler.ErrorMsg = Ex;
                            objErrorHandler.CreateLog();
                        }
                    }
                    else
                    {
                        //if (Session["ITEM_ERROR"] != null)
                        // {
                        //   if (Session["ITEM_ERROR"].ToString().Replace(",", "") != "")
                        //  {
                        //if (ErrItems != "" || ClrItems != "" || DuplicateItem_Prod_idCount > 0)
                        if ((tbErrorItems != null && tbErrorItems.Rows.Count > 0) || DuplicateItem_Prod_idCount > 0)
                        {
                            Session["ShowPop"] = "Yes";

                            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                            {
                                Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], true);
                            }
                            else
                            {
                                Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", true);
                            }
                        }
                        //}
                        // }


                    }
                }
                else
                {
                    Response.Redirect("ConfirmMessage.aspx?Result=NOECOMMERCE", false);
                }
                Load_UserRole(Session["USER_ID"].ToString());
                //For Using Enter Key
                txtSFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSAdd1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSAdd2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSAdd3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSCity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSPhone.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtSZip.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbillFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbillLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbilladd1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbilladd2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbilladd3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbillcity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbillphone.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
                txtbillzip.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnShipProceed.UniqueID + "').click();return false;}} else {return true}; ");
             
                tt1.Attributes.Add("onchange", "return isAlphabetic();");
                tt1.Attributes.Add("onblur", "return isAlphabetic();");
            }
            else
            {
                Response.Redirect("home.aspx", false);
            }


            if (!IsPostBack)
            {
                if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])) || Convert.ToInt16(Session["USER_ROLE"]) == 1)  // Drop shipment available as per user role
                {
                    drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));
                }
            }

            if (Request.QueryString["ApproveOrder"] != null && IsPostBack == false)
            {


                if (orderdstatus != Enum.GetName(typeof(OrderServices.OrderStatus), tmpOrdStatus))
                {
                    if (!IsPostBack)
                    {
                        GetApproveOrderDetails(OrderID);
                    }
                }
                else
                {
                    SetSessionVlaue();
                }
            }
            else
            {
                SetSessionVlaue();
            }
            if (drpSM1.SelectedValue == "Please Select Shipping Method")
            {
                trpm.Visible = false;
                Session["trpm"] = "false";
                ImageButton2.Visible = false;
                divpaymentoption.Style.Add("display", "none");

            }

            else
            {
                if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
                {
                   // objErrorHandler.CreateLog("on pageload dropship");
                    //if ((txtPostCode.Text == "") && (hfdrpstate_txt.Value != ""))

                    //{
                    //drpstate_txt.Text = hfdrpstate_txt.Value;
                    //txtPostCode.Text = hfPostCode.Value;
                    //}
                    if (txtSuburb.Text !="")
                    {

                        if (Session[txtSuburb.Text] != null)
                        { 
                            string[] shipx=Session[txtSuburb.Text] .ToString().Split('-');
                            drpstate_txt.Text = shipx[1];
                            txtPostCode.Text = shipx[0];
                        }
                    }
                   
                    Calculate_shipping();
                }
                if (hfisppp.Value == "1")
                {
                    lblppp.Text = "P";
                }
                else
                {
                    lblppp.Text = "";
                }

                if (!IsPostBack)
                {
                    if (drpSM1.SelectedValue.ToString().Trim() != "Drop Shipment Order")
                    {
                        Calculate_shipping();
                    }
                }
                // divpaymentoption.Visible = true;
            }
            if (objOrderServices.IsNativeCountry(OrderID) == 0) // is other then au
            {
                //objErrorHandler.CreateLog("inside Native country");
                drpSM1.Items.Clear();
                //drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                drpSM1.Items.Add(new ListItem("International Shipping - TBA", "International Shipping - TBA"));
                drpSM1.SelectedIndex = 0;
                divpaymentoption.Style.Add("display", "none");
                ImageButton2.Visible = true;
                if (!IsPostBack)
                {
                  //  objErrorHandler.CreateLog("Inside Native country post back");
                    divpaymentoption.Style.Add("display", "none");
                    divInternationalpayoption.Style.Add("display", "none");
                      
                }


                //if (rbinternationaldefault.Checked == true)
                //{
                //    // GetPaymentTerm(Userid.ToString() );
                //    divintdirdep.Style.Add("display", "block");
                //    divinternationalpaypal.Style.Add("display", "none");
                  
                //}
                //else if (rbinternationalpaypal.Checked == true)
                //{
                //    divintdirdep.Style.Add("display", "none");
                //    divinternationalpaypal.Style.Add("display", "block");
                //}

               // ImageButton2.Visible = true;
            //    HyperLink1.Visible = false;
                //PopDiv.
                //liPayOption.Visible = false;
                //liFinalReview.Visible = false;
                //ImageButton2.Text = "Submit Order";
            }
            else if (ispickuponly_zone(OrderID) == true && drpSM1.SelectedValue != "Please Select Shipping Method")
            {





                if (!IsPostBack)
                {
                    divpaymentoption.Style.Add("display", "none");
                    divInternationalpayoption.Style.Add("display", "none");
                    ImageButton2.Visible = true;
                    showPaymentType(Userid.ToString());
                  
                    string ZONE = GetZone(OrderID);
                    if (ZONE == "REMOTE")
                    {
                        decimal producttotalweight = 0;



                        DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                        if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < dsOD.Tables[0].Rows.Count; k++)
                            {
                                producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[k]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[k]["qty"].ToString()));
                                // objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

                            }
                        }
                        lblweight.Text = producttotalweight.ToString();
                        lblweight.Visible = true;
                        //objErrorHandler.CreateLog(lblweight.Text + "ONLOAD");
                        if ((producttotalweight < Convert.ToDecimal(remoteweight)))
                        {
                            divpaymentoption.Style.Add("display", "block");
                            ImageButton2.Visible = false;
                            trpm.Visible = false;
                            Session["trpm"] = "false";
                        }

                    }


                }



            }
             if ((ispickuponly_product(OrderID) == true) && (objOrderServices.IsNativeCountry(OrderID) == 1)  &&  (drpSM1.SelectedValue != "Please Select Shipping Method"  ))
            {
                lblppp.Text = "P"; 
                if (!IsPostBack)
                {
                    divpaymentoption.Style.Add("display", "none");
                    divInternationalpayoption.Style.Add("display", "none");
                    ImageButton2.Visible = true;
                    showPaymentType(Userid.ToString());
                    //drpSM1.Items.Clear();
                    //drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
                    //drpSM1.Items.Add(new ListItem("Courier Pick Up", "Courier Pick Up"));
                    //drpSM1.Items.Add(new ListItem("Shop Counter Pickup", "Shop Counter Pickup"));
                    //drpSM1.SelectedIndex = 0;
                }
              
            }
               


           
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:HidePanels();", true);

           // drpSM1.Attributes.Add("onclick", "javascript:CheckShippment();");
          //  drpSM1.Attributes.Add("onchange", "javascript:CheckShippment();");
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());   
        }

       


    }

    public bool ispickuponly_zone(int orderid)
    {


        try
        {
            string zipcode = objOrderServices.GetZipCode(orderid);
            if ((drpSM1.SelectedValue == "Drop Shipment Order") && (txtPostCode.Text != ""))
            {
                zipcode = txtPostCode.Text;

            }
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


    public static bool ispickuponly_zone(int orderid,string drpsm1,string shipzip)
    {

        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        UserServices objUserServices = new UserServices();
        try
        {
           string zipcode =shipzip;
            
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
            
            return false;
        }

    }
    private string GetZone(int orderid)
    {


        try
        {
            string zipcode = objOrderServices.GetZipCode(orderid);


            if ((drpSM1.SelectedValue == "Drop Shipment Order") && (txtPostCode.Text !="") )
            {
                zipcode = txtPostCode.Text;

            }
            lblzonezip.Text = zipcode;
          //  objErrorHandler.CreateLog("zipcode" + zipcode + "orderid" + orderid);
            if (zipcode != null)
            {
                DataSet ds = objUserServices.GetZONE(Convert.ToInt32(zipcode));
                if ((ds != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    return ds.Tables[0].Rows[0]["zone"].ToString().ToUpper();
                }
                else
                {

                    return "NOTFOUND";
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "";
        }

    }
    

          private string GET_PRODUCTWEIGHT(int orderid)
    {


        try
        {
            string zipcode = objOrderServices.GetZipCode(orderid);
            if (zipcode != null)
            {
                DataSet ds = objUserServices.GetZONE(Convert.ToInt32(zipcode));
                if ((ds != null) && (ds.Tables[0].Rows.Count > 0))
                {
                    return ds.Tables[0].Rows[0]["zone"].ToString().ToUpper();
                }
                else
                {

                    return "ISREMOTE";
                }
            }
            else
            {
                return "ISREMOTE";
            } 
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "";
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
    public static  bool ispickuponly_product_static(int orderid)
    {
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        UserServices objUserServices = new UserServices();
        try
        {

            DataSet dsOD = objOrderServices.GetOrderItems(orderid);

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
            //objErrorHandler.CreateLog(ex.ToString());
            return false;
        }
    }

    private void showPaymentType(string userid)
    {
        try
        {
            int i = 0;
            DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(userid));

            if (ds != null)
            {
                i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

            }

            if (i == 2 || i == 3 || i == 4 || i == 6)
            {
                trpm.Visible = true;
                Session["trpm"] = "true";
                divsubmitordertype.Visible = true;
                idpayseldivvisible = true;


                if (i == 2 || i == 3 || i == 6)
                {

                    // RBpaymenttype.Checked = true;

                    divofflinemastercard.Style.Add("display", "none");
                    if (RBOnPay.Checked != true)
                    {
                        RBOffpay.Checked = true;
                    }
                    divofflineonacc.Style.Add("display", "Block");

                }

                  //Credit Card
                else if (i == 4)
                {


                    string cardtype = ds.Tables[0].Rows[0]["CARD_TYPE"].ToString();


                    // RBpaymenttype.Checked = true;

                    divofflinemastercard.Style.Add("display", "block");
                    if (RBOnPay.Checked != true)
                    {
                        RBOffpay.Checked = true;
                    }
                    divofflineonacc.Style.Add("display", "none");

                    //if (cardtype=="VISA" || cardtype=="VTSA"  ||cardtype=="VIA")


                    //{
                    //    imgvisa.Visible = true;
                    //    imgmc.Visible = false;
                    //}

                    //else if (cardtype == "AMEX" || cardtype == "AMX" )
                    //{
                    //    imgamex.Visible = true;
                    //    imgmc.Visible = false;

                    //}
                    string x = "Pay on Master Card";
                    if (ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("4") == true)
                    {

                        imgofflinevisa.Visible = true;
                        imgofmaster.Visible = false;
                        x = "Pay on VISA";
                    }

                    else if ((ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("34") == true) || (ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("37") == true))
                    {

                        imgofflinevisa.Visible = true;
                        imgofmaster.Visible = false;
                        x = "Pay on Master Card";
                    }
                    else
                    {
                        imgofflinevisa.Visible = false;
                        imgofmaster.Visible = true;
                    }
                    divofflineonacc.Style.Add("display", "none");

                    lblofflinemastercard.Text = x + " " + ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().Substring(0, 4) + " - " + ds.Tables[0].Rows[0]["CR_CARDL3"].ToString();
                    lblofflinecarddate.Text = "Exp:" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(0, 2) + "/" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(2, 2);

                }


            }
            else
            {
                trpm.Visible = false;
                Session["trpm"] = "false";
                divsubmitordertype.Visible = false;
            }
        }
        catch (Exception ex)
        { 

        }
    
    }
    private void GetPaymentTerm(string userid)
    {

        try
        {
            //ImageButton2.Visible = false;
            divsubmitordertype.Visible = false;
            trpm.Visible = false;
            Session["trpm"] = "false";
            int i = 0;
            DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(userid));
       
            if (ds != null)
            {
                i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());
                //objErrorHandler.CreatePayLog("Userid--" + userid + "-----" + i.ToString());
            }
           
            divdirectdeposit.Style.Add("display", "block");

            //Cash
            try
            {
                if (i == 1 || i == 5 || i == 8 || i == 7)
                {
                    //objErrorHandler.CreateLog("Userid--" + userid + "-----" + "Bank Transfer");
                    RBpaymenttype.Text = "Bank Transfer";

                    Session["RBpaymenttype"] = "Bank Transfer";
                    divpayoaccount.Style.Add("display", "none");
                    divdirectdeposit.Style.Add("display", "block");
                    divmastercard.Style.Add("display", "none");

                    divD1.Visible = true;
                    divD2.Visible = false;
                    divD3.Visible = false;
                }
                //30 days
                else if (i == 2 || i == 3 || i == 6 )
                {
                    //objErrorHandler.CreateLog("inside by account" + i + "--" + userid);
                    RBpaymenttype.Text = "On Account";
                    Session["RBpaymenttype"] = "On Account";
                RBpaymenttype.Checked = true;
                    //objErrorHandler.CreateLog("Userid--" + userid + "-----" + "On Account");
                    divpayoaccount.Style.Add("display", "block");

                    divdirectdeposit.Style.Add("display", "none");
                    divmastercard.Style.Add("display", "none");
                
                    divD1.Visible = false;
                    divD2.Visible = true;
                    divD3.Visible = false;
                }
                //Credit Card
                else if (i == 4)
                {


                    string cardtype = ds.Tables[0].Rows[0]["CARD_TYPE"].ToString();


                   // RBpaymenttype.Checked = true;

                    divmastercard.Style.Add("display", "block");

                    //if (cardtype=="VISA" || cardtype=="VTSA"  ||cardtype=="VIA")


                    //{
                    //    imgvisa.Visible = true;
                    //    imgmc.Visible = false;
                    //}

                    //else if (cardtype == "AMEX" || cardtype == "AMX" )
                    //{
                    //    imgamex.Visible = true;
                    //    imgmc.Visible = false;

                    //}
                    //objErrorHandler.CreateLog("Userid--" + userid + "-----" + "mastercard");
                    if (ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("4") == true)
                    {
                        RBpaymenttype.Text = "VISA";
                        Session["RBpaymenttype"] = "VISA";
                        imgvisa.Visible = true;
                        imgmc.Visible = false;
                    }

                    else if ((ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("34") == true) || (ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().StartsWith("37") == true))
                    {
                        RBpaymenttype.Text = "AMEX";
                        Session["RBpaymenttype"] = "AMEX";
                        imgamex.Visible = true;
                        imgmc.Visible = false;

                    }
                    else
                    {
                        RBpaymenttype.Text = "Master Card";
                        Session["RBpaymenttype"] = "Master Card";
                        imgamex.Visible = false;
                        imgvisa.Visible = false;
                        imgmc.Visible = true;
                    }
                    divdirectdeposit.Style.Add("display", "none");
                    divpayoaccount.Style.Add("display", "none");
                    lblmastercardno.Text = ds.Tables[0].Rows[0]["CR_CARDF6"].ToString().Substring(0, 4) + " " + "xxxx" + " " + "xxxx" + " " + ds.Tables[0].Rows[0]["CR_CARDL3"].ToString();
                    Session["lblmastercardno"] = lblmastercardno.Text;
                    lblmasterexpirydate.Text = "Exp:" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(0, 2) + "/" + ds.Tables[0].Rows[0]["EXPIRY_DATE"].ToString().Substring(2, 2);
                    divD1.Visible = false;
                    divD2.Visible = false;
                    divD3.Visible = true;
                    RBCreditCard.Text = "New Credit Card";
                }
                //Cash on pickup
                //else if (i == 7)
                //{
                //    objErrorHandler.CreateLog("Userid--" + userid + "-----" + "Bank Deposit");
                //    RBpaymenttype.Text = "Bank Deposit";
                //    //  RBdefautpaymenttype.Checked = true;
                //    divmastercard.Style.Add("display", "none");
                //    divdirectdeposit.Style.Add("display", "none");
                //    divpayoaccount.Style.Add("display", "none");
                //    divD1.Visible = false;
                //    divD2.Visible = true;
                //    divD3.Visible = false;
                //    divcreditcard.Style.Add("display", "block");
                //    divdedault.Style.Add("display", "none");

                //}
                else if (i == 9)
                {
                    divpaymentoption.Visible = false;
                    divonlinesubmitordererror.Visible = true;
                    ImageButton2.Visible = false;
                    trpm.Visible = false;
                    Session["trpm"] = "false";
                    //   Response.Redirect("home.aspx");
                }
            }
            catch( Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
            
              string ZONE = GetZone(OrderID);
            if ((i == 1 || i == 5 || i == 7 || i == 9 || i==8))
                //|| (ZONE =="NOTFOUND"))
            {
              //  objErrorHandler.CreateLog("inside if"+ZONE+"Userid--" + userid + "-----" + i + "1,5,7,9,8");
                RBCreditCard.Checked = true;
                //RBPaypal.Checked = false;
                //RBpaymenttype.Checked = false;
                lbldefaultpayment.Style.Add("float", "right");

                divdedault.Style.Add("display", "none");
                divcreditcard.Style.Add("display", "block");
                divdedault.Style.Add("display", "none");

                divpaypal.Style.Add("display", "none");
                RBCreditCard.Checked = true;
              //  objErrorHandler.CreateLog("Userid--" + userid + "-----" + i + "1,5,7,9");

            }
            else
            {

                RBpaymenttype.Checked = true;
                divcreditcard.Style.Add("display", "none");
                divdedault.Style.Add("display", "block");
                divpaypal.Style.Add("display", "none");

              //  objErrorHandler.CreateLog("inside else");
             
            }
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            lbltotalamt.Text = oOrderInfo.TotalAmount.ToString();
            lbltotalamount1.Text = objHelperServices.FixDecPlace(objHelperServices.CDEC(oOrderInfo.ProdTotalPrice+ oOrderInfo.ShipCost+oOrderInfo.TaxAmount)).ToString();
            //hftotalamt.Value = oOrdInfo.TotalAmount.ToString();
            //objErrorHandler.CreateLog("before Native country");
            //if (objOrderServices.IsNativeCountry(OrderID) == 0) // is other then au
            //{
            //    objErrorHandler.CreateLog("inside Native country");
            //    drpSM1.Items.Clear();
            //    drpSM1.Items.Add(new ListItem("Please Select Shipping Method", "Please Select Shipping Method"));
            //    drpSM1.Items.Add(new ListItem("International Delivery", "International Delivery"));
            //    drpSM1.SelectedIndex = 1;
            //    divpaymentoption.Style.Add("display", "none");

            //    //liPayOption.Visible = false;
            //    //liFinalReview.Visible = false;
            //    //ImageButton2.Text = "Submit Order";
            //}
          int c=  objOrderServices.checkcardnoexsist(HttpContext.Current.Session["USER_ID"].ToString(),"", "","");
            objErrorHandler.CreateLog(c + "checkcardnoexsist");

            if (c== 1)
            {
                creditflag1.Style.Add("display", "block");
                creditflag2.Style.Add("display", "none");
                creditflag3.Visible = false;
                imgsecurepay.Src = "../images/cards_sm.png";
                //drpExpyear.Items.Clear();
                //for (int y = DateTime.Now.Year; y <= DateTime.Now.Year + 20; y++)
                //{
                //    drpExpyear.Items.Add(y.ToString());
                //}
            }
            else
            {
                creditflag1.Style.Add("display", "none");
                creditflag2.Style.Add("display", "block");
                creditflag3.Visible = false;
                imgsecurepay.Src = "../images/cards_sm.png";
            }
            //else
            //{
            //    creditflag1.Visible = false;
            //    creditflag2.Visible = false;
            //    creditflag3.Visible = true;

            //}


        }
        catch (Exception ex)
        {
           
            objErrorHandler.CreateLog(ex.ToString());  
        }
    }
    public string GetLeaveDuplicateProducts()
    {

        try
        {
        string LeaveDuplicateProds = "";
        if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
        {
            LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();
            LeaveDuplicateProds = LeaveDuplicateProds.Replace("-", "");
            if (LeaveDuplicateProds.StartsWith(",") == true)
                LeaveDuplicateProds = LeaveDuplicateProds.Substring(1);

            if (LeaveDuplicateProds.EndsWith(",") == true)
                LeaveDuplicateProds = LeaveDuplicateProds.Substring(0, LeaveDuplicateProds.Length - 1);

        }
        return LeaveDuplicateProds;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty; 
        }
    }
    private void Load_UserRole(string UserName)
    {
        try
        {
            //Helper oHelper = new Helper();            
            //ConnectionDB oConStr = new ConnectionDB();
            //SqlConnection oCon = new SqlConnection(oConStr.ConnectionString.Replace("provider=SQLOLEDB;", ""));
            //SqlDataAdapter oDa = new SqlDataAdapter("SELECT COMPANY_ID, CONTACT, USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE USER_ID = '" + Session["USER_ID"] + "' ", oCon);
            //SqlDataAdapter oDa = new SqlDataAdapter("SELECT COMPANY_ID,CONTACT,USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE USER_ROLE IN (1,2) AND COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS Where USER_ID = '" + Session["USER_ID"] + "')", oCon);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Users");
            string userid = Session["USER_ID"].ToString();
            if (userid == "")
                userid = "0";
            int TotalUsers = 0;

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED", CompanyGroupDB.ReturnType.RTDataSet);
            if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            {

                oDs.Tables[0].TableName = "Users";
                //for (int i = 0; i < oDs.Tables["Users"].Rows.Count; i++)
                //{
                //    lblUserRoleName.Text = oDs.Tables["Users"].Rows[i][1].ToString();
                //}

                //string str1 = "";
                //str1 = "<table width=100% cellpadding=5 cellspacing=0 border=0 style=border-collapse><tr valign=top> <td align=left>";

                foreach (DataRow rItem in oDs.Tables["Users"].Rows)
                {
                    if (TotalUsers == 6)
                    {
                        UserList = UserList + ", "; //+ "<br/>";
                        TotalUsers = 0;
                    }

                    UserList = UserList + (TotalUsers == 0 ? "" : ", ") + rItem["CONTACT"].ToString();
                    TotalUsers = TotalUsers + 1;
                }

                //lblUserRoleName.Text = UserList.Substring(1,UserList.Length-1);
                //str1 = str1 + UserList.Substring(1, UserList.Length - 1) + "</td> </tr></table>";
                //lblUserRoleName.Text = str1;

                lblUserRoleName.Text = UserList.ToString();
            }
            else
                lblUserRoleName.Text = "";
        }
        catch (Exception ex)
        {

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


    private static string Get_ADMIN_APPROVED_UserEmils(int UserID)
    {
        DataSet oDs = new DataSet();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        string emails = "";

        string userid = HttpContext.Current.Session["USER_ID"].ToString();
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
    private static string Get_ADMIN_UserEmils(int UserID)
    {
        DataSet oDs = new DataSet();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        string emails = "";

        //string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //if (userid == "")
        //    userid = "0";


        try
        {

            oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(UserID.ToString(), "GET_COMPANY_USER_ADMIN_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
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
   

    
    #region "Functions"

    public string LoadShippingInfostr(string sUserID)
    {
        string str = "";
        try
        {
            int _UserID;
            string cmpName = "";
            _UserID = objHelperServices.CI(sUserID);
            UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);

            //objErrorHandler.CreateLog("_UserID" + "oOrdShippInfo");
            str = "DELIVERY TO : ";
            cmpName = objUserServices.GetCompanyName(_UserID);
            //objErrorHandler.CreateLog("cmpName" + cmpName);
            if (cmpName!="")
                str = str + "\n\n" + cmpName;
            else
                str = str + "\n" + cmpName;

            //objErrorHandler.CreateLog(str);

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.FirstName.Trim()) ? "" : "\n" + oOrdShippInfo.FirstName.Trim());

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.MiddleName.Trim()) ? "" : " " + oOrdShippInfo.MiddleName.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.LastName.Trim()) ? "" : " " + oOrdShippInfo.LastName);
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress1.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress1.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress2.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress2.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipAddress3.Trim()) ? "" : "\n" + oOrdShippInfo.ShipAddress3.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipCity.Trim()) ? "" : "\n" + oOrdShippInfo.ShipCity.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipState.Trim()) ? "" : "\n" + oOrdShippInfo.ShipState.Trim());
            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipZip.Trim()) ? "" : " - " + oOrdShippInfo.ShipZip.Trim());
            str = str + string.Format("\n {0}", oOrdShippInfo.ShipCountry);
            //objErrorHandler.CreateLog(str);
            //if (oOrdShippInfo.ShipCountry.Trim().Length < 3)
            //{
            //    drpShipCountry.SelectedValue = oOrdShippInfo.ShipCountry;

            //    str = str + "\n  " + (drpShipCountry.SelectedItem.Text != "(Select Country)" ? drpShipCountry.SelectedItem.Text : "");
            //}
            //else str += string.Format("\n {0}", oOrdShippInfo.ShipCountry);

            str = str + (string.IsNullOrEmpty(oOrdShippInfo.ShipPhone.Trim()) ? "" : "\nPhone No: " + oOrdShippInfo.ShipPhone.Trim());
            double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
            if (Dist <= 50 && Dist > 0)
            {
                oLstItem.Text = "Friendly Driver";
                oLstItem.Value = "FRIENDLYDRIVER";
                oLstItem.Selected = true;
                cmbProvider.Items.Add(oLstItem);
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        return str;
    }

    public string LoadBillInfostr(string sUserID)
    {
        string str = "";
        UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
        try
        {
            int _UserID;
            string strcmpName="";
            _UserID = objHelperServices.CI(sUserID);
            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserID);
            strcmpName=objUserServices.GetBillToCompanyName(_UserID);
            str = "BILL TO : ";
            if (strcmpName != "")
                str = str + "\n\n" + strcmpName;
            else
                str = str + "\n";
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.FirstName.Trim()) ? "" : "\n" + oOrdBillInfo.FirstName.Trim());
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.MiddleName.Trim()) ? "" : " " + oOrdBillInfo.MiddleName.Trim());
            //str = str + (string.IsNullOrEmpty(oOrdBillInfo.LastName.Trim()) ? "" : " " + oOrdBillInfo.LastName.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress1.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress1.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress2.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress2.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillAddress3.Trim()) ? "" : "\n" + oOrdBillInfo.BillAddress3.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillCity.Trim()) ? "" : "\n" + oOrdBillInfo.BillCity.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillState.Trim()) ? "" : "\n" + oOrdBillInfo.BillState.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillZip.Trim()) ? "" : " - " + oOrdBillInfo.BillZip.Trim());
            str = str + string.Format("\n{0}", oOrdBillInfo.BillCountry.Trim());
            //if (oOrdBillInfo.BillCountry.Trim().Length < 3)
            //{
            //    drpBillCountry.SelectedValue = oOrdBillInfo.BillCountry;
            //    str = str + (drpBillCountry.SelectedItem.Text != "(Select Country)" ? "\n" + drpBillCountry.SelectedItem.Text : "");
            //}
            //else str += string.Format("\n{0}", oOrdBillInfo.BillCountry.Trim());
            str = str + (string.IsNullOrEmpty(oOrdBillInfo.BillPhone.Trim()) ? "" : "\nPhone No: " + oOrdBillInfo.BillPhone.Trim());
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");
        str = str.Replace("\n  \n", "\n");

        return str;
    }
    public void LoadShipPhoneNo(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);

           
            oOrdInfo = objOrderServices.GetOrder(OrderID);
            if (oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
            {
                txtMobileNumber.Text = oOrdInfo.ShipPhone;
            }

            else if (oOrdShippInfo.MobilePhone != null && oOrdShippInfo.MobilePhone != "" && oOrdShippInfo.MobilePhone.Substring(0, 2) == "04" && oOrdShippInfo.MobilePhone.ToString().Trim().Length == 10)
            {

                // lblorderready.Text = oOrdShippInfo.MobilePhone;
                txtMobileNumber.Text = oOrdShippInfo.MobilePhone;

            }
            else if (oOrdShippInfo.Ship_Phone != null && oOrdShippInfo.Ship_Phone != "" && oOrdShippInfo.Ship_Phone.Substring(0, 2) == "04" && oOrdShippInfo.Ship_Phone.ToString().Trim().Length == 10)
            {

                //lblorderready.Text = oOrdShippInfo.Ship_Phone;
                txtMobileNumber.Text = oOrdShippInfo.Ship_Phone;


            }

            Session["txtMobileNumber"] = txtMobileNumber.Text;



        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadShippingInfo(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oOrdShippInfo = objUserServices.GetUserShipInfo(_UserID);

            txtSFName.Text = oOrdShippInfo.FirstName;
            txtSLName.Text = oOrdShippInfo.LastName;
            txtbillMName.Text = oOrdShippInfo.MiddleName;
            txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
            txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
            txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
            txtSCity.Text = oOrdShippInfo.ShipCity;
            drpShipState.Text = oOrdShippInfo.ShipState;
            txtSZip.Text = oOrdShippInfo.ShipZip;
            drpShipCountry.SelectedValue = oOrdShippInfo.ShipCountry;
            Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
            txtSPhone.Text = oOrdShippInfo.ShipPhone;
           
            oOrdInfo =  objOrderServices.GetOrder(OrderID);
            if (oOrdInfo.ShipPhone != null && oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone.Substring(0, 2) == "04" && oOrdInfo.ShipPhone.ToString().Trim().Length == 10)
            {
                txtMobileNumber.Text = oOrdInfo.ShipPhone;
            }

            else if (oOrdShippInfo.MobilePhone != null && oOrdShippInfo.MobilePhone != "" && oOrdShippInfo.MobilePhone.Substring(0, 2) == "04" && oOrdShippInfo.MobilePhone.ToString().Trim().Length == 10)
            {
              
               // lblorderready.Text = oOrdShippInfo.MobilePhone;
                txtMobileNumber.Text = oOrdShippInfo.MobilePhone;
               
            }
            else if (oOrdShippInfo.Ship_Phone != null && oOrdShippInfo.Ship_Phone != "" && oOrdShippInfo.Ship_Phone.Substring(0, 2) == "04" && oOrdShippInfo.Ship_Phone.ToString().Trim().Length == 10)
            {
              
                //lblorderready.Text = oOrdShippInfo.Ship_Phone;
                txtMobileNumber.Text = oOrdShippInfo.Ship_Phone;
            

            }
           

            double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
            if (Dist <= 50 && Dist > 0)
            {
                oLstItem.Text = "Friendly Driver";
                oLstItem.Value = "FRIENDLYDRIVER";
                oLstItem.Selected = true;
                cmbProvider.Items.Add(oLstItem);
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadBillInfo(string sUserID)
    {
        UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oOrdBillInfo = objUserServices.GetUserBillInfo(_UserID);
            txtbillFName.Text = oOrdBillInfo.FirstName;
            txtbillLName.Text = oOrdBillInfo.LastName;
            txtbillMName.Text = oOrdBillInfo.MiddleName;
            txtbilladd1.Text = oOrdBillInfo.BillAddress1;
            txtbilladd2.Text = oOrdBillInfo.BillAddress2;
            txtbilladd3.Text = oOrdBillInfo.BillAddress3;
            txtbillcity.Text = oOrdBillInfo.BillCity;
            drpBillState.Text = oOrdBillInfo.BillState;
            txtbillzip.Text = oOrdBillInfo.BillZip;
            drpBillCountry.SelectedValue = oOrdBillInfo.BillCountry;
            Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
            txtbillphone.Text = oOrdBillInfo.BillPhone;
          
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void ClearShippingInfo()
    {
        txtSFName.Text = "";
        txtSLName.Text = "";
        txtSAdd1.Text = "";
        txtSAdd2.Text = "";
        txtSAdd3.Text = "";
        txtSCity.Text = "";
        txtSZip.Text = "";
        txtSPhone.Text = "";
    }

    public void ClearBillingInfo()
    {
        txtbilladd1.Text = "";
        txtbilladd2.Text = "";
        txtbilladd3.Text = "";
        txtbillcity.Text = "";
        txtbillFName.Text = "";
        txtbillLName.Text = "";
        txtbillphone.Text = "";
        txtbillzip.Text = "";
    }

    private void GetApproveOrderDetails(int OrderID)
    {
        try
        {
            DataSet dsOD = new DataSet();
            dsOD = objOrderServices.GetApproveOrderItems(OrderID);

            oOrdInfo1 = objOrderServices.GetOrder(OrderID);

            if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsOD.Tables[0].Rows)
                {
                    tt1.Text = string.IsNullOrEmpty(row["PO_RELEASE"].ToString()) ? "" : row["PO_RELEASE"].ToString();

                    if (row["SHIP_METHOD"].ToString() == "Drop Shipment Order" && Checkdrpdownlistvalue(drpSM1, row["SHIP_METHOD"].ToString()) == false)
                        drpSM1.Items.Add(new ListItem("Drop Shipment Order", "Drop Shipment Order"));

                    //drpSM1.SelectedValue = string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString();
                    //Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(row["SHIP_METHOD"].ToString()) ? "" : row["SHIP_METHOD"].ToString());

                    TextBox1.Text = string.IsNullOrEmpty(row["COMMENTS"].ToString()) ? "" : row["COMMENTS"].ToString();
                    if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                    {
                        txtCompany.Text = objHelperServices.Prepare(oOrdInfo1.ShipCompName);
                        txtAttentionTo.Text = objHelperServices.Prepare(oOrdInfo1.ShipFName);
                        txtAddressLine1.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd1);
                        txtAddressLine2.Text = objHelperServices.Prepare(oOrdInfo1.ShipAdd2);
                        txtSuburb.Text = objHelperServices.Prepare(oOrdInfo1.ShipCity);
                        drpstate_txt.Text = objHelperServices.Prepare(oOrdInfo1.ShipState);
                        txtCountry.Text = objHelperServices.Prepare(oOrdInfo1.ShipCountry);
                        txtPostCode.Text = objHelperServices.Prepare(oOrdInfo1.ShipZip);
                        // txtReceiverContactName.Text = objHelperServices.Prepare(oOrdInfo1.ReceiverContact);
                        txtDeliveryInstructions.Text = objHelperServices.Prepare(oOrdInfo1.DeliveryInstr);
                        txtShipPhoneNumber.Text = objHelperServices.Prepare(oOrdInfo1.ShipPhone);

                        if (objOrderServices.IsUserCanDropShip(objHelperServices.CI(Session["USER_ID"])) == false)  // Drop shipment available as per user role
                        {
                            txtCompany.Enabled = false;
                            txtAttentionTo.Enabled = false;
                            txtAddressLine1.Enabled = false;
                            txtAddressLine2.Enabled = false;
                            txtSuburb.Enabled = false;
                            drpstate_txt.Enabled = false;
                            txtCountry.Enabled = false;
                            txtPostCode.Enabled = false;
                            txtDeliveryInstructions.Enabled = false;
                            txtShipPhoneNumber.Enabled = false;
                            drpSM1.Enabled = false;
                            tt1.Enabled = false;
                            TextBox1.Enabled = false;
                        }
                    }




                }
            }
            else
            {
                SetSessionVlaue();
            }
        }
        catch (Exception ex)
        { 
        }
    }

    #endregion

    #region "Control Events"
    private void SetSessionVlaue()
    {
        if (!IsPostBack)
        {
            if (Session["ORDER_NO"] != null)
            {
                tt1.Text = Session["ORDER_NO"].ToString();
            }
            if (Session["SHIPPING"] != null)
            {

                Setdrpdownlistvalue(drpSM1, string.IsNullOrEmpty(Session["SHIPPING"].ToString()) ? "" : Session["SHIPPING"].ToString());
            }
            if (Session["DELIVERY"] != null)
            {

                TextBox1.Text = Session["DELIVERY"].ToString();
            }
            if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
            {
                if (Session["DROPSHIP"] != null)
                {
                    string[] cmpadd = Session["DROPSHIP"].ToString().Split(new string[] { "####" }, StringSplitOptions.None);
                    if (cmpadd.Length > 0)
                    {
                        txtCompany.Text = cmpadd[0].ToString();
                        txtAttentionTo.Text = cmpadd[1].ToString();
                        txtAddressLine1.Text = cmpadd[2].ToString();
                        txtAddressLine2.Text = cmpadd[3].ToString();
                        txtSuburb.Text = cmpadd[4].ToString();
                        drpstate_txt.Text = cmpadd[5].ToString();
                        txtCountry.Text = cmpadd[6].ToString();
                        txtPostCode.Text = cmpadd[7].ToString();
                        txtDeliveryInstructions.Text = cmpadd[8].ToString();
                        txtShipPhoneNumber.Text = cmpadd[9].ToString();
                        ConnectionDB objConnection = new ConnectionDB();
                        try
                        {
                            if ((txtPostCode.Text == "") && (txtSuburb.Text != ""))
                            {
                              

                                //string[] suburb = DName.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries);

                                using (SqlCommand cmd = new SqlCommand("select suburb,postcode,state from wes_postcode_au where  (Suburb +' , '+state+' '+ PostCode) = ''+@SearchText+''", objConnection.GetConnection()))
                                {



                                    cmd.Parameters.AddWithValue("@SearchText", txtSuburb.Text);

                                    SqlDataReader dr = cmd.ExecuteReader();

                                    while (dr.Read())
                                    {

                                        drpstate_txt.Text = dr["state"].ToString();

                                        txtPostCode.Text = dr["postcode"].ToString();


                                    }

                                }




                            }
                        }
                        catch (Exception ex)
                        { }
                        finally
                        {

                            objConnection.CloseConnection();
                        
                        }
                        if (Session["SHIPPING"] != null)
                        {
                            Calculate_shipping();

                        }
                    }
                }
            }
        }
    }

    protected void btnShipProceed_Click(object sender, EventArgs e)
    {
        try
        {
            QuoteServices objQuoteServices = new QuoteServices();
            int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            decimal TaxAmount;
            decimal ProdTotCost;
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            int QuoteId = 0;
            QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
            if (Request["OrderId"] != null)
                OrderID = objHelperServices.CI(Request["OrderId"].ToString());

           

            if (Request["QteId"] != null)
            {
                QuoteID = objHelperServices.CI(Request["QteId"].ToString());
                OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
                OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
            }

            //string status=objOrderServices.GetOrderStatus(OrderID);
            //if (status != "OPEN")
            //{
            //    if (QuoteId != 0)
            //    {
            //        OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteId));
            //        OrdStatus = (int)OrderServices.OrderStatus.PLACEQUOTE;
            //    }
            //}
            //    else
            //    {
            //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);

            //    }

            ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
           // decimal shipcost = calcShipcost();
            string calship = Calculate_shipping();
            string[] charge = calship.Split('-');

            decimal shipcost = objHelperServices.CDEC(charge[0]);
            string SHIP_CODE = charge[1]; 
       //     TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost, OrderID.ToString());
          
            decimal UpdRst = 0;
            oOrdInfo.OrderID = OrderID;
            // oOrdInfo.OrderStatus = OrdStatus;
            oOrdInfo.OrderStatus = OrdStatus;
            if (cmbProvider.SelectedValue != "Please Select")
            {
                oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
            }
            else
            {
                oOrdInfo.ShipCompany = "";
            }
            oOrdInfo.ShipMethod = drpSM1.SelectedValue;

            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            {
                oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
                oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
                oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
                oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
                oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
            //    objErrorHandler.CreateLog("drpstate_txt" + drpstate_txt.Text + "drpselectedval" + drpstate_txt.SelectedValue + "drpselectedtext" + drpstate_txt.SelectedItem);
                if (txtSuburb.Text != "")
                {

                    if (Session[txtSuburb.Text] != null)
                    {
                        string[] shipx = Session[txtSuburb.Text].ToString().Split('-');
                        drpstate_txt.Text = shipx[1];
                        txtPostCode.Text = shipx[0];
                    }
                }

              //  objErrorHandler.CreateLog("postcode" + txtPostCode.Text );
            
                oOrdInfo.ShipState = objHelperServices.Prepare(drpstate_txt.Text);
                oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
                oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
                //   oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
                oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);
            }
            else
            {
                oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
                oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
                oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
                oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
                oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
                oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
                oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.Text);
                oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
            }
            //LoadShipPhoneNo(Session["USER_ID"].ToString());
            if (drpSM1.SelectedValue.Trim() != "Counter Pickup")
            {
                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
            }
            else
            {
                if (Session["Nothanks"] == null || Session["Nothanks"] == "false")
                {
                    if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                    {
                        oOrdInfo.ShipPhone = objHelperServices.Prepare(oOrdInfo.ShipPhone);
                    }
                    else
                    {
                        oOrdInfo.ShipPhone = objHelperServices.Prepare(txtMobileNumber.Text);
                    }
                }
                else {
                    oOrdInfo.ShipPhone = "";
                }
              
            }
            oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);
            oOrdInfo.isEmailSent = false;
            oOrdInfo.isInvoiceSent = false;
            oOrdInfo.IsShipped = false;

            oOrdInfo.BillFName = objHelperServices.Prepare(txtbillFName.Text);
            oOrdInfo.BillLName = objHelperServices.Prepare(txtbillLName.Text);
            oOrdInfo.BillMName = objHelperServices.Prepare(txtbillMName.Text);
            oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtbilladd1.Text);
            oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtbilladd2.Text);
            oOrdInfo.BillAdd3 = objHelperServices.Prepare(txtbilladd3.Text);
            oOrdInfo.BillCity = objHelperServices.Prepare(txtbillcity.Text);
            oOrdInfo.BillState = objHelperServices.Prepare(drpBillState.Text);
            oOrdInfo.BillCountry = objHelperServices.Prepare(drpBillCountry.Text);
            oOrdInfo.BillZip = objHelperServices.Prepare(txtbillzip.Text);
            oOrdInfo.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
            oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);


            if (oOrdInfo.ShipCost == 0)
            {

                oOrdInfo.ShipCost = shipcost;
            }
          //  oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
              TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID)+ objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
            oOrdInfo.TaxAmount = TaxAmount;
            oOrdInfo.TotalAmount = ProdTotCost + TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
            oOrdInfo.TrackingNo = "";
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
            oOrdInfo.SHIP_CODE = SHIP_CODE;
            UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
            double Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
            if (UpdRst > 0)
            {
                Session["ShipCost"] = oOrdInfo.ShipCost;
                if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                {
                    Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                }
                else
                {
                    Response.Redirect("Payment.aspx?OrdId=" + OrderID, false);
                }
            }
            
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    //public decimal CalculateTaxAmount(decimal ProdTotalPrice)
    //{
    //    try
    //    {
    //        CountryServices objCountryServices = new CountryServices();
    //        OrderServices objOrderServices = new OrderServices();
    //        string BillState;
    //        string BillCountry;
    //        decimal RetTax = 0;
    //        if (objUserServices.GetTaxExempt(objHelperServices.CI(Session["USER_ID"])) == false)
    //        {

    //            //BillState = drpBillState.Text;
    //            //BillCountry = drpBillCountry.SelectedValue;
    //            //decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
    //            //RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));

    //            if (FIXED_TAX.ToUpper() == "TRUE")
    //            {

    //                decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
    //                RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }
    //            else
    //            {
    //                BillState = drpBillState.Text;
    //                BillCountry = drpBillCountry.SelectedValue;
    //                decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

    //                RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }

    //            return RetTax;
    //        }
    //        return RetTax;
    //    }
    //    catch (Exception Ex)
    //    {
    //        objErrorHandler.ErrorMsg = Ex;
    //        objErrorHandler.CreateLog();
    //        return -1;
    //    }
    //}

    protected void ChkShippingAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkShippingAdd.Checked == false)
            {
                ClearShippingInfo();
            }
            else
            {
                LoadShippingInfo(Session["USER_ID"].ToString());
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void ChkbillingAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkbillingAdd.Checked == false)
            {
                ClearBillingInfo();
            }
            else
            {
                LoadBillInfo(Session["USER_ID"].ToString());
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    //Update User table Shipping Address
    protected void ChkDefaultShipAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkShipDefaultaddr.Checked == true)
            {
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oOrdShipAddr = new UserServices.UserInfo();
                oOrdShipAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdShipAddr.FirstName = objHelperServices.Prepare(txtSFName.Text);
                oOrdShipAddr.LastName = objHelperServices.Prepare(txtSLName.Text);
                oOrdShippInfo.MiddleName = objHelperServices.Prepare(txtSMName.Text);
                oOrdShipAddr.ShipAddress1 = objHelperServices.Prepare(txtSAdd1.Text);
                oOrdShipAddr.ShipAddress2 = objHelperServices.Prepare(txtSAdd2.Text);
                oOrdShipAddr.ShipAddress3 = objHelperServices.Prepare(txtSAdd3.Text);
                oOrdShipAddr.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                oOrdShipAddr.ShipState = objHelperServices.Prepare(drpShipState.Text);
                oOrdShipAddr.ShipCountry = objHelperServices.Prepare(drpShipCountry.Text);
                oOrdShipAddr.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                oOrdShipAddr.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                objUserServices.UpdateShippingInfo(oOrdShipAddr);
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }
    //Update User table Billing Address
    protected void ChkDefaultBillAdd_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkBillDefaultaddr.Checked == true)
            {
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oOrdBillAddr = new UserServices.UserInfo();
                oOrdBillAddr.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdBillAddr.FirstName = objHelperServices.Prepare(txtbillFName.Text);
                oOrdBillAddr.LastName = objHelperServices.Prepare(txtbillLName.Text);
                oOrdBillAddr.MiddleName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdBillAddr.BillAddress1 = objHelperServices.Prepare(txtbilladd1.Text);
                oOrdBillAddr.BillAddress2 = objHelperServices.Prepare(txtbilladd2.Text);
                oOrdBillAddr.BillAddress3 = objHelperServices.Prepare(txtbilladd3.Text);
                oOrdBillAddr.BillCity = objHelperServices.Prepare(txtbillcity.Text);
                oOrdBillAddr.BillState = objHelperServices.Prepare(drpBillState.Text);
                oOrdBillAddr.BillCountry = objHelperServices.Prepare(drpBillCountry.Text);
                oOrdBillAddr.BillZip = objHelperServices.Prepare(txtbillzip.Text);
                oOrdBillAddr.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
                objUserServices.UpdateBillingInfo(oOrdBillAddr);
            }
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadCountryList()
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpShipCountry.Items.Clear();
            drpShipCountry.DataSource = oDs;
            drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpShipCountry.DataBind();
            drpShipCountry.Items.Add(new ListItem("(Select Country)", "", true));
            //oDs = new DataSet();
            //oDs = objCountryServices.GetCountries();
            drpBillCountry.Items.Clear();
            drpBillCountry.DataSource = oDs;
            drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpBillCountry.DataBind();
            drpBillCountry.Items.Add(new ListItem("(Select Country)", "", true));
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    public void LoadStates(String conCode)
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            // drpShipState.DataSource = oDs;
            // drpShipState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpShipState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //  drpShipState.DataBind();

            oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            // drpBillState.DataSource = oDs;
            // drpBillState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpBillState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //drpBillState.DataBind();
        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    protected decimal CalculateShippingCost(int OrderID)
    {
        decimal ShippingValue = 0;
        try
        {
            DataSet dsOItem = new DataSet();
            dsOItem = objOrderServices.GetOrderItems(OrderID);
            decimal ProductCost;

            if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        ProductCost = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"]) * objHelperServices.CDEC(rItem["QTY"]);
                        DataSet DS = new DataSet();
                        DS = objOrderServices.GetItemDetailsFromInventory(Convert.ToInt32(rItem["PRODUCT_ID"]));
                        foreach (DataRow oDR in DS.Tables[0].Rows)
                        {
                            if (objHelperServices.CB(oDR["IS_SHIPPING"]) == 1)
                            {
                                ShippingValue = ShippingValue + ((ProductCost * objHelperServices.CDEC(oDR["PROD_SHIP_COST"])) / 100);
                                ShippingValue = objHelperServices.CDEC(ShippingValue);
                            }
                        }
                    }
                }
                return ShippingValue;
            }
            else
            {
                if (objOrderServices.GetCurrentProductTotalCost(OrderID) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
                {
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            ProductCost = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"]) * objHelperServices.CDEC(rItem["QTY"]);
                            ShippingValue = ShippingValue + ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                            ShippingValue = objHelperServices.CDEC(ShippingValue);
                        }
                    }
                }
                return ShippingValue;
            }
        }
        catch (Exception ex)
        { 
        }
        return ShippingValue;
    }


    #endregion

    protected void txtSZip_TextChanged(object sender, EventArgs e)
    {
        IsZipCodeChange = true;
    }

    protected void ImageButton5_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
            {

                if (drpstate_txt.Text == "")
                {
                    drpstate_txt.Text = hfdrpstate_txt.Value;
                }
                if (txtPostCode.Text == "")
                {
                    txtPostCode.Text = hfPostCode.Value;
                }

                Session["ORDER_NO"] = tt1.Text.Trim();
                Session["SHIPPING"] = drpSM1.Text.Trim();
                Session["DELIVERY"] = TextBox1.Text.Trim();
                Session["DROPSHIP"] = txtCompany.Text.Trim() + "####" + txtAttentionTo.Text.Trim()
                        + "####" + txtAddressLine1.Text.Trim()
                        + "####" + txtAddressLine2.Text.Trim()
                        + "####" + txtSuburb.Text.Trim()
                        + "####" + drpstate_txt.Text.Trim()
                        + "####" + txtCountry.Text.Trim()
                        + "####" + txtPostCode.Text.Trim()
                        + "####" + txtDeliveryInstructions.Text.Trim()
                        + "####" + txtShipPhoneNumber.Text.Trim()
                    ;


              

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
                }
            }

        }
        catch (Exception Ex)
        {

        }
    }


    //protected void Txtpostcode_textchange(object sender, EventArgs e)
    //{
    //    //try
    //    //{

    //    //    ConnectionDB objConnection = new ConnectionDB();

    //    //    using (SqlCommand cmd = new SqlCommand("select distinct state from wes_postcode_au where postcode = '"+ txtPostCode.Text +"'", objConnection.GetConnection()))
    //    //    {



              

    //    //        SqlDataReader dr = cmd.ExecuteReader();

    //    //        while (dr.Read())
    //    //        {
    //    //           drpstate_txt.SelectedValue=dr["state"].ToString();
    //    //            dr["postcode"].ToString();


    //    //        }
    //    //    }
    //    //}
    //    //catch (Exception ex)
    //    //{ }

    //    Calculate_shipping();
    //}

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        try
        {

          
            if (drpstate_txt.Text == "")
            {
                drpstate_txt.Text = hfdrpstate_txt.Value;
            }
            if (txtPostCode.Text == "")
            {
                txtPostCode.Text = hfPostCode.Value;
            }
            Session["ORDER_NO"] = tt1.Text.Trim();
            Session["SHIPPING"] = drpSM1.Text.Trim();
            Session["DELIVERY"] = TextBox1.Text.Trim();
            Session["DROPSHIP"] = txtCompany.Text.Trim() + "####" + txtAttentionTo.Text.Trim()
                    + "####" + txtAddressLine1.Text.Trim()
                    + "####" + txtAddressLine2.Text.Trim()
                    + "####" + txtSuburb.Text.Trim()
                    + "####" + drpstate_txt.Text.Trim()
                    + "####" + txtCountry.Text.Trim()
                    + "####" + hfPostCode.Value.Trim()
                    + "####" + txtDeliveryInstructions.Text.Trim()
                    + "####" + txtShipPhoneNumber.Text.Trim()
                ;

            RBpaymenttype.Checked = false;
            RBCreditCard.Checked = false;
            RBPaypal.Checked = false;
            if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
            {
                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"], false);
                }
                else
                {
                    Response.Redirect("orderDetails.aspx?&bulkorder=1&Pid=0", false);
                }
            }

        }
        catch (Exception Ex)
        {

        }
    }
    private bool Checkspecialchar(string instr, string allowchrs)
    {
        foreach (char c in instr)
        {
            if (char.IsLetterOrDigit(c.ToString(), 0) == false && allowchrs.Contains(c.ToString()) == false)
            {
                return true;
            }
        }
        return false;
    }
    protected void ImageButton4_Click(object sender, EventArgs e)
    {
        try
        {
            //if (drpSM1.SelectedValue == "Please Select Shipping Method")
            //{
            //    pnlMessageBox.Style.Add("display", "block");
            //    lblpopmessage.Text = "Please Select Shipping Method";
            //}
            if (Session["USER_ID"].ToString() == null || Session["USER_ID"].ToString() == "" || Session["USER_ID"].ToString() == "999")
            {
                Response.Redirect("login.aspx");
            }

            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            if (oOrderInfo.ProdTotalPrice == 0)
            {
          //  objErrorHandler.CreatePayLog("ImageButton4_Click start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }

            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (totalitemsum.ToString() == "0.00")
            {
                objErrorHandler.CreatePayLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }
            int i = 0;
            if (chkRSpwd == true)
                return;


                 //int i = objUserServices.GetCheckOutOption(Userid);
                 //if ((i == 1) && (tt1.Text == ""))
                 //{
                 //    txterr.Text = "Please Enter Order No";
                 //    lblMessage.Text = "Please Enter Order No";
                 //    hftt1.Value = "1"; 
                 //    mpeMessageBox.Show();
                   
                 //    if (objOrderServices.IsNativeCountry(OrderID) != 0)
                 //    {
                 //        divdedault.Style.Add("display", "block");
                 //        divcreditcard.Style.Add("display", "none");
                 //        divpaypal.Style.Add("display", "none");
                 //    }
                 //    OrderID = -1;
                 //    Session["OrderId"] = "-1";
                 //    return;

                 //}
                 //else
                 //{
                 //    hftt1.Value = "1"; 
                 //    //tt1.Style.Add("bor = "#73ACCF #88CEF9 #88CEF9 !important";
                 //}
            QuoteServices objQuoteServices = new QuoteServices();
            OrderDB objOrderDB = new OrderDB();
            HelperServices objHelperService = new HelperServices();
            //int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            int OrdStatus = 0;
            string ApproveOrder = string.Empty;
            //Direct  Order / Approve Order (Comes from Pending order Page)
            lblpostcode2err.Text = "";
            lbladdline1err.Text = "";
            lbladdline2err.Text = "";


            if (Request.QueryString["ApproveOrder"] == null)
            {
                if (Session["USER_ROLE"] != null)
                {
                    switch (Convert.ToInt16(Session["USER_ROLE"]))
                    {
                        case 1:

                            if (trpm.Visible == true)
                                {
                                    if (RBOffpay.Checked == true)
                                    {
                                        OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                    }
                                    else if (RBOnPay.Checked == true)
                                    {
                                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                    
                                    }
                                }

                           
                            else
                            {
                                if (RBpaymenttype.Text != "Bank Transfer")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                                else
                                {

                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                            }
                            break;
                        case 2:
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN ;
                            break;
                        case 3:
                            OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                            break;
                    }
                }
                else
                {
                    OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                }
            }
            else if (Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2))
            {
                if (trpm.Visible == true)
                {
                   
                        if (RBOffpay.Checked == true)
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }
                        else if (RBOnPay.Checked == true)
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                        }
                    
                }
                else
                {

                    if (RBOffpay.Checked == true)
                    {
                       
                        if (RBpaymenttype.Text == "Bank Transfer")
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                        }
                        else
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }
                    }
                    else if (RBOnPay.Checked == true)
                    {
                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                    }

                }
            }
            else if (Request.QueryString["ApproveOrder"] != null)
                OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

            //OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
            decimal TaxAmount;
            decimal ProdTotCost;
            if (string.IsNullOrEmpty(Request["OrderID"]))
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            else 
                OrderID = Convert.ToInt32(Request["OrderID"].ToString());


       

            int oldorderID = OrderID;
            //int QuoteId = 0;
            //QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));

            if (tt1.Text == "")
                tt1.Text = "WES" + OrderID.ToString();

            refid = objHelperServices.CS(tt1.Text);

            //else if (Checkspecialchar(tt1.Text,@"/\")==true)
            //{
            //    txterr.Text = "Special character not allowed";
            //    OrderID = -1;
            //    Session["OrderId"] = "-1";
            //}
            if (OrderID <= 0 || tt1.Text.Length==0)
            {
                txterr.Text = "Please enter valid Order No, Order No should be 1 digit or more than 1 digits";
                OrderID = -1;
                Session["OrderId"] = "-1";
            }
            else if (hfduporderproceed.Value == "1")
            {

            }
            else
            {
                txterr.Text = "";
                string querystr = "";

                if (Request.QueryString["ApproveOrder"] == null)
                {
                    DataSet DS = new DataSet();
                    //querystr = "select count(*) from TBWC_PAYMENT where order_id in(select order_id from dbo.tbwc_order where [user_id] in(select [user_id] from TBWC_COMPANY_BUYERS where company_id in(select company_id from dbo.TBWC_COMPANY_BUYERS where [user_id]=" + objHelperServices.CI(Session["USER_ID"].ToString()) + "))) and po_release='" + refid + "'";
                    //objHelperServices.SQLString = querystr;
                    //DS = objHelperServices.GetDataSet();
                    DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);

                    if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
                    {
                        txterr.Text = "Order No already exists, please Re-enter Order No";
                        OrderID = -1;
                    }
                }

            }

            //int coupon_id = 0;
            //if (txtCouponCode.Text.Trim().Length > 0)
            //{
            //    DataTable dscoupon = new DataTable();
            //    dscoupon = objOrderServices.GetCouponDetails(txtCouponCode.Text.Trim(), "GET_COUPON_IS_EXPIRY");
            //    if (dscoupon != null)
            //    {
            //        if (dscoupon.Rows.Count > 0)
            //        {
            //            if ((Convert.ToInt32(dscoupon.Rows[0]["IS_EXPIRY"].ToString()) < 0))
            //            {
            //                //coupon_id = Convert.ToInt32(dscoupon.Rows[0]["COUPON_ID"].ToString());
            //                lblcouponerrmsg.Visible = true;
            //                lblcouponerrmsg.Text = "Coupon Code is Expired.";
            //                // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Coupon Code is Expired.');</script>", false);
            //                txtCouponCode.Focus();
            //                // txtcoucode.BorderColor = System.Drawing.Color.Red;
            //                //ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //                txtCouponCode.Style.Add("border-color", "red red red !important");
            //                return;
            //            }
            //            else if ((Convert.ToInt32(dscoupon.Rows[0]["IS_EXPIRY"].ToString()) >= 0))
            //            {
            //                coupon_id = Convert.ToInt32(dscoupon.Rows[0]["COUPON_ID"].ToString());
            //                lblcouponerrmsg.Visible = false;
            //                lblcouponerrmsg.Text = "";
            //                txtCouponCode.Style.Add("border-color", "#73ACCF #88CEF9 #88CEF9 !important;");
            //            }

            //        }
            //        else
            //        {
            //            lblcouponerrmsg.Visible = true;
            //            lblcouponerrmsg.Text = "Invalid Coupon Code.";
            //            // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Invalid Coupon Code.');</script>", false);
            //            lblcouponerrmsg.Focus();
            //            txtCouponCode.Style.Add("border-color", "red red red !important");
            //            //ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        lblcouponerrmsg.Visible = true;
            //        lblcouponerrmsg.Text = "Invalid Coupon Code.";
            //        // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Invalid Coupon Code.');</script>", false);
            //        lblcouponerrmsg.Focus();
            //        txtCouponCode.Style.Add("border-color", "red red red !important");
            //        // ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //        return;
            //    }
            //}


            //if (Request["QteId"] != null)
            //{
            //    QuoteID = objHelperServices.CI(Request["QteId"].ToString());
            //    OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
            //    OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
            //}

            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            {
                if (objOrderServices.GetDropShipmentKeyExist(txtPostCode.Text.ToString(), "PostCode") == true)
                {
                    //lblpostcode2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                    //OrderID = -1;
                    //ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    if (objOrderServices.GetDropShipmentKeyExist(txtAddressLine1.Text.ToString(), "") == true)
                    {
                        //lbladdline1err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                        // ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>var x=window.confirm('Non-Standard Delivery Area. We will contact you to confirm costing');if (x)window.alert('Good!')</script>", false);

                    }
                    if (txtAddressLine2.Text.ToString() != "" && objOrderServices.GetDropShipmentKeyExist(txtAddressLine2.Text.ToString(), "") == true)
                    {
                        //lbladdline2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    }
                }
                //else
                //{
                //    OrderID = -1;
                //    ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                //}
            }

            //string status=objOrderServices.GetOrderStatus(OrderID);
            //if (status != "OPEN")
            //{
            //    if (QuoteId != 0)
            //    {
            //        OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteId));
            //        OrdStatus = (int)OrderServices.OrderStatus.PLACEQUOTE;
            //    }
            //}
            //    else
            //    {
            //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            //    }
            if (OrderID > 0)
            {
                ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
             // decimal shipcost=  calcShipcost();
               // string calship = Calculate_shipping();

                string calship = Session["shipcost"].ToString();
                string[] charge = calship.Split('-');

                decimal shipcost = objHelperServices.CDEC(charge[0]); ;
                string SHIP_CODE = charge[1]; 
             // TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost + shipcost, OrderID.ToString());
                TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());

                decimal UpdRst = 0;
                oOrdInfo.OrderID = OrderID;
                if (OrdStatus == 0)
                {
                    OrdStatus = 1;
                }
                oOrdInfo.OrderStatus = OrdStatus;
                ///////////////////////////
                int _UserrID;
                _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                txtSFName.Text = oOrdShippInfo.FirstName;
                txtSLName.Text = oOrdShippInfo.LastName;
                txtSMName.Text = oOrdShippInfo.MiddleName;
                txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
                txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
                txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
                txtSCity.Text = oOrdShippInfo.ShipCity;
                drpShipState.Text = oOrdShippInfo.ShipState;
                txtSZip.Text = oOrdShippInfo.ShipZip;
                Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
                //if (oOrdShippInfo.ShipCountry.Length > 3)
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByText(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByValue(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}

                txtSPhone.Text = oOrdShippInfo.ShipPhone;
                //double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
                //if (Dist <= 50 && Dist > 0)
                //{
                //    oLstItem.Text = "Friendly Driver";
                //    oLstItem.Value = "FRIENDLYDRIVER";
                //    oLstItem.Selected = true;
                //    cmbProvider.Items.Add(oLstItem);
                //}
                UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
                oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
               
                txtbillFName.Text = oOrdBillInfo.FirstName;
                txtbillLName.Text = oOrdBillInfo.LastName;
                txtbillMName.Text = oOrdBillInfo.MiddleName;
                txtbilladd1.Text = oOrdBillInfo.BillAddress1;
                txtbilladd2.Text = oOrdBillInfo.BillAddress2;
                txtbilladd3.Text = oOrdBillInfo.BillAddress3;
                txtbillcity.Text = oOrdBillInfo.BillCity;
                drpBillState.Text = oOrdBillInfo.BillState;
                txtbillzip.Text = oOrdBillInfo.BillZip;
                Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
                //if (oOrdBillInfo.BillCountry.Length > 3)
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByText(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByValue(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                txtbillphone.Text = oOrdBillInfo.BillPhone;
                ///////////////////////////
                oOrdInfo.OrderStatus = OrdStatus;
                if (cmbProvider.SelectedValue != "Please Select")
                {
                    oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
                }
                else
                {
                 oOrdInfo.ShipCompany ="";
              
                }
                oOrdInfo.ShipMethod = drpSM1.SelectedValue;
                //LoadShipPhoneNo(Session["USER_ID"].ToString());
                if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
                {
                    oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
                    //drpstate_txt.Text = hfdrpstate_txt.Value;
                    //txtPostCode.Text = hfPostCode.Value;
                    if (txtSuburb.Text != "")
                    {

                        if (Session[txtSuburb.Text] != null)
                        {
                            string[] shipx = Session[txtSuburb.Text].ToString().Split('-');
                            drpstate_txt.Text = shipx[1];
                            txtPostCode.Text = shipx[0];
                        }
                    }
                  //  objErrorHandler.CreateLog("postcode" + txtPostCode.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpstate_txt.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
                    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
                    //  oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
                    oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);

                }
                else
                {
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
                    oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
                    oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
                    oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.SelectedItem.ToString());
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                   // oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                    if (drpSM1.SelectedValue.Trim() != "Counter Pickup")
                    {
                        oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                    }
                    else
                    {
                        if (Session["Nothanks"] == null || Session["Nothanks"] == "false")
                        {

                            if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(oOrdInfo.ShipPhone);
                            }
                            else
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtMobileNumber.Text);
                            }
                        }
                        else
                        {
                            oOrdInfo.ShipPhone = "";
                        }
                    }
                    DataSet objds = new DataSet();
                    objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    {
                        oOrdInfo.DeliveryInstr = objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                    }

                }


                oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);

                
                    //if( oOrdInfo.ShipNotes==""  && txtCouponCode.Text !="" && coupon_id>0)
                    //    oOrdInfo.ShipNotes = "Coupon Code Entered:" + txtCouponCode.Text.Trim();
                    //else if( oOrdInfo.ShipNotes!=""  && txtCouponCode.Text !="" && coupon_id>0)
                    //    oOrdInfo.ShipNotes =oOrdInfo.ShipNotes +  Environment.NewLine + "Coupon Code Entered:" + txtCouponCode.Text.Trim();

                TextBox1.Text = oOrdInfo.ShipNotes;
 

                string strHostName = System.Net.Dns.GetHostName();
                string clientIPAddress = "";
                if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                {
                    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    else
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }


                oOrdInfo.ClientIPAddress = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
                //oOrdInfo.ClientIPAddress = Request.Params["REMOTE_ADDR"];
                //oOrdInfo.ClientIPAddress = Request.UserHostAddress;
                oOrdInfo.isEmailSent = false;
                oOrdInfo.isInvoiceSent = false;
                oOrdInfo.IsShipped = false;

                oOrdInfo.BillFName = objHelperServices.Prepare(txtbillFName.Text);
                oOrdInfo.BillLName = objHelperServices.Prepare(txtbillLName.Text);
                oOrdInfo.BillMName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtbilladd1.Text);
                oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtbilladd2.Text);
                oOrdInfo.BillAdd3 = objHelperServices.Prepare(txtbilladd3.Text);
                oOrdInfo.BillCity = objHelperServices.Prepare(txtbillcity.Text);
                oOrdInfo.BillState = objHelperServices.Prepare(drpBillState.Text);
                oOrdInfo.BillCountry = objHelperServices.Prepare(drpBillCountry.SelectedItem.Text);
                oOrdInfo.BillZip = objHelperServices.Prepare(txtbillzip.Text);
                oOrdInfo.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
                oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
                oOrdInfo.BillcompanyName = objHelperServices.Prepare(oOrdBillInfo.COMPANY_NAME);
               //// oOrdInfo.ShipCost = CalculateShippingCost(OrderID);
                if (oOrdInfo.ShipCost == 0)
                {

                    oOrdInfo.ShipCost = shipcost;
                }
                objErrorHandler.CreateLog("imagbutton4"+ SHIP_CODE);
                oOrdInfo.SHIP_CODE = SHIP_CODE;
                oOrdInfo.TaxAmount = TaxAmount;
                oOrdInfo.TotalAmount = ProdTotCost + TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                oOrdInfo.TrackingNo = "";
                if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                {
                    oOrdInfo.DropShip = 1;
                }
                oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
                string PAYMENTSELECTION = string.Empty;
                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                //    if (rbinternationaldefault.Checked == true )
                //    {
                //        PAYMENTSELECTION = "BT";
                //    }
                //    else if (rbinternationalpaypal.Checked == true)
                //    {
                //        PAYMENTSELECTION = "PP";
                //    }
                //    if (UpdRst > 0)
                //    {
                //        UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                //    }
                }

                else if (RBpaymenttype.Checked == true)
                {


                    DataSet ds = objUserServices.GetPaymentoption(_UserrID);
                    if (ds != null)
                    {
                        i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

                    }
                    if (i == 1 || i == 5 || i == 8 || i==7)
                    {

                        PAYMENTSELECTION = "BT";
                        if (UpdRst > 0)
                        {
                            UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                        }
                    }



                }

                else
                {



                    if (emailconfimation == "" && RBpaymenttype.Checked==true)
                    {
                        DataSet ds = objUserServices.GetPaymentoption(_UserrID);
                        if (ds != null)
                        {
                            i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

                        }
                        if (i == 1 || i == 5 || i == 8)
                        {

                            PAYMENTSELECTION = "BT";
                         
                        }
                    
                    }
                    //else if (rbinternationalpaypal.Checked == true)
                    //{
                    //    PAYMENTSELECTION = "PP";
                    //}
                    //else if (rbremotezone_sp.Checked == true)
                    //{
                    //    PAYMENTSELECTION = "SP";
                    //}
                    if (PAYMENTSELECTION != string.Empty)
                    {

                       if (UpdRst > 0)
                            {
                                UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                            }

                    }
                
                
                
                
                }
                //if (coupon_id > 0)
                //     objOrderServices.UpdateCouponId(coupon_id, OrderID, "UPDATE_ORDER_COUPON_ID");


                if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                {
                    Session["PrevOrderID"] = "0";
                }

               // Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
                if (UpdRst > 0)
                {
                    objOrderServices.UpdateCustomFields(oOrdInfo);


                    Session["ORDER_NO"] = null;
                    Session["SHIPPING"] = null;
                    Session["DELIVERY"] = null;
                    Session["DROPSHIP"] = null;

                    Session["ShipCost"] = oOrdInfo.ShipCost;
                    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                    {
                        Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                    }
                    else
                    {
                        ProceedFunction();
                        PnlOrderInvoice.Visible = true;
                        PnlOrderContents.Visible = false;
                        PHOrderConfirm.Visible = true;
                        try
                        {
                            if (RBpaymenttype.Checked == true)
                            {
                                lblgreenalert.Text = "Your order has been successfully submitted to us for processing using " + RBpaymenttype.Text + ". Thank You!";

                                if (lblmastercardno.Text == "")
                                {
                                    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using " + RBpaymenttype.Text + ". Thank You!";

                                }
                                else
                                {

                                    objErrorHandler.CreateLog(lblmastercardno.Text + "Master card " + lblmastercardno.Text.Length);
                                    if (lblmastercardno.Text.Length == 18)
                                    {
                                        lblgreenalert.Text = "Your order has been successfully submitted to us for processing using " + RBpaymenttype.Text + " ending with " + lblmastercardno.Text.Substring(15, 3) + ". Thank You!";
                                    }
                                    else if (lblmastercardno.Text.Length>=17)
                                    {


                                        lblgreenalert.Text = "Your order has been successfully submitted to us for processing using " + RBpaymenttype.Text + " ending with " + lblmastercardno.Text.Substring(14, 3) + ". Thank You!";
                                    }

                                }
                            }
                            else if ((ispickuponly_zone(OrderID) == true && (objOrderServices.IsNativeCountry(OrderID) ==1)))
                            {
                                lblgreenalert.Text = "Your order has been successfully submitted to us for processing. Thank You!";

                            }
                            else if (objOrderServices.IsNativeCountry(OrderID) == 0)
                            {

                                lblgreenalert.Text = "Your order has been successfully submitted to us for processing. Thank You!";
                                //UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                                //oUserInfo = objUserServices.GetUserInfo(Userid);
                                //objErrorHandler.CreateLog("ship country:" + oOrdInfo.ShipCountry);
                                //if (oUserInfo.ShipCountry.ToLower() != "new zealand")
                                //{
                                //    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using Paypal. Thank You!";

                                //    //lblintsp.Visible = false;
                                //    //lblintpp.Visible = true;
                                //    //divremotesecurepay.Style.Add("display", "none");
                                //    //divinternationalpaypal.Style.Add("display", "block");

                                //    //rbinternationalpaypal.Checked = true;
                                //}
                                //else
                                //{
                                //    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using Credit Card. Thank You!";

                                //}
                  
                                //if (rbinternationalpaypal.Checked == true)
                                //{
                                //    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using Paypal. Thank You!";

                                //}
                                //else if (rbremotezone_sp.Checked == true)
                                //{
                                //    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using Credit Card .Thank You!";

                                //}
                                //else if (rbinternationaldefault.Checked == true || rbinternationaldefault_remote==true)
                                //{
                                //    lblgreenalert.Text = "Your order has been successfully submitted to us for processing using Bank Transfer. Thank You!";

                                //}

                            }
                            Session["ORDER_ID"] = "0";
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString() + lblmastercardno.Text);
                            lblgreenalert.Text = "Your order has been successfully submitted to us for processing using " + RBpaymenttype.Text + ". Thank You!";
                            Session["ORDER_ID"] = "0";
                        }
                            divpaymentoption.Visible = false;
                        divInternationalpayoption.Visible = false;
                        tt1.Enabled = false;
                        drpSM1.Enabled = false;

                        /* Drop Shipment Fields  */
                        drpstate_txt.Enabled = false;
                        txtCompany.Enabled = false;
                        txtPostCode.Enabled = false;
                        txtAddressLine1.Enabled = false;
                        txtAddressLine2.Enabled = false;
                        txtAttentionTo.Enabled = false;
                        txtCountry.Enabled = false;
                        drpShipState.Enabled = false;
                        // txtReceiverContactName.Enabled=false;
                        txtSuburb.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;
                        txtShipPhoneNumber.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;

                       // ImageButton4.Visible = false;
                        ImageButton1.Visible = false;
                      //  ImageButton5.Visible = false;
                        //TextBox1.Enabled = false;
                        TextBox1.ReadOnly = true;
                        ImageButton2.Visible = false;
                        trpm.Visible = false;
                        Session["trpm"] = "false";
                        //Response.Redirect("Confirm.aspx?OrdId=99999&ViewType=Confirm");
                        //Response.Redirect("Payment.aspx?OrdId=" + OrderID, false);

                      
                        
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void ProceedFunction()
    {
        try
        {
            //color.BgColor = "FFFFFF";
          //  color1.BgColor = "FFFFFF";
            colo2.BgColor = "FFFFFF";
            bool isau = false;
           // color5.BgColor = "FFFFFF";
           // QuoteServices objQuoteServices = new QuoteServices();
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
                int OrdStatus =22;
                 int i = 0;
        DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(Userid));

        if (ds != null)
        {
            i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());
          
        }
        if (i == 2 || i == 3 || i == 4 || i == 6)
        {
            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
        }
        else if (i == 1 || i == 5 || i == 8 || i == 7)
        {
            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
        }


                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                    isau = false;
                    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                }
                else if ((ispickuponly_zone(OrderID) == true || ispickuponly_product(OrderID) == true) && (RBpaymenttype.Checked == false))
                {
                    isau = false;
                    if (trpm.Visible == true)
                    {

                        if (RBOffpay.Checked == true)
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }
                        else
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                        }
                    }
                    else
                    {
                        if (RBpaymenttype.Text == "Bank Transfer")
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                        }
                        else
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }
                    
                    }
                }
                else
                {
                    isau = true;
                    switch (Convert.ToInt16(Session["USER_ROLE"]))
                    {
                        case 1:

                            if (trpm.Visible == true)
                            {

                                if (RBOffpay.Checked == true)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                                else if (RBOnPay.Checked == true)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                                }
                            }
                            else
                            {
                                if (RBpaymenttype.Text == "Bank Transfer")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                            }
                            break;
                        case 2:

                            if (trpm.Visible == true)
                            {

                                if (RBOffpay.Checked == true)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                                else if (RBOnPay.Checked == true)
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                                }
                            }
                            else
                            {
                                if (RBpaymenttype.Text == "Bank Transfer")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
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
                    oPayInfo.PORelease = refid;
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
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        else
                            cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                       
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
                //        objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //SendNotification(OrderID);
                        if (RBpaymenttype.Text != "Bank Transfer")
                        {
                            SendMail(OrderID, OrdStatus);
                        }
                      //  SendMail(OrderID, OrdStatus);
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
                  //      objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
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
    public delegate string SyncDelegateorder(int OrderID, string drpsm1, string isoffline, string rbtype, string tt1, string comments, string shipzip, string trpm,string emailconfimation,string Userid,string userrole, string url,string stemplate, PaymentServices.PayInfo oPayInfo);
    public static void call_ProceedFunction(int OrderID, string drpsm1, string isoffline, string rbtype, string tt1, string comments, string shipzip)
    {
        SyncDelegateorder syncDelegate = new SyncDelegateorder(ProceedFunction);
        string trpm = "false";
        if (HttpContext.Current.Session["trpm"] != null)
        {
            trpm = HttpContext.Current.Session["trpm"].ToString();
        }
        string emailconfimation = "";
        if (HttpContext.Current.Session["emailconfimation"] != null)
        {
            emailconfimation = HttpContext.Current.Session["emailconfimation"].ToString();
        }
        string Userid = HttpContext.Current.Session["user_id"].ToString();
        string USER_ROLE = HttpContext.Current.Session["USER_ROLE"].ToString();
        string url = HttpContext.Current.Request.Url.Authority.ToString();
        HelperServices objHelperServices = new HelperServices();
      string  stemplate=  HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        HttpContext.Current.Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
        PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
        if (HttpContext.Current.Session["PAYMENTINFO"] != null)
        {
            oPayInfo = (PaymentServices.PayInfo)HttpContext.Current.Session["PAYMENTINFO"];
        }
        IAsyncResult asyncResult = syncDelegate.BeginInvoke(OrderID, drpsm1, isoffline, rbtype, tt1, comments, shipzip, trpm,emailconfimation, Userid, USER_ROLE,url, stemplate,oPayInfo, null, null);


    }
    public static string ProceedFunction(int OrderID,string drpsm1,string isoffline, string rbtype,string tt1 , string comments ,string shipzip,string trpm,string emailconfimation, string Userid, string userrole, string url,string stemplate, PaymentServices.PayInfo oPayInfo)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        try
        {
            //color.BgColor = "FFFFFF";
            //  color1.BgColor = "FFFFFF";
          


            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            OrderServices objOrderServices = new OrderServices();
           
            UserServices objUserServices = new UserServices();
            QuoteServices objQuoteServices = new QuoteServices();
            UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
            bool isau = false;
            // color5.BgColor = "FFFFFF";
            // QuoteServices objQuoteServices = new QuoteServices();
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
                int OrdStatus = 22;
                int i = 0;

              
                DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(Userid));

                if (ds != null)
                {
                    i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

                }
                if (i == 2 || i == 3 || i == 4 || i == 6)
                {
                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                }
                else if (i == 1 || i == 5 || i == 8 || i == 7)
                {
                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                }


                if (objOrderServices.IsNativeCountry(OrderID) == 0)
                {
                    objErrorHandler.CreatePayLog("1");
                    isau = false;
                    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                }
                else if ((ispickuponly_zone(OrderID,drpsm1,shipzip   ) == true || ispickuponly_product_static(OrderID) == true) && (rbtype  == "false"))
                {
                    objErrorHandler.CreatePayLog("2");
                    isau = false;
                    if (trpm == "true")
                    {

                        if (isoffline  == "true")
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }
                        else
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                        }
                    }
                    else
                    {
                        if (rbtype  == "Bank Transfer")
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                        }
                        else
                        {
                            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        }

                    }
                }
                else
                {
                   // objErrorHandler.CreatePayLog("3"+"userrole"+ HttpContext.Current.Session["USER_ROLE"].ToString()+"trm"+ trpm+ isoffline);
                    isau = true;

                    switch (Convert.ToInt16(userrole))
                    {
                        case 1:

                            if (trpm == "true")
                            {

                                if (isoffline== "true")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                                }
                            }
                            else
                            {
                                if (rbtype== "Bank Transfer")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                            }
                            break;
                        case 2:

                            if (trpm =="true")
                            {

                                if (isoffline  == "true")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                                }
                            }
                            else
                            {
                                if (rbtype == "Bank Transfer")
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                                }
                                else
                                {
                                    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                                }
                            }

                            break;
                        case 3:
                            OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                            break;
                    }
                }

                PaymentServices objPaymentServices = new PaymentServices();
               

                oPayInfo = objPaymentServices.GetPayment(OrderID);
                if (oPayInfo.OrderID == OrderID && (oPayInfo.PaymentType == PaymentServices.PaymentType.CCPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CCPaymentDeclined || oPayInfo.PaymentType == PaymentServices.PaymentType.CHEPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CODPayment))
                {
                    ChkOrderExist = 1;
                }
                string refid = "";

                if (tt1 == "")
                    tt1 = "WES" + OrderID.ToString();

                refid = objHelperServices.CS(tt1);

                //if (Session["PAYMENTINFO"] != null || Session["PAYMENTINFO"].ToString() != null)
                {
                  
                    decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                    oPayInfo.PayResponse = "";
                    oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                    oPayInfo.OrderID = OrderID;
                    oPayInfo.PONumber = objHelperServices.Prepare("");
                    oPayInfo.PORelease = refid;
                    oPayInfo.Amount = TotCost;
                    oPayInfo.UserId = OrderID;

                }
                if (objUserServices.GetUserStatus_withoutsession(objHelperServices.CI(Userid)) == 1)
                {
                    if (ChkOrderExist == 0)
                    {
                        ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                        int cStatus = 0;
                        if (isau == false || OrdStatus == 22)
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        else
                            cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());

                    }
                    else if (ChkOrderExist == 1)
                    {
                        ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                        int cStatus = 0;
                        if (isau == false || OrdStatus==22)
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        else
                            cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                    }
                    if (UptOrderStatus != -1)
                    {
                       // int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
               //         objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //SendNotification(OrderID);
                        if (rbtype != "Bank Transfer")
                        {
                          
                            SendMail(OrderID, OrdStatus, comments, drpsm1,emailconfimation,url,stemplate,userrole);
                        }
                        //  SendMail(OrderID, OrdStatus);
                        //if (HttpContext.Current.Request["QteFlag"] != null && HttpContext.Current.Request["QteFlag"].ToString() == "1")
                        //{
                        //    //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        //    return "Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1";
                        //}
                        //else
                        //{
                        //    //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        //}
                    }

                }
                else if (objUserServices.GetUserStatus_withoutsession(objHelperServices.CI(Userid)) == 4)
                {
                    
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
                        //int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        //objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //if (HttpContext.Current.Request["QteFlag"] != null && HttpContext.Current.Request["QteFlag"].ToString() == "1")
                        //{
                        //   // Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        //    return "Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1";
                        //}
                        //else
                        //{
                        //    //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        //}
                    }

                }
            }


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            objHelperServices.Mail_Error_Log("", OrderID,"", ex.ToString(), 0, 0, 0, 1);
        }
        return "";
    }

    protected void ProceedFunction_Onlinepayment()
    {
        try
        {
            //color.BgColor = "FFFFFF";
            //  color1.BgColor = "FFFFFF";
            colo2.BgColor = "FFFFFF";
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
                    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                }
                else
                {
                    isau = true;
                    OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                    //switch (Convert.ToInt16(Session["USER_ROLE"]))
                    //{
                    //    //case 1:
                    //    //    //OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                    //    //    break;
                    //    //case 2:
                    //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                    //        break;
                    //    case 3:
                    //        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                    //        break;
                    //}
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
                    oPayInfo.PORelease = refid;
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
                            cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        else
                            cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());

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
                        //   SendMail(OrderID, OrdStatus);
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

    protected static string ProceedFunction_Onlinepayment(string paymentmethod,int OrderID, string refid,string ORDERSTATUS,bool ISNATIVE)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            OrderServices objOrderServices = new OrderServices();
            UserServices objUserServices = new UserServices();
            HelperServices objHelperServices = new HelperServices();
            PaymentServices objPaymentServices = new PaymentServices();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            //colo2.BgColor = "FFFFFF";
            bool isau = false;
            QuoteServices objQuoteServices = new QuoteServices();
            //if (objOrderServices.GetOrderStatus(OrderID) != "")
            if (ORDERSTATUS != "")
            {
                int OrdStatusVerify = (int)OrderServices.OrderStatus.MANUALPROCESS;
                //DataSet oDs = new DataSet();
                //oDs = objOrderServices.GetOrderItems(OrderID);
                int ChkOrderExist = 0;

                int UptOrderStatus = -1;
                int OrdStatus = 0;
                //if (objOrderServices.IsNativeCountry(OrderID) == 0)
                //{
                //    isau = false;
                //    OrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                //}
                //else
                //{
                    isau = true;
                    OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                //}



                oPayInfo = objPaymentServices.GetPayment(OrderID);
                if (oPayInfo.OrderID == OrderID && (oPayInfo.PaymentType == PaymentServices.PaymentType.CCPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CCPaymentDeclined || oPayInfo.PaymentType == PaymentServices.PaymentType.CHEPayment || oPayInfo.PaymentType == PaymentServices.PaymentType.CODPayment))
                {
                    ChkOrderExist = 1;
                }
                    HttpContext.Current.Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
                    decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                    oPayInfo.PayResponse = "";
                    oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                    oPayInfo.OrderID = OrderID;
                    oPayInfo.PONumber = objHelperServices.Prepare("");
                    oPayInfo.PORelease = refid;
                    oPayInfo.Amount = TotCost;
                    oPayInfo.UserId = OrderID;


                //if (objUserServices.GetUserStatus(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString())) == 1)
                string userstatus = "";
                if (HttpContext.Current.Session["USER_STATUS"] == null)
                {

                    userstatus = objUserServices.GetUserStatus(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString())).ToString();
                }
                else {

                    userstatus = HttpContext.Current.Session["USER_STATUS"].ToString();
                }
                  
                if(userstatus == "1")
                {
                    if (ChkOrderExist == 0)
                    {
                        ChkOrderExist = objPaymentServices.CreatePayment(oPayInfo);
                        
                        UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                        int cStatus = 0;
                        //if (isau == false)
                        //    cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                        //else
                        //    cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                  
                    }
                    else if (ChkOrderExist == 1)
                    {
                        ChkOrderExist = objPaymentServices.UpdatePayment(oPayInfo);
                        if (paymentmethod != "BR")
                        {
                            UptOrderStatus = objOrderServices.UpdateOrderStatus(OrderID, OrdStatus);
                            int cStatus = 0;
                            //if (isau == false)
                            //    cStatus = objOrderServices.SentSignal("0", OrderID.ToString(), "150");
                            //else
                            //    cStatus = objOrderServices.SentSignalOrderNotification(OrderID.ToString());
                        }
                        }
                    //if (UptOrderStatus != -1)
                    //{
                    //    int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                    //    objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                    //    if (HttpContext.Current.Request["QteFlag"] != null && HttpContext.Current.Request["QteFlag"].ToString() == "1")
                    //    {
                    //        //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                    //        return "Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1";
                    //    }
                    //    else
                    //    {
                    //        //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                    //    }
                    //}

                }
                //else if (objUserServices.GetUserStatus(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString())) == 4)
                else if (HttpContext.Current.Session["USER_STATUS"].ToString() == "4")
                {
                    if (HttpContext.Current.Session["PAYMENTINFO"] != null)
                    {
                        oPayInfo = (PaymentServices.PayInfo)HttpContext.Current.Session["PAYMENTINFO"];
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
                        int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                        objQuoteServices.UpdateQuoteStatus(QID, objHelperServices.CI(QuoteServices.QuoteStatus.CLOSED));
                        //if (HttpContext.Current.Request["QteFlag"] != null && HttpContext.Current.Request["QteFlag"].ToString() == "1")
                        //{
                        //    //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1", false);
                        //    return "Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm&QteFlag=1";
                        //}
                        //else
                        //{
                        //    //Response.Redirect("Confirm.aspx?OrdId=" + OrderID + "&ViewType=Confirm");
                        //}
                    }

                }
            }


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        return "";
    }
  
    
    public  void SendMail(int OrderId, int OrderStatus)
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
            string shippingnotes = TextBox1.Text.Trim();




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
                    //if (oOrderInfo.Payment_Selection != "BT")
                    //{
                    //    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row");
                    //}
                    //else
                    //{
                        _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row_BankTrasfer");
                    //}
                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                {
                    objErrorHandler.CreatePayLog("Payment Type"+oOrderInfo.Payment_Selection);
                    if (  (objOrderServices.IsNativeCountry(OrderID) == 0))// is other then au
           
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_int");
                    }
                    else if (oOrderInfo.Payment_Selection == "BT")   
                    {
                       
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_BankTrasfer");
                        
                    }

                    else if (emailconfimation == "Remote")
                    {

                        if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue == "Drop Shipment Order"))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_RemotePPP");
                        }
                        else
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                        }
                    }
                    else
                    {

                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                    }
                   

                   
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");
                }

                _stmpl_container.SetAttribute("OrderTotalAmount", oOrderInfo.TotalAmount);
                _stmpl_container.SetAttribute("InvoiceNo", oOrderInfo.InvoiceNo);
           
                _stmpl_container.SetAttribute("OrderDate", Createdon);
                _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                if (oOrderInfo.Payment_Selection == "BT")
                {
                    _stmpl_container.SetAttribute("PayOrderNo", oPayInfo.OrderID);
                }
              
                    _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);
              
                _stmpl_container.SetAttribute("CreatedBy", Createdby);

                //if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    _stmpl_container.SetAttribute("SubmittedBy", SubmittedBy);
                //else
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
             //objErrorHandler.CreateLog(sHTML);
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




  




    public static void SendMail(int OrderId, int OrderStatus, string shipnotes, string drpSM1,string  emailconfimation,string url,string stemplatepath,string userrole)
           

    {
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            string BillAdd;
            string ShippAdd;
           
            DataSet dsOItem = new DataSet();
          OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
            PaymentServices objPaymentServices = new PaymentServices();
            NotificationServices objNotificationServices = new NotificationServices();
            PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
            //HelperDB objHelperDB = new HelperDB();
         
           
            OrderServices objOrderServices = new OrderServices();
            oPayInfo = objPaymentServices.GetPayment(OrderId);
            oOrderInfo = objOrderServices.GetOrder(OrderId);

            int UserID = objHelperServices.CI(oOrderInfo.UserID);
            
            //oUserInfo = objUserServices.GetUserInfo(UserID);
            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            dsOItem = objOrderServices.GetOrderItems(OrderId);
            BillAdd = GetBillingAddress(oOrderInfo);
            ShippAdd = GetShippingAddress(oOrderInfo);

            string ShippingMethod = oOrderInfo.ShipMethod;
            string CustomerOrderNo = oPayInfo.PORelease;
            string shippingnotes = shipnotes.Trim();
            oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            string Createdby = oUserInfo.Contact + "&nbsp;&nbsp;" + string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate); //String.Format("dd/MM/yyyy hh:mm tt", oOrderInfo.CreatedDate
            string Createdon = string.Format("{0:dd/MM/yyyy hh:mm tt}", oOrderInfo.CreatedDate);
            string Emailadd = oUserInfo.AlternateEmail;


            
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

               
                int ictrows = 0;

                DataSet dscat = new DataSet();
                DataTable dt = null;
                _stg_records = new StringTemplateGroup("row", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


                lstrecords = new TBWDataList[dsOItem.Tables[0].Rows.Count + 1];



                int ictrecords = 0;

                foreach (DataRow dr in dsOItem.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf("mail" + "\\" + "row_BankTrasfer");
                    _stmpl_records.SetAttribute("Code", dr["CATALOG_ITEM_NO"].ToString());
                    _stmpl_records.SetAttribute("Qty", dr["QTY"].ToString());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

                if (Convert.ToInt16(userrole) == 1 || Convert.ToInt16(userrole) == 2)
                {
                    objErrorHandler.CreatePayLog("Payment Type" + oOrderInfo.Payment_Selection);
                   
                    //if ((objOrderServices.IsNativeCountry(OrderId) == 0))// is other then au
                    //{
                    //    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_int");
                    //}
                    if (oOrderInfo.Payment_Selection == "BT")
                    {

                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_BankTrasfer");

                    }

                    else if (emailconfimation == "Remote")
                    {

                        if ((drpSM1 == "Courier") || (drpSM1 == "Mail") || (drpSM1 == "Drop Shipment Order"))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted_RemotePPP");
                        }
                        else
                        {
                            _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                        }
                    }
                    else
                    {

                        _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "OrderSubmitted");
                    }



                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "PendingOrder");
                }

                _stmpl_container.SetAttribute("OrderTotalAmount", oOrderInfo.TotalAmount);
                _stmpl_container.SetAttribute("InvoiceNo", oOrderInfo.InvoiceNo);

                _stmpl_container.SetAttribute("OrderDate", Createdon);
                _stmpl_container.SetAttribute("PendingOrderurl", PendingorderURL);
                if (oOrderInfo.Payment_Selection == "BT")
                {
                    _stmpl_container.SetAttribute("PayOrderNo", oPayInfo.OrderID);
                }

                _stmpl_container.SetAttribute("CustOrderNo", oPayInfo.PORelease);

                _stmpl_container.SetAttribute("CreatedBy", Createdby);

                if (Convert.ToInt16(userrole) == 1 || Convert.ToInt16(userrole) == 2)
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
                //objErrorHandler.CreateLog(sHTML);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                objHelperServices.Mail_Error_Log("", OrderId, "", ex.ToString(), 0, 0, 0, 1);
                sHTML = "";
            }
            if (sHTML != "")
            {
                string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
      
                System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues_withoutsession("ADMIN EMAIL").ToString());

                string emails = "";
                string Adminemails = "";
                if (Convert.ToInt16(userrole) == 1 || Convert.ToInt16(userrole) == 2)
                {
                    MessageObj.To.Add(Emailadd.ToString());
                    MessageObj.Bcc.Add("indumathi@jtechindia.com");

                    Adminemails = Get_ADMIN_UserEmils(UserID);
                }
                else
                {
                    emails = Get_ADMIN_APPROVED_UserEmils(UserID);
                    MessageObj.To.Add(Emailadd.ToString());
                    MessageObj.Bcc.Add ("indumathi@jtechindia.com");
                }

                if (Convert.ToInt16(userrole) == 1 || Convert.ToInt16(userrole) == 2)
                {
                    MessageObj.Subject = "WES Australasia Order Confirmation - Order No : " + CustomerOrderNo.ToString();
                }
                else
                {
                    MessageObj.Subject = "WES Australasia Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                }

                MessageObj.IsBodyHtml = true;
                MessageObj.Body = sHTML;
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues_withoutsession("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues_withoutsession("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                objHelperServices.Mail_Log("", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                if (Convert.ToInt16(userrole) == 1 || Convert.ToInt16(userrole) == 2)
                {
                    if (ApprovedUserEmailadd.ToUpper().ToString() != "" && Emailadd.ToUpper().ToString() != ApprovedUserEmailadd.ToUpper().ToString())
                    {
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
                                    MessageObj.To.Clear();
                                    MessageObj.To.Add(id.ToString());
                                    smtpclient.Send(MessageObj);
                                    objHelperServices.Mail_Log("", oOrderInfo.OrderID, MessageObj.To.ToString(),"");
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
                                objHelperServices.Mail_Log("", oOrderInfo.OrderID, MessageObj.To.ToString(), Adminemails.ToString());
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
                                    objHelperServices.Mail_Log("", oOrderInfo.OrderID, MessageObj.To.ToString(), id.ToString());
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
                                objHelperServices.Mail_Log("", oOrderInfo.OrderID, MessageObj.To.ToString(),"");
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
            
            objHelperServices.Mail_Error_Log("", OrderId, "", ex.ToString(),0,0,0,1 );
        }
    }
   
    
    public void SendNotification(int OrderID)
    {
        // objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.NEWORDER.ToString()))
        {
            DataSet dsOrder = objNotificationServices.BuildNotifyInfo();
            OrderServices objOrderServices = new OrderServices();
            string sTemplate = "";
            string sEmailMessage = "";
            string sUser = "";
            sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
            decimal Tax = objOrderServices.GetTaxAmount(OrderID);
            decimal SCost = objOrderServices.GetShippingCost(OrderID);
            decimal Total = objOrderServices.GetOrderTotalCost(OrderID);
            string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
            try
            {
                DataRow oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.FROMCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelperServices.GetOptionValues("COMPANY ADDRESS").ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TOCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = GetShippingAddress(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.FIRSTNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objUserServices.UserFirstName(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.ORDERDATE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = System.DateTime.Now.ToLongDateString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.ORDERID.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = refid;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.CONSTRUCTTABLE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = ConstructOrderDetails(OrderID);
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.SUBTOTAL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + SubTotal;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TAX.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + Tax;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.SHIPCHARGES + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + SCost;
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.OrderReceipt.TOTAL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = currency + Total;
                dsOrder.Tables[0].Rows.Add(oRow);

                sTemplate = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                sEmailMessage = objNotificationServices.ParseTemplateMessage(sTemplate, dsOrder);


                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                ArrayList CCList = new ArrayList();
                CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                objNotificationServices.NotifyCC = CCList;
                objNotificationServices.NotifyTo.Add(sUser);
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                string EmailSubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
                objNotificationServices.NotifySubject = EmailSubject;
                objNotificationServices.NotifyMessage = sEmailMessage;
                objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.NEWORDER.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }

    public string ConstructOrderDetails(int OrderID)
    {
        int Qty = 0;
        double Price = 0.0;
        string CatalogItemNo = "";
        string sOrderDetails = "";
        string description = "";
        DataSet dsOD = new DataSet();
        dsOD = objOrderServices.GetOrderDetails(OrderID);
        string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
        string oRowHead = "<TABLE><TR><TD ALIGN=CENTER WIDTH=60>Qty</TD><TD ALIGN=CENTER WIDTH=350>Item#</TD><TD ALIGN=LEFT WIDTH=400>Description</TD><TD ALIGN=CENTER WIDTH=100>Price</TD></TR>";
        foreach (DataRow row in dsOD.Tables[0].Rows)
        {
            CatalogItemNo = row["CATALOG_ITEM_NO"].ToString();
            Qty = objHelperServices.CI(row["QTY"]);
            Price = objHelperServices.CD(row["PRICE_EXT_APPLIED"]) * Qty;
            description = row["Description"].ToString().Replace("<ars>g</ars>", "&rarr;");
            SubTotal = SubTotal + Price;
            sOrderDetails = sOrderDetails + @"<TR><TD width=60 align=Center><FONT FACE=TAHOMA SIZE=2>" + Qty.ToString() + "</FONT></TD><TD width=350 align=LEFT><FONT FACE=TAHOMA SIZE=2>" + CatalogItemNo + "</FONT></TD><TD width=400 align=Center><FONT FACE=TAHOMA SIZE=2>" + description + "</TD><TD width=100 align=right><FONT FACE=TAHOMA SIZE=2>" + currency + Price.ToString("#,#0.00") + "</FONT></TD></TR>";
        }
        sOrderDetails = oRowHead + sOrderDetails + "</TABLE>";
        return sOrderDetails;
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

    public string  GetBillingAddress(int OrderID)
    {
        string sBillingAddress = "";
        OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
        oBI = objOrderServices.GetOrder(OrderID);
        if(oBI.BillcompanyName.Trim().Length > 0)
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

    public static string GetShippingAddress(OrderServices.OrderInfo oOI)
    {
        string sShippingAddress = "";
      //  OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
      //  OrderServices objOrderServices = new OrderServices();
      //  oOI = objOrderServices.GetOrder(Convert.ToInt32(OrderID));

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

    public static string GetBillingAddress(OrderServices.OrderInfo oBI)
    {
        string sBillingAddress = "";
       // OrderServices.OrderInfo oBI = new OrderServices.OrderInfo();
      //  OrderServices objOrderServices = new OrderServices();
      //  oBI = objOrderServices.GetOrder(Convert.ToInt32(OrderID));
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
  


    private string Setdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        string returnselected = "";
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {
                d.SelectedIndex = i;
                returnselected = li.Text.ToUpper();
                break;
            }
        }
        return returnselected;
    }
    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {
        this.modalPop.Hide();
        Response.Redirect("home.aspx");
    }
    protected void btnGoHome_Click(object sender, EventArgs e)
    {
        Security objSecurity = new Security();
        this.modalPop.Hide();
        //Response.Redirect("home.aspx");
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
        {
            HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
            LoginInfoCookie["Login"] = objSecurity.StringEnCrypt("False");
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
        }      
        Response.Redirect("login.aspx");
     
    }
    private bool Checkdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        bool blnreturn = false;
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {

                blnreturn = true;
                break;
            }
        }
        return blnreturn;
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
    private void GetParams()
    {
        try
        {
            if (Request.Url.Query != null && Request.Url.Query != "")
            {

                string id = null;
                if (Request.Url.Query.Contains("key=") == false)
                {

                    id = Request.Url.Query.Replace("?", "");
                    int n;
                    bool isNumeric = int.TryParse(id, out n);
                    //  objErrorHandler.CreateLog("Getparams" + id);
                    id = DecryptSP(id);


                    if ((id == null) && (isNumeric == true))
                    {
                        id = Request.Url.Query.Replace("?", "");

                        if (id.Contains("PaySP") == false)
                        {
                            id = id + "#####" + "PaySP";
                        }
                    }
                }

                else
                {

                    if (HttpContext.Current.Session["Mchkout"] != null)
                    {
                        id = HttpContext.Current.Session["Mchkout"].ToString();
                        id = DecryptSP(id);
                    }
                }
                if (id != null)
                {
                    string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

                    OrderID = objHelperServices.CI(ids[0]);
                    //     objErrorHandler.CreateLog(OrderID.ToString());
                    OrderIDaspx = OrderID;

                    if (ids.Length > 1)
                    {
                        PaytabType = ids[1];
                        //     objErrorHandler.CreateLog(PaytabType);
                        Paytab = true;
                    }
                    if (ids.Length > 2)
                    {
                        paidtab = true;
                    }
                }
                else
                {
                    OrderID = 0;
                    PaytabType = "";
                    OrderIDaspx = 0;
                    // div1.InnerHtml = "";
                    div2.InnerHtml = "Invalid Data";
                    //div2.Visible = true;
                    div2.Style.Add("display", "block");
                    //div2.Focus();
                    Scroll_To_Control("ctl00_maincontent_div2");
                    //  div1.Visible = false;
                    //  return 0;
                }

            }
            else
            {
                OrderID = 0;
                PaytabType = "";
                OrderIDaspx = 0;
                //div1.Visible = false;
                div2.InnerHtml = "Invalid Data";
                //div2.Visible = true;
                div2.Style.Add("display", "block");
                Scroll_To_Control("ctl00_maincontent_div2");
                // return 0;
            }
            //  return 1;
        }
        catch (Exception ex)
        {
            //  return 0;
         //   objErrorHandler.CreateLog(ex.ToString());
        }

    }
    private void Scroll_To_Control(string controlId)
    {
        controlId = "ctl00_maincontent_divpaymentoption";
        string script =
                     "$(document).ready(function() {" +
                         "$('html,body').animate({ " +
                             "scrollTop: $('#" + controlId + "').offset().top" +
                         "}, 0);" +
                     "});";

        if (!Page.ClientScript.IsStartupScriptRegistered("ScrollToElement"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ScrollToElement", script, true);
    }
    private void order_submit_process()
    {
        try
        {
            if (chkRSpwd == true)
                return;

            QuoteServices objQuoteServices = new QuoteServices();
            OrderDB objOrderDB = new OrderDB();
            HelperServices objHelperService = new HelperServices();
            //int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            int OrdStatus = 0;
            string ApproveOrder = string.Empty;
            //Direct  Order / Approve Order (Comes from Pending order Page)
            lblpostcode2err.Text = "";
            lbladdline1err.Text = "";
            lbladdline2err.Text = "";


            //if (Request.QueryString["ApproveOrder"] == null)
            //{
                //if (Session["USER_ROLE"] != null)
                //{
                    //switch (Convert.ToInt16(Session["USER_ROLE"]))
                    //{
                        //case 1:
                        //    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                        //    break;
                        //case 2:
                            OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                    //        break;
                    //    case 3:
                    //        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                    //        break;
                    //}
                //}
                //else
                //{
                //    OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                //}
            //}
            //else if (Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2))
            //{

            //    OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
            ////}
            //else if (Request.QueryString["ApproveOrder"] != null)
            //    OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

            //OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
            decimal TaxAmount;
            decimal ProdTotCost;
            if (string.IsNullOrEmpty(Request["OrderID"]))
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), 1);
            else
                OrderID = Convert.ToInt32(Request["OrderID"].ToString());




            int oldorderID = OrderID;
            int QuoteId = 0;
            QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));

            if (tt1.Text == "")
                tt1.Text = "WES" + OrderID.ToString();

            refid = objHelperServices.CS(tt1.Text);

            //else if (Checkspecialchar(tt1.Text,@"/\")==true)
            //{
            //    txterr.Text = "Special character not allowed";
            //    OrderID = -1;
            //    Session["OrderId"] = "-1";
            //}
            if (OrderID <= 0 || tt1.Text.Length == 0)
            {
                txterr.Text = "Please enter valid Order No, Order No should be 1 digit or more than 1 digits";
                OrderID = -1;
                Session["OrderId"] = "-1";
            }
            else if (hfduporderproceed.Value == "1")
            {

            }
            else
            {
                //txterr.Text = "";
                //string querystr = "";

                //if (Request.QueryString["ApproveOrder"] == null)
                //{
                //    DataSet DS = new DataSet();
                //    //querystr = "select count(*) from TBWC_PAYMENT where order_id in(select order_id from dbo.tbwc_order where [user_id] in(select [user_id] from TBWC_COMPANY_BUYERS where company_id in(select company_id from dbo.TBWC_COMPANY_BUYERS where [user_id]=" + objHelperServices.CI(Session["USER_ID"].ToString()) + "))) and po_release='" + refid + "'";
                //    //objHelperServices.SQLString = querystr;
                //    //DS = objHelperServices.GetDataSet();
                //    DS = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["USER_ID"].ToString(), refid, "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);

                //    if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
                //    {
                //        txterr.Text = "Order No already exists, please Re-enter Order No";
                //        OrderID = -1;
                //    }
                //}

            }

            //int coupon_id = 0;
            //if (txtCouponCode.Text.Trim().Length > 0)
            //{
            //    DataTable dscoupon = new DataTable();
            //    dscoupon = objOrderServices.GetCouponDetails(txtCouponCode.Text.Trim(), "GET_COUPON_IS_EXPIRY");
            //    if (dscoupon != null)
            //    {
            //        if (dscoupon.Rows.Count > 0)
            //        {
            //            if ((Convert.ToInt32(dscoupon.Rows[0]["IS_EXPIRY"].ToString()) < 0))
            //            {
            //                //coupon_id = Convert.ToInt32(dscoupon.Rows[0]["COUPON_ID"].ToString());
            //                lblcouponerrmsg.Visible = true;
            //                lblcouponerrmsg.Text = "Coupon Code is Expired.";
            //                // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Coupon Code is Expired.');</script>", false);
            //                txtCouponCode.Focus();
            //                // txtcoucode.BorderColor = System.Drawing.Color.Red;
            //                //ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //                txtCouponCode.Style.Add("border-color", "red red red !important");
            //                return;
            //            }
            //            else if ((Convert.ToInt32(dscoupon.Rows[0]["IS_EXPIRY"].ToString()) >= 0))
            //            {
            //                coupon_id = Convert.ToInt32(dscoupon.Rows[0]["COUPON_ID"].ToString());
            //                lblcouponerrmsg.Visible = false;
            //                lblcouponerrmsg.Text = "";
            //                txtCouponCode.Style.Add("border-color", "#73ACCF #88CEF9 #88CEF9 !important;");
            //            }

            //        }
            //        else
            //        {
            //            lblcouponerrmsg.Visible = true;
            //            lblcouponerrmsg.Text = "Invalid Coupon Code.";
            //            // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Invalid Coupon Code.');</script>", false);
            //            lblcouponerrmsg.Focus();
            //            txtCouponCode.Style.Add("border-color", "red red red !important");
            //            //ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        lblcouponerrmsg.Visible = true;
            //        lblcouponerrmsg.Text = "Invalid Coupon Code.";
            //        // ClientScript.RegisterStartupScript(typeof(Page), "WagnerAlert", "<script type='text/javascript'>alert('Invalid Coupon Code.');</script>", false);
            //        lblcouponerrmsg.Focus();
            //        txtCouponCode.Style.Add("border-color", "red red red !important");
            //        // ClientScript.RegisterStartupScript(GetType(), "id", "couponcodeError()", true);
            //        return;
            //    }
            //}


            //if (Request["QteId"] != null)
            //{
            //    QuoteID = objHelperServices.CI(Request["QteId"].ToString());
            //    OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
            //    OrdStatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
            //}

            if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            {
              
                if (objOrderServices.GetDropShipmentKeyExist(txtPostCode.Text.ToString(), "PostCode") == true)
                {
                    //lblpostcode2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                    //OrderID = -1;
                    //ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    if (objOrderServices.GetDropShipmentKeyExist(txtAddressLine1.Text.ToString(), "") == true)
                    {
                        //lbladdline1err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                        // ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>var x=window.confirm('Non-Standard Delivery Area. We will contact you to confirm costing');if (x)window.alert('Good!')</script>", false);

                    }
                    if (txtAddressLine2.Text.ToString() != "" && objOrderServices.GetDropShipmentKeyExist(txtAddressLine2.Text.ToString(), "") == true)
                    {
                        //lbladdline2err.Text = "Non-Standard Delivery Area. We will contact you to confirm costing";
                        //   OrderID = -1;
                        ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                    }
                  
                }
                //else
                //{
                //    OrderID = -1;
                //    ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
                //}
            }

            //string status=objOrderServices.GetOrderStatus(OrderID);
            //if (status != "OPEN")
            //{
            //    if (QuoteId != 0)
            //    {
            //        OrderID = objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteId));
            //        OrdStatus = (int)OrderServices.OrderStatus.PLACEQUOTE;
            //    }
            //}
            //    else
            //    {
            //        OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            //        OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OrdStatus);
            //    }
            if (OrderID > 0)
            {
                ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
          // decimal shipcost=     calcShipcost();

                decimal shipcost =0;
                string SHIP_CODE = "";

                //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
                //if (Sqltbs != null)
                //{
                //    if (Sqltbs.Tables[0].Rows.Count > 0)
                //    {

                //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                //        oOrdInfo.ShipCost = price;
                //        shipcost = price;
                //        // objErrorHandler.CreateLog("delivery cost 7" + price.ToString());
                //    }

                //}
                if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue=="Drop Shipment Order"))
                {
                    objErrorHandler.CreateLog("lblshipingcost" + lblshipingcost.Text);
                    if (lblshipingcost.Text != "")
                    {
                        if (objHelperServices.CDEC(lblshipingcost.Text) > 0)
                        {
                            shipcost = objHelperServices.CDEC(lblshipingcost.Text);
                            SHIP_CODE = hfshipcode.Value;
                        }
                        else
                        {
                            string calship = Session["shipcost"].ToString();
                            string[] charge = calship.Split('-');

                            shipcost = objHelperServices.CDEC(charge[0]);
                             SHIP_CODE = charge[1];
                            objErrorHandler.CreateLog("order s 1"+SHIP_CODE);
                        }


                    }
                    else {

                        string calship = Session["shipcost"].ToString();
                        string[] charge = calship.Split('-');

                        shipcost = objHelperServices.CDEC(charge[0]);
                         SHIP_CODE = charge[1];
                        objErrorHandler.CreateLog("order s 2" + SHIP_CODE);
                    }
                
                }
                
        //  TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost + shipcost, OrderID.ToString());
                TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
                decimal UpdRst = 0;
                oOrdInfo.OrderID = OrderID;
                oOrdInfo.OrderStatus = OrdStatus;
                ///////////////////////////
                int _UserrID;
                _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
                oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                txtSFName.Text = oOrdShippInfo.FirstName;
                txtSLName.Text = oOrdShippInfo.LastName;
                txtSMName.Text = oOrdShippInfo.MiddleName;
                txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
                txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
                txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
                txtSCity.Text = oOrdShippInfo.ShipCity;
                drpShipState.Text = oOrdShippInfo.ShipState;
                txtSZip.Text = oOrdShippInfo.ShipZip;
                Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
                //if (oOrdShippInfo.ShipCountry.Length > 3)
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByText(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpShipCountry.Items.FindByValue(oOrdShippInfo.ShipCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}

                txtSPhone.Text = oOrdShippInfo.ShipPhone;
                //double Dist = objCountryServices.GetDistanceUsingZip(oOrdShippInfo.ShipZip);
                //if (Dist <= 50 && Dist > 0)
                //{
                //    oLstItem.Text = "Friendly Driver";
                //    oLstItem.Value = "FRIENDLYDRIVER";
                //    oLstItem.Selected = true;
                //    cmbProvider.Items.Add(oLstItem);
                //}
                UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
                oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                txtbillFName.Text = oOrdBillInfo.FirstName;
                txtbillLName.Text = oOrdBillInfo.LastName;
                txtbillMName.Text = oOrdBillInfo.MiddleName;
                txtbilladd1.Text = oOrdBillInfo.BillAddress1;
                txtbilladd2.Text = oOrdBillInfo.BillAddress2;
                txtbilladd3.Text = oOrdBillInfo.BillAddress3;
                txtbillcity.Text = oOrdBillInfo.BillCity;
                drpBillState.Text = oOrdBillInfo.BillState;
                txtbillzip.Text = oOrdBillInfo.BillZip;
                Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
                //if (oOrdBillInfo.BillCountry.Length > 3)
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByText(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                //else
                //{
                //    ListItem cntry = drpBillCountry.Items.FindByValue(oOrdBillInfo.BillCountry);
                //    if (cntry != null)
                //    {
                //        cntry.Selected = true;
                //        drpShipCountry.SelectedValue = cntry.Value;
                //    }
                //}
                txtbillphone.Text = oOrdBillInfo.BillPhone;
                ///////////////////////////
                oOrdInfo.OrderStatus = OrdStatus;
                if (cmbProvider.SelectedValue != "Please Select")
                {
                    oOrdInfo.ShipCompany = cmbProvider.SelectedValue;
                }
                else
                {
                    oOrdInfo.ShipCompany = "";
                }
           
                oOrdInfo.ShipMethod = drpSM1.SelectedValue;

                if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
                {
                    oOrdInfo.ShipCompName = objHelperServices.Prepare(txtCompany.Text);
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtAttentionTo.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtAddressLine1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtAddressLine2.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSuburb.Text);
                  //  objErrorHandler.CreateLog("drpstate_txt" + drpstate_txt.Text + "drpselectedval" + drpstate_txt.SelectedValue + "drpselectedtext" + drpstate_txt.SelectedItem);

                    //drpstate_txt.Text = hfdrpstate_txt.Value;
                    //txtPostCode.Text = hfPostCode.Value;
                    if (txtSuburb.Text != "")
                    {

                        if (Session[txtSuburb.Text] != null)
                        {
                            string[] shipx = Session[txtSuburb.Text].ToString().Split('-');
                            drpstate_txt.Text = shipx[1];
                            txtPostCode.Text = shipx[0];
                        }
                    }
                   // objErrorHandler.CreateLog("postcode" + txtPostCode.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpstate_txt.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(txtCountry.Text);
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtPostCode.Text);
                    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(txtDeliveryInstructions.Text);
                    //  oOrdInfo.ReceiverContact = objHelperServices.Prepare(txtReceiverContactName.Text);
                    oOrdInfo.ShipPhone = objHelperServices.Prepare(txtShipPhoneNumber.Text);

                }
                else
                {
                    oOrdInfo.ShipFName = objHelperServices.Prepare(txtSFName.Text);
                    oOrdInfo.ShipLName = objHelperServices.Prepare(txtSLName.Text);
                    oOrdInfo.ShipMName = objHelperServices.Prepare(txtbillMName.Text);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(txtSAdd1.Text);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(txtSAdd2.Text);
                    oOrdInfo.ShipAdd3 = objHelperServices.Prepare(txtSAdd3.Text);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(txtSCity.Text);
                    oOrdInfo.ShipState = objHelperServices.Prepare(drpShipState.Text);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(drpShipCountry.SelectedItem.ToString());
                    oOrdInfo.ShipZip = objHelperServices.Prepare(txtSZip.Text);
                 //   LoadShipPhoneNo(Session["USER_ID"].ToString());
                    if (drpSM1.SelectedValue != "Counter Pickup")
                    {
                        oOrdInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                    }
                    else {
                        if (Session["Nothanks"] == null || Session["Nothanks"] == "false")
                        {
                            if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(oOrdInfo.ShipPhone);
                            }
                            else
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtMobileNumber.Text);
                            }
                        }
                        else
                        {
                            oOrdInfo.ShipPhone = "";
                        }
                    }

                    DataSet objds = new DataSet();
                    objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    {
                        oOrdInfo.DeliveryInstr = objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                    }

                }


                oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1.Text);


                //if (oOrdInfo.ShipNotes == "" && txtCouponCode.Text != "" && coupon_id > 0)
                //    oOrdInfo.ShipNotes = "Coupon Code Entered:" + txtCouponCode.Text.Trim();
                //else if (oOrdInfo.ShipNotes != "" && txtCouponCode.Text != "" && coupon_id > 0)
                //    oOrdInfo.ShipNotes = oOrdInfo.ShipNotes + Environment.NewLine + "Coupon Code Entered:" + txtCouponCode.Text.Trim();

                TextBox1.Text = oOrdInfo.ShipNotes;


                string strHostName = System.Net.Dns.GetHostName();
                string clientIPAddress = "";
                if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                {
                    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    else
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }


                oOrdInfo.ClientIPAddress = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
                //oOrdInfo.ClientIPAddress = Request.Params["REMOTE_ADDR"];
                //oOrdInfo.ClientIPAddress = Request.UserHostAddress;
                oOrdInfo.isEmailSent = false;
                oOrdInfo.isInvoiceSent = false;
                oOrdInfo.IsShipped = false;

                oOrdInfo.BillFName = objHelperServices.Prepare(txtbillFName.Text);
                oOrdInfo.BillLName = objHelperServices.Prepare(txtbillLName.Text);
                oOrdInfo.BillMName = objHelperServices.Prepare(txtbillMName.Text);
                oOrdInfo.BillAdd1 = objHelperServices.Prepare(txtbilladd1.Text);
                oOrdInfo.BillAdd2 = objHelperServices.Prepare(txtbilladd2.Text);
                oOrdInfo.BillAdd3 = objHelperServices.Prepare(txtbilladd3.Text);
                oOrdInfo.BillCity = objHelperServices.Prepare(txtbillcity.Text);
                oOrdInfo.BillState = objHelperServices.Prepare(drpBillState.Text);
                oOrdInfo.BillCountry = objHelperServices.Prepare(drpBillCountry.SelectedItem.Text);
                oOrdInfo.BillZip = objHelperServices.Prepare(txtbillzip.Text);
                oOrdInfo.BillPhone = objHelperServices.Prepare(txtbillphone.Text);
                oOrdInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
                oOrdInfo.ShipCost = shipcost;
                oOrdInfo.SHIP_CODE = SHIP_CODE;
        
                oOrdInfo.TaxAmount = TaxAmount;
                oOrdInfo.TotalAmount = ProdTotCost + TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                oOrdInfo.TrackingNo = "";
                if (drpSM1.SelectedValue.ToString().Trim().Contains("Drop Shipment Order"))
                {
                    oOrdInfo.DropShip = 1;
                }
                oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());

                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);
         
                //if (coupon_id > 0)
                //    objOrderServices.UpdateCouponId(coupon_id, OrderID, "UPDATE_ORDER_COUPON_ID");


                if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
                {
                    Session["PrevOrderID"] = "0";
                }

                //Dist = objCountryServices.GetDistanceUsingZip(oOrdInfo.ShipZip);
                string PAYMENTSELECTION = "";
                if (RBCreditCard.Checked == true )
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["creditflag"].ToString() == "1")
                    {
                        PAYMENTSELECTION = "SP";
                    }
                    else if (System.Configuration.ConfigurationManager.AppSettings["creditflag"].ToString() == "2")
                    {
                        PAYMENTSELECTION = "BR";
                    }
                   
                }
                else if (RBPaypal.Checked == true)
                {
                    PAYMENTSELECTION = "PP";
                }
                if (UpdRst > 0)
                {
                    UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                }
                if (UpdRst > 0)
                {
                    objOrderServices.UpdateCustomFields(oOrdInfo);


                    Session["ORDER_NO"] = null;
                    Session["SHIPPING"] = null;
                    Session["DELIVERY"] = null;
                    Session["DROPSHIP"] = null;

                    Session["ShipCost"] = oOrdInfo.ShipCost;
                    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                    {
                        Response.Redirect("Payment.aspx?OrdId=" + OrderID + "&QteFlag=1", false);
                    }
                    else
                    {
                       ProceedFunction_Onlinepayment();
                        PnlOrderInvoice.Visible = true;
                        PnlOrderContents.Visible = false;
                        PHOrderConfirm.Visible = true;
                        tt1.Enabled = false;
                      //  drpSM1.Enabled = false;

                        /* Drop Shipment Fields  */
                        drpstate_txt.Enabled = false;
                        txtCompany.Enabled = false;
                        txtPostCode.Enabled = false;
                        txtAddressLine1.Enabled = false;
                        txtAddressLine2.Enabled = false;
                        txtAttentionTo.Enabled = false;
                        txtCountry.Enabled = false;
                        drpShipState.Enabled = false;
                        // txtReceiverContactName.Enabled=false;
                        txtSuburb.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;
                        txtShipPhoneNumber.Enabled = false;
                        txtDeliveryInstructions.Enabled = false;

                        // ImageButton4.Visible = false;
                        ImageButton1.Visible = false;
                        //  ImageButton5.Visible = false;
                        //TextBox1.Enabled = false;
                        TextBox1.ReadOnly = true;
                        ImageButton2.Visible = false;
                        trpm.Visible = false;
                        //Response.Redirect("Confirm.aspx?OrdId=99999&ViewType=Confirm");
                        //Response.Redirect("Payment.aspx?OrdId=" + OrderID, false);



                    }
                }
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.CreateLog(Ex.ToString()); 
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }
    
    
    }

    private static string order_submit_process(int OrderID, string paymentmethod ,string tt1, string drpSM1, string TextBox1, string txtMobileNumber , string cmbProvider, ShipInfo shipInfo)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            //if (chkRSpwd == true)
            //    return;

            QuoteServices objQuoteServices = new QuoteServices();
            OrderDB objOrderDB = new OrderDB();
            HelperServices objHelperServices = new HelperServices();
            OrderServices objOrderServices = new OrderServices();
            UserServices objUserServices = new UserServices();

            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
            UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
            UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
            int OrdStatus = 0;
          //  int OrderID = 0;
            string ApproveOrder = string.Empty;
            //string lblshipingcost = string.Empty;
            //Direct  Order / Approve Order (Comes from Pending order Page)
            //lblpostcode2err.Text = "";
            //lbladdline1err.Text = "";
            //lbladdline2err.Text = "";

            OrdStatus = (int)OrderServices.OrderStatus.OPEN;
            decimal TaxAmount;
            decimal ProdTotCost;
            //if (string.IsNullOrEmpty(HttpContext.Current.Request["OrderID"]))
            //    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), 1);
            //else
            //    OrderID = Convert.ToInt32(HttpContext.Current.Request["OrderID"].ToString());

            int oldorderID = OrderID;
            //int QuoteId = 0;
            //QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));

            if (tt1 == "")
                tt1 = "WES" + OrderID.ToString();

            string refid = objHelperServices.CS(tt1);

            if (OrderID <= 0 || tt1.Length == 0)
            {
                //txterr.Text = "Please enter valid Order No, Order No should be 1 digit or more than 1 digits";
                OrderID = -1;
                HttpContext.Current.Session["OrderId"] = "-1";
            }
            //else if (hfduporderproceed.Value == "1")
            //{

            //}
            //else
            //{
            //}

            //if (drpSM1.SelectedValue.ToString().Trim() == "Drop Shipment Order")
            //{

            //    if (objOrderServices.GetDropShipmentKeyExist(txtPostCode.Text.ToString(), "PostCode") == true)
            //    {
            //        if (objOrderServices.GetDropShipmentKeyExist(txtAddressLine1.Text.ToString(), "") == true)
            //        {
            //            ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
            //        }
            //        if (txtAddressLine2.Text.ToString() != "" && objOrderServices.GetDropShipmentKeyExist(txtAddressLine2.Text.ToString(), "") == true)
            //        {
            //            ClientScript.RegisterStartupScript(typeof(Page), "CellinkAlert", "<script type='text/javascript'>alert('Non-Standard Delivery Area. We will contact you to confirm costing');</script>", false);
            //        }

            //    }
            //}

            if (OrderID > 0)
            {
                ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
         
                decimal shipcost = 0;
                string SHIP_CODE = "";

                if ((drpSM1 == "Courier") || (drpSM1 == "Mail") || (drpSM1 == "Drop Shipment Order"))
                {
                    if (shipInfo.lblshipingcost != "")
                    {
                        if (objHelperServices.CDEC(shipInfo.lblshipingcost) > 0)
                        {
                            shipcost = objHelperServices.CDEC(shipInfo.lblshipingcost);
                            SHIP_CODE = shipInfo.shipcode;
                            //SHIP_CODE = hfshipcode.Value;                         //uncomment after testing
                        }
                        else
                        {
                            string calship = HttpContext.Current.Session["shipcost"].ToString();
                            string[] charge = calship.Split('-');

                            shipcost = objHelperServices.CDEC(charge[0]);
                            SHIP_CODE = charge[1];
                        }
                    }
                    else if (HttpContext.Current.Session["shipcost"]!=null && HttpContext.Current.Session["shipcost"].ToString().Contains("-")==true)
                    {

                        string calship = HttpContext.Current.Session["shipcost"].ToString();
                        string[] charge = calship.Split('-');
                      
                        shipcost = objHelperServices.CDEC(charge[0]);
                        SHIP_CODE = charge[1];
                    }
                    if (shipcost == 0 && HttpContext.Current.Session["ShipCost_br"]!=null)
                    {
                        
                        shipcost = objHelperServices.CDEC(HttpContext.Current.Session["ShipCost_br"].ToString());
                        objErrorHandler.CreatePayLog("ShipCost_br" + shipcost);

                        SHIP_CODE = shipInfo.shipcode;
                    }
                }

                TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
                decimal UpdRst = 0;
                oOrdInfo.OrderID = OrderID;
                oOrdInfo.OrderStatus = OrdStatus;
                ///////////////////////////
                int _UserrID;
                _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                oOrdShippInfo = objUserServices.GetUserShipInfo(_UserrID);
                //txtSFName.Text = oOrdShippInfo.FirstName;
                //txtSLName.Text = oOrdShippInfo.LastName;
                //txtSMName.Text = oOrdShippInfo.MiddleName;
                //txtSAdd1.Text = oOrdShippInfo.ShipAddress1;
                //txtSAdd2.Text = oOrdShippInfo.ShipAddress2;
                //txtSAdd3.Text = oOrdShippInfo.ShipAddress3;
                //txtSCity.Text = oOrdShippInfo.ShipCity;
                //drpShipState.Text = oOrdShippInfo.ShipState;
                //txtSZip.Text = oOrdShippInfo.ShipZip;
                //Setdrpdownlistvalue(drpShipCountry, oOrdShippInfo.ShipCountry.ToString());
                //txtSPhone.Text = oOrdShippInfo.ShipPhone;
                
                oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                //txtbillFName.Text = oOrdBillInfo.FirstName;
                //txtbillLName.Text = oOrdBillInfo.LastName;
                //txtbillMName.Text = oOrdBillInfo.MiddleName;
                //txtbilladd1.Text = oOrdBillInfo.BillAddress1;
                //txtbilladd2.Text = oOrdBillInfo.BillAddress2;
                //txtbilladd3.Text = oOrdBillInfo.BillAddress3;
                //txtbillcity.Text = oOrdBillInfo.BillCity;
                //drpBillState.Text = oOrdBillInfo.BillState;
                //txtbillzip.Text = oOrdBillInfo.BillZip;
                //Setdrpdownlistvalue(drpBillCountry, oOrdBillInfo.BillCountry.ToString());
                //txtbillphone.Text = oOrdBillInfo.BillPhone;
                oOrdInfo.OrderStatus = OrdStatus;
                if (cmbProvider != "Please Select")
                {
                    oOrdInfo.ShipCompany = cmbProvider;
                }
                else
                {
                    oOrdInfo.ShipCompany = "";
                }

                oOrdInfo.ShipMethod = drpSM1;

                if (drpSM1.ToString().Trim() == "Drop Shipment Order")
                {
                    oOrdInfo.ShipCompName = objHelperServices.Prepare(shipInfo.drpShipCompName);
                    oOrdInfo.ShipFName = objHelperServices.Prepare(shipInfo.drpAttentionTo);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(shipInfo.drpaddress1);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(shipInfo.drpaddress2);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(shipInfo.drpsuburb);
                    //if (txtSuburb.Text != "")
                    //{

                    //    if (HttpContext.Current.Session[txtSuburb.Text] != null)
                    //    {
                    //        string[] shipx = HttpContext.Current.Session[txtSuburb.Text].ToString().Split('-');
                    //        drpstate_txt.Text = shipx[1];
                    //        txtPostCode.Text = shipx[0];
                    //    }
                    //}
                    oOrdInfo.ShipState = objHelperServices.Prepare(shipInfo.drpstate);
                    oOrdInfo.ShipZip = objHelperServices.Prepare(shipInfo.drppostcode);
                    if (shipInfo.drpstate == "")
                    {

                        if (HttpContext.Current.Session["suburb"] != null)
                        {
                            string[] shipx = HttpContext.Current.Session["suburb"].ToString().Split('-');
                            oOrdInfo.ShipState =  shipx[1];
                            oOrdInfo.ShipZip = shipx[0];
                        }
                    }
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(shipInfo.drpcountry);
                 
                    oOrdInfo.DeliveryInstr = objHelperServices.Prepare(shipInfo.drpinstruction);
                    oOrdInfo.ShipPhone = objHelperServices.Prepare(shipInfo.drpshipphone);

                }
                else
                {
                    oOrdInfo.ShipFName = objHelperServices.Prepare(oOrdShippInfo.FirstName);
                    oOrdInfo.ShipLName = objHelperServices.Prepare(oOrdShippInfo.LastName);
                    oOrdInfo.ShipMName = objHelperServices.Prepare(oOrdShippInfo.MiddleName);
                    oOrdInfo.ShipAdd1 = objHelperServices.Prepare(oOrdShippInfo.ShipAddress1);
                    oOrdInfo.ShipAdd2 = objHelperServices.Prepare(oOrdShippInfo.ShipAddress2);
                    oOrdInfo.ShipAdd3 = objHelperServices.Prepare(oOrdShippInfo.ShipAddress3);
                    oOrdInfo.ShipCity = objHelperServices.Prepare(oOrdShippInfo.ShipCity);
                    oOrdInfo.ShipState = objHelperServices.Prepare(oOrdShippInfo.ShipState);
                    oOrdInfo.ShipCountry = objHelperServices.Prepare(oOrdShippInfo.ShipCountry);
                    oOrdInfo.ShipZip = objHelperServices.Prepare(oOrdShippInfo.ShipZip);
                    oOrdInfo.ShipCompName = objHelperServices.Prepare(oOrdShippInfo.COMPANY_NAME);
                    if (drpSM1 != "Counter Pickup")
                    {
                        oOrdInfo.ShipPhone = objHelperServices.Prepare(oOrdShippInfo.ShipPhone);
                    }
                    else
                    {
                        objErrorHandler.CreatePayLog("inside order_submit_process for shipphone");
                        if (HttpContext.Current.Session["Nothanks"] == null || HttpContext.Current.Session["Nothanks"].ToString() == "false")
                        {
                            if (oOrdInfo.ShipPhone != "" && oOrdInfo.ShipPhone != null)
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(oOrdInfo.ShipPhone);
                            }
                            else if (txtMobileNumber != "")
                            {
                                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtMobileNumber);
                            }
                            else if (HttpContext.Current.Session["txtMobileNumber"] != null)
                            {

                                oOrdInfo.ShipPhone = objHelperServices.Prepare(txtMobileNumber);
                            }
                            objErrorHandler.CreatePayLog("shipphone:"+ oOrdInfo.ShipPhone);
                        }
                        else
                        {
                            oOrdInfo.ShipPhone = oOrdShippInfo.ShipPhone;
                        }
                    }

                    //DataSet objds = new DataSet();
                    //objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                    //if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    //{
                    //    oOrdInfo.DeliveryInstr = objHelperServices.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                    //}
                    oOrdInfo.DeliveryInstr = oOrdShippInfo.DeliveryInst;
                }


                oOrdInfo.ShipNotes = objHelperServices.Prepare(TextBox1);
                //TextBox1.Text = oOrdInfo.ShipNotes;


                string strHostName = System.Net.Dns.GetHostName();
                string clientIPAddress = "";
                if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                {
                    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    else
                        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }


                oOrdInfo.ClientIPAddress = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
                oOrdInfo.isEmailSent = false;
                oOrdInfo.isInvoiceSent = false;
                oOrdInfo.IsShipped = false;

                oOrdInfo.BillFName = objHelperServices.Prepare(oOrdBillInfo.FirstName);
                oOrdInfo.BillLName = objHelperServices.Prepare(oOrdBillInfo.LastName);
                oOrdInfo.BillMName = objHelperServices.Prepare(oOrdBillInfo.MiddleName);
                oOrdInfo.BillAdd1 = objHelperServices.Prepare(oOrdBillInfo.BillAddress1);
                oOrdInfo.BillAdd2 = objHelperServices.Prepare(oOrdBillInfo.BillAddress2);
                oOrdInfo.BillAdd3 = objHelperServices.Prepare(oOrdBillInfo.BillAddress3);
                oOrdInfo.BillCity = objHelperServices.Prepare(oOrdBillInfo.BillCity);
                oOrdInfo.BillState = objHelperServices.Prepare(oOrdBillInfo.BillState);
                oOrdInfo.BillCountry = objHelperServices.Prepare(oOrdBillInfo.BillCountry);
                oOrdInfo.BillZip = objHelperServices.Prepare(oOrdBillInfo.BillZip);
                oOrdInfo.BillPhone = objHelperServices.Prepare(oOrdBillInfo.BillPhone);
                oOrdInfo.BillcompanyName = objHelperServices.Prepare(oOrdBillInfo.COMPANY_NAME);
                //objErrorHandler.CreateLog("BillcompanyName" + oOrdInfo.BillcompanyName);
                oOrdInfo.ProdTotalPrice = ProdTotCost;
                oOrdInfo.ShipCost = shipcost;
                oOrdInfo.SHIP_CODE = SHIP_CODE;

                oOrdInfo.TaxAmount = TaxAmount;
                oOrdInfo.TotalAmount = ProdTotCost + TaxAmount + objHelperServices.CDEC(oOrdInfo.ShipCost);
                oOrdInfo.TrackingNo = "";
                if (drpSM1.ToString().Trim().Contains("Drop Shipment Order"))
                {
                    oOrdInfo.DropShip = 1;
                }
                oOrdInfo.UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

                UpdRst = objOrderServices.UpdateOrder(oOrdInfo);

                if (HttpContext.Current.Session["PrevOrderID"] != null && Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]) > 0)
                {
                    HttpContext.Current.Session["PrevOrderID"] = "0";
                }

                string PAYMENTSELECTION = "";
                PAYMENTSELECTION = paymentmethod;
                //if (RBCreditCard.Checked == true)    // uncomment the line after testing
                //{
                //    PAYMENTSELECTION = "SP";
                //}
                //else if (RBPaypal.Checked == true)
                //{
                //    PAYMENTSELECTION = "PP";
                //}
                //if (UpdRst > 0)
                //{
                //    UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                //}
                if (UpdRst > 0)
                {
                    objOrderServices.UpdateCustomFields(oOrdInfo);


                    HttpContext.Current.Session["ORDER_NO"] = null;
                    HttpContext.Current.Session["SHIPPING"] = null;
                    HttpContext.Current.Session["DELIVERY"] = null;
                    HttpContext.Current.Session["DROPSHIP"] = null;

                    HttpContext.Current.Session["ShipCost"] = oOrdInfo.ShipCost;
                   
                  
                        return ProceedFunction_Onlinepayment(paymentmethod,OrderID, refid, oOrdInfo.OrderStatus.ToString(),true);
                       
                }
            }

        }
        catch (Exception Ex)
        {
            objErrorHandler.CreateLog(Ex.ToString());
            objErrorHandler.ErrorMsg = Ex;
            objErrorHandler.CreateLog();
        }

        return "";
    }



    protected void btnSecurePay_Click(object sender, EventArgs e)
    {
        try
        {

            if (issecurepayclicked == false)
            {
                string rtnstr = "";
               
                if (Page.IsValid == true)
                {
                    btnSP.Visible = false;
                    BtnProgressSP.Visible = true;
                    issecurepayclicked = true;
                    //try
                    //{
                    //    txtCardNumber.Style.Remove("border");
                    //    drpExpmonth.Style.Remove("border");
                    //    drpExpyear.Style.Remove("border");
                    //    txtCVV.Style.Remove("border");
                    //    //drppaymentmethod.Style.Remove("border");
                    //}
                    //catch
                    //{
                    //}

                    SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();


                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    if (oOrderInfo.ProdTotalPrice == 0)
                    {
                        objErrorHandler.CreatePayLog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                        Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    }

                    DataSet tmpds = GetOrderItemDetailSum(OrderID);
                    decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                    {
                        totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    }

                    if (totalitemsum.ToString() == "0.00")
                    {
                        objErrorHandler.CreatePayLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                        Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    }

                    UserServices objUserServices = new UserServices();
                    UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                    //int i = objUserServices.GetCheckOutOption(Userid);
                    //if ((i == 1) && (tt1.Text == ""))
                    //{
                    //    txterr.Text = "Please Enter Order No";
                    //    lblMessage.Text = "Please Enter Order No";
                    //    hftt1.Value = "1";
                    //    mpeMessageBox.Show();
                    //    OrderID = -1;
                    //    Session["OrderId"] = "-1";
                    //    divcreditcard.Style.Add("display", "block");
                    //    divdedault.Style.Add("display", "none");
                    //    divpaypal.Style.Add("display", "none");
                    //    btnSP.Visible = true;
                    //    BtnProgressSP.Visible = false;
                    //    issecurepayclicked = false;
                    //    return;
                    //}
                    //else
                    //{
                    //    hftt1.Value = "0";
                    //}
                    try
                    {

                        int currentyear = DateTime.Now.Year;
                        int currentmonth = DateTime.Now.Month;
                        bool isexpvalid = false;
                        //if (Convert.ToInt32(drpExpyear.SelectedValue) > currentyear)
                        //{
                        //    isexpvalid = true;
                        //}
                        //else if (Convert.ToInt32(drpExpyear.SelectedValue) == currentyear)
                        //{
                        //    if (Convert.ToInt32(drpExpmonth.SelectedValue) >= currentmonth)
                        //    {

                        //        isexpvalid = true;
                        //    }
                        //}


                        if (isexpvalid == true)
                        {
                            oPayInfo = objPaymentServices.GetPayment(OrderID);

                            //  decimal    TaxAmount = objOrderServices.CalculateTaxAmount(oOrderInfo.ProdTotalPrice + shipcost, OrderID.ToString());
                            decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
                            objErrorHandler.CreatePayLog(OrderID.ToString() + "paymnet amount:" + oPayInfo.Amount + "ProdTotalPrice:" + oOrderInfo.ProdTotalPrice + "ShipCost" + oOrderInfo.ShipCost + "taxamount" + TaxAmount);
                            if (oPayInfo.PaymentID == 0 || oPayInfo.Amount != (oOrderInfo.ProdTotalPrice + oOrderInfo.ShipCost + TaxAmount))
                            {


                                objErrorHandler.CreatePayLog("inside before opayinfo");
                                order_submit_process();
                                objErrorHandler.CreatePayLog("inside after opayinfo");
                                oPayInfo = objPaymentServices.GetPayment(OrderID);
                                PaymentID = oPayInfo.PaymentID;
                            }
                            else
                            {
                                PaymentID = oPayInfo.PaymentID;
                            }
                         //   objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtnamecard.Text, txtcreditcardno.Text, txtCVV.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text, HttpContext.Current.Session["USER_ID"].ToString());
                            objErrorHandler.CreatePayLog(objPRInfo.Error_Text);
                            //   objPRInfo.Error_Text = "";
                            if (objPRInfo.Error_Text != "")
                            {
                                btnSP.Style.Add("display", "block");
                                BtnProgressSP.Style.Add("display", "none");
                                // ImgBtnEditShipping.Style.Add("display", "block");

                                div2.InnerHtml = "Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
                                isordersubmited.Value = "true";
                                divpaymentoption.Style.Add("display", "block");
                                trpm.Visible = false;
                                //div2.Visible = true;
                                div2.Style.Add("display", "block");
                                //  div2.Focus();
                                Scroll_To_Control("ctl00_maincontent_div2");
                                divcreditcard.Style.Add("display", "block");
                                divdedault.Style.Add("display", "none");
                                divpaypal.Style.Add("display", "none");
                                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:creditcardclick();", true);
                                greenalert.Visible = false;
                                //   Page.RegisterStartupScript("page", "<script language="'javascript'">creditcardclick()</script>");
                                ImageButton2.Visible = false;
                                trpm.Visible = false;


                                HttpContext.Current.Session["payflowresponse"] = "FAIL";

                                if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
                                {
                                    //txtcreditcardno.Style.Add("border", "1px solid #FF0000");
                                }
                                else
                                {
                                   // txtcreditcardno.Style.Remove("border");
                                }

                                if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
                                {
                                    txtCVV.Style.Add("border", "1px solid #FF0000");
                                }
                                else
                                {

                                    txtCVV.Style.Remove("border");
                                }

                                if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
                                {
                                    //drpExpmonth.Style.Add("border", "1px solid #FF0000");
                                    //drpExpyear.Style.Add("border", "1px solid #FF0000");
                                }
                                else
                                {

                                   // drpExpmonth.Style.Remove("border");

                                }
                                if (objPRInfo.Error_Text.ToLower().Contains("card type") == true)
                                {
                                    //drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
                                }
                                HttpContext.Current.Session["paySPresponse"] = "";
                                btnSP.Visible = true;
                                BtnProgressSP.Visible = false;
                                issecurepayclicked = false;
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
                                HttpContext.Current.Session["payflowresponse"] = "SUCCESS";
                                //int x = txtcreditcardno.Text.Length;
                                ////     objErrorHandler.CreatePayLog("txtcreditcardno"+"startinglength:"+ (x-4).ToString() +"endinglength:"+(x-1).ToString());
                                //Response.Redirect("BillInfoSP.aspx?Paytype=direct&key=" + OrderID.ToString()  + "&cn=" + txtcreditcardno.Text.Substring(x - 4, 4));

                                //Response.Redirect("BillInfoSP.aspx?key=" + OrderID.ToString() + "#####" + "&PaySP" + "#####" + "Paid");
                            }









                        }
                        else
                        {
                            RBexpirydate.Visible = true;
                            RBexpirydate.Text = "Please Select valid expiry date";

                            div2.InnerHtml = "Error found in details you have entered. Please Select valid expiry date."; //objPRInfo.Error_Text;
                            //div2.Visible = true;
                            //


                            div2.Style.Add("display", "block");
                            Scroll_To_Control("ctl00_maincontent_div2");
                            // div2.Focus();
                            divcreditcard.Style.Add("display", "block");
                            divdedault.Style.Add("display", "none");
                            divpaypal.Style.Add("display", "none");
                            btnSP.Visible = true;
                            BtnProgressSP.Visible = false;
                            issecurepayclicked = false;

                        }




                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString());
                    }
                }
                else
                {
                    divcreditcard.Style.Add("display", "block");
                    divdedault.Style.Add("display", "none");
                    divpaypal.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }

    }

    public static DataSet GetOrderItemDetailSum(int OrderID)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        //objErrorHandler.CreateLog("GetOrderItemDetailSum");
        try
        {
            OrderDB objOrderDB = new OrderDB();
            return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_ITEM_DETAIL_SUM", OrderDB.ReturnType.RTDataSet);
          
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return null;
        }
        
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


            if (oOrderInfo.CreatedUser != 999)
            {

                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
            }
            else
            {
                oUserInfo = objUserServices.GetUserInfo(oOrderInfo.UserID);
            }

            //   oUserInfo = objUserServices.GetUserInfo(oOrderInfo.CreatedUser);
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

                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner3 OrderId=" + OrderId);
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
                    //MessageObj.Bcc.Add(webadminmail);
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
                MessageObj.Subject = "WES Order Payment Successful - Order No : " + CustomerOrderNo.ToString();
                //}
                //else
                //{
                //    MessageObj.Subject = "Wagner Pending Order Notification - Order No : " + CustomerOrderNo.ToString();
                // }

                MessageObj.IsBodyHtml = true;
                MessageObj.Body = sHTML;

                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner4 OrderId=" + OrderId);
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                smtpclient.UseDefaultCredentials = false;
                smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                smtpclient.Send(MessageObj);
                objHelperServices.Mail_Log("PP", oOrderInfo.OrderID, MessageObj.To.ToString(), "");
                objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner5 OrderId=" + OrderId);

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


        }
        objErrorHandler.CreatePayLog("SendMail_AfterPaymentPP inner6 OrderId=" + OrderId);
    }
   
    protected void btnPayApi_Click(object sender, EventArgs e)
    {
        try
        {
            objErrorHandler.CreatePayLog("btnPayApi_Click start Orderid=" + OrderID);

            GetParams();
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();

            oOrderInfo = objOrderServices.GetOrder(OrderID);
            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreatePayLog("btnPayApi start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }

            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (totalitemsum.ToString() == "0.00")
            {
                objErrorHandler.CreatePayLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }

            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


            renUrl = renUrl.Replace("MCheckOut", "MCheckOut");
            renUrl = renUrl + "?key=" + EncryptSP("Paid");


            oOrderInfo = objOrderServices.GetOrder(OrderID);
            oUserInfo = objUserServices.GetUserInfo(Userid);

            //if (oUserInfo.Country.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oUserInfo.Country.ToLower()).ToLower() == "au")
            //{
            //    div1.InnerHtml = "";
            //    div2.InnerHtml = "Please email sales@wagneronline.com.au to process your order.<br/>In your email please include items you would like to order and shipping location";
            //    return;
            //}
            if (oPayInfo.PayResponse.ToLower() == "yes")
            {
                divContent.InnerHtml = "Already payment has been made , Ref. Payment History";
                return;
            }

            string Requeststr = objPayPalApiService.PayPalSetECRequest(OrderID, PaymentID, oOrderInfo, renUrl);

            if (Requeststr.Contains("Form") == false)
                divContent.InnerHtml = Requeststr;
            else
                this.Page.Controls.Add(new LiteralControl(Requeststr));

            objErrorHandler.CreatePayLog("btnPayApi_Click end Orderid=" + OrderID);
        }
        catch (Exception ex)
        {
 
        }
    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
            oOrderInfo = objOrderServices.GetOrder(OrderID);
            if (oOrderInfo.ProdTotalPrice == 0)
            {
                objErrorHandler.CreatePayLog("btnPay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }

            DataSet tmpds = GetOrderItemDetailSum(OrderID);
            decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
            {
                totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
            }

            if (totalitemsum.ToString() == "0.00")
            {
                objErrorHandler.CreatePayLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
            }

            UserServices objUserServices = new UserServices();
            int i = objUserServices.GetCheckOutOption(Userid);
            if ((i == 1) && (tt1.Text == ""))
            {
                txterr.Text = "Please Enter Order No";
                lblMessage.Text = "Please Enter Order No";
                hftt1.Value = "1";
                mpeMessageBox.Show();

                OrderID = -1;
                Session["OrderId"] = "-1";
                divcreditcard.Style.Add("display", "none");
                divpaypal.Style.Add("display", "block");
                divdedault.Style.Add("display", "none");
                return;
            }
            else
            {
                hftt1.Value = "0";
            }
            objErrorHandler.CreatePayLog("btnPay_Click start Orderid=" + OrderID);
            //div2.Visible = false;
            div2.Style.Add("display", "none");
            //  GetParams();
           // objErrorHandler.CreateLog("b4 GetPayment");
            oPayInfo = objPaymentServices.GetPayment(OrderID);
            //objErrorHandler.CreateLog("after GetPayment");
            //  decimal TaxAmount = objOrderServices.CalculateTaxAmount(oOrderInfo.ProdTotalPrice + shipcost, OrderID.ToString());
            //objErrorHandler.CreateLog("b4 GetTotalOrderTaxAmount");
            decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
            objErrorHandler.CreateLog("after GetTotalOrderTaxAmount");
            if (oPayInfo.PaymentID == 0 || oPayInfo.Amount != (oOrderInfo.ProdTotalPrice + oOrderInfo.ShipCost + TaxAmount))
            {
                //objErrorHandler.CreateLog("b4 order_submit_process");
                order_submit_process();
                objErrorHandler.CreateLog("after order_submit_process");
                //oPayInfo = objPaymentServices.GetPayment(OrderID);
                //PaymentID = oPayInfo.PaymentID;
            }
            else
            {
                PaymentID = oPayInfo.PaymentID;
            }
          
            
            UserServices.UserInfo oUserInfo = new UserServices.UserInfo();


            //objErrorHandler.CreateLog("before gt");
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
            greenalert.Visible = false;
            divpaymentoption.Visible = false;
          
            string returnurl = "/Billinfo.aspx?Paytype=direct&key=" + EncryptSP(OrderID.ToString() + "#####" + "PayPP" + "#####" + "Paid");
            objErrorHandler.CreatePayLog(returnurl);
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

    protected void OnTextChanged(object sender, EventArgs e)
    {
        Calculate_shipping();
    }
    protected void BtnNothanks_Click(object sender, EventArgs e)
    {
        txtMobileNumber.Text = "";
      //  hfnothanks.Value = "1";
    //    hfordernumber.Value = "";
       // ImageButton4_Click(sender, e);

    }
    protected void btnNoThanksChange_Click(object sender, EventArgs e)
    {
       // txtchangemobilenumber.Text = hfphonenumber.Value;
        cbmobilechange.Checked = true;
        lblorderreadytext.Text = "SMS Order ready notification will NOT be sent.";
       //hfnothanks.Value = "1";
        lblorderready.Text = "";
       // hfordernumber.Value = "";
        //hfchange.Value = "1";
        txtMobileNumber.Text = "";
        ImageButton2.Focus();
    }
    protected void drpSM1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //objErrorHandler.CreatePayLog("Inside dropshipchange");
        //objErrorHandler.CreatePayLog("Selectedvalue"+ drpSM1.SelectedValue.ToString());
        if (drpSM1.SelectedValue.ToString().Trim() == "Counter Pickup" || drpSM1.SelectedValue.ToString().Trim() == "Shop Counter Pickup")
        {
            //objErrorHandler.CreatePayLog("Inside counter"+ drpSM1.SelectedValue.ToString());

            ImageButton2.Visible = false;
            lblshipingcost.Text = "0";
          decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(oOrdInfo.OrderID) ;
            HttpContext.Current.Session["shipcode"] = "";
            lbltotalamount1.Text = (objHelperServices.FixDecPlace(objHelperServices.CDEC(oOrdInfo.ProdTotalPrice+ TaxAmount))).ToString();
            objErrorHandler.CreatePayLog("Ilbltotalamount1" + lbltotalamount1.Text + oOrdInfo.OrderID);
        }
        else
        {
            Calculate_shipping();
        }
        if (drpSM1.SelectedValue == "Mail")
        {
            string msg = "**** NOTE ****<br> Mail will be used for parcels up to 500 grams including packaging. Parcels over 500 grams will be sent by the most economical way e.g. Courier, Road, etc. ";
            lblMessage.Text = msg;
            mpeMessageBox.Show();
        }
        else if (drpSM1.SelectedValue == "Courier Pickup")
        {
            string msg = "**** NOTE ****<br>Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter into the Comments / Notes box the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            lblMessage.Text = msg;
           
            mpeMessageBox.Show();
        }
        //if (drpSM1.SelectedValue == "Counter Pickup")
        //{
        //    if (lblorderready.Text != "")
        //    {
        //        smspopup.Visible = true;
        //    }
        //    else
        //    {

        //    }
        //}
       // GetPaymentTerm(Userid.ToString());
        //[WebMethod]
        //public static bool GetDropShipmentKeyExists(string strvalue,string type)
        //{
        //    OrderServices objOrderServices1 =new OrderServices();

        //    bool retval = false;
        //    retval=objOrderServices1.GetDropShipmentKeyExist(strvalue, type);
        //    return retval;
        //}
        GenerateClientToken();
    }

    private void visibilepayment()
    {
        //objErrorHandler.CreateLog("inside visibilepayment");
        divpaymentoption.Visible = true;
        divpaymentoption.Style.Add("display", "block");
        trpm.Visible = false;
        ImageButton2.Visible = false;
        rbinternationaldefault_remote = false;
    }
    //Logic Change has per mail dated on 14 March 2018
    public string Calculate_shipping()
    {
        try
        {

            //objErrorHandler.CreateLog("inside Calculate_shipping");
            if (drpSM1.SelectedValue != "Please Select Shipping Method")
            {

               
                if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue == "Drop Shipment Order"))
                {
                    string ZONE = GetZone(OrderID);
                    lblzone.Text = ZONE;
                 
                    decimal producttotalweight_all = 0;

                    DataSet dsODall = objOrderServices.GetOrderItems(OrderID);

                    if (dsODall != null && dsODall.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsODall.Tables[0].Rows.Count; i++)
                        {

                            producttotalweight_all = producttotalweight_all + objUserServices.GET_PRODUCTWEIGHT(dsODall.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsODall.Tables[0].Rows[i]["qty"].ToString()));
                           // objErrorHandler.CreateLog("product wight for" + dsODall.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight_all);

                        }
                    }
                    lblweight.Text = producttotalweight_all.ToString();

                    if (ispickuponly_product(OrderID) == true)
                    {
                        divpaymentoption.Style.Add("display", "none");

                        divInternationalpayoption.Style.Add("display", "none");
                        ImageButton2.Visible = true;
                        showPaymentType(Userid.ToString());
                        // rb.Checked = true;
                        RBpaymenttype.Checked = false;
                        rbinternationaldefault_remote = true;
                        Session["emailconfimation"] = "Remote";
                        emailconfimation = "Remote";
                        lblshipingcost.Text = "0";
                        oOrdInfo.ShipCost = 0;
                        oOrdInfo.SHIP_CODE = "";
                        hfshipcode.Value = "";
                    }
                    //Local Start
                    else if (ZONE == "LOCAL")
                    {
                        visibilepayment();

                        if (drpSM1.SelectedValue == "Courier")
                        {

                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
                            if (Sqltbs != null)
                            {
                                if (Sqltbs.Tables[0].Rows.Count > 0)
                                {

                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                    oOrdInfo.ShipCost = price;
                                  //  objErrorHandler.CreateLog("delivery cost 4 localzone " + price.ToString());
                                    lblshipingcost.Text = price.ToString();
                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
                                    if (dsprodcode != null)
                                    {
                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                    }
                                  
                                }

                            }
                            //decimal producttotalweight = 0;

                            //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                            //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                            //    {

                            //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
                            //        objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

                            //    }
                            //}
                            //lblweight.Text = producttotalweight.ToString();
                            //lblweight.Visible = true;
                            //objErrorHandler.CreateLog("product total weight" + lblweight.Text);
                            //if ((producttotalweight < Convert.ToDecimal(1000)))
                            //{
                            //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74555", Userid.ToString());
                            //    if (Sqltbs != null)
                            //    {
                            //        if (Sqltbs.Tables[0].Rows.Count > 0)
                            //        {

                            //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                            //            oOrdInfo.ShipCost = price;
                            //            lblshipingcost.Text = price.ToString();
                            //            objErrorHandler.CreateLog("delivery cost 4 LOCAL" + price.ToString());
                            //        }

                            //    }

                            //}
                            //else
                            //{
                            //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
                            //    if (Sqltbs != null)
                            //    {
                            //        if (Sqltbs.Tables[0].Rows.Count > 0)
                            //        {

                            //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                            //            oOrdInfo.ShipCost = price;
                            //            objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                            //            lblshipingcost.Text = price.ToString();
                            //        }

                            //    }

                            //}
                        }

                        //Local courier ends here
                        else if (drpSM1.SelectedValue == "Mail")
                        {

                            //decimal producttotalweight = 0;



                            //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                            //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                            //    {

                            //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
                            //        objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

                            //    }
                            //}
                            //lblweight.Text = producttotalweight.ToString();
                            //Console.WriteLine("Total Weight"+producttotalweight);
                            //lblweight.Visible = true;
                            //objErrorHandler.CreateLog("product total weight" + producttotalweight);
                            if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
                            {
                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
                                if (Sqltbs != null)
                                {
                                    if (Sqltbs.Tables[0].Rows.Count > 0)
                                    {

                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                        oOrdInfo.ShipCost = price;
                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
                                        if (dsprodcode != null)
                                        {
                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        }
                                           
                                        lblshipingcost.Text = price.ToString();
                                        //objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
                                    }

                                }
                            }
                            else
                            {

                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
                                if (Sqltbs != null)
                                {
                                    if (Sqltbs.Tables[0].Rows.Count > 0)
                                    {

                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                        oOrdInfo.ShipCost = price;
                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
                                        if (dsprodcode != null)
                                        {
                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        }

                                        //objErrorHandler.CreateLog("delivery cost 4 localzone" + price.ToString());
                                        lblshipingcost.Text = price.ToString();
                                    }

                                }


                            }




                        }
                        else if (drpSM1.SelectedValue == "Drop Shipment Order")
                        {
                            divpaymentoption.Style.Add("display", "block");
                            GetPaymentTerm(Userid.ToString());
                            trpm.Visible = false;
                            ImageButton2.Visible = false;

                            //DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "84528", Userid.ToString());
                            
                            if (Sqltbs != null)
                            {
                                if (Sqltbs.Tables[0].Rows.Count > 0)
                                {

                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                    oOrdInfo.ShipCost = price;
                                    lblshipingcost.Text = price.ToString();
                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 84528  and attribute_id=1");
                                    if (dsprodcode != null)
                                    {
                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                    }
                                   // objErrorHandler.CreateLog("delivery cost 7 localzone" + price.ToString());
                                  
                                }

                            }
                        }
                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
                        {
                            showselectedtab_ondropdownchange();

                        }
                        
                    }
                    //////////////////// Zone/////////////////
                    else if (ZONE == "" || ZONE == null)
                    {

                        visibilepayment();
                        //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
                        //if (Sqltbs != null)
                        //{
                        //    if (Sqltbs.Tables[0].Rows.Count > 0)
                        //    {

                        //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                        //        oOrdInfo.ShipCost = price;
                        //        lblshipingcost.Text = price.ToString();
                        //        objErrorHandler.CreateLog("delivery cost 7 LOCAL" + price.ToString());
                        //    }

                        //}

                      //  objErrorHandler.CreateLog("inside zone null or empty"); 
                        if (drpSM1.SelectedValue == "Courier")
                        {

                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                            if (Sqltbs != null)
                            {
                                if (Sqltbs.Tables[0].Rows.Count > 0)
                                {

                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                    oOrdInfo.ShipCost = price;
                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
                                    if (dsprodcode != null)
                                    {
                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                    }
                                    //objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                    lblshipingcost.Text = price.ToString();
                                }

                            }
                        }
                        else if (drpSM1.SelectedValue == "Mail")
                        {

                            //decimal producttotalweight_mail = 0;



                            //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                            //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                            //    {
                            //        try
                            //        {
                            //            producttotalweight_mail = producttotalweight_mail + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
                            //            objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight_mail);
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            objErrorHandler.CreateLog(ex.ToString());
                            //        }
                            //    }
                            //}
                            //lblweight.Text = producttotalweight_mail.ToString();
                            //lblweight.Visible = true;
                            //Console.WriteLine(producttotalweight_mail);
                            //objErrorHandler.CreateLog("product total weight" + producttotalweight_mail.ToString());
                            if ((producttotalweight_all< Convert.ToDecimal(Lweight)))
                            {
                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
                               
                                if (Sqltbs != null)
                                {
                                    if (Sqltbs.Tables[0].Rows.Count > 0)
                                    {

                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                        oOrdInfo.ShipCost = price;
                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
                                        if (dsprodcode != null)
                                        {
                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        }
                                        lblshipingcost.Text = price.ToString();
                                       // objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
                                    }

                                }
                            }
                            else
                            {
                              //  objErrorHandler.CreateLog("inside else mail" + "GetProductPriceEA" + "96051-74556");
                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                              //  objErrorHandler.CreateLog("after sql" + Sqltbs.Tables[0].Rows.Count);
                                if (Sqltbs != null)
                                {
                                    if (Sqltbs.Tables[0].Rows.Count > 0)
                                    {

                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                        oOrdInfo.ShipCost = price;
                                     //   objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
                                        if (dsprodcode != null)
                                        {
                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        }
                                        lblshipingcost.Text = price.ToString();
                                    }

                                }


                            }
                        }
                        else if (drpSM1.SelectedValue == "Drop Shipment Order")
                        {
                            divpaymentoption.Style.Add("display", "block");
                            GetPaymentTerm(Userid.ToString());
                            trpm.Visible = false;
                            ImageButton2.Visible = false;
                            // DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "84528", Userid.ToString());
                            if (Sqltbs != null)
                            {
                                if (Sqltbs.Tables[0].Rows.Count > 0)
                                {

                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                    oOrdInfo.ShipCost = price;
                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 84528  and attribute_id=1");
                                    if (dsprodcode != null)
                                    {
                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                    }
                                    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                    lblshipingcost.Text = price.ToString();
                                }

                            }
                        }
                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
                        {
                            showselectedtab_ondropdownchange();

                        }

                    }

                        ///////Remote///
                    else if ((ZONE == "REMOTE") || (ZONE == "NOTFOUND"))
                    {
                     objErrorHandler.CreateLog("inside Remote zone");
                        //decimal producttotalweight = 0;



                        //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

                        //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
                        //    {
                        //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
                        //        objErrorHandler.CreateLog("product weight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

                        //    }
                        //}
                        //lblweight.Text = producttotalweight.ToString();
                        //lblweight.Visible = true;
                        //objErrorHandler.CreateLog("product total weight" + lblweight.Text);
                         //|| ((producttotalweight == 0) && (ZONE == "NOTFOUND") )
                        if ((producttotalweight_all >= Convert.ToDecimal(remoteweight)))
                        {
                            divpaymentoption.Style.Add("display", "none");

                            divInternationalpayoption.Style.Add("display", "none");
                            ImageButton2.Visible = true;
                            showPaymentType(Userid.ToString());
                            RBpaymenttype.Checked = false;
                            rbinternationaldefault_remote = true;
                            lblshipingcost.Text = "0";
                            oOrdInfo.ShipCost = 0   ;
                            oOrdInfo.SHIP_CODE = "";
                            hfshipcode.Value = "";
                            emailconfimation = "Remote";
                            Session["emailconfimation"] = "Remote";
                        }
                        else
                        {
                            if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
                            {
                                showselectedtab_ondropdownchange();

                            }
                            else
                            {

                                GetPaymentTerm(Userid.ToString());
                                showselectedtab_ondropdownchange();
                            }


                            divpaymentoption.Style.Add("display", "block");
                            trpm.Visible = false;
                            ImageButton2.Visible = false;
                            rbinternationaldefault_remote = false;
                            //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
                            //if (Sqltbs != null)
                            //{
                            //    if (Sqltbs.Tables[0].Rows.Count > 0)
                            //    {

                            //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                            //        oOrdInfo.ShipCost = price;
                            //        objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                            //        lblshipingcost.Text = price.ToString();
                            //    }

                            //}
                            if (drpSM1.SelectedValue == "Courier")
                            {

                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                                if (Sqltbs != null)
                                {
                                    if (Sqltbs.Tables[0].Rows.Count > 0)
                                    {

                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                        oOrdInfo.ShipCost = price;
                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
                                        if (dsprodcode != null)
                                        {
                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                        }
                                       // objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                        lblshipingcost.Text = price.ToString();
                                    }

                                }
                            }
                            else if (drpSM1.SelectedValue == "Mail")
                            {

                               
                                if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
                                {
                                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
                                    if (Sqltbs != null)
                                    {
                                        if (Sqltbs.Tables[0].Rows.Count > 0)
                                        {

                                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                            oOrdInfo.ShipCost = price;

                                            DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
                                            if (dsprodcode != null)
                                            {
                                                oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                                hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            }
                                           
                                          
                                            lblshipingcost.Text = price.ToString();
                                           // objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
                                        }

                                    }
                                }
                                else
                                {

                                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                                    if (Sqltbs != null)
                                    {
                                        if (Sqltbs.Tables[0].Rows.Count > 0)
                                        {

                                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                            oOrdInfo.ShipCost = price;
                                            DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
                                            if (dsprodcode != null)
                                            {
                                                oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                                hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            }
                                        //    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                            lblshipingcost.Text = price.ToString();
                                        }

                                    }


                                }
                            }
                            else if (drpSM1.SelectedValue == "Drop Shipment Order" && txtPostCode.Text!="")
                            {
                                objErrorHandler.CreateLog(producttotalweight_all + "Drop Shipment Order");
                                if ((producttotalweight_all >= Convert.ToDecimal(remoteweight_drop)))
                                {
                                    objErrorHandler.CreateLog(producttotalweight_all + "Drop Shipment Order"); 
                                    divpaymentoption.Style.Add("display", "none");

                                    divInternationalpayoption.Style.Add("display", "none");
                                    ImageButton2.Visible = true;
                                    showPaymentType(Userid.ToString());
                                    RBpaymenttype.Checked = false;
                                    rbinternationaldefault_remote = true;
                                    lblshipingcost.Text = "0";
                                    oOrdInfo.ShipCost = 0;
                                    oOrdInfo.SHIP_CODE = "";
                                    hfshipcode.Value = "";
                                    emailconfimation = "Remote";
                                    Session["emailconfimation"] = "Remote";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Call_dropshippopup();", true);
                                }
                                else
                                {

                                    divpaymentoption.Style.Add("display", "block");
                                    GetPaymentTerm(Userid.ToString());
                                    trpm.Visible = false;
                                    ImageButton2.Visible = false;
                                    //DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
                                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "84528", Userid.ToString());
                                    if (Sqltbs != null)
                                    {
                                        if (Sqltbs.Tables[0].Rows.Count > 0)
                                        {

                                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                                            oOrdInfo.ShipCost = price;
                                            DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 84528  and attribute_id=1");
                                            if (dsprodcode != null)
                                            {
                                                oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
                                                hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
                                            }
                                            // objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                                            lblshipingcost.Text = price.ToString();
                                        }

                                    }
                                }
                            }
                        }


                    }

                    ////////Remote zone ending
                    //ZONE == "" ||
                    //else if (ZONE == "NOTFOUND")
                    //{
                    //    divpaymentoption.Style.Add("display", "none");

                    //    divInternationalpayoption.Style.Add("display", "none");
                    //    ImageButton2.Visible = true;
                    //    RBpaymenttype.Checked = false;
                    //    rbinternationaldefault_remote = true;
                    //    emailconfimation = "Remote";
                    //}
                }
                //else if (drpSM1.SelectedValue == "Drop Shipment Order")
                //{
                //    divpaymentoption.Style.Add("display", "block");
                //    ImageButton2.Visible = false;
                //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
                //    if (Sqltbs != null)
                //    {
                //        if (Sqltbs.Tables[0].Rows.Count > 0)
                //        {

                //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
                //            oOrdInfo.ShipCost = price;
                //            objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
                //            lblshipingcost.Text = price.ToString();
                //        }

                //    }
                //}
                else
                {
                    lblweight.Text = "";
                    lblzonezip.Text = "";
                    lblzone.Text = "";
                    if (drpSM1.SelectedValue != "International Shipping - TBA")
                    {
                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
                        {
                            showselectedtab_ondropdownchange();

                        }
                        else
                        {

                            GetPaymentTerm(Userid.ToString());
                            showselectedtab_ondropdownchange();
                        }

                        rbinternationaldefault_remote = false;
                        divpaymentoption.Style.Add("display", "block");
                        ImageButton2.Visible = false;
                        trpm.Visible = false;
                    }

                }
            }

            if (HttpContext.Current.Session["USER_ROLE"] != null && HttpContext.Current.Session["USER_ROLE"].ToString() == "3")
            {
                //objErrorHandler.CreateLog("inside user role 3");
                divpaymentoption.Style.Add("display", "none");
                trpm.Visible = false;
                ImageButton2.Visible = true;
                hfrole3.Value = "true";
            }
            else {
                objErrorHandler.CreateLog("inside user role 3 else");
                hfrole3.Value = "0"; 
            }
        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }



        Session["shipcost"] = oOrdInfo.ShipCost + "-" + oOrdInfo.SHIP_CODE;
        Session["shipcode"] = oOrdInfo.SHIP_CODE;
        HttpContext.Current.Session["ShipCost_br"] = oOrdInfo.ShipCost;
         decimal shipcost = oOrdInfo.ShipCost;
        decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(oOrdInfo.OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
        lbltotalamount1.Text = objHelperServices.FixDecPlace((objHelperServices.CDEC(oOrdInfo.ProdTotalPrice + shipcost + TaxAmount))).ToString();
        return oOrdInfo.ShipCost + "-"+oOrdInfo.SHIP_CODE;
    }


    //public static string Calculate_shipping(string drpSM1)
    //{
    //    try
    //    {
    //        OrderServices objOrderServices = new OrderServices();
    //        UserServices objUserServices = new UserServices();
    //        HelperServices objHelperServices = new HelperServices();
    //        OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    //        HelperDB objHelperDB = new HelperDB();
           
    //        int OrderID = 0;
    //        if (HttpContext.Current.Session["ORDER_ID"] != null)
    //        {
    //            OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
    //        }
    //        int _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());

    //        if (drpSM1 != "Please Select Shipping Method")
    //        {


    //            if ((drpSM1 == "Courier") || (drpSM1 == "Mail") || (drpSM1 == "Drop Shipment Order"))
    //            {
    //                string ZONE = GetZone(OrderID);
    //                //lblzone.Text = ZONE;

    //                decimal producttotalweight_all = 0;

    //                DataSet dsODall = objOrderServices.GetOrderItems(OrderID);

    //                if (dsODall != null && dsODall.Tables[0].Rows.Count > 0)
    //                {
    //                    for (int i = 0; i < dsODall.Tables[0].Rows.Count; i++)
    //                    {
    //                        producttotalweight_all = producttotalweight_all + objUserServices.GET_PRODUCTWEIGHT(dsODall.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsODall.Tables[0].Rows[i]["qty"].ToString()));
    //                    }
    //                }
    //                lblweight.Text = producttotalweight_all.ToString();

    //                if (ispickuponly_product(OrderID) == true)
    //                {
    //                    divpaymentoption.Style.Add("display", "none");

    //                    divInternationalpayoption.Style.Add("display", "none");
    //                    ImageButton2.Visible = true;
    //                    showPaymentType(Userid.ToString());
    //                    RBpaymenttype.Checked = false;
    //                    rbinternationaldefault_remote = true;
    //                    Session["emailconfimation"] = "Remote";
    //                    emailconfimation = "Remote";
    //                    lblshipingcost.Text = "0";
    //                    oOrdInfo.ShipCost = 0;
    //                    oOrdInfo.SHIP_CODE = "";
    //                    hfshipcode.Value = "";
    //                }
    //                //Local Start
    //                else if (ZONE == "LOCAL")
    //                {
    //                    visibilepayment();

    //                    if (drpSM1.SelectedValue == "Courier")
    //                    {

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                lblshipingcost.Text = price.ToString();
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }

    //                            }

    //                        }
    //                    }

    //                    //Local courier ends here
    //                    else if (drpSM1.SelectedValue == "Mail")
    //                    {
    //                        if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                        {
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }

    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                        else
    //                        {

    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (drpSM1.SelectedValue == "Drop Shipment Order")
    //                    {
    //                        divpaymentoption.Style.Add("display", "block");
    //                        GetPaymentTerm(Userid.ToString());
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                lblshipingcost.Text = price.ToString();
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                            }
    //                        }
    //                    }
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }

    //                }
    //                else if (ZONE == "" || ZONE == null)
    //                {

    //                    visibilepayment();
    //                    if (drpSM1.SelectedValue == "Courier")
    //                    {

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {
    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                                lblshipingcost.Text = price.ToString();
    //                            }

    //                        }
    //                    }
    //                    else if (drpSM1.SelectedValue == "Mail")
    //                    {
    //                        if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                        {
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());

    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {
    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                        else
    //                        {
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }


    //                        }
    //                    }
    //                    else if (drpSM1 == "Drop Shipment Order")
    //                    {
    //                        divpaymentoption.Style.Add("display", "block");
    //                        GetPaymentTerm(Userid.ToString());
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;
    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                                objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                lblshipingcost.Text = price.ToString();
    //                            }

    //                        }
    //                    }
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }

    //                }
    //                else if ((ZONE == "REMOTE") || (ZONE == "NOTFOUND"))
    //                {
    //                    objErrorHandler.CreateLog("inside Remote zone");
    //                    if ((producttotalweight_all >= Convert.ToDecimal(remoteweight)))
    //                    {
    //                        divpaymentoption.Style.Add("display", "none");

    //                        divInternationalpayoption.Style.Add("display", "none");
    //                        ImageButton2.Visible = true;
    //                        showPaymentType(Userid.ToString());
    //                        RBpaymenttype.Checked = false;
    //                        rbinternationaldefault_remote = true;
    //                        lblshipingcost.Text = "0";
    //                        oOrdInfo.ShipCost = 0;
    //                        oOrdInfo.SHIP_CODE = "";
    //                        hfshipcode.Value = "";
    //                        emailconfimation = "Remote";
    //                        Session["emailconfimation"] = "Remote";
    //                    }
    //                    else
    //                    {
    //                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                        {
    //                            showselectedtab_ondropdownchange();

    //                        }
    //                        else
    //                        {

    //                            GetPaymentTerm(Userid.ToString());
    //                            showselectedtab_ondropdownchange();
    //                        }


    //                        divpaymentoption.Style.Add("display", "block");
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;
    //                        rbinternationaldefault_remote = false;
    //                        if (drpSM1.SelectedValue == "Courier")
    //                        {

    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    // objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                        else if (drpSM1.SelectedValue == "Mail")
    //                        {


    //                            if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                            {
    //                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
    //                                if (Sqltbs != null)
    //                                {
    //                                    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                    {

    //                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                        oOrdInfo.ShipCost = price;

    //                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                        if (dsprodcode != null)
    //                                        {
    //                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        }


    //                                        lblshipingcost.Text = price.ToString();
    //                                    }

    //                                }
    //                            }
    //                            else
    //                            {

    //                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                                if (Sqltbs != null)
    //                                {
    //                                    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                    {

    //                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                        oOrdInfo.ShipCost = price;
    //                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                        if (dsprodcode != null)
    //                                        {
    //                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        }
    //                                        lblshipingcost.Text = price.ToString();
    //                                    }

    //                                }


    //                            }
    //                        }
    //                        else if (drpSM1.SelectedValue == "Drop Shipment Order")
    //                        {
    //                            divpaymentoption.Style.Add("display", "block");
    //                            GetPaymentTerm(Userid.ToString());
    //                            trpm.Visible = false;
    //                            ImageButton2.Visible = false;
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                    }


    //                }

    //            }
    //            else
    //            {
    //                lblweight.Text = "";
    //                lblzonezip.Text = "";
    //                lblzone.Text = "";
    //                if (drpSM1.SelectedValue != "International Shipping - TBA")
    //                {
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }
    //                    else
    //                    {

    //                        GetPaymentTerm(Userid.ToString());
    //                        showselectedtab_ondropdownchange();
    //                    }

    //                    rbinternationaldefault_remote = false;
    //                    divpaymentoption.Style.Add("display", "block");
    //                    ImageButton2.Visible = false;
    //                    trpm.Visible = false;
    //                }

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        objErrorHandler.CreateLog(ex.ToString());
    //    }
    //    Session["shipcost"] = oOrdInfo.ShipCost + "-" + oOrdInfo.SHIP_CODE;
    //    return oOrdInfo.ShipCost + "-" + oOrdInfo.SHIP_CODE;
    //}



    //public static string Calculate_shipping(string drpSM1)
    //{
    //    try
    //    {


    //        if (drpSM1!= "Please Select Shipping Method")
    //        {


    //            if ((drpSM1== "Courier") || (drpSM1== "Mail") || (drpSM1== "Drop Shipment Order"))
    //            {
    //                string ZONE = GetZone(OrderID);
    //                lblzone.Text = ZONE;

    //                decimal producttotalweight_all = 0;

    //                DataSet dsODall = objOrderServices.GetOrderItems(OrderID);

    //                if (dsODall != null && dsODall.Tables[0].Rows.Count > 0)
    //                {
    //                    for (int i = 0; i < dsODall.Tables[0].Rows.Count; i++)
    //                    {

    //                        producttotalweight_all = producttotalweight_all + objUserServices.GET_PRODUCTWEIGHT(dsODall.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsODall.Tables[0].Rows[i]["qty"].ToString()));
    //                        // objErrorHandler.CreateLog("product wight for" + dsODall.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight_all);

    //                    }
    //                }
    //                lblweight.Text = producttotalweight_all.ToString();

    //                if (ispickuponly_product(OrderID) == true)
    //                {
    //                    divpaymentoption.Style.Add("display", "none");

    //                    divInternationalpayoption.Style.Add("display", "none");
    //                    ImageButton2.Visible = true;
    //                    showPaymentType(Userid.ToString());
    //                    // rb.Checked = true;
    //                    RBpaymenttype.Checked = false;
    //                    rbinternationaldefault_remote = true;
    //                    emailconfimation = "Remote";
    //                    lblshipingcost.Text = "0";
    //                    oOrdInfo.ShipCost = 0;
    //                    oOrdInfo.SHIP_CODE = "";
    //                    hfshipcode.Value = "";
    //                }
    //                //Local Start
    //                else if (ZONE == "LOCAL")
    //                {
    //                    visibilepayment();

    //                    if (drpSM1== "Courier")
    //                    {

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                //  objErrorHandler.CreateLog("delivery cost 4 localzone " + price.ToString());
    //                                lblshipingcost.Text = price.ToString();
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }

    //                            }

    //                        }
    //                        //decimal producttotalweight = 0;

    //                        //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                        //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                        //{
    //                        //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                        //    {

    //                        //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
    //                        //        objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

    //                        //    }
    //                        //}
    //                        //lblweight.Text = producttotalweight.ToString();
    //                        //lblweight.Visible = true;
    //                        //objErrorHandler.CreateLog("product total weight" + lblweight.Text);
    //                        //if ((producttotalweight < Convert.ToDecimal(1000)))
    //                        //{
    //                        //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74555", Userid.ToString());
    //                        //    if (Sqltbs != null)
    //                        //    {
    //                        //        if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        //        {

    //                        //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                        //            oOrdInfo.ShipCost = price;
    //                        //            lblshipingcost.Text = price.ToString();
    //                        //            objErrorHandler.CreateLog("delivery cost 4 LOCAL" + price.ToString());
    //                        //        }

    //                        //    }

    //                        //}
    //                        //else
    //                        //{
    //                        //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                        //    if (Sqltbs != null)
    //                        //    {
    //                        //        if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        //        {

    //                        //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                        //            oOrdInfo.ShipCost = price;
    //                        //            objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                        //            lblshipingcost.Text = price.ToString();
    //                        //        }

    //                        //    }

    //                        //}
    //                    }

    //                    //Local courier ends here
    //                    else if (drpSM1== "Mail")
    //                    {

    //                        //decimal producttotalweight = 0;



    //                        //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                        //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                        //{
    //                        //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                        //    {

    //                        //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
    //                        //        objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

    //                        //    }
    //                        //}
    //                        //lblweight.Text = producttotalweight.ToString();
    //                        //Console.WriteLine("Total Weight"+producttotalweight);
    //                        //lblweight.Visible = true;
    //                        //objErrorHandler.CreateLog("product total weight" + producttotalweight);
    //                        if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                        {
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }

    //                                    lblshipingcost.Text = price.ToString();
    //                                    //objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
    //                                }

    //                            }
    //                        }
    //                        else
    //                        {

    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74555", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74555  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }

    //                                    //objErrorHandler.CreateLog("delivery cost 4 localzone" + price.ToString());
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }


    //                        }




    //                    }
    //                    else if (drpSM1== "Drop Shipment Order")
    //                    {
    //                        divpaymentoption.Style.Add("display", "block");
    //                        GetPaymentTerm(Userid.ToString());
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                lblshipingcost.Text = price.ToString();
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                                // objErrorHandler.CreateLog("delivery cost 7 localzone" + price.ToString());

    //                            }

    //                        }
    //                    }
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }

    //                }
    //                //////////////////// Zone/////////////////
    //                else if (ZONE == "" || ZONE == null)
    //                {

    //                    visibilepayment();
    //                    //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                    //if (Sqltbs != null)
    //                    //{
    //                    //    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                    //    {

    //                    //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                    //        oOrdInfo.ShipCost = price;
    //                    //        lblshipingcost.Text = price.ToString();
    //                    //        objErrorHandler.CreateLog("delivery cost 7 LOCAL" + price.ToString());
    //                    //    }

    //                    //}

    //                    //  objErrorHandler.CreateLog("inside zone null or empty"); 
    //                    if (drpSM1== "Courier")
    //                    {

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                                //objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                lblshipingcost.Text = price.ToString();
    //                            }

    //                        }
    //                    }
    //                    else if (drpSM1== "Mail")
    //                    {

    //                        //decimal producttotalweight_mail = 0;



    //                        //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                        //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                        //{
    //                        //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                        //    {
    //                        //        try
    //                        //        {
    //                        //            producttotalweight_mail = producttotalweight_mail + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
    //                        //            objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight_mail);
    //                        //        }
    //                        //        catch (Exception ex)
    //                        //        {
    //                        //            objErrorHandler.CreateLog(ex.ToString());
    //                        //        }
    //                        //    }
    //                        //}
    //                        //lblweight.Text = producttotalweight_mail.ToString();
    //                        //lblweight.Visible = true;
    //                        //Console.WriteLine(producttotalweight_mail);
    //                        //objErrorHandler.CreateLog("product total weight" + producttotalweight_mail.ToString());
    //                        if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                        {
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());

    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                    // objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
    //                                }

    //                            }
    //                        }
    //                        else
    //                        {
    //                            //  objErrorHandler.CreateLog("inside else mail" + "GetProductPriceEA" + "96051-74556");
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            //  objErrorHandler.CreateLog("after sql" + Sqltbs.Tables[0].Rows.Count);
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    //   objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }


    //                        }
    //                    }
    //                    else if (drpSM1== "Drop Shipment Order")
    //                    {
    //                        divpaymentoption.Style.Add("display", "block");
    //                        GetPaymentTerm(Userid.ToString());
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;
    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                                DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                if (dsprodcode != null)
    //                                {
    //                                    oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                }
    //                                objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                lblshipingcost.Text = price.ToString();
    //                            }

    //                        }
    //                    }
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }

    //                }

    //                    ///////Remote///
    //                else if ((ZONE == "REMOTE") || (ZONE == "NOTFOUND"))
    //                {
    //                    objErrorHandler.CreateLog("inside Remote zone");
    //                    //decimal producttotalweight = 0;



    //                    //DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                    //if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                    //{
    //                    //    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                    //    {
    //                    //        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
    //                    //        objErrorHandler.CreateLog("product weight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

    //                    //    }
    //                    //}
    //                    //lblweight.Text = producttotalweight.ToString();
    //                    //lblweight.Visible = true;
    //                    //objErrorHandler.CreateLog("product total weight" + lblweight.Text);
    //                    //|| ((producttotalweight == 0) && (ZONE == "NOTFOUND") )
    //                    if ((producttotalweight_all >= Convert.ToDecimal(remoteweight)))
    //                    {
    //                        divpaymentoption.Style.Add("display", "none");

    //                        divInternationalpayoption.Style.Add("display", "none");
    //                        ImageButton2.Visible = true;
    //                        showPaymentType(Userid.ToString());
    //                        RBpaymenttype.Checked = false;
    //                        rbinternationaldefault_remote = true;
    //                        lblshipingcost.Text = "0";
    //                        oOrdInfo.ShipCost = 0;
    //                        oOrdInfo.SHIP_CODE = "";
    //                        hfshipcode.Value = "";
    //                        emailconfimation = "Remote";
    //                    }
    //                    else
    //                    {
    //                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                        {
    //                            showselectedtab_ondropdownchange();

    //                        }
    //                        else
    //                        {

    //                            GetPaymentTerm(Userid.ToString());
    //                            showselectedtab_ondropdownchange();
    //                        }


    //                        divpaymentoption.Style.Add("display", "block");
    //                        trpm.Visible = false;
    //                        ImageButton2.Visible = false;
    //                        rbinternationaldefault_remote = false;
    //                        //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                        //if (Sqltbs != null)
    //                        //{
    //                        //    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        //    {

    //                        //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                        //        oOrdInfo.ShipCost = price;
    //                        //        objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                        //        lblshipingcost.Text = price.ToString();
    //                        //    }

    //                        //}
    //                        if (drpSM1== "Courier")
    //                        {

    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    // objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                        else if (drpSM1== "Mail")
    //                        {


    //                            if ((producttotalweight_all < Convert.ToDecimal(Lweight)))
    //                            {
    //                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74554", Userid.ToString());
    //                                if (Sqltbs != null)
    //                                {
    //                                    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                    {

    //                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                        oOrdInfo.ShipCost = price;

    //                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74554  and attribute_id=1");
    //                                        if (dsprodcode != null)
    //                                        {
    //                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        }


    //                                        lblshipingcost.Text = price.ToString();
    //                                        // objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
    //                                    }

    //                                }
    //                            }
    //                            else
    //                            {

    //                                DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                                if (Sqltbs != null)
    //                                {
    //                                    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                    {

    //                                        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                        oOrdInfo.ShipCost = price;
    //                                        DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                        if (dsprodcode != null)
    //                                        {
    //                                            oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                            hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        }
    //                                        //    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                        lblshipingcost.Text = price.ToString();
    //                                    }

    //                                }


    //                            }
    //                        }
    //                        else if (drpSM1== "Drop Shipment Order")
    //                        {
    //                            divpaymentoption.Style.Add("display", "block");
    //                            GetPaymentTerm(Userid.ToString());
    //                            trpm.Visible = false;
    //                            ImageButton2.Visible = false;
    //                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("960751", "74556", Userid.ToString());
    //                            if (Sqltbs != null)
    //                            {
    //                                if (Sqltbs.Tables[0].Rows.Count > 0)
    //                                {

    //                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                    oOrdInfo.ShipCost = price;
    //                                    DataSet dsprodcode = objHelperDB.GetDataSetDB("SELECT String_value FROM TB_prod_specs WHERE product_ID = 74556  and attribute_id=1");
    //                                    if (dsprodcode != null)
    //                                    {
    //                                        oOrdInfo.SHIP_CODE = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                        hfshipcode.Value = dsprodcode.Tables[0].Rows[0][0].ToString();
    //                                    }
    //                                    // objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //                                    lblshipingcost.Text = price.ToString();
    //                                }

    //                            }
    //                        }
    //                    }


    //                }

    //                ////////Remote zone ending
    //                //ZONE == "" ||
    //                //else if (ZONE == "NOTFOUND")
    //                //{
    //                //    divpaymentoption.Style.Add("display", "none");

    //                //    divInternationalpayoption.Style.Add("display", "none");
    //                //    ImageButton2.Visible = true;
    //                //    RBpaymenttype.Checked = false;
    //                //    rbinternationaldefault_remote = true;
    //                //    emailconfimation = "Remote";
    //                //}
    //            }
    //            //else if (drpSM1== "Drop Shipment Order")
    //            //{
    //            //    divpaymentoption.Style.Add("display", "block");
    //            //    ImageButton2.Visible = false;
    //            //    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //            //    if (Sqltbs != null)
    //            //    {
    //            //        if (Sqltbs.Tables[0].Rows.Count > 0)
    //            //        {

    //            //            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //            //            oOrdInfo.ShipCost = price;
    //            //            objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
    //            //            lblshipingcost.Text = price.ToString();
    //            //        }

    //            //    }
    //            //}
    //            else
    //            {
    //                lblweight.Text = "";
    //                lblzonezip.Text = "";
    //                lblzone.Text = "";
    //                if (drpSM1!= "International Shipping - TBA")
    //                {
    //                    if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
    //                    {
    //                        showselectedtab_ondropdownchange();

    //                    }
    //                    else
    //                    {

    //                        GetPaymentTerm(Userid.ToString());
    //                        showselectedtab_ondropdownchange();
    //                    }

    //                    rbinternationaldefault_remote = false;
    //                    divpaymentoption.Style.Add("display", "block");
    //                    ImageButton2.Visible = false;
    //                    trpm.Visible = false;
    //                }

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        objErrorHandler.CreateLog(ex.ToString());
    //    }
    //    return oOrdInfo.ShipCost + "-" + oOrdInfo.SHIP_CODE;
    //}

//    private decimal Calculate_shipping()
//    {
//        try
//        {
//            if (drpSM1.SelectedValue != "Please Select Shipping Method")
//            {

//                string ZONE = GetZone(OrderID);
//                objErrorHandler.CreateLog(ZONE + drpSM1.SelectedValue);
//                if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail"))
//                {

//                    if (ispickuponly_product(OrderID) == true)
//                    {
//                        divpaymentoption.Style.Add("display", "none");

//                        divInternationalpayoption.Style.Add("display", "none");
//                       ()); ImageButton2.Visible = true;
//                       // rb.Checked = true;
//                        RBpaymenttype.Checked = false;
//                        rbinternationaldefault_remote = true;
//                        emailconfimation = "Remote";
//                    }
////Local Start
//                    else if (ZONE == "LOCAL" )
//                    {
                      
//                        if (drpSM1.SelectedValue == "Courier")
//                        {
//                            decimal producttotalweight = 0;

//                             DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

//                        if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
//                        {
//                            for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
//                            {
                               
//                                producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
//                                objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

//                            }
//                        }
//                        lblweight.Text = producttotalweight.ToString();
//                        lblweight.Visible = true;
//                        objErrorHandler.CreateLog("product total weight" + lblweight.Text);
//                        if ((producttotalweight < Convert.ToDecimal(1000)))
//                        {
//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74555", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    lblshipingcost.Text = price.ToString();
//                                    objErrorHandler.CreateLog("delivery cost 4 LOCAL" + price.ToString());
//                                }

//                            }

//                        }
//                        else
//                        {
//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
//                                    lblshipingcost.Text = price.ToString();
//                                }

//                            }
                        
//                        }
//                        }
//                            //Local courier ends here
//                        else if (drpSM1.SelectedValue == "Mail")
//                        { 
                        
//                             decimal producttotalweight = 0;



//                        DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

//                        if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
//                        {
//                            for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
//                            {
                               
//                                producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
//                                objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

//                            }
//                        }
//                        lblweight.Text = producttotalweight.ToString();
//                        lblweight.Visible = true;
//                        objErrorHandler.CreateLog("product total weight" + lblweight.Text);
//                        if ((producttotalweight < Convert.ToDecimal(0.3)))
//                        {
//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74554", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    lblshipingcost.Text = price.ToString();
//                                    objErrorHandler.CreateLog("delivery cost 2 LOCAL" + price.ToString());
//                                }

//                            }
//                        }
//                        else
//                        {

//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
//                                    lblshipingcost.Text = price.ToString();
//                                }

//                            }


//                        }



                        
//                        }

//                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
//                        {
//                            showselectedtab_ondropdownchange();

//                        }
//                    }
//                    //////////////////// Zone/////////////////
//                    else if (ZONE == "" || ZONE == null)
//                    {
                       

//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    lblshipingcost.Text = price.ToString();
//                                    objErrorHandler.CreateLog("delivery cost 7 LOCAL" + price.ToString());
//                                }

//                            }
                       
                      

                     

//                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
//                        {
//                            showselectedtab_ondropdownchange();

//                        }
//                    }

//                        ///////Remote///
//                    else if (ZONE == "REMOTE")
//                    {
//                        objErrorHandler.CreateLog("inside Remote zone");
//                        decimal producttotalweight = 0;



//                        DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

//                        if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
//                        {
//                            for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
//                            {
//                                producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[i]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[i]["qty"].ToString()));
//                                objErrorHandler.CreateLog("product weight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);

//                            }
//                        }
//                        lblweight.Text = producttotalweight.ToString();
//                        lblweight.Visible = true;
//                        objErrorHandler.CreateLog("product total weight" + lblweight.Text);
//                        if ((producttotalweight >= Convert.ToDecimal(2.7)) || (producttotalweight == 0))
//                        {
//                            divpaymentoption.Style.Add("display", "none");

//                            divInternationalpayoption.Style.Add("display", "none");
//                            ImageButton2.Visible = true;
//                            RBpaymenttype.Checked = false;
//                            rbinternationaldefault_remote = true;

//                            emailconfimation = "Remote";
//                        }
//                        else
//                        {
//                            if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
//                            {
//                                showselectedtab_ondropdownchange();

//                            }
//                            else
//                            {

//                                GetPaymentTerm(Userid.ToString());
//                                showselectedtab_ondropdownchange();
//                            }
//                            divpaymentoption.Style.Add("display", "block");
//                            ImageButton2.Visible = false;
//                            rbinternationaldefault_remote = false;
//                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
//                            if (Sqltbs != null)
//                            {
//                                if (Sqltbs.Tables[0].Rows.Count > 0)
//                                {

//                                    decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
//                                    oOrdInfo.ShipCost = price;
//                                    objErrorHandler.CreateLog("delivery cost 7 remotezone" + price.ToString());
//                                    lblshipingcost.Text = price.ToString();
//                                }

//                            }


//                        }


//                    }

//                    ////////Remote zone ending
//                    //ZONE == "" ||
//                    else if (ZONE == "NOTFOUND")
//                    {
//                        divpaymentoption.Style.Add("display", "none");

//                        divInternationalpayoption.Style.Add("display", "none");
//                        ImageButton2.Visible = true;
//                        RBpaymenttype.Checked = false;
//                        rbinternationaldefault_remote = true;
//                        emailconfimation = "Remote";
//                    }
//                }
//                else
//                {
//                    if (drpSM1.SelectedValue != "International Shipping - TBA")
//                    {
//                        if (RBpaymenttype.Checked == true || RBCreditCard.Checked == true || RBPaypal.Checked == true)
//                        {
//                            showselectedtab_ondropdownchange();

//                        }
//                        else
//                        {

//                            GetPaymentTerm(Userid.ToString());
//                            showselectedtab_ondropdownchange();
//                        }
                  
//                     rbinternationaldefault_remote = false;
//                        divpaymentoption.Style.Add("display", "block");
//                        ImageButton2.Visible = false;
                       
//                    } 

//                }
//            }
//        }
//        catch (Exception ex)
//        {

//            objErrorHandler.CreateLog(ex.ToString());
//        }
//        return oOrdInfo.ShipCost;
//    }







    //private decimal calcShipcost()
    //{
    //    try
    //    {
    //        if (drpSM1.SelectedValue != "Please Select Shipping Method")
    //        {
    //            if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail"))
    //            {




    //                string ZONE = GetZone(OrderID);
    //               // objErrorHandler.CreateLog("Zonr is" + ZONE);
    //                if (ZONE == "LOCAL")
    //                {
    //                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74555", Userid.ToString());
    //                    if (Sqltbs != null)
    //                    {
    //                        if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        {

    //                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                            oOrdInfo.ShipCost = price;
    //                           // objErrorHandler.CreateLog("price for local" + ZONE);
    //                        }

    //                    }

    //                }

    //                else if (ZONE == "" || ZONE == null || ispickuponly_product(OrderID)==true)
                    
    //                {
    //                divpaymentoption.Style.Add("display", "none");
                           
    //                            divInternationalpayoption.Style.Add("display", "none");
    //                            ImageButton2.Visible = true;
    //                            rbinternationaldefault.Checked = true;
                        
                    
    //                }
                    
    //                else if (ZONE.ToUpper() =="REMOTE" ) 
    //                {
    //                    //DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                    //if (Sqltbs != null)
    //                    //{
    //                    //    if (Sqltbs.Tables[0].Rows.Count > 0)
    //                    //    {

    //                    //        decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                    //        oOrdInfo.ShipCost = price;
    //                    //       // objErrorHandler.CreateLog("delivery cost 7" + price.ToString());
    //                    //    }

    //                    //}

    //                    decimal producttotalweight = 0;



    //                    DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                    if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                    {
    //                        for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                        {
    //                            producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[0]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[0]["qty"].ToString()));
    //                            // objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);
    //                        }
    //                    }

    //                    if (producttotalweight > 2700) 
    //                    {
    //                        divpaymentoption.Style.Add("display", "none");
                          
    //                            divInternationalpayoption.Style.Add("display", "none");
    //                            ImageButton2.Visible = true;
    //                            rbinternationaldefault.Checked = true;
                            
    //                    }
    //                    else
    //                    {

    //                        DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                        if (Sqltbs != null)
    //                        {
    //                            if (Sqltbs.Tables[0].Rows.Count > 0)
    //                            {

    //                                decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                                oOrdInfo.ShipCost = price;
    //                               objErrorHandler.CreateLog("delivery cost 7" + price.ToString());
    //                            }

    //                        }
                        
                        
    //                    }
    //                }
    //            }
    //            else if ((drpSM1.SelectedValue == "Mail") && (ispickuponly_zone(OrderID) == false))
    //            {


    //                decimal producttotalweight = 0;



    //                DataSet dsOD = objOrderServices.GetOrderItems(OrderID);

    //                if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
    //                {
    //                    for (int i = 0; i < dsOD.Tables[0].Rows.Count; i++)
    //                    {
    //                        producttotalweight = producttotalweight + objUserServices.GET_PRODUCTWEIGHT(dsOD.Tables[0].Rows[0]["product_id"].ToString(), Convert.ToDecimal(dsOD.Tables[0].Rows[0]["qty"].ToString()));
    //                       // objErrorHandler.CreateLog("product wight for" + dsOD.Tables[0].Rows[0]["product_id"].ToString() + producttotalweight);
    //                    }
    //                }
    //                lblweight.Text = producttotalweight.ToString();
    //               // objErrorHandler.CreateLog("total product weight" + producttotalweight.ToString());
    //                if (producttotalweight < 300)
    //                {

    //                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74554", Userid.ToString());
    //                    if (Sqltbs != null)
    //                    {
    //                        if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        {

    //                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                            oOrdInfo.ShipCost = price;
    //                          objErrorHandler.CreateLog("delivery cost 2" + price.ToString());
    //                        }

    //                    }


    //                }
    //                else
    //                {

    //                    DataSet Sqltbs = objHelperDB.GetProductPriceEA("", "74556", Userid.ToString());
    //                    if (Sqltbs != null)
    //                    {
    //                        if (Sqltbs.Tables[0].Rows.Count > 0)
    //                        {

    //                            decimal price = Convert.ToDecimal(Sqltbs.Tables[0].Rows[0]["price"].ToString());
    //                            oOrdInfo.ShipCost = price;
    //                            objErrorHandler.CreateLog("delivery cost 7" + price.ToString());
    //                        }

    //                    }

    //                }
    //            }
    //            lblshipingcost.Text = objHelperServices.FixDecPlace(oOrdInfo.ShipCost).ToString();

    //            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();


    //            oOrderInfo = objOrderServices.GetOrder(OrderID);
    //            decimal taxamt = objOrderServices.CalculateTaxAmount(oOrderInfo.ProdTotalPrice + oOrdInfo.ShipCost, OrderID.ToString());
    //            lbltotalamt.Text = objHelperServices.FixDecPlace(oOrdInfo.ShipCost + taxamt + oOrderInfo.ProdTotalPrice).ToString();
    //            lbltotalamount.Text = objHelperServices.FixDecPlace(oOrdInfo.ShipCost + taxamt + oOrderInfo.ProdTotalPrice).ToString();
    //            divtotalamt.Visible = true;

    //        }
    //        else
    //        {

    //            divtotalamt.Visible = false;
    //        }
    //    }
        
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.CreateLog(ex.ToString());
        
    //    }
    //    return oOrdInfo.ShipCost;
    //}

    private void showselectedtab_ondropdownchange()
    {

       
         if (RBCreditCard.Checked == true)
        {
            divdedault.Style.Add("display", "none");
            divcreditcard.Style.Add("display", "block");
            divpaypal.Style.Add("display", "none");
        }
         else   if (RBpaymenttype.Checked == true)
        {
           // GetPaymentTerm(Userid.ToString() );
            divdedault.Style.Add("display", "block");
            divcreditcard.Style.Add("display", "none");
            divpaypal.Style.Add("display", "none");
        }
        else if (RBPaypal.Checked == true)
        {
            divdedault.Style.Add("display", "none");
            divcreditcard.Style.Add("display", "none");
            divpaypal.Style.Add("display", "block");
        }
    }




   [System.Web.Services.WebMethod]

    public static List<string> GetData(string DName)

    {
        ErrorHandler objErrorHandler = new ErrorHandler();
     
        List<string> result = new List<string>();
        try
        {
            if (DName != "")
            {
                ConnectionDB objConnection = new ConnectionDB();

                using (SqlCommand cmd = new SqlCommand("select distinct (Suburb +' , '+state+' '+ PostCode) as suburb_f, suburb,state,postcode from wes_postcode_au where suburb like ''+@SearchText+'%'  ", objConnection.GetConnection()))
                {



                    cmd.Parameters.AddWithValue("@SearchText", DName);

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                      //  result.Add(dr["suburb"].ToString());
                        result.Add(string.Format("{0}/{1}/{2}/{3}", dr["suburb_f"], dr["suburb"], dr["state"], dr["postcode"]));
                      
                    }

                  

                }
            }
            return result;
        }
        catch (Exception Ex)
        {
            objErrorHandler.CreateLog(Ex.ToString());
            result.Add(Ex.ToString());
            return result;
        }

   

}


   [System.Web.Services.WebMethod]

   public static string Loadpostcode(string DName)
   {
       ErrorHandler objErrorHandler = new ErrorHandler();
      
       string result = "";
       try
       {
           if (DName != "")
           {
               //ConnectionDB objConnection = new ConnectionDB();

               ////string[] suburb = DName.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries);

               //using (SqlCommand cmd = new SqlCommand("select suburb,postcode,state from wes_postcode_au where  (Suburb +' , '+state+' '+ PostCode) = '+@SearchText+'", objConnection.GetConnection()))
               //{
                   

               //    DName = HttpUtility.UrlDecode(DName);
               //    objErrorHandler.CreateLog(DName); 
               //    cmd.Parameters.AddWithValue("@SearchText", DName));

               //    SqlDataReader dr = cmd.ExecuteReader();

               //    while (dr.Read())
               //    {
               //        objErrorHandler.CreateLog(dr["postcode"].ToString());
               //        HttpContext.Current.Session[dr["suburb"].ToString()] = dr["postcode"].ToString() + "-" + dr["state"].ToString() + "-" + dr["suburb"].ToString();
                      
               //        return dr["postcode"].ToString() + "-" + dr["state"].ToString() + "-" + dr["suburb"].ToString();


               //    }
                 
               //}
               DName = HttpUtility.UrlDecode(DName);
               try
               {
                   string[] suburb = DName.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries);

                   string[] suburb1 = suburb[1].Split(' ');
                   HttpContext.Current.Session[suburb[0]] = suburb1[1] + "-" + suburb1[0] + "-" + suburb[0];
                 //  objErrorHandler.CreateLog(suburb1[1] + "-" + suburb1[0] + "-" + suburb[0]);
                   result = suburb1[1] + "-" + suburb1[0] + "-" + suburb[0];
                    HttpContext.Current.Session["suburb"] = result;
                   return suburb1[1] + "-" + suburb1[0] + "-" + suburb[0];
               }
               catch(Exception ex)
               {

                   ConnectionDB objConnection = new ConnectionDB();

                   //string[] suburb = DName.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries);

                   using (SqlCommand cmd = new SqlCommand("select suburb,postcode,state from wes_postcode_au where  (Suburb +' , '+state+' '+ PostCode) = '+@SearchText+'", objConnection.GetConnection()))
                   {


                       DName = HttpUtility.UrlDecode(DName);
                    //   objErrorHandler.CreateLog(DName); 
                       cmd.Parameters.AddWithValue("@SearchText", DName);

                       SqlDataReader dr = cmd.ExecuteReader();

                       while (dr.Read())
                       {
                         //  objErrorHandler.CreateLog(dr["postcode"].ToString());
                           HttpContext.Current.Session[dr["suburb"].ToString()] = dr["postcode"].ToString() + "-" + dr["state"].ToString() + "-" + dr["suburb"].ToString();

                           return dr["postcode"].ToString() + "-" + dr["state"].ToString() + "-" + dr["suburb"].ToString();


                       }

                   }
               }
           }
           return result;
       }
       catch (Exception Ex)
       {
           objErrorHandler.CreateLog(Ex.ToString());
          
           return result;
       }



   }

    [WebMethod]
  public static string MobileNoChange_Click(string isuserchecked,string mobileno)
   {
       OrderServices objOrderServices = new OrderServices();
       int _UserrID;
       HelperServices objHelperServices = new HelperServices();
       _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
       string OrderID = HttpContext.Current.Session["ORDER_ID"].ToString();
       HttpContext.Current.Session["Nothanks"] = "false"; 
       if (isuserchecked == "true")
       {
       
          
           decimal Updpr = objOrderServices.Update_MOBILE_NUMBER(mobileno, _UserrID.ToString(), OrderID.ToString(), true);
           if (Updpr > 0)
           {

               HttpContext.Current.Session["Mobileno"] = mobileno;
               return mobileno;
           }

       }
       else
       {

         
           int UP = objOrderServices.Update_SHIP_NUMBER(mobileno,OrderID);
           if (UP > 0)
           {
               HttpContext.Current.Session["Mobileno"] = mobileno;
               return mobileno;
           }
       }
       return "";
   }

    [WebMethod]
    public static string NoChange_Click(string isuserchecked, string mobileno)
    {
        OrderServices objOrderServices = new OrderServices();
        int _UserrID;
        HelperServices objHelperServices = new HelperServices();
        _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
        string OrderID = HttpContext.Current.Session["ORDER_ID"].ToString();
        HttpContext.Current.Session["Nothanks"] = "true"; 
        //if (isuserchecked == "true")
        //{


        //    decimal Updpr = objOrderServices.Update_MOBILE_NUMBER(mobileno, _UserrID.ToString(), OrderID.ToString(), true);
        //    if (Updpr > 0)
        //    {
        //        return mobileno;
        //    }

        //}
        //else
        //{


            int UP = objOrderServices.Update_SHIP_NUMBER(mobileno, OrderID);
            if (UP > 0)
            {

                return mobileno;
            }
        //}
        return "";
    }
    //[WebMethod]
    //public static string securepayclick()
    //{
    //    try
    //    {

    //        if (issecurepayclicked == false)
    //        {
    //            string rtnstr = "";

    //            if (Page.IsValid == true)
    //            {
    //                btnSP.Visible = false;
    //                BtnProgressSP.Visible = true;
    //                issecurepayclicked = true;
    //                //try
    //                //{
    //                //    txtCardNumber.Style.Remove("border");
    //                //    drpExpmonth.Style.Remove("border");
    //                //    drpExpyear.Style.Remove("border");
    //                //    txtCVV.Style.Remove("border");
    //                //    //drppaymentmethod.Style.Remove("border");
    //                //}
    //                //catch
    //                //{
    //                //}

    //                SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
    //                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();


    //                oOrderInfo = objOrderServices.GetOrder(OrderID);
    //                if (oOrderInfo.ProdTotalPrice == 0)
    //                {
    //                    objErrorHandler.CreateLog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
    //                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
    //                }

    //                DataSet tmpds = GetOrderItemDetailSum(OrderID);
    //                decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
    //                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
    //                {
    //                    totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
    //                }

    //                if (totalitemsum.ToString() == "0.00")
    //                {
    //                    objErrorHandler.CreateLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
    //                    Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
    //                }

    //                UserServices objUserServices = new UserServices();
    //                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
    //                int i = objUserServices.GetCheckOutOption(Userid);
    //                if ((i == 1) && (tt1.Text == ""))
    //                {
    //                    txterr.Text = "Please Enter Order No";
    //                    lblMessage.Text = "Please Enter Order No";
    //                    hftt1.Value = "1";
    //                    mpeMessageBox.Show();
    //                    OrderID = -1;
    //                    Session["OrderId"] = "-1";
    //                    divcreditcard.Style.Add("display", "block");
    //                    divdedault.Style.Add("display", "none");
    //                    divpaypal.Style.Add("display", "none");
    //                    btnSP.Visible = true;
    //                    BtnProgressSP.Visible = false;
    //                    issecurepayclicked = false;
    //                    return;
    //                }
    //                else
    //                {
    //                    hftt1.Value = "0";
    //                }
    //                try
    //                {

    //                    int currentyear = DateTime.Now.Year;
    //                    int currentmonth = DateTime.Now.Month;
    //                    bool isexpvalid = false;
    //                    if (Convert.ToInt32(drpExpyear.SelectedValue) > currentyear)
    //                    {
    //                        isexpvalid = true;
    //                    }
    //                    else if (Convert.ToInt32(drpExpyear.SelectedValue) == currentyear)
    //                    {
    //                        if (Convert.ToInt32(drpExpmonth.SelectedValue) >= currentmonth)
    //                        {

    //                            isexpvalid = true;
    //                        }
    //                    }


    //                    if (isexpvalid == true)
    //                    {
    //                        oPayInfo = objPaymentServices.GetPayment(OrderID);

    //                        //  decimal    TaxAmount = objOrderServices.CalculateTaxAmount(oOrderInfo.ProdTotalPrice + shipcost, OrderID.ToString());
    //                        decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
    //                        objErrorHandler.CreatePayLog(OrderID.ToString() + "paymnet amount:" + oPayInfo.Amount + "ProdTotalPrice:" + oOrderInfo.ProdTotalPrice + "ShipCost" + oOrderInfo.ShipCost + "taxamount" + TaxAmount);
    //                        if (oPayInfo.PaymentID == 0 || oPayInfo.Amount != (oOrderInfo.ProdTotalPrice + oOrderInfo.ShipCost + TaxAmount))
    //                        {


    //                            objErrorHandler.CreatePayLog("inside before opayinfo");
    //                            order_submit_process();
    //                            objErrorHandler.CreatePayLog("inside after opayinfo");
    //                            oPayInfo = objPaymentServices.GetPayment(OrderID);
    //                            PaymentID = oPayInfo.PaymentID;
    //                        }
    //                        else
    //                        {
    //                            PaymentID = oPayInfo.PaymentID;
    //                        }
    //                        objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", txtnamecard.Text, txtcreditcardno.Text, txtCVV.Text, drpExpmonth.SelectedItem.Text + "/" + drpExpyear.SelectedItem.Text);
    //                        objErrorHandler.CreatePayLog(objPRInfo.Error_Text);
    //                        //   objPRInfo.Error_Text = "";
    //                        if (objPRInfo.Error_Text != "")
    //                        {
    //                            btnSP.Style.Add("display", "block");
    //                            BtnProgressSP.Style.Add("display", "none");
    //                            // ImgBtnEditShipping.Style.Add("display", "block");

    //                            div2.InnerHtml = "Error found in details you have entered. Please check all fields for errors and try again."; //objPRInfo.Error_Text;
    //                            isordersubmited.Value = "true";
    //                            divpaymentoption.Style.Add("display", "block");
    //                            trpm.Visible = false;
    //                            div2.Visible = true;
    //                            div2.Style.Add("display", "block");
    //                            //  div2.Focus();
    //                            Scroll_To_Control("ctl00_maincontent_div2");
    //                            divcreditcard.Style.Add("display", "block");
    //                            divdedault.Style.Add("display", "none");
    //                            divpaypal.Style.Add("display", "none");
    //                            //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:creditcardclick();", true);
    //                            greenalert.Visible = false;
    //                            //   Page.RegisterStartupScript("page", "<script language="'javascript'">creditcardclick()</script>");
    //                            ImageButton2.Visible = false;
    //                            trpm.Visible = false;


    //                            HttpContext.Current.Session["payflowresponse"] = "FAIL";

    //                            if (objPRInfo.Error_Text.ToLower().Contains("card number") == true)
    //                            {
    //                                txtcreditcardno.Style.Add("border", "1px solid #FF0000");
    //                            }
    //                            else
    //                            {
    //                                txtcreditcardno.Style.Remove("border");
    //                            }

    //                            if (objPRInfo.Error_Text.ToLower().Contains("cvv") == true || objPRInfo.Error_Text.ToLower().Contains("do not honour") == true)
    //                            {
    //                                txtCVV.Style.Add("border", "1px solid #FF0000");
    //                            }
    //                            else
    //                            {

    //                                txtCVV.Style.Remove("border");
    //                            }

    //                            if (objPRInfo.Error_Text.ToLower().Contains("date") == true || objPRInfo.Error_Text.ToLower().Contains("expired") == true)
    //                            {
    //                                drpExpmonth.Style.Add("border", "1px solid #FF0000");
    //                                drpExpyear.Style.Add("border", "1px solid #FF0000");
    //                            }
    //                            else
    //                            {

    //                                drpExpmonth.Style.Remove("border");

    //                            }
    //                            if (objPRInfo.Error_Text.ToLower().Contains("card type") == true)
    //                            {
    //                                //drppaymentmethod.Style.Add("border", "1px solid #FF0000");                       
    //                            }
    //                            HttpContext.Current.Session["paySPresponse"] = "";
    //                            btnSP.Visible = true;
    //                            BtnProgressSP.Visible = false;
    //                            issecurepayclicked = false;
    //                        }
    //                        else
    //                        {

    //                            //Session["Pay"] = "End";
    //                            Session["XpayMS"] = null;
    //                            // div1.InnerHtml = "";
    //                            //div2.InnerHtml = "XXXXXXXXXXXXXXXXXXXXXXX " + OrderID.ToString() + " Payment succeeded" + strBackLink;
    //                            //div2.InnerHtml = "";
    //                            // div2.Visible = false;
    //                            HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
    //                            HttpContext.Current.Session["Mchkout"] = EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
    //                            HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
    //                            HttpContext.Current.Session["payflowresponse"] = "SUCCESS";
    //                            int x = txtcreditcardno.Text.Length;
    //                            //     objErrorHandler.CreatePayLog("txtcreditcardno"+"startinglength:"+ (x-4).ToString() +"endinglength:"+(x-1).ToString());
    //                            Response.Redirect("BillInfoSP.aspx?Paytype=direct&key=" + EncryptSP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid") + "&cn=" + txtcreditcardno.Text.Substring(x - 4, 4));

    //                            //Response.Redirect("BillInfoSP.aspx?key=" + OrderID.ToString() + "#####" + "&PaySP" + "#####" + "Paid");
    //                        }









    //                    }
    //                    else
    //                    {
    //                        RBexpirydate.Visible = true;
    //                        RBexpirydate.Text = "Please Select valid expiry date";

    //                        div2.InnerHtml = "Error found in details you have entered. Please Select valid expiry date."; //objPRInfo.Error_Text;
    //                        div2.Visible = true;
    //                        //


    //                        div2.Style.Add("display", "block");
    //                        Scroll_To_Control("ctl00_maincontent_div2");
    //                        // div2.Focus();
    //                        divcreditcard.Style.Add("display", "block");
    //                        divdedault.Style.Add("display", "none");
    //                        divpaypal.Style.Add("display", "none");
    //                        btnSP.Visible = true;
    //                        BtnProgressSP.Visible = false;
    //                        issecurepayclicked = false;

    //                    }




    //                }
    //                catch (Exception ex)
    //                {
    //                    objErrorHandler.CreateLog(ex.ToString());
    //                }
    //            }
    //            else
    //            {
    //                divcreditcard.Style.Add("display", "block");
    //                divdedault.Style.Add("display", "none");
    //                divpaypal.Style.Add("display", "none");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.CreateLog(ex.ToString());
    //    }

    
    
    
    
    
    
    //}

//string isoffline,  string drpSM1,  string comments ,string tt1

    [WebMethod]
    public static object btnimagebutton_click(ShipInfo shipInfo)
    {

        try
        {
            string isoffline = "";
            if (shipInfo.paymenttypechecked == "RBOffpay")
            {
                isoffline = "true";
            }
            else {
                isoffline = "false";

            }
            string trpm = "false";
            string emailconfimation = "";
            string Mastercard = "";
            string Mobileno = shipInfo.txtMobileNumber ;
            string rbtype= shipInfo.paymenttype;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["ORDER_ID"] != null)
            {
                if (HttpContext.Current.Session["trpm"] != null)
                {
                    trpm = HttpContext.Current.Session["trpm"].ToString();
                }

                if (HttpContext.Current.Session["emailconfimation"] != null)
                {
                    emailconfimation = HttpContext.Current.Session["emailconfimation"].ToString();
                }
                if (HttpContext.Current.Session["lblmastercardno"] != null)
                {
                    Mastercard = HttpContext.Current.Session["lblmastercardno"].ToString();
                }
                if (HttpContext.Current.Session["RBpaymenttype"] != null)
                {
                    rbtype = HttpContext.Current.Session["RBpaymenttype"].ToString();
                }
                //if (HttpContext.Current.Session["Mobileno"] != null)
                //{
                //    Mobileno = HttpContext.Current.Session["Mobileno"].ToString();
                //}
                int OrderID = 0;
                if (HttpContext.Current.Session["ORDER_ID"] != null)
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                }

                int i = 0;
                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                OrderServices objOrderServices = new OrderServices();
                HelperServices objHelperServices = new HelperServices();
                UserServices objUserServices = new UserServices();
                UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                // UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
                ErrorHandler objErrorHandler = new ErrorHandler();
                if (HttpContext.Current.Session["USER_ID"].ToString() == null || HttpContext.Current.Session["USER_ID"].ToString() == "" || HttpContext.Current.Session["USER_ID"].ToString() == "999")
                {
                    //return "login.aspx";
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "login.aspx" };
                }

                if (HttpContext.Current.Session["ORDER_ID"].ToString() == null || HttpContext.Current.Session["ORDER_ID"].ToString() == "")
                {
                    // return "login.aspx";
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "login.aspx" };
                }


                oOrderInfo = objOrderServices.GetOrder(OrderID);
                if (oOrderInfo.ProdTotalPrice == 0)
                {

                    //return "ConfirmMessage.aspx?Result=QTEEMPTY";
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                }

                DataSet tmpds = GetOrderItemDetailSum(OrderID);

                decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                }

                if (totalitemsum.ToString() == "0.00")
                {
                    //return "ConfirmMessage.aspx?Result=QTEEMPTY";
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };

                }





                QuoteServices objQuoteServices = new QuoteServices();
                OrderDB objOrderDB = new OrderDB();
                HelperServices objHelperService = new HelperServices();
                //int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                int OrdStatus = 0;
                string ApproveOrder = string.Empty;
                //Direct  Order / Approve Order (Comes from Pending order Page)


                //if (HttpContext.Current.Request.QueryString["ApproveOrder"] == null)
                //{
                //    if (HttpContext.Current.Session["USER_ROLE"] != null)
                //    {
                //        switch (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]))
                //        {
                //            case 1:

                //                if (trpm == "true")
                //                {
                //                    objErrorHandler.CreatePayLog(shipInfo.paymenttypechecked);
                //                    if (shipInfo.paymenttypechecked == "RBOffpay")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //                    }
                //                    else if (shipInfo.paymenttypechecked == "RBOnPay")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                //                    }
                //                }
                //                else
                //                {
                //                    if (rbtype != "Bank Transfer")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //                    }
                //                    else
                //                    {

                //                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                //                    }
                //                }
                //                break;
                //            case 2:
                //                if (trpm == "true")
                //                {
                //                    objErrorHandler.CreatePayLog(shipInfo.paymenttypechecked +"case2");
                //                    if (shipInfo.paymenttypechecked == "RBOffpay")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //                    }
                //                    else if (shipInfo.paymenttypechecked == "RBOnPay")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                //                    }
                //                }
                //                else
                //                {
                //                    if (rbtype != "Bank Transfer")
                //                    {
                //                        OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //                    }
                //                    else
                //                    {

                //                        OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                //                    }
                //                }
                //                break;
                //            case 3:
                //                OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                //    }
                //}
                //else if (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 1 || Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) == 2))
                //{
                //    if (trpm == "true")
                //    {
                //        objErrorHandler.CreatePayLog(shipInfo.paymenttypechecked + shipInfo.tt1 + trpm);
                //        if (shipInfo.paymenttypechecked == "RBOffpay")
                //        {
                //            OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //        }
                //        else if (shipInfo.paymenttypechecked == "RBOnPay")
                //        {
                //            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                //        }

                //    }
                //    else
                //    {
                //        objErrorHandler.CreatePayLog(shipInfo.paymenttypechecked + shipInfo.tt1 +rbtype);
                //        if (shipInfo.paymenttypechecked == "RBOffpay")
                //        {

                //            if (rbtype == "Bank Transfer")
                //            {
                //                OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;
                //            }
                //            else
                //            {
                //                OrdStatus = (int)OrderServices.OrderStatus.ORDERPLACED;
                //            }
                //        }
                //        else if (shipInfo.paymenttypechecked == "RBOnPay")
                //        {
                //            OrdStatus = (int)OrderServices.OrderStatus.Online_Payment;

                //        }

                //    }
                //}
                //else if (HttpContext.Current.Request.QueryString["ApproveOrder"] != null)
                //    OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;

                //OrdStatus = (int)OrderServices.OrderStatus.CAU_PENDING;
                decimal TaxAmount;
                decimal ProdTotCost;
                if (string.IsNullOrEmpty(HttpContext.Current.Session["ORDER_ID"].ToString()))
                    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OrdStatus);
                else
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());




                int oldorderID = OrderID;
                int QuoteId = 0;
                QuoteId = objQuoteServices.GetQuoteID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));




              
              
                if (OrderID > 0)
                {
                    ProdTotCost = objOrderServices.GetCurrentProductTotalCost(OrderID);
                    // decimal shipcost=  calcShipcost();
                    string calship = string.Empty;
                    string SHIP_CODE = string.Empty;
                    decimal shipcost = 0;
                    string[] charge = null;
                    if (shipInfo.drpSM1 != "Courier Pick Up" && shipInfo.drpSM1 != "Shop Counter Pickup")
                    {
                        if (HttpContext.Current.Session["shipcost"] != null && HttpContext.Current.Session["shipcost"].ToString().Trim() != "")
                        {
                            try
                            {
                                calship = HttpContext.Current.Session["shipcost"].ToString();
                                charge = calship.Split('-');
                                shipcost = objHelperServices.CDEC(charge[0]); ;
                                SHIP_CODE = charge[1];
                            }
                            catch
                            { }
                        }
                        else
                        {
                            if (shipcost == 0 && HttpContext.Current.Session["ShipCost_br"] != null)
                            {

                                shipcost = objHelperServices.CDEC(HttpContext.Current.Session["ShipCost_br"].ToString());
                                objErrorHandler.CreatePayLog("ShipCost_br" + shipcost);


                            }
                            SHIP_CODE = shipInfo.shipcode;


}
                    }
                    // TaxAmount = objOrderServices.CalculateTaxAmount(ProdTotCost + shipcost, OrderID.ToString());
                    TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());

                    decimal UpdRst = 0;
                    oOrderInfo.OrderID = OrderID;
                    if (OrdStatus == 0)
                    {
                        OrdStatus = 1;
                    }
                    oOrderInfo.OrderStatus = OrdStatus;
                    ///////////////////////////
                    int _UserrID;
                    _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                    
                    oUserInfo = objUserServices.GetUserInfo(_UserrID);
                  
                    oOrderInfo.OrderStatus = OrdStatus;
                   
                    oOrderInfo.ShipMethod = shipInfo.drpSM1;
                   
                    if (shipInfo.drpSM1 == "Drop Shipment Order")
                    {
                        oOrderInfo.ShipCompName = objHelperServices.Prepare(shipInfo.drpShipCompName);
                        oOrderInfo.ShipFName = objHelperServices.Prepare(shipInfo.drpAttentionTo);
                        oOrderInfo.ShipAdd1 = objHelperServices.Prepare(shipInfo.drpaddress1);
                        oOrderInfo.ShipAdd2 = objHelperServices.Prepare(shipInfo.drpaddress2);
                        oOrderInfo.ShipCity = objHelperServices.Prepare(shipInfo.drpsuburb);
                       
                        oOrderInfo.ShipState = objHelperServices.Prepare(shipInfo.drpstate);
                        oOrderInfo.ShipZip = objHelperServices.Prepare(shipInfo.drppostcode);
                        if (shipInfo.drpstate == "")
                        {

                            if (HttpContext.Current.Session["suburb"] != null)
                            {
                                string[] shipx = HttpContext.Current.Session["suburb"].ToString().Split('-');
                                oOrderInfo.ShipState = shipx[1];
                                oOrderInfo.ShipZip = shipx[0];
                            }
                        }
                        oOrderInfo.ShipCountry = objHelperServices.Prepare(shipInfo.drpcountry);
                       
                        oOrderInfo.DeliveryInstr = objHelperServices.Prepare(shipInfo.drpinstruction);
                        oOrderInfo.ShipPhone = objHelperServices.Prepare(shipInfo.drpshipphone);
                    }
                    else
                    {
                        oOrderInfo.ShipFName = objHelperServices.Prepare(oUserInfo.FirstName);
                        oOrderInfo.ShipLName = objHelperServices.Prepare(oUserInfo.LastName);
                        oOrderInfo.ShipMName = objHelperServices.Prepare(oUserInfo.MiddleName);
                        oOrderInfo.ShipAdd1 = objHelperServices.Prepare(oUserInfo.ShipAddress1);
                        oOrderInfo.ShipAdd2 = objHelperServices.Prepare(oUserInfo.ShipAddress2);
                        oOrderInfo.ShipAdd3 = objHelperServices.Prepare(oUserInfo.ShipAddress3);
                        oOrderInfo.ShipCity = objHelperServices.Prepare(oUserInfo.ShipCity);
                        oOrderInfo.ShipState = objHelperServices.Prepare(oUserInfo.ShipState);
                        oOrderInfo.ShipCountry = objHelperServices.Prepare(oUserInfo.ShipCountry);
                        oOrderInfo.ShipZip = objHelperServices.Prepare(oUserInfo.ShipZip);
                        oOrderInfo.ShipCompName= objHelperServices.Prepare(oUserInfo.COMPANY_NAME);
                        // oOrderInfo.ShipPhone = objHelperServices.Prepare(txtSPhone.Text);
                        if (shipInfo.drpSM1 != "Counter Pickup")
                        {
                            oOrderInfo.ShipPhone = objHelperServices.Prepare(oUserInfo.ShipPhone);
                        }
                        else
                        {
                            if (HttpContext.Current.Session["Nothanks"] == null || HttpContext.Current.Session["Nothanks"] == "false")
                            {
                                oOrderInfo.ShipPhone =Mobileno;
                                objErrorHandler.CreateLog("Mobileno" + Mobileno);
                                if (oOrderInfo.ShipPhone == "" || oOrderInfo.ShipPhone == null)
                                {
                                    oOrderInfo.ShipPhone = objHelperServices.Prepare(oUserInfo.ShipPhone);
                                }
                                else if(HttpContext.Current.Session["txtMobileNumber"]!=null)
                                {
                                    oOrderInfo.ShipPhone = objHelperServices.Prepare(HttpContext.Current.Session["txtMobileNumber"].ToString());
                                }
                            }
                            else
                            {
                                oOrderInfo.ShipPhone = objHelperServices.Prepare(oUserInfo.ShipPhone);
                            }
                        }
                        //DataSet objds = new DataSet();
                        //objds = (DataSet)objOrderDB.GetGenericDataDB(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()).ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
                        //if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                        //{
                            oOrderInfo.DeliveryInstr = oUserInfo.DeliveryInst;
                        //objHelperService.CS(objds.Tables[0].Rows[0]["DELIVERY_INST"].ToString());
                        //}

                    }


                    oOrderInfo.ShipNotes = objHelperServices.Prepare(shipInfo.TextBox1);


                  

                    //string strHostName = System.Net.Dns.GetHostName();
                    string clientIPAddress = "";
                    //if (System.Net.Dns.GetHostAddresses(strHostName) != null)
                    //{
                    //    if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    //    else
                    //        clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                    //}

                   // string x = HttpContext.Current.Request.UserHostAddress;
                    //objErrorHandler.CreateLog(x+"ipaddress"); 
                    oOrderInfo.ClientIPAddress = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
                                                                    //oOrderInfo.ClientIPAddress = Request.Params["REMOTE_ADDR"];
                                                                    //oOrderInfo.ClientIPAddress = Request.UserHostAddress;
                    oOrderInfo.isEmailSent = false;
                    oOrderInfo.isInvoiceSent = false;
                    oOrderInfo.IsShipped = false;
                    UserServices.UserInfo oOrdBillInfo = new UserServices.UserInfo();
                    oOrdBillInfo = objUserServices.GetUserBillInfo(_UserrID);
                    oOrderInfo.BillFName = objHelperServices.Prepare(oOrdBillInfo.FirstName);
                    oOrderInfo.BillLName = objHelperServices.Prepare(oOrdBillInfo.LastName);
                    oOrderInfo.BillMName = objHelperServices.Prepare(oOrdBillInfo.MiddleName);
                    oOrderInfo.BillAdd1 = objHelperServices.Prepare(oOrdBillInfo.BillAddress1);
                    oOrderInfo.BillAdd2 = objHelperServices.Prepare(oOrdBillInfo.BillAddress2);
                    oOrderInfo.BillAdd3 = objHelperServices.Prepare(oOrdBillInfo.BillAddress3);
                    oOrderInfo.BillCity = objHelperServices.Prepare(oOrdBillInfo.BillCity);
                    oOrderInfo.BillState = objHelperServices.Prepare(oOrdBillInfo.BillState);
                    oOrderInfo.BillCountry = objHelperServices.Prepare(oUserInfo.BillCountry);
                    oOrderInfo.BillZip = objHelperServices.Prepare(oOrdBillInfo.BillZip);
                    oOrderInfo.BillPhone = objHelperServices.Prepare(oOrdBillInfo.BillPhone);
                    oOrderInfo.ProdTotalPrice = objOrderServices.GetCurrentProductTotalCost(OrderID);
                    oOrderInfo.BillcompanyName = objHelperServices.Prepare(oOrdBillInfo.COMPANY_NAME);
                    //// oOrderInfo.ShipCost = CalculateShippingCost(OrderID);
                    if (oOrderInfo.ShipCost == 0)
                    {

                        oOrderInfo.ShipCost = shipcost;
                    }
                    oOrderInfo.SHIP_CODE = SHIP_CODE;
                    oOrderInfo.TaxAmount = TaxAmount;
                    oOrderInfo.TotalAmount = ProdTotCost + TaxAmount + objHelperServices.CDEC(oOrderInfo.ShipCost);
                    oOrderInfo.TrackingNo = "";
                    if (shipInfo.drpSM1.Trim().Contains("Drop Shipment Order"))
                    {
                        oOrderInfo.DropShip = 1;
                    }
                    oOrderInfo.UserID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
                    UpdRst = objOrderServices.UpdateOrder(oOrderInfo);
                    string PAYMENTSELECTION = string.Empty;
                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                    {

                    }

                    else if (shipInfo.paymenttypechecked == "RBpaymenttype")
                    {

                        DataSet ds = objUserServices.GetPaymentoption(_UserrID);
                        if (ds != null)
                        {
                            i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

                        }
                        if (i == 1 || i == 5 || i == 8 || i == 7)
                        {

                            oOrderInfo.Payment_Selection = "BT";
                            PAYMENTSELECTION = "BT";
                            if (UpdRst > 0)
                            {
                                objErrorHandler.CreatePayLog("i" + i + "Payment_Selection" + oOrderInfo.Payment_Selection);
                                UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                            }
                        }
                        else
                        {
                            oOrderInfo.Payment_Selection = "";
                        }
                    }
                    else
                    {
                        if (emailconfimation == "" && shipInfo.paymenttypechecked == "RBpaymenttype")
                        {
                            DataSet ds = objUserServices.GetPaymentoption(_UserrID);
                            if (ds != null)
                            {
                                i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());
                                objErrorHandler.CreatePayLog("Payment term" + i);
                            }
                            if (i == 1 || i == 5 || i == 8)
                            {

                                oOrderInfo.Payment_Selection = "BT";
                                PAYMENTSELECTION = "BT";
                                objErrorHandler.CreateLog("i" + i + "Payment_Selection" + oOrderInfo.Payment_Selection);
                                UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                            }
                            else {
                                oOrderInfo.Payment_Selection = "";

                            }

                        }

                        if (PAYMENTSELECTION != "")
                        {

                            if (UpdRst > 0)
                            {
                                objErrorHandler.CreateLog(PAYMENTSELECTION +"orderid"+ OrderID);
                                UpdRst = objOrderServices.UpdatePAYMENTSELECTION(OrderID, PAYMENTSELECTION);
                            }
                        }
                    }

                    if (HttpContext.Current.Session["PrevOrderID"] != null && Convert.ToInt32(HttpContext.Current.Session["PrevOrderID"]) > 0)
                    {
                        HttpContext.Current.Session["PrevOrderID"] = "0";
                    }


                    if (UpdRst > 0)
                    {
                        objOrderServices.UpdateCustomFields(oOrderInfo);


                        HttpContext.Current.Session["ORDER_NO"] = null;
                        HttpContext.Current.Session["SHIPPING"] = null;
                        HttpContext.Current.Session["DELIVERY"] = null;
                        HttpContext.Current.Session["DROPSHIP"] = null;

                        HttpContext.Current.Session["ShipCost"] = oOrderInfo.ShipCost;
                        if (HttpContext.Current.Request["QteFlag"] != null && HttpContext.Current.Request["QteFlag"].ToString() == "1")
                        {
                            // return "Payment.aspx?OrdId=" + OrderID + "&QteFlag=1";
                            return new ResponseValue { Status = "Success", Message = "", RedirectTo = "Payment.aspx?OrdId=" + OrderID + "&QteFlag=1" };
                        }
                        else
                        {
                            string returnval = "";
                            call_ProceedFunction(OrderID, shipInfo.drpSM1, isoffline, rbtype, shipInfo.tt1, shipInfo.TextBox1,oOrderInfo.ShipZip);

                            if (returnval.Trim() != "")
                                return new ResponseValue { Status = "Failure", Message = "", RedirectTo = returnval };
                            //PnlOrderInvoice.Visible = true;
                            //PnlOrderContents.Visible = false;
                            //PHOrderConfirm.Visible = true;

                            string lblmastercardno = "";
                            if (HttpContext.Current.Session["lblmastercardno"] != null)
                            {
                                lblmastercardno = HttpContext.Current.Session["lblmastercardno"].ToString();
                            }
                            else
                            {
                                lblmastercardno = shipInfo.cardno;
                            }
                            if (lblmastercardno == null)
                                lblmastercardno = "";

                            try
                            {
                                HttpContext.Current.Session["ORDER_ID"] = "0";
                                if (HttpContext.Current.Session["USER_ROLE"].ToString() == "3")
                                {
                                    //return "Your order has been successfully submitted to us for processing. Thank You!";
                                    return new ResponseValue { Status = "Success", Message = "Thanks for your order.Your order will be pending order on the system.. Thank You!", RedirectTo = "" };
                                }
                                else if (shipInfo.paymenttypechecked == "RBpaymenttype")
                                {
                                    string mess = "Your order has been successfully submitted to us for processing using " + rbtype + ". Thank You!";

                                    if (lblmastercardno == "")
                                    {
                                        mess = "Your order has been successfully submitted to us for processing using " + rbtype + ". Thank You!";
                                    }
                                    else
                                    {
                                        // objErrorHandler.CreateLog(lblmastercardno + "Master card " + lblmastercardno.Length);

                                        if (lblmastercardno.Length == 18)
                                        {
                                            mess = "Your order has been successfully submitted to us for processing using " + rbtype + " ending with " + lblmastercardno.Substring(15, 3) + ". Thank You!";
                                        }
                                        else if (lblmastercardno.Length >= 17)
                                        {
                                            mess = "Your order has been successfully submitted to us for processing using " + rbtype + " ending with " + lblmastercardno.Substring(14, 3) + ". Thank You!";
                                        }

                                    }
                                    return new ResponseValue { Status = "Success", Message = mess.ToString(), RedirectTo = "" };
                                }
                                else if ((ispickuponly_zone(OrderID, shipInfo.drpSM1,oOrderInfo.ShipZip ) == true  ))
                                {
                                    //return  "Your order has been successfully submitted to us for processing. Thank You!";
                                    return new ResponseValue { Status = "Success", Message = "Your order has been successfully submitted to us for processing. Thank You!", RedirectTo = "" };
                                }
                                else 
                                {
                                    //return "Your order has been successfully submitted to us for processing. Thank You!";
                                    return new ResponseValue { Status = "Success", Message = "Your order has been successfully submitted to us for processing. Thank You!", RedirectTo = "" };
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                HttpContext.Current.Session["ORDER_ID"] = "0";
                                // return "Your order has been successfully submitted to us for processing using " + rbtype + ". Thank You!";
                                return new ResponseValue { Status = "Failure", Message = ex.ToString(), RedirectTo = "" };
                            }

                       }
                    }
                }
            }
            else
            {

                //HttpContext.Current.Response.Redirect("login.aspx");
                return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "login.aspx" };
            }
                //return "Your order has been successfully submitted to us for processing using " + rbtype + ". Thank You!";
            }
        catch (Exception Ex)
        {
            //return Ex.ToString();
            return new ResponseValue { Status = "Failure", Message = Ex.ToString(), RedirectTo = "" };
        }
        return new ResponseValue { Status = "Success", Message = "", RedirectTo = "" };
    }
    
    
    
    [WebMethod]
    public static string CheckOrderID(string refid)
    {
        string isexist = "1";
        DataSet DS = new DataSet();
        HelperDB objHelperDB = new HelperDB();

        //querystr = "select count(*) from TBWC_PAYMENT where order_id in(select order_id from dbo.tbwc_order where [user_id] in(select [user_id] from TBWC_COMPANY_BUYERS where company_id in(select company_id from dbo.TBWC_COMPANY_BUYERS where [user_id]=" + objHelperServices.CI(Session["USER_ID"].ToString()) + "))) and po_release='" + refid + "'";
        //objHelperServices.SQLString = querystr;
        //DS = objHelperServices.GetDataSet();
        string orderid = HttpContext.Current.Session["ORDER_ID"].ToString();
        UserServices objUserServices = new UserServices();
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        int _UserrID = objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString());
        if (orderid == "0")
        {



            orderid = objOrderServices.GetOrderID(_UserrID, 1).ToString();
            HttpContext.Current.Session["ORDER_ID"] = orderid;
        }
        DS = (DataSet)objHelperDB.GetGenericPageDataDB("", HttpContext.Current.Session["USER_ID"].ToString(), refid, orderid, "GET_SHIPPING_PAYMENT_COUNT", HelperDB.ReturnType.RTDataSet);
        if (DS != null)
        {

            if (Convert.ToInt32(DS.Tables[0].Rows[0][0]) > 0)
            {

                int chkdup_orderno = objUserServices.GET_DUP_ORDERNO_OPTION(_UserrID);
                if (chkdup_orderno == 1)
                {
                    return "2";
                }
                else
                {
                    return "0";
                }
            }
        }
        else
        {
            return "1";
        }
        return "1";
    }

    [System.Web.Services.WebMethod]
    public static object SaleTrans( ShipInfo shipInfo)
    {
        try
        {

            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["ORDER_ID"] != null)
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


                string OrderID = HttpContext.Current.Session["ORDER_ID"].ToString();
                int intOrderID = Convert.ToInt32(OrderID);

                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                OrderServices objOrderServices = new OrderServices();
                HelperServices objHelperServices = new HelperServices();
                oOrderInfo = objOrderServices.GetOrder(intOrderID);

                ErrorHandler objerrhandler = new ErrorHandler();



                if (oOrderInfo.ProdTotalPrice == 0)
                {
                    objerrhandler.CreateLog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                    //Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                }

                DataSet tmpds = GetOrderItemDetailSum(oOrderInfo.OrderID);
                objerrhandler.CreateLog("GetOrderItemDetailSum");
                decimal totalitemsum = 0;
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                }

                if (totalitemsum.ToString() == "0.00")
                {
                    objerrhandler.CreateLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                    // Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                    return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                }
                if (oOrderInfo.OrderStatus == 1)

                {


                    oPayInfo = objPaymentServices.GetPayment(intOrderID);
                    decimal shipcost = 0;
                    if (shipInfo.lblshipingcost!="")
                    {
                        shipcost = Convert.ToDecimal(shipInfo.lblshipingcost);
                            }
                    decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(intOrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
                   objerrhandler.CreatePayLog(OrderID.ToString() + "paymnet amount:" + oPayInfo.Amount + "ProdTotalPrice:" + oOrderInfo.ProdTotalPrice + "ShipCost" + oOrderInfo.ShipCost + "taxamount" + TaxAmount);
                    int PaymentID = oPayInfo.PaymentID;
                    if (oPayInfo.PaymentID == 0 || oPayInfo.Amount != (oOrderInfo.ProdTotalPrice + shipcost + TaxAmount)  || (oOrderInfo.TotalAmount != Convert.ToDecimal(shipInfo.Amount)))
                    {
                        objerrhandler.CreatePayLog("inside before opayinfo"+ intOrderID);
                      
                        order_submit_process(intOrderID, "BR", shipInfo.tt1, shipInfo.drpSM1, shipInfo.TextBox1, shipInfo.txtMobileNumber, shipInfo.cmbProvider, shipInfo);               //need to uncomment 
                        objerrhandler.CreatePayLog("inside after opayinfo"+ intOrderID);

                        oPayInfo = objPaymentServices.GetPayment(intOrderID);
                        PaymentID = oPayInfo.PaymentID;
                        oOrderInfo = objOrderServices.GetOrder(intOrderID);
                    }
                    else
                    {
                        PaymentID = oPayInfo.PaymentID;
                    }

                  
                    if (oOrderInfo.TotalAmount != Convert.ToDecimal(shipInfo.Amount))

                    {

                        x = "Error " + "Order Amount Missmatch";
                        objerrhandler.CreatePayLog("Order Amount Missmatch" + oOrderInfo.TotalAmount + "--" + shipInfo.Amount +"--"+intOrderID.ToString());
                        return new ResponseValue { Status = "Failure", Message = x, RedirectTo = "OrderDetails.aspx" };
                    }


                   // oPayInfo = objPaymentServices.GetPayment(intOrderID);



                    //MerchantAccountId = "wagnerelectronicsAUD",

                    objerrhandler.CreatePayLog(oOrderInfo.TotalAmount + "TotalAmount"+intOrderID.ToString());
                    var request = new TransactionRequest
                    {
                        Amount = Convert.ToDecimal(oOrderInfo.TotalAmount),
                        //MerchantAccountId = "wesallianceptyltdAUD",
                        PaymentMethodNonce = shipInfo.nounce,

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
                        DeviceData = HttpContext.Current.Request.Form["device_data"]
                    };
                    if (shipInfo.nounce == "no")
                    {
                        x = "Error " + "Please Try again or use a different card / payment method.";
                        objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes",oOrderInfo.TotalAmount.ToString(), HttpContext.Current.Session["USER_ID"].ToString());
                        objerrhandler.CreatePayLog(" br  Orderid=" + OrderID + "Error Processing PARes");
                       
                        return new ResponseValue { Status = "Failure", Message = x, RedirectTo = "" };
                    }
                    //MerchantAccountId = "wagnerelectronicsAUD",
                    PaymentMethodNonce paymentMethodNonce = null;
                    try
                    {
                        paymentMethodNonce = gateway.PaymentMethodNonce.Find(shipInfo.nounce);
                    }
                    catch (Exception ex)
                    {
                        objerrhandler.CreateLog(ex.ToString() + "nounce" + shipInfo.nounce);

                    }
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

                        objerrhandler.CreatePayLog("IsSucess:" + result.IsSuccess());

                        if (result.IsSuccess() == true)
                        {


                            Transaction transaction = result.Target;
                            objerrhandler.CreateLog("Status:" + transaction.Status.ToString());
                            string ResponseId = transaction.Id;
                            string ResponseText = transaction.ProcessorResponseText;
                            string Responsecode = transaction.ProcessorResponseCode;

                            string cardtype = result.Target.CreditCard.CardType.GetType().ToString();
                           objSecurePayService.call_GetPaymentRequest_braintree(intOrderID, PaymentID, result.Target.CreditCard.CardType.ToString(), result.Target.CreditCard.CardholderName, result.Target.CreditCard.MaskedNumber, result.Target.CvvResponseCode, result.Target.CreditCard.ExpirationDate, "YES", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, LiabilityShifted.ToString(), Status, oOrderInfo.TotalAmount.ToString(),HttpContext.Current.Session["USER_ID"].ToString());
                            objerrhandler.CreatePayLog("b4 UpdatePaymentOrderStatus_DirectOnlinepayment");
                            objOrderServices.call_DirectOnlinepayment(intOrderID, PaymentID, false);
                            int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "BR");
                            objerrhandler.CreatePayLog("after UpdatePaymentOrderStatus_DirectOnlinepayment");

                            BillInfoSP billinfo = new BillInfoSP();
                            objerrhandler.CreatePayLog("b4 SendMail_AfterPaymentSP");
                           
                            billinfo.call_SendMail_AfterPaymentSP(intOrderID, (int)OrderServices.OrderStatus.Online_Payment, false,oPayInfo,oOrderInfo,"BR");
                            objerrhandler.CreatePayLog("after SendMail_AfterPaymentSP");
                            HttpContext.Current.Session["paySPresponse"] = "SUCCESS";


                            HttpContext.Current.Session["XpayMS"] = null;
                            HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                            HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                            HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                            HttpContext.Current.Session["payflowresponse"] = "SUCCESS";

                            int l = result.Target.CreditCard.MaskedNumber.ToString().Length;
                            string Message = "Transaction approved! Thank you for your order." + "<br/>Payment Method :  Credit Card" + "<br/>Card Number:"+ result.Target.CreditCard.MaskedNumber + "<br/> Order Ref  : " + oPayInfo.PORelease;
                            
                            HttpContext.Current.Session["ORDER_ID"] = "0";
                            // string str = "BillInfoSP.aspx?Paytype=direct&key="+intOrderID.ToString()  + "&cn=" + result.Target.CreditCard.MaskedNumber.Substring(l - 4, 4);
                            return new ResponseValue { Status = "Success", Message = Message, RedirectTo = "" };

                            //  x = "true";
                        }
                        else
                        {
                            try
                            {
                                Transaction transaction = result.Transaction;
                                objerrhandler.CreatePayLog(result.Transaction.AvsErrorResponseCode);
                                // objerrhandler.CreateLog("Card No:" + result.Transaction.CreditCard.MaskedNumber);
                                string errorMessages = "";
     objerrhandler.CreatePayLog("Error Status:" + transaction.Status.ToString());
                                objerrhandler.CreatePayLog("Error Status:" + transaction.GatewayRejectionReason.ToString());
                                objerrhandler.CreatePayLog("************************");
                               
                                objerrhandler.CreatePayLog(" br  Orderid=" + OrderID + errorMessages);
                                objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, LiabilityShifted.ToString(), Status, oOrderInfo.TotalAmount.ToString(), HttpContext.Current.Session["USER_ID"].ToString());
                                x = "Error " + "Please Try again or use a different card / payment method.";

                                HttpContext.Current.Session["payflowresponse"] = "FAIL";

                               
                                return new ResponseValue { Status = "Failure", Message = objPRInfo.Error_Text, RedirectTo = "" };
                            }
                            catch (Exception ex)
                            {
                                x = "Error " + "Please Try again or use a different card / payment method.";
                                objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status, oOrderInfo.TotalAmount.ToString(), HttpContext.Current.Session["USER_ID"].ToString());
                                objerrhandler.CreatePayLog(" br  Orderid=" + OrderID + Status);
                            }
                        }


                        return new ResponseValue { Status = "Failure", Message = objPRInfo.Error_Text, RedirectTo = "" };
                    }
                    else
                    {
                        //   objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                        x = "Error " + "Please Try again or use a different card / payment method.";
                        objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", TRANSACTIONID, "", "No", "", "Failed", "", LiabilityShifted.ToString(), Status, oOrderInfo.TotalAmount.ToString(), HttpContext.Current.Session["USER_ID"].ToString());
                        objerrhandler.CreatePayLog(" br  Orderid=" + OrderID + Status);

                        return new ResponseValue { Status = "Failure", Message = objPRInfo.Error_Text, RedirectTo = "" };
                    }


                }
                else
                {

                    HttpContext.Current.Session["ORDER_ID"] = 0;

               
                    return new ResponseValue { Status = "Failure", Message = x, RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                }
            }
            else
            {

                //HttpContext.Current.Response.Redirect("login.aspx");
                return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "login.aspx" };
            }

        }

        catch (Exception ex)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            objErrorHandler.CreatePayLog(ex.ToString());
            return new ResponseValue { Status = "Failure", Message = ex.ToString(), RedirectTo = "" };
        }
    }


    //string tt1, string drpSM1, string TextBox1, string txtMobileNumber, string cmbProvider, string cardname, string creditcardno, string cvv, string expmonth, string expyr,
    [WebMethod]
    public static object btnSecurePay(ShipInfo shipInfo)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
       // UserServices.UserInfo oOrdShippInfo = new UserServices.UserInfo();
        try
        {
            bool issecurepayclicked = false;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["ORDER_ID"] != null)
            {
                if (issecurepayclicked == false)
            {
                string rtnstr = "";
                bool isValid = true;

                //Page.IsValid == true
                if (isValid == true)
                {
                    //btnSP.Visible = false;
                    //BtnProgressSP.Visible = true;
                    issecurepayclicked = true;
                       
                    SecurePayService.PaymentRequestInfo objPRInfo = new SecurePayService.PaymentRequestInfo();
                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    OrderServices objOrderServices = new OrderServices();
                    HelperServices objHelperServices = new HelperServices();
                    PaymentServices objPaymentServices = new PaymentServices();
                    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
                    SecurePayService objSecurePayService = new SecurePayService();
                    int PaymentID = 0;
                    int OrderID = 0;
                    decimal shipcost = 0;
                    if (HttpContext.Current.Session["ORDER_ID"] != null)
                    {
                        OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());
                    }
                        int x = shipInfo.cardno.Length;

                        int c=    objOrderServices.checkcardnoexsist(HttpContext.Current.Session["USER_ID"].ToString(),"", shipInfo.cardno.Substring(x - 4, 4), shipInfo.cardno.Substring(0, 3));

                        if (c == 0)
                        {
                           
                            return new ResponseValue { Status = "BR", Message = "Sorry ..Please Try Again", RedirectTo = "" };

                        }

                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    shipcost = oOrderInfo.ShipCost;
                    if (oOrderInfo.ProdTotalPrice == 0)
                    {
                        objErrorHandler.CreatePayLog("btnSecurepay start Orderid :" + OrderID + "ProdTotalPrice" + "0");
                        //Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                        return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                    }

                    DataSet tmpds = GetOrderItemDetailSum(OrderID);
                    decimal totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                    {
                        totalitemsum = objHelperServices.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    }

                    if (totalitemsum.ToString() == "0.00")
                    {
                        objErrorHandler.CreatePayLog("Prodtotalprice:" + oOrderInfo.ProdTotalPrice + " " + "totalitemsum :" + totalitemsum);
                       // Response.Redirect("ConfirmMessage.aspx?Result=QTEEMPTY", true);
                        return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "ConfirmMessage.aspx?Result=QTEEMPTY" };
                    }

                  //  UserServices objUserServices = new UserServices();
                   // UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
                    try
                    {

                        int currentyear = DateTime.Now.Year;
                            currentyear = Convert.ToInt32(currentyear.ToString().Substring(2, 2));

                            int currentmonth = DateTime.Now.Month;
                            objErrorHandler.CreatePayLog(currentyear + "currentyear" + currentmonth+ "currentmonth");
                            bool isexpvalid = false;
                            string expmonth="";
                            string expyear="" ;
                            if (shipInfo.expyr.Contains("/") == true  )
                                {
                                if (shipInfo.expyr.Length == 5)
                                {
                                    expmonth = shipInfo.expyr.Substring(0, 2);
                                    if (Convert.ToInt16(expmonth) < 10 && expmonth.Contains("0") == false)
                                    {
                                        expmonth = "0" + expmonth;

                                    }
                                }
                                else {
                                    expmonth = shipInfo.expyr.Substring(0, 1);
                                    expmonth = "0" + expmonth;
                                }
                                //objErrorHandler.CreatePayLog(shipInfo.expmonth + "month");
                                expyear = shipInfo.expyr.Substring(3, 2);
                                //objErrorHandler.CreateLog(shipInfo.expyr + "year");

                            }
                        //if (expyear > currentyear)
                        //{
                        //    isexpvalid = true;
                        //} 
                        //else if (expyear == currentyear)
                        //{
                        //        objErrorHandler.CreateLog("valid year");
                        //    if (expmonth >= currentmonth)
                        //    {
                        //            objErrorHandler.CreateLog("valid month");
                        //            isexpvalid = true;
                        //    }
                        //}


                        //if (isexpvalid == true)
                        //{
                            oPayInfo = objPaymentServices.GetPayment(OrderID);
                            decimal TaxAmount = objOrderServices.GetTotalOrderTaxAmount(OrderID) + objOrderServices.CalculateTaxAmount(shipcost, OrderID.ToString());
                            objErrorHandler.CreatePayLog(OrderID.ToString() + "paymnet amount:" + oPayInfo.Amount + "ProdTotalPrice:" + oOrderInfo.ProdTotalPrice + "ShipCost" + oOrderInfo.ShipCost + "taxamount" + TaxAmount);
                            if (oPayInfo.PaymentID == 0 || oPayInfo.Amount != (oOrderInfo.ProdTotalPrice + oOrderInfo.ShipCost + TaxAmount))
                            {
                                objErrorHandler.CreatePayLog("inside before opayinfo");
                                    int intOrderID = Convert.ToInt32(OrderID);
                                    order_submit_process(intOrderID,"SP", shipInfo.tt1, shipInfo.drpSM1, shipInfo.TextBox1, shipInfo.txtMobileNumber, shipInfo.cmbProvider,shipInfo);               //need to uncomment 
                                objErrorHandler.CreatePayLog("inside after opayinfo");
                                oPayInfo = objPaymentServices.GetPayment(OrderID);
                                PaymentID = oPayInfo.PaymentID;
                            }
                            else
                            {
                                PaymentID = oPayInfo.PaymentID;
                            }
                            objPRInfo.Error_Text = "";
                            objPRInfo = objSecurePayService.GetPaymentRequest(OrderID, PaymentID, "", shipInfo.cardname, shipInfo.cardno.Replace(" ",""), shipInfo.cvv, expmonth + "/" +"20"+expyear, HttpContext.Current.Session["USER_ID"].ToString());
                            objErrorHandler.CreatePayLog(objPRInfo.Error_Text);
                            if (objPRInfo.Error_Text != "")
                            {
                              
                                HttpContext.Current.Session["payflowresponse"] = "FAIL";
                                    if (HttpContext.Current.Session["NoAttempt"] != null)
                                    {

                                        int noatt = Convert.ToInt32(HttpContext.Current.Session["NoAttempt"].ToString());
                                        if (noatt >= 2)
                                        {
                                            HttpContext.Current.Session["NoAttempt"] = 0;
                                            return new ResponseValue { Status = "BR", Message = "Sorry ..Please Try Again", RedirectTo = "" };

                                        }
                                        noatt = noatt + 1;
                                        HttpContext.Current.Session["NoAttempt"] = noatt;
                                    }
                                    else
                                    {
                                        HttpContext.Current.Session["NoAttempt"] = 1;
                                    }

                                        issecurepayclicked = false;
                                return new ResponseValue { Status = "Failure", Message = objPRInfo.Error_Text, RedirectTo = "" };
                            }
                            else
                            {
                                    //HttpContext.Current.Session["XpayMS"] = null;
                                    //HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                                    //HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                                    //HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                                    //HttpContext.Current.Session["payflowresponse"] = "SUCCESS";
                                    // int x = shipInfo.cardno.Length;
                                    // Response.Redirect("BillInfoSP.aspx?Paytype=direct&key=" + Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid") + "&cn=" + txtcreditcardno.Text.Substring(x - 4, 4));
                                    //string str = "BillInfoSP.aspx?Paytype=direct&key=" +OrderID.ToString() + "&cn=" + shipInfo.cardno.Substring(x - 4, 4);
                                    //return new ResponseValue { Status = "Success", Message = "", RedirectTo = str };
                      

    int intOrderID = Convert.ToInt32(OrderID.ToString());
                                    objErrorHandler.CreatePayLog("b4 UpdatePaymentOrderStatus_DirectOnlinepayment");
                                    objOrderServices.call_DirectOnlinepayment(intOrderID, PaymentID, false);
                                    objErrorHandler.CreatePayLog("after UpdatePaymentOrderStatus_DirectOnlinepayment");
                              int UpdRst = objOrderServices.UpdatePAYMENTSELECTION(intOrderID, "SP");
                                    objErrorHandler.CreatePayLog("b4 SendMail_AfterPaymentSP");

                                    BillInfoSP billinfo = new BillInfoSP();
                                    billinfo.call_SendMail_AfterPaymentSP(intOrderID, (int)OrderServices.OrderStatus.Online_Payment, false, oPayInfo, oOrderInfo,"SP");

                                    objErrorHandler.CreatePayLog("after SendMail_AfterPaymentSP");
                                    
                                    HttpContext.Current.Session["paySPresponse"] = "SUCCESS";

                                    HttpContext.Current.Session["NoAttempt"] = 0;
                                    HttpContext.Current.Session["XpayMS"] = null;
                                    HttpContext.Current.Session["paySPresponse"] = "SUCCESS";
                                    HttpContext.Current.Session["Mchkout"] = Encrypt_SP(OrderID.ToString() + "#####" + "PaySP" + "#####" + "Paid");
                                    HttpContext.Current.Session["P_Oid"] = OrderID.ToString();
                                    HttpContext.Current.Session["payflowresponse"] = "SUCCESS";

                                    // int l = result.Target.CreditCard.MaskedNumber.ToString().Length;
                                    string cardno = shipInfo.cardno.Substring(0 , 4)+ " xxxx xxxx " +shipInfo.cardno.Substring(x - 4, 4);
                                    string Message = "Transaction approved! Thank you for your order." + "<br/>Payment Method :  Credit Card" + "<br/>Card Number:" + cardno+ "<br/> Order Ref  : " + oPayInfo.PORelease;

                                    HttpContext.Current.Session["ORDER_ID"] = "0";
                                    // string str = "BillInfoSP.aspx?Paytype=direct&key="+intOrderID.ToString()  + "&cn=" + result.Target.CreditCard.MaskedNumber.Substring(l - 4, 4);
                                    return new ResponseValue { Status = "Success", Message = Message, RedirectTo = "" };

                                }
                        //    }
                        //else
                        //{
                        //    issecurepayclicked = false;
                        //    return new ResponseValue { Status = "Failure", Message = "Error found in details you have entered. Please Select valid expiry date.", RedirectTo = "" };
                        //}
                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString());
                    }
                }
                else
                {
                    //divcreditcard.Style.Add("display", "block");
                    //divdedault.Style.Add("display", "none");
                    //divpaypal.Style.Add("display", "none");
                }
            }


        }
            else
            { 

            //HttpContext.Current.Response.Redirect("login.aspx");
            return new ResponseValue { Status = "Failure", Message = "", RedirectTo = "login.aspx" };
        }
    }



        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return new ResponseValue { Status = "Failure", Message = "Please Try Again", RedirectTo = "" };
        }
        return new ResponseValue { Status = "Failure", Message = "Please Try Again", RedirectTo = "" };
    }

    
    
    //private void showselectedtab_ondropdownchange_inter()
    //{

    //    if (rbinternationaldefault.Checked == true)
    //    {
    //        //GetPaymentTerm(Userid.ToString());
    //        divintdirdep.Visible = true;
    //        divicreditcard.Visible = false;
    //        divpaypal.Visible = false;
    //    }
    //    else if (RBdefautpaymenttype.Checked == true)
    //    {
    //        divcreditcard.Visible = true;
    //        divpaypal.Visible = false;
    //        divdedault.Visible = false;
    //    }
    //    else if (RBdefautpaymenttype.Checked == true)
    //    {
    //        divcreditcard.Visible = true;
    //        divpaypal.Visible = false;
    //        divdedault.Visible = false;
    //    }
    //}

    }