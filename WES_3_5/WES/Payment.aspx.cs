using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GCheckout.Util;
using System.Net;
using System.IO;
using TradingBell.WebCat;
using GCheckout.Checkout;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class payment : System.Web.UI.Page
{
    #region "Declarations"

    HelperServices objHelperServices = new HelperServices();
    UserServices objUserServices = new UserServices();
    OrderServices objOrderServices = new OrderServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB oCon = new ConnectionDB();
    int OrderID = 0;
    int UsrID;
    double SubTotal = 0.0;
    int OrdStatus = 0;
    int usrManulSataus = (int)UserServices.UserStatus.MANUALVERIFY;

    #endregion "Declarations"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            DataSet dsPayOPtions = new DataSet();

            QuoteServices objQuoteServices = new QuoteServices();
            decimal CCAmount;
            if (Session["USER_NAME"] == null)
            {
                Session["USER"] = "";
                Session["COUNT"] = "0";
                Response.Redirect("Login.aspx");
            }
            if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
            {
                lblShoppingCart.Text = "Quote Cart";
            }

            UsrID = objHelperServices.CI(Session["USER_ID"].ToString());
            dsPayOPtions = objUserServices.GetUserPaymentOptions(UsrID);
            OrderID = objHelperServices.CI(Request["OrdId"].ToString());
            bool POOption = false;
            bool CCOption = false;
            bool CODOption = false;
            CCAmount = objOrderServices.GetOrderTotalCost(OrderID);

            if (dsPayOPtions != null)
            {
                POOption = (bool)dsPayOPtions.Tables[0].Rows[0].ItemArray[0];
                CCOption = (bool)dsPayOPtions.Tables[0].Rows[0].ItemArray[1];
                CODOption = (bool)dsPayOPtions.Tables[0].Rows[0].ItemArray[2];
            }

            if (!IsPostBack)
            {
                //Load payment type in dropdown list.
                cmdPaymentOptions.Items.Clear();
                cmdPaymentOptions.Items.Add((string)GetLocalResourceObject("Select"));
                cmdPaymentOptions.Items[0].Value = "Select";
                if (CODOption == true)
                {
                    cmdPaymentOptions.Items.Add((string)GetLocalResourceObject("PayType1"));
                    cmdPaymentOptions.Items[1].Value = "COD";
                }

                if (CCOption == true)
                {
                    if (objHelperServices.GetOptionValues("CARTTYPE").ToString() != "....")
                    {
                        cmdPaymentOptions.Items.Add((string)GetLocalResourceObject("PayType2"));
                        if (CODOption == false)
                        {
                            cmdPaymentOptions.Items[1].Value = "CC";
                            cmdPaymentOptions.Items.FindByValue("CC").Selected = true;
                        }
                        else
                        {
                            cmdPaymentOptions.Items[2].Value = "CC";
                        }
                    }
                }
                if (Request["CSelType"] != null)
                {
                    cmdPaymentOptions.Items.FindByText("Credit card").Selected = true;
                }
                if (objHelperServices.GetOptionValues("GOOGLECHECKOUT ENABLED").ToString() == "YES")
                {
                    btnGCheckout.Visible = true;
                }
            }

            lblPONumber.Visible = false;
            txtPoNo.Visible = false;
            lblPOReleaseNumber.Visible = false;
            txtPoRNo.Visible = false;
            lblPaymentType.Visible = false;
            cmdPaymentOptions.Visible = false;
            lblNoPayoptions.Visible = false;

            if (CODOption == true && CCOption == false)
            {
                btnNext.Visible = true;
                cmdPaymentOptions.Visible = true;
                lblPONumber.Visible = false;
            }
            else if (CODOption == false && CCOption == true)
            {
                lblNoPayoptions.Visible = false;
                btnGCheckout.Visible = false;
                cmdPaymentOptions.Visible = false;
                lblPONumber.Visible = false;
            }
            else
            {
                txtPoNo.Enabled = true;
                txtPoRNo.Enabled = true;
            }

            if (CCOption == true)
            {
                if (CCOption == true & CODOption == true)
                {
                    btnNext.Visible = true;
                }
            }
            if (POOption == false && CCOption == false && CODOption == false)
            {

                lblNoPayoptions.Visible = true;
                btnNext.Visible = false;
                btnNext.Visible = false;
                btnGCheckout.Visible = false;
            }
            else
            {
                if (POOption == true)
                {
                    lblCardDetails.Visible = true;
                    lblPONumber.Visible = true;
                    txtPoNo.Visible = true;
                    lblPOReleaseNumber.Visible = true;
                    txtPoRNo.Visible = true;
                    btnGCheckout.Visible = false;
                    btnNext.Visible = true;
                    if (CCOption == true)
                    {
                        cmdPaymentOptions.Visible = false;
                        lblPaymentType.Visible = false;
                        btnGCheckout.Visible = false;
                    }
                    else if (CODOption == true)
                    {
                        lblPaymentType.Visible = true;
                        cmdPaymentOptions.Visible = true;
                        btnNext.Visible = true;
                        btnGCheckout.Visible = false;
                    }
                }
                if (CODOption == true && POOption == false && CCOption == false)
                {
                    Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
                    decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
                    oPayInfo.PayResponse = "";
                    oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
                    oPayInfo.OrderID = OrderID;
                    oPayInfo.PONumber = objHelperServices.Prepare(txtPoNo.Text);
                    oPayInfo.PORelease = objHelperServices.Prepare(txtPoRNo.Text);
                    oPayInfo.Amount = TotCost;
                    oPayInfo.UserId = UsrID;
                    Response.Redirect("OrderReview.aspx?OrdId=" + OrderID + "&ViewType=REVIEW", false);
                    Session["PAYMENTINFO"] = oPayInfo;
                }
                else if (CCOption == true && CODOption == false)
                {
                    if (CCOption == true)//Gog
                    {
                        btnNext.Visible = false;
                        lblCardDetails.Visible = true;
                        btnGCheckout.Visible = false;
                        if (POOption == true)
                        {
                            btnNext.Visible = true;
                            lblCardDetails.Visible = true;
                        }
                    }
                    else
                    {
                        if (POOption == true)
                            btnNext.Visible = true;
                    }
                }
                else if (CODOption == true && CCOption == false)
                {
                    cmdPaymentOptions.Visible = true;
                    lblPaymentType.Visible = true;
                    btnNext.Visible = true;
                }
                else if (CODOption == false && CCOption == true)
                {
                    lblNoPayoptions.Visible = false;
                    btnGCheckout.Visible = false;
                    cmdPaymentOptions.Visible = false;
                    lblPONumber.Visible = false;
                }
                else if (CODOption == true && CCOption == true)
                {
                    btnNext.Visible = false;
                    lblPaymentType.Visible = true;
                    cmdPaymentOptions.Visible = true;
                    btnGCheckout.Visible = false;
                }
            }
            //For Using Enter Key
            txtPoNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnNext.UniqueID + "').click();return false;}} else {return true}; ");
            txtPoRNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnNext.UniqueID + "').click();return false;}} else {return true}; ");

            if (objHelperServices.GetOptionValues("GOOGLECHECKOUT ENABLED").ToString() == "YES")
            {
                btnGCheckout.Visible = true;
            }
            if (cmdPaymentOptions.SelectedValue == "COD")
            {
                btnNext.Visible = true;
                txtPoNo.Enabled = true;
                txtPoRNo.Enabled = true;
            }
            else if (cmdPaymentOptions.SelectedValue == "CC")
            {
                lblNoPayoptions.Visible = false;
                btnGCheckout.Visible = false;
            }
            else
            {
                txtPoNo.Enabled = true;
                txtPoRNo.Enabled = true;
            }

            #region "Meta Tags"
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
            #endregion
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #region "Controls Events"

    protected void btnGCheckout_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
        int nQty = 0;
        decimal UntPrice = 0;
        string CatalogItemNo = "";
        string ProductDescription = "";
        double TaxRate = 0.00;
        string sRedirectUrl = "http://webdev/webcat/default.aspx";
        CheckoutShoppingCartRequest Req = btnGCheckout.CreateRequest();
        DataSet dsOrderItm = objOrderServices.GetOrderItems(OrderID);
        if (dsOrderItm != null)
        {
            foreach (DataRow oRow in dsOrderItm.Tables[0].Rows)
            {
                CatalogItemNo = oRow["CATALOG_ITEM_NO"].ToString();
                UntPrice = objHelperServices.CDEC(oRow["PRICE_EXT_APPLIED"].ToString());
                nQty = objHelperServices.CI(oRow["QTY"].ToString());
                Req.AddItem(CatalogItemNo, ProductDescription, UntPrice, nQty);
                Req.AddCountryTaxRule(GCheckout.AutoGen.USAreas.ALL, TaxRate, true);
            }
            Req.ContinueShoppingUrl = sRedirectUrl;
            GCheckoutResponse Resp = Req.Send();            
            Response.Redirect(Resp.RedirectUrl, true);
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnProceed_Click(object sender, EventArgs e)
    {

        try
        {
        Session["PAYMENT_TYPE"] = PaymentServices.PaymentType.CODPayment;
        decimal TotCost = objHelperServices.CDEC(objOrderServices.GetOrderTotalCost(OrderID));
        oPayInfo.PayResponse = "";
        oPayInfo.PaymentType = PaymentServices.PaymentType.CODPayment;
        oPayInfo.OrderID = OrderID;
        oPayInfo.PONumber = objHelperServices.Prepare(txtPoNo.Text);
        oPayInfo.PORelease = objHelperServices.Prepare(txtPoRNo.Text);
        oPayInfo.Amount = TotCost;
        oPayInfo.UserId = UsrID;
        if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
        {
            Response.Redirect("OrderReview.aspx?OrdId=" + OrderID + "&ViewType=REVIEW&QteFlag=1", false);
        }
        else
        {
            Response.Redirect("OrderReview.aspx?OrdId=" + OrderID + "&ViewType=REVIEW", false);
        }
        Session["PAYMENTINFO"] = oPayInfo;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void cmdPaymentOptions_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
        if (cmdPaymentOptions.SelectedValue == "COD")
        {
            btnNext.Visible = true;
        }
        else if (cmdPaymentOptions.SelectedValue == "CC")
        {
            if (objHelperServices.GetOptionValues("GOOGLECHECKOUT ENABLED").ToString() == "YES")
            {
                btnGCheckout.Visible = true;
                btnNext.Visible = false;
            }
        }

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #endregion
}