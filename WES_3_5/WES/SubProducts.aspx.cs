using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
using System.Data;

public partial class SubProducts : System.Web.UI.Page
{
    decimal _ClarifyQty = 0;

    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ClrQty"] != null && Convert.ToDecimal(Request.QueryString["ClrQty"]) > 0)
        {
            _ClarifyQty = Convert.ToDecimal(Request.QueryString["ClrQty"]);
        }
        else
        {
            _ClarifyQty = 1;
        }
    }

    public string GetProducts()
    {
        string Retval = "";
        string _item = Request["Item"].ToString();
        string _cla_id = Request["cla_id"].ToString();
        //old string sSql = string.Format("select PRODUCT_ID, (SELECT replace(replace(STRING_VALUE, '_Images', '_TH50'),'\','/') FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.PRODUCT_ID AND ATTRIBUTE_ID = 7) IMG, (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.product_id AND ATTRIBUTE_ID = 1) CODE, (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.product_id AND ATTRIBUTE_ID = 449) [DESC] FROM ( select PRODUCT_ID from TB_PROD_SPECS WHERE ATTRIBUTE_ID = 450 and STRING_VALUE in ( select PRODUCT_CODE from WESTB_PROD_SORTKEY where SORTKEY = '{0}')) prod1 ", _item);
        //string sSql = string.Format("SELECT TP.PRODUCT_ID, TP.IMG, TP.CODE, TP.[DESC], TPF.FAMILY_ID FROM(select PRODUCT_ID, (SELECT replace(replace(STRING_VALUE, '_Images', '_TH50'),'','/') FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.PRODUCT_ID AND ATTRIBUTE_ID = 7)IMG, (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.product_id AND ATTRIBUTE_ID = 1) CODE, (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = prod1.product_id AND ATTRIBUTE_ID = 449) [DESC] FROM (select PRODUCT_ID from TB_PROD_SPECS WHERE ATTRIBUTE_ID = 450 and STRING_VALUE in ( select PRODUCT_CODE from WESTB_PROD_SORTKEY where SORTKEY = '{0}')) prod1) AS TP, TB_PROD_FAMILY TPF WHERE TP.PRODUCT_ID = TPF.PRODUCT_ID", _item);
        //objHelperServices.SQLString = sSql;
        string ProdImgPath = "/ProdImages";
        string str1 = "";

        //DataSet oDs = objHelperServices.GetDataSet("PRODUCTS");
        DataSet oDs = (DataSet)objHelperDB.GetGenericPageDataDB(_item, "GET_SUBPRODUCT_DETAILS", HelperDB.ReturnType.RTDataSet);

        if (oDs != null)
        {
            oDs.Tables[0].TableName = "PRODUCTS";
            Retval = "";
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            foreach (DataRow oDr in oDs.Tables["PRODUCTS"].Rows)
            {

                FileInfo Fil = new FileInfo(strFile + oDr["IMG"].ToString());
                if (Fil.Exists)
                {
                    Retval += string.Format("<tr><td><img src=\"{0}/{1}\" class=\"tx_img\" /></td>", ProdImgPath, oDr["IMG"].ToString().Replace("\\", "/"));

                }
                else
                {
                    Retval += string.Format("<tr><td><img src=\"{0}/{1}\" class=\"tx_img\" /></td>", ProdImgPath, "images/noimage.gif");
                }

                //Retval += string.Format("<tr><td><img src=\"{0}/{1}\" class=\"tx_img\" /></td>", ProdImgPath, oDr["IMG"]);
                Retval += string.Format("<td>{0}</td><td>{1}</td>", oDr["CODE"], oDr["DESC"]);

                if ((Session["ORDER_ID"] == null || Session["ORDER_ID"].ToString().Equals("")) && Request.QueryString["OrderID"] == null)
                {
                    str1 = "0";
                }
                else
                {
                    str1 = Request.QueryString["OrderID"].ToString();
                }

                if (Request.QueryString["Tempid"] != null)
                {
                    str1 = Request.QueryString["Tempid"].ToString();
                }
                else
                {
                    str1 = Request.QueryString["OrderID"].ToString();
                }

                if (Request.QueryString["Tempid"] != null)
                    Retval += string.Format("<td class=\"\"><a style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" href=\"OrderTemplate.aspx?pid={0}&Qty=" + _ClarifyQty + "&bulkorder=1&rma=CI&DelQty=" + _ClarifyQty + "&cla_id=" + _cla_id + "&Item={1}&Tempid={2}\">Update Order Template</a></td>", oDr["PRODUCT_ID"], _item, str1);
                else
                    Retval += string.Format("<td class=\"\"><a style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" href=\"OrderDetails.aspx?pid={0}&Qty=" + _ClarifyQty + "&bulkorder=1&rma=CI&DelQty=" + _ClarifyQty + "&cla_id=" + _cla_id + "&Item={1}&ORDER_ID={2}\">Update Order</a></td>", oDr["PRODUCT_ID"], _item, str1);
                //Retval += string.Format("<td class=\"tx_7_blue1\"><a href=\"ProductDetails.aspx?pid={0}&fid={1}\">More Info</a></td></tr>", oDr["PRODUCT_ID"], oDr["FAMILY_ID"]);

                //Retval += string.Format("<td class=\"tx_7_blue1\"><a href=\"Powersearch.aspx?srctext={0}\">More Info</a></td></tr>", oDr["CODE"]);

            }
        }
        return Retval;
    }



}