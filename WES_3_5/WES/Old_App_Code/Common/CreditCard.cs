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
using System.Net;
using System.IO;
using System.ComponentModel;

namespace TradingBell.Common
{
    /// <summary>
    /// Setting the Properties for the Credit Card Types
    /// </summary>
    /// <remarks>
    /// Used to get the runtime values from the User
    /// </remarks>

    public class CreditCard
    {
        CardType _CreditCardType;
        Response _ResponseTransaction;

        # region "Properties"
        /// <summary>
        /// Set the Enum Values for Card Type
        /// </summary>
        /// <example>
        /// CreditCard.CardType.Visa.ToString();
        /// </example>
        /// 
        public enum CardType
        {
            /// <summary>
            /// American Express offers individuals online access to its world-class Card,Financial, and Travel services
            /// </summary>
            AmericanExpress = 1,
            /// <summary>
            /// Discover offers individuals online access to its world-class Card
            /// </summary>
            Discover = 2,
            /// <summary>
            /// MasterCard Worldwide manages online access, widely accepted payment cards
            /// </summary>
            MasterCard = 3,
            /// <summary>
            /// Payment card used by people around the world, Visa cards offer convenience and reliability
            /// </summary>
            Visa = 4
        }
        /// <summary>
        /// Set the Enum Values for Credit Card Response 
        /// </summary>
        /// <example>
        /// CreditCard.Response.CardValid.ToString();
        /// </example>
        public enum Response
        {
            /// <summary>
            /// Credit Card is Not Valid(Invalid Card Number,Name,CSC Code..)
            /// </summary>
            CardInvalid = 3,
            /// <summary>
            /// UnAuthorized User
            /// </summary>
            CardDeclined = 2,
            /// <summary>
            /// Valid Card
            /// </summary>
            CardValid = 1
        }
        /// <summary>
        ///  Set the Property for getting Credit Card Types 
        /// (Visa,Master,American Express,Discovery)  
        /// </summary>        
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public CardType CreditCardType
        {
            get
            {
                return _CreditCardType;
            }
            set
            {
                _CreditCardType = value;
            }
        }
        /// <summary>
        /// Set the Property for Response Transaction
        /// </summary>
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public Response ResponseTransaction
        {
            get
            {
                return _ResponseTransaction;
            }
            set
            {
                _ResponseTransaction = value;
            }
        }
        # endregion
    }
}
