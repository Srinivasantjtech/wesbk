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
using System.Collections.Generic;
//using System.Windows.Forms;
using TradingBell.WebCat;
using GCheckout.Checkout;
using GCheckout.Util;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.TemplateRender;
public partial class ProductFinder : System.Web.UI.Page
{

    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();

    protected void Page_PreInit()
    {
      
      //  Page.MasterPageFile = "~/AddtoCardPopup.Master";
       
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        Page.Title = "Product Finder";
    }

    [System.Web.Services.WebMethod]
    public static string SetViewType(string viewtype)
    {



        HttpContext.Current.Session["PL_VIEW_MODE"] = viewtype;

        return "";
    }
    [System.Web.Services.WebMethod]
    public static string FindCableLeftRightImages(string strvalue, string strCable1value, string strconnector_type)
    {
        try
        {
            string  tempEa="";
          
            if (strconnector_type == "")
                strconnector_type = "Cables";
            

            if (strCable1value == "")
            {
                if (HttpContext.Current.Session["CableL"] == null || HttpContext.Current.Session["CableL"] == "")
                {
                    UI_ProductFinder Profi = new UI_ProductFinder();
                    Profi.FindCableL("");
                }
                if (HttpContext.Current.Session["CableL"] != null)
                {
                    TBWTemplateRenderProductFinder objTBWTemplateRenderProductFinder = new TBWTemplateRenderProductFinder();

                    return objTBWTemplateRenderProductFinder.ST_CableLImage_Load((DataTable)HttpContext.Current.Session["CableL"], strvalue, strconnector_type);
                }
            }
            else if (strCable1value != "")
            {
                string Ea = "AllProducts////WESAUSTRALASIA";

                if (HttpContext.Current.Session["CableLR"] == null || HttpContext.Current.Session["Cable_images"] == "")
                {
                    UI_ProductFinder Profi = new UI_ProductFinder();

                    Profi.FindCableLR("", strCable1value);
                }                
                if (HttpContext.Current.Session["CableLR"] != null && HttpContext.Current.Session["Cable_images"] != null)
                {
                    TBWTemplateRenderProductFinder objTBWTemplateRenderProductFinder = new TBWTemplateRenderProductFinder();

                    tempEa = Ea + "////AttribSelect=CableL = '" + strCable1value + "'";

                    return objTBWTemplateRenderProductFinder.ST_CableRImage_Load((DataTable)HttpContext.Current.Session["CableLR"], strCable1value, tempEa, strvalue, strconnector_type);
                }

            }
            
            return "";
            
        }
        catch(Exception ex)
        {
            return "";
        }

    }
    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string ViewMode, string irecords)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            try
            {
                string userid = "";
                if (HttpContext.Current.Session["USER_ID"] != null)
                {

                    userid = HttpContext.Current.Session["USER_ID"].ToString();
                    if (userid == "")
                    {
                        objHelperServices.CheckCredential();
                        if ((HttpContext.Current.Session["USER_ID"] == null) || (HttpContext.Current.Session["USER_ID"].ToString() == ""))
                        {

                            return "LOGIN";
                        }
                    }
                }
                else
                {

                    objHelperServices.CheckCredential();

                    if ((HttpContext.Current.Session["USER_ID"] == null) || (HttpContext.Current.Session["USER_ID"].ToString() == ""))
                    {


                        return "LOGIN";

                    }



                }
            }
            catch
            {

                return "LOGIN";
            }

            if (ipageno <= iTotalPages)
            {
                if (strvalue != null)
                {
                    //string x = HttpContext.Current.Request.Form["ctl00_Popupcontent_ProductFinder1_ProductFinderPL1_htmlviewmode"];
                     string val = strvalue;
                    //val = val.Replace("amp;", "");
                    //HttpContext.Current.RewritePath(val, false);
                    search_ProductFinderPL objnew = new search_ProductFinderPL();
                    System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                    eapath = eapath.Replace("###", "'");

                    getPostsText.Append(objnew.DynamicPagJson(val, ipageno, eapath, ViewMode, irecords));
                    return getPostsText.ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

        //HttpContext.Current.Session["stprodlist"] = getPostsText.ToString();
        // getPostsText.AppendFormat("<div style='height:15px;'></div>");


    }
}


