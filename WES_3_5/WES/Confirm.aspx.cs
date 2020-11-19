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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class Confirm : System.Web.UI.Page
{
    #region "Declarations"
    int OrderId = 0;
    HelperServices objHelperServices = new HelperServices();
    OrderServices objOrderServices = new OrderServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    DataSet ds = new DataSet();
    OrderServices.OrderInfo oOI = new OrderServices.OrderInfo();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].Equals("View")))
            {
                Session["PageUrl"] = "orderDetails.aspx?bulkorder=1&Pid=0&ORDER_ID=" + Session["ORDER_ID"];
            }
            else
            {
                Session["PageUrl"] = "orderDetails.aspx?bulkorder=1&Pid=0";
            }

            if (!IsPostBack)
            {
                int UsrStatus = (int)UserServices.UserStatus.ACTIVE;
                if (Session["USER_ID"] != "")
                {
                    if (Request["ViewType"] == "Confirm")
                    {
                        decimal CCTotal = objOrderServices.GetOrderTotalCost(objHelperServices.CI(Request["OrdId"]));
                        string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                        string ConMsg = "";
                        oPayInfo = objPaymentServices.GetPayment(objHelperServices.CI(Request["OrdId"]));

                        if (oPayInfo.PaymentType.ToString() == "CODPayment")
                        {
                            //lblConfirm1.Text = GetLocalResourceObject("CashOnDelivery").ToString();
                            //lblConfirmValue.Text = " " + CurSymbol + CCTotal + " ";
                            lblConfirm2.Text = GetLocalResourceObject("ConfirmMsg2").ToString();
                        }
                        else if (oPayInfo.PaymentType.ToString() == "CCPayment")
                        {
                            if (objUserServices.GetUserStatus(objHelperServices.CI(Session["USER_ID"].ToString())) == UsrStatus)
                            {
                                //lblConfirm1.Text = GetLocalResourceObject("ConfirmMsg1").ToString();
                                //lblConfirmValue.Text = " " + CurSymbol + CCTotal + " ";
                                lblConfirm2.Text = GetLocalResourceObject("ConfirmMsg2").ToString();
                            }
                            else
                            {
                                ConMsg = GetLocalResourceObject("ConfirmVerify").ToString();
                                lblConfirm1.Text = ConMsg;
                            }

                        }
                        else
                        {
                            ConMsg = GetLocalResourceObject("ConfirmVerify").ToString();
                            lblConfirm1.Text = ConMsg;
                        }

                        lblPageHead.Visible = false;
                    }
                    else
                    {
                        lblBill.Visible = false;
                        lblCheck.Visible = false;
                        lblConfirm1.Visible = false;
                        lblReviewOrder.Visible = false;
                        lblShip.Visible = false;
                        lblShoppingCart.Visible = false;
                        lblPageHead.Visible = true;
                    }
                    if (Request["QteFlag"] != null && Request["QteFlag"].ToString() == "1")
                    {
                        lblShoppingCart.Text = "Quote Cart >";
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
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
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected override void OnInit(EventArgs e)
    {
        OrderId = objHelperServices.CI(Request["OrdId"]);
        string Scr = @"<script type='text/javascript'>
            function OpenInvoice()
             {
                var strReturn = window.open('PrintInvoice.aspx?OrdId=" + OrderId + @"&','WESAUSTRALASIA','');                                        
            }           
           </script>";
        Page.RegisterClientScriptBlock("OpenInvoice", Scr);
        base.OnInit(e);
    }
}
