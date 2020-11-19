using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Braintree;
using System.Web.Configuration;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using System.Text;
using System.Data;
using TradingBell.WebCat.Helpers;

namespace WES
{
    public partial class OnlineInvoicePayment : System.Web.UI.Page
    {


        public string ClientToken = string.Empty;
        public string NAME_ONCARD = string.Empty;
        public string CARD_NO = string.Empty;
        public string  CARD_EXPIRYDATE = string.Empty;
        Security objSecurity = new Security();
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateClientToken();
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
                if (Request.QueryString["payref"] != null)
                {
                   
                    int invoiceno = Convert.ToInt32(Request.QueryString["payref"].ToString()); /*Request.QueryString["invoiceno"].ToString();*/
                    
                    string strSQL = "Exec STP_TBWC_PICK_ONLINE_PAYMENT " + invoiceno + ",'Select'";
                    HelperDB objHelperDB = new HelperDB();
                    DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

                    if (dsCables != null && dsCables.Tables[0].Rows.Count > 0)

                    {
                       
                        lblinvoice.Text = dsCables.Tables[0].Rows[0]["INVOICENO"].ToString();
                        Session["InvoiceNo"] = lblinvoice.Text;
                        lblorder.Text = dsCables.Tables[0].Rows[0]["CS_ORDER_REF"].ToString();
                        lblamount.Text = dsCables.Tables[0].Rows[0]["AMOUNT_CHARGED"].ToString();
                        lblinvoicetotal.Text = dsCables.Tables[0].Rows[0]["AMOUNT_CHARGED"].ToString();
                        if(dsCables.Tables[0].Rows[0]["NAME_ONCARD"].ToString()!="")
                        {
                        NAME_ONCARD = dsCables.Tables[0].Rows[0]["NAME_ONCARD"].ToString();
                        NAME_ONCARD = objSecurity.StringDeCrypt(NAME_ONCARD);
                        }
                        ////CARD_NO = dsCables.Tables[0].Rows[0]["CARD_NO"].ToString();
                        ////CARD_NO = objSecurity.StringDeCrypt(CARD_NO);
                        ////CARD_EXPIRYDATE = dsCables.Tables[0].Rows[0]["CARD_EXPIRYDATE"].ToString();
                        ////CARD_EXPIRYDATE = objSecurity.StringDeCrypt(CARD_EXPIRYDATE);
                        if (dsCables.Tables[0].Rows[0]["Approved"].ToString().ToUpper() == "YES")
                        {
                            lblerror.Text = "This invoice has been already been paid.";
                            divcaption.Style.Add("display", "none");
                            loading.Visible = false;
                            imgerr.Visible = true;
                            divleft.Style.Add("display", "block");
                        }
                        else
                        {
                            this.ClientToken = gateway.ClientToken.Generate();
                            lblerror.Text = "";
                            imgerr.Visible = false;
                            divcaption.Style.Add("display", "block");
                            loading.Visible = true;
                            divpaidmsg.Visible = false;
                            divleft.Style.Add("display", "block");
                        }
                    }
                    else
                    {

                        lblerror.Text = "Invalid Request";
                        divcaption.Style.Add("display", "none");
                        loading.Visible = false;
                        imgerr.Visible = false;
                        divleft.Style.Add("display", "none");
                    }
                }
                else {

                    lblerror.Text = "Invalid Request";
                    divcaption.Style.Add("display", "none");
                    loading.Visible = false;
                    imgerr.Visible = false;
                    divleft.Style.Add("display", "none");
                }
                //var creditCardRequest = new CreditCardRequest
                //{
                //    CustomerId = NAME_ONCARD,
                //    Number = CARD_NO,
                //    ExpirationDate = "06/22",
                //    CVV = "100"
                //};

                //CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
                //     objErrorHandler.CreateLogEA("clientToken:" + this.ClientToken);
            }
            catch (Exception ex)
            {

               // objErrorHandler.CreateLog(ex.ToString());
            }
        }

        [System.Web.Services.WebMethod]
        public static string SaleTrans(string nounce, string Amount)
        {
            string x = "";
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "mjff7p7mgb4qmp77",
                PublicKey = "h673fc8hc4v7pqh4",
                PrivateKey = "92c877d009ac2dc337a38fd5737301e3"
            };


        

         
            if (HttpContext.Current.Session["Invoiceno"] !=null   )

                if (nounce == "no")
                {
                    x = "Error " + "Please Try again or use a different card / payment method.";
                   // objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, "", "", "", "No Nounce", "", "No", "", "No Nounce", "", "", "Error Processing PARes");
                    //  objerrhandler.CreateLogEA(" br  Orderid=" + OrderID + "Error Processing PARes");

                    return x;
                }


                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(Amount),

                    PaymentMethodNonce = nounce,

                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    },
                    OrderId = HttpContext.Current.Session["Invoiceno"].ToString() 

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

                    string cardtype = result.Target.CreditCard.CardType.ToString();


                    HelperDB objHelper = new HelperDB();
                    string sSQL = "Exec STP_TBWC_PICK_ONLINE_PAYMENT '" + HttpContext.Current.Session["Invoiceno"].ToString() + "'";
                    sSQL = sSQL + ",'Update','"+ cardtype +"','" + result.Target.CreditCard.CardholderName  + "','" + result.Target.CreditCard.MaskedNumber + "','" + result.Target.CreditCard.ExpirationDate + "','" + ResponseId + "',";
                    sSQL = sSQL + "'" + Responsecode + "','"+ ResponseText + "', '"+ LiabilityShifted.ToString() +"','"+ Status +"','Yes',";
                    sSQL = sSQL + "'1','BR'";
                    int   rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                    if (rtnvalue <= 0)
                    {
                      return   "Unable to Create Request Data";
                    }


                    ////   objErrorHandler.CreatePayLog("SP before SendMail_AfterPaymentSP  Orderid=" + OrderID);
                    //tbw tbwtEngine = new TBWTemplateEngine();
                    //tbwtEngine.SendMail_AfterPaymentSP(intOrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success, false, result.Target.CreditCard.CardType.ToString());
                    //   tbwtEngine.SendMail_Review(intOrderID, (int)OrderServices.OrderStatus.Payment_Successful, false);
                    HttpContext.Current.Session["InvoiceNo"] = "0";
                    


                        return "Sucess";

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
                        ErrorHandler objErrorHandler = new ErrorHandler();
                      
                       
                    HelperDB objHelper = new HelperDB();
                    string sSQL = "Exec STP_TBWC_PICK_ONLINE_PAYMENT '" + HttpContext.Current.Session["Invoiceno"].ToString() + "'";
                    sSQL = sSQL + ",'Update','" + transaction.CreditCard.CardType  + "'," + transaction.CreditCard.CardholderName + "," + transaction.CreditCard.MaskedNumber + "," + transaction.CreditCard.ExpirationDate + ",'"+ transaction.Id +"',";
                    sSQL = sSQL + "'" + transaction.NetworkResponseCode + "','" + transaction.NetworkResponseText + "', '" + LiabilityShifted.ToString() + "','" + Status + "','No'";
                    int rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);

                    x = "Error " + errorMessages;
                    }


                    return x;
                }
                else
                {
                    //   objPRInfo = objSecurePayService.GetPaymentRequest_braintree(intOrderID, PaymentID, transaction.CreditCard.CardType.ToString(), transaction.CreditCard.CardholderName, transaction.CreditCard.MaskedNumber, transaction.CvvResponseCode, transaction.CreditCard.ExpirationDate, "No", transaction.ProcessorResponseCode, transaction.ProcessorResponseText, transaction.Id, transaction.NetworkResponseCode, transaction.NetworkResponseText);
                    x = "Error " + "Please Try again or use a different card / payment method.";
                HelperDB objHelper = new HelperDB();
                string sSQL = "Exec STP_TBWC_PICK_ONLINE_PAYMENT '" + HttpContext.Current.Session["Invoiceno"].ToString() + "'";
                sSQL = sSQL + ",'Update','','','','','',";
                sSQL = sSQL + "'','', '" + LiabilityShifted.ToString() + "','" + Status + "','No'";
                int rtnvalue = objHelper.ExecuteSQLQueryDB(sSQL);
                return x;
                }

            }
           
        }
            
       

          
}