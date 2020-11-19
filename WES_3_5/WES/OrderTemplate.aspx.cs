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
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Configuration;
public partial class OrderTemplate : System.Web.UI.Page
{

   
        

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();        
    }

    [WebMethod]
    public static List<string> WestestAutoCompleteData(string strvalue)
    {
        List<string> result = new List<string>();
        ConnectionDB objConnectionDB = new ConnectionDB();
        OrderDB objOrderDB = new OrderDB();
       /* //SqlCommand objSqlCommand;
     
      //  using (objSqlCommand = new SqlCommand("select top 10 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection()));
            using (objSqlCommand = new SqlCommand("select top 15 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection())) ;
            {
               //objSqlCommand.CommandType = CommandType.Text;
                objSqlCommand.Parameters.Add("@STRING_VALUE", strvalue);
                SqlDataReader dr = objSqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr["STRING_VALUE"].ToString());
                }
                return result;
          //  }
        }
        * */
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        DataTable Tbl = (DataTable)objOrderDB.GetGenericDataDB(WesCatalogId, strvalue, "GET_BULK_ORDER_AUTO_COMPLETE_PRODUCT", OrderDB.ReturnType.RTTable);
        if (Tbl != null)
        {
            foreach (DataRow dr in Tbl.Rows)
            {
                result.Add(dr["ProductCode"].ToString());
            }            
        }
            
        return result;

    }  
}
