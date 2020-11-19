using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.Common
{

    /// <summary>
    /// This is Used to get the Family Layouts from the Table.
    /// </summary>
    /// <remarks>
    /// Get Family Layout Values.
    /// </remarks>
    /// <example>
    /// Layout oLay = new Layout();
    /// </example>
    public class Layout
    {
        Helper oHelper = new Helper();
        ErrorHandler oErr = new ErrorHandler();
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Layout()
        {
        }
        #region "Functions"
        /// <summary>
        /// This is used to get the Family Layouts from the Table and return the DataSet
        /// </summary>
        /// <returns>Dataset</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Layout oLay = new Layout();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oLay.GetFamilyLayout();
        /// }
        /// </code>
        /// </example>
        public DataSet GetFamilyLayout()
        {
            try
            {
                string sSQL = "SELECT FAMILY_ID,LAYOUT,GROUPED_COLUMNS FROM TBWC_PROD_FAMILY_LAYOUT";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                // oErr.CreateLog();
                return null;
            }
        }

        #endregion
    }
}
