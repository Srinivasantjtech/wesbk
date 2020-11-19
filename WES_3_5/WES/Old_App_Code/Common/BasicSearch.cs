using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;


//[assembly: TagPrefix("TradingBell.Common", "WebCat")]
//[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.Common
{
    /// <summary> 
    ///  This is used to search the text in the WebCat Engine Products and its Description.   
    /// <para>
    /// Search the Text by executing the Store Procedure stp_tbwc_search.
    /// </para>    
    /// </summary>  
    /// <remarks>
    /// Used to Search the Text.
    /// It executes the Stored Procedure and the results are stored in DataSet.
    /// </remarks>
      
    
    public class BasicSearch
    {

        Helper oHelper = new Helper();
        DataSet oDs = new DataSet();
        ErrorHandler oErr = new ErrorHandler();

        /// <summary> 
        /// This is used to search the text in the WebCat Engine Products and its Description.               
        /// It executes the Stored Procedure and the results are stored in DataSet.
        /// </summary> 
        /// <param name="SearchText">string</param>
        /// <param name="Userid">int</param>
        /// 
        /// <returns>
        /// Returns the Dataset for search Results.
        /// </returns>
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
        ///     string SearchText;
        ///     int Userid;
        ///     BasicSearch oBS = new BasicSearch(SearchText,Userid);
        /// }
        /// </code>
        /// </example>
        ///

        public BasicSearch(string SearchText, int Userid, int CatalogID, int ProductAttributeId,int ProductImageAttributeID)
        {

            string strCMD;
            //int CatId =oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
            if (oHelper.GetOptionValues("ENABLED CUSTOM PRICE").ToString() == "YES")
            {
                strCMD = "exec stp_tbwc_search '" + SearchText + "','LOW','SIMPLE',1,'YES'," + CatalogID + "," + Userid + "," + ProductAttributeId + "," + ProductImageAttributeID;

            }
            else
            {
                strCMD = "exec stp_tbwc_search '" + SearchText + "','LOW','SIMPLE',1,'NO'," + CatalogID + "," + Userid + "," + ProductAttributeId + "," + ProductImageAttributeID;
            }

            if (oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString() == "YES")
            {
                strCMD = strCMD + ",'YES'";
            }
            else
            {
                strCMD = strCMD + ",'NO'";
            }

            string CatName = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =2";
            string sMFGName = oHelper.GetValue("ATTRIBUTE_NAME").ToString();
            oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =3";
            string dPrice = oHelper.GetValue("ATTRIBUTE_NAME").ToString();
            oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =4";
            string sSupp = oHelper.GetValue("ATTRIBUTE_NAME").ToString();

            oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID =" + ProductAttributeId;
            string CustomAttrName = oHelper.GetValue("ATTRIBUTE_NAME").ToString();

            oHelper.SQLString = strCMD;
            oDs = oHelper.GetDataSet();
            if (oDs != null)
            {
                oDs.Tables[0].Columns["Category_ID"].ColumnMapping = MappingType.Hidden;
                oDs.Tables[0].Columns["CATEGORY_NAME"].Caption = "CATEGORY";
                oDs.Tables[0].Columns["FAMILY_NAME"].Caption = "FAMILY";
                oDs.Tables[0].Columns["CATALOG_ITEM_NO"].Caption = "ITEM NO";
                oDs.Tables[0].Columns["MFG_PART_NO"].Caption = sMFGName;
                oDs.Tables[0].Columns["SUPPLIER_NAME"].Caption = sSupp;
                oDs.Tables[0].Columns["MIN_ORD_QTY"].Caption = "MIN QTY";
                if (ProductAttributeId > 0)
                {
                    oDs.Tables[0].Columns["FEATURE"].Caption = CustomAttrName;
                }

                //oDs.Tables[0].Columns["FAMILY_NAME"].Caption = "PRODUCT";
                oDs.Tables[0].Columns["Family_ID"].ColumnMapping = MappingType.Hidden;
                oDs.Tables[0].Columns["MIN_ORD_QTY"].ColumnMapping = MappingType.Hidden;
                oDs.Tables[0].Columns["IS_SHIPPING"].ColumnMapping = MappingType.Hidden;
                oDs.Tables[0].Columns["MFG_PART_NO"].ColumnMapping = MappingType.Hidden;
                //oDs.Tables[0].Columns["SUPPLIER_NAME"].ColumnMapping = MappingType.Hidden;
                // if (ProductAttributeId == 0)
                // {
                //    oDs.Tables[0].Columns["FEATURE"].ColumnMapping = MappingType.Hidden;
                // }
                if (oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                {
                    oDs.Tables[0].Columns["RESTRICTED"].ColumnMapping = MappingType.Hidden;
                }


                // oDs.Tables[0].Columns["-CART-"].ColumnMapping = MappingType.Hidden;
            }
        }

        /// <summary>
        /// Retrives the Dataset Results(Search Results)
        /// </summary>
        /// 
        public DataSet GetResult
        {
            get
            {
                return oDs;
            }

        }
        /// <summary>
        /// Retrives the Cart Position the Search Result Page
        /// </summary>
        ///
        public int GetCartPosition
        {
            get
            {
                return oDs.Tables[0].Columns.IndexOf("-CART-");
            }
        }

    }
}