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
    public partial class BR : System.Web.UI.Page
    {
        public string ClientToken = string.Empty;
        public string NAME_ONCARD = string.Empty;
        public string CARD_NO = string.Empty;
        public string CARD_EXPIRYDATE = string.Empty;
        Security objSecurity = new Security();
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateClientToken();
            lblinvoice.Text = "inv656";
            lblamount.Text = "100";
            lblinvoicetotal.Text = "100";
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
                int invoiceno = 900900; /*Request.QueryString["invoiceno"].ToString();*/
                Session["InvoiceNo"] = 900900;
                //string strSQL = "Exec STP_TBWC_PICK_ONLINE_PAYMENT " + invoiceno + ",'Select'";
                //HelperDB objHelperDB = new HelperDB();
                //DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

                //if (dsCables != null && dsCables.Tables[0].Rows.Count > 0)
                //{

                //    lblinvoice.Text = dsCables.Tables[0].Rows[0]["INVOICENO"].ToString();
                //    lblorder.Text= dsCables.Tables[0].Rows[0]["CS_ORDER_REF"].ToString();
                //    lblamount.Text = dsCables.Tables[0].Rows[0]["AMOUNT_CHARGED"].ToString();
                //    lblinvoicetotal.Text = dsCables.Tables[0].Rows[0]["AMOUNT_CHARGED"].ToString();

                //     NAME_ONCARD = dsCables.Tables[0].Rows[0]["NAME_ONCARD"].ToString();
                //    NAME_ONCARD = objSecurity.StringDeCrypt(NAME_ONCARD);

                //     CARD_NO = dsCables.Tables[0].Rows[0]["CARD_NO"].ToString();
                //    CARD_NO= objSecurity.StringDeCrypt(CARD_NO);
                //     CARD_EXPIRYDATE = dsCables.Tables[0].Rows[0]["CARD_EXPIRYDATE"].ToString();
                //    CARD_EXPIRYDATE = objSecurity.StringDeCrypt(CARD_EXPIRYDATE);
                //}
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
    }
}