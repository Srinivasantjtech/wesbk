using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class Compare : System.Web.UI.Page
{

    public string[] ProdID = new string[10];
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    //CSRender oCsrender = new CSRender();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();       
        if (Session["CloseWin"] != null)
        {
            ProdID = Regex.Split(Session["CloseWin"].ToString(), ",");
        }
        else
        {
            Response.Redirect("home.aspx", false);
        }
    }

    public string ST_Compare()
    {
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("COMPARE", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH")), objConnectionDB.ConnectionString);
        tbwtEngine.GDataSet = GetDataSet();
        tbwtEngine.RenderHTML("Column");
        return (tbwtEngine.RenderedHTML);
    }

    private DataSet GetDataSet()
    {

        try
        {
        DataSet ds = new DataSet();

        //string SQLQuery = " SELECT ta.attribute_name,ta.attribute_type,ta.attribute_datatype,'" + Session["Closelos"].ToString() + "' as category_id,ps.* FROM tb_product tp,TB_PROD_SPECS ps,tb_attribute ta " +
        //                  " WHERE ps.PRODUCT_ID IN (" + Session["CloseWin"].ToString() + ")  and ta.attribute_id=ps.attribute_id and tp.product_id=ps.product_id "; 

        //SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
        //da.Fill(ds, "generictable");       
        ds =(DataSet)objHelperDB.GetGenericPageDataDB("", Session["Closelos"].ToString(), Session["CloseWin"].ToString(), "GET_COMPARE_ATTRIBUTE", HelperDB.ReturnType.RTDataSet);


        for (int len = 0; len < ProdID.Length; len++)
        {
            DataSet dsattr = new DataSet();
            //SQLQuery = " select distinct ps.attribute_id, ta.attribute_name,ta.attribute_type,ta.attribute_datatype,'" + Session["Closelos"].ToString() + "' as category_id from tb_product tp,tb_prod_specs ps,tb_attribute ta where (ps.product_id in (" + Session["CloseWin"].ToString() + ")" +
            //           " and ps.attribute_id not in (select attribute_id from tb_prod_specs where product_id in (" + ProdID[len] + "))) and ta.attribute_id=ps.attribute_id and tp.product_id=ps.product_id";
            //da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
            //da.Fill(dsattr, "generictable");
            dsattr = (DataSet)objHelperDB.GetGenericPageDataDB("", Session["Closelos"].ToString(), Session["CloseWin"].ToString(), ProdID[len].ToString() , "GET_COMPARE_ATTRIBUTE_PRODUCT", HelperDB.ReturnType.RTDataSet);
            foreach(DataRow dr in dsattr.Tables[0].Rows)
            {
                DataRow newrow = ds.Tables[0].NewRow();
                newrow["product_id"] = Convert.ToInt32(ProdID[len].ToString());
                newrow["attribute_id"] = dr["attribute_id"];
                newrow["attribute_name"] = dr["attribute_name"];
                newrow["attribute_type"] = dr["attribute_type"];
                newrow["attribute_datatype"] = dr["attribute_datatype"];
                newrow["category_id"] = dr["category_id"];
                ds.Tables[0].Rows.Add(newrow);
            }
        }
        return ds;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }
    }


}
