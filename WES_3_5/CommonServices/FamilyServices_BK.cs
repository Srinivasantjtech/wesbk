using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class FamilyServices
    {
        /*********************************** DECLARATION ***********************************/      
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperDB objHelperDb = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        Security objSecurity = new Security();
        DataSet DsPreview = new DataSet();
        HelperServices objHelperServices = new HelperServices();
        UserServices objUserServices = new UserServices();
        ProductServices objProductServices = new ProductServices();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
       // Order oOrder = new Order();
        //BuyerGroup oBG = new BuyerGroup();
        //Product oPro = new Product();
        string Restricted = "NO";
        int ProdID;
        //private int _familyID;
        private int _UserID;
        private int _catalogID;
        private bool _displayHeader;
        int pricecode = 0;
        public enum DefaultBG
        {
            /// <summary>
            /// Default Buyer Group
            /// </summary>
            DEFAULTBG = 1,
            /// <summary>
            /// Buyer Group default method is Percentage
            /// </summary>
            PERCENTAGEMETHOD = 2,
            /// <summary>
            /// Buyer Group default method is Amount
            /// </summary>
            AMOUNTMETHOD = 3
        }
        /*********************************** OLD CODE ***********************************/
        //public int FamilyID
        //{
        //    get
        //    {
        //        return _familyID;
        //    }
        //    set
        //    {
        //        _familyID = value;
        //    }
        //}
        //public int UserID
        //{
        //    get
        //    {
        //        return _UserID;
        //    }
        //    set
        //    {
        //        _UserID = value;
        //    }
        //}
        //public int CatalogID
        //{
        //    get
        //    {
        //        return _catalogID;
        //    }
        //    set
        //    {
        //        _catalogID = value;

        //    }
        //}
        /*********************************** OLD CODE ***********************************/


        public bool DisplayHeaders
        {
            get
            {
                return _displayHeader;
            }
            set
            {
                _displayHeader = value;
            }
        }
        string Prefix = string.Empty; string Suffix = string.Empty; string EmptyCondition = string.Empty; string ReplaceText = string.Empty; string Headeroptions = string.Empty;

        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE CODE  ***/
        /********************************************************************************/
        public int GetPriceCode()
        {
            //int pc = -1;
            //DataTable Sqltb = new DataTable();
            //string userid = HttpContext.Current.Session["USER_ID"].ToString();
            //if (!string.IsNullOrEmpty(userid))
            //{
            //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //    Sqltb = oHelper.GetDataTable(sSQL);
            //    if (Sqltb != null && Sqltb.Rows.Count > 0)
            //        pc = Convert.ToInt32(Sqltb.Rows[0]["price_code"]);
            //}
            
            //return pc;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            return objHelperDb.GetPriceCode(userid);
        }
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public string GenerateHorizontalHTML(string _familyID, DataSet Ds)
        //{
        //    //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
        //    DataSet dsBgDisc = new DataSet();
        //    decimal untPrice = 0;
        //    string AttrID = string.Empty;
        //    string HypColumn = "";
        //    int Min_ord_qty = 0;
        //    int Qty_avail;
        //    int flagtemp = 0;
        //    string _StockStatus = "NO STATUS AVAILABLE";
        //    string _AvilableQty = "0";

        //    string _Category_id = "";
        //    string _EA_Path = "";

        //    DataRow[] tempPriceDr;

        //    DataTable tempPriceDt;
        //    //int ProdID;
        //    int AttrType;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();

        //    string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //    string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //    string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //    string _parentFamily_Id = "0";
        //    if (EComState == "YES")
        //        if (!objHelperServices.GetIsEcomEnabled(userid))
        //            EComState = "NO";
        //    StringBuilder strBldr = new StringBuilder();
        //    StringBuilder strBldrcost = new StringBuilder();

        //    if (HttpContext.Current.Request.QueryString["path"] != null)
        //        _EA_Path = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

        //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //        _Category_id = HttpContext.Current.Request.QueryString["cid"];

        //    DsPreview = Ds;
        //    if (DsPreview.Tables[_familyID] == null)
        //        return "";


        //    DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //    if (_parentFamily_Id == "0")
        //        _parentFamily_Id = _familyID;

        //    strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=0 cellspacing=1 Class=\"FamilyPageTable\" cellpadding=3>");
        //    //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");


        //    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //    //oHelper.SQLString = sSQL;
        //    //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //    pricecode = GetPriceCode();
        //    DisplayHeaders = true;
        //    if (DisplayHeaders == true)
        //    {
        //        strBldrcost = new StringBuilder();
        //        strBldr.Append("<TR>");
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());

        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
        //                if (AttrType != 3)
        //                {
        //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
        //                    {
        //                        strBldrcost.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldrcost.Append("</TD>");
        //                    }
        //                    else
        //                    {
        //                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldr.Append("</TD>");
        //                    }
        //                }
        //                else
        //                {
        //                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >");
        //                    strBldr.Append("</TD>");
        //                }
        //            }
        //        }
        //        if (DsPreview.Tables[_familyID].Rows.Count > 0)
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >More Info</TD>");

        //        if (strBldrcost.ToString() != "")
        //        {
        //            strBldr.Append(strBldrcost);
        //        }
        //        if (EComState.ToUpper() == "YES" && DsPreview.Tables[_familyID].Rows.Count > 0)
        //        {
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width:120px;    \" >Cart</TD>");
        //        }
        //        strBldr.Append("</TR>");
        //    }
        //    string ValueFortag = string.Empty;
        //    bool rowcolor = false;

        //    if (_EA_Path == "" && _Category_id == "")
        //    {
        //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //        if (tmpds != null && tmpds.Tables.Count > 0)
        //        {
        //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
        //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //        }
        //    }



        //    for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //    {
        //        strBldr.Append("<TR>");
        //        if (rowcolor == false && i != 0)
        //        {
        //            rowcolor = true;
        //        }
        //        else if (rowcolor == true)
        //        {
        //            rowcolor = false;
        //        }
        //        tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //        if (tempPriceDr.Length > 0)
        //            tempPriceDt = tempPriceDr.CopyToDataTable();
        //        else
        //            tempPriceDt = null;
        //        strBldrcost = new StringBuilder();
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {

        //            string alignVal = "LEFT";

        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


        //                //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
        //                //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
        //                //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
        //                //DataSet DSS = oHelper.GetDataSet();
        //                //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //                //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


        //                //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                {
        //                    alignVal = "RIGHT";
        //                }
        //                if (AttrType == 3)
        //                {
        //                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
        //                    if (ValueFortag != "" && ValueFortag != null)
        //                    {
        //                        FileInfo Fil;
        //                        string strFile = HttpContext.Current.Server.MapPath("ProdImages");
        //                        Fil = new FileInfo(strFile + ValueFortag);
        //                        if (Fil.Exists)
        //                        {
        //                            ValueFortag = "prodimages" + ValueFortag;
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = "Images/NoImage.gif";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ValueFortag = "Images/NoImage.gif";
        //                    }

        //                    if (rowcolor == false)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");

        //                    }
        //                }
        //                // strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[_familyID].Rows[i][j].ToString()));
        //                else  //if (chkAttrType[j] == 4)
        //                {
        //                    if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                    {
        //                        if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else
        //                        {
        //                            if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                            {
        //                                if (AttrType == 4)
        //                                {
        //                                    //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                                    //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            

        //                                    if (tempPriceDt != null)
        //                                        ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;
        //                                    else
        //                                        ValueFortag = Prefix + " " + "" + " " + Suffix;


        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = string.Empty;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                        {
        //                            ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                        }
        //                    }
        //                    //if (DsPreview.Tables[_familyID].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
        //                    //{
        //                    //    ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i][0].ToString());
        //                    //    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                    //    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                    //    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
        //                    //    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

        //                    //    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
        //                    //}
        //                    if (AttrType == 4)
        //                    {
        //                        _StockStatus = "NO STATUS AVAILABLE";
        //                        _AvilableQty = "0";
        //                        string _ProCode = "";
        //                        if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                            _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
        //                        if (tempPriceDt != null)
        //                        {
        //                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();

        //                        }
        //                        string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
        //                        if (Convert.ToInt32(userid) > 0)
        //                        {

        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
        //                        }
        //                        else
        //                        {
        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
        //                        }

        //                        if (dsBgDisc != null)
        //                        {
        //                            if (dsBgDisc.Tables[0].Rows.Count > 0)
        //                            {
        //                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
        //                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
        //                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
        //                                untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                                bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
        //                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
        //                                {
        //                                    ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

        //                                }
        //                            }
        //                        }
        //                        ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
        //                        //ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
        //                    }
        //                    if (rowcolor == false)
        //                    {
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");


        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                    }
        //                }
        //                //else
        //                //{
        //                //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
        //                //}

        //                //Add the Shipping and Cart Images
        //                if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
        //                {

        //                    ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                    //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
        //                    //int IsAvailable = oPro.GetProductAvailability(ProdID);
        //                    string ShipImgPath = "";
        //                    int IsAvailable = 0;
        //                    _StockStatus = "NO STATUS AVAILABLE";
        //                    _AvilableQty = "0";
        //                    Boolean IsShipping = false;
        //                    if (tempPriceDt != null)
        //                    {
        //                        IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
        //                        if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
        //                            IsAvailable = 1;
        //                        else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
        //                            IsAvailable = 0;
        //                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                    }

        //                    if (IsShipping == true)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
        //                        string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
        //                        ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
        //                    }
        //                    else if (IsShipping == false)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
        //                        ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
        //                    }
        //                    string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));
        //                    ShipImgPath = "<a href=\"productdetails.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
        //                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";


        //                    if (rowcolor == false)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</TD>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</TD>");
        //                    }
        //                    if (strBldrcost.ToString() != "")
        //                    {
        //                        strBldr.Append(strBldrcost);
        //                    }
        //                    if (EComState.ToUpper() == "YES")
        //                    {
        //                        //Add the Cart Image
        //                        string CartImgPath = "";
        //                        //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
        //                        if (Restricted.ToUpper() == "YES")
        //                        {
        //                            CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
        //                            string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
        //                            CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
        //                        }
        //                        else
        //                        {
        //                            if (IsAvailable == 1)
        //                            {
        //                                CartImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("CARTIMGPATH").ToString();

        //                                //Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
        //                                //string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
        //                                _StockStatus = "NO STATUS AVAILABLE";
        //                                _AvilableQty = "0";
        //                                if (tempPriceDt != null)
        //                                {
        //                                    Min_ord_qty = Convert.ToInt32(tempPriceDt.Rows[0]["MIN_ORD_QTY"].ToString());
        //                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                                }
        //                                string CartUrl = objHelperServices.GetOptionValues("CARTURL").ToString();

        //                                CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                                CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                                CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";




        //                                string _StockStatusTrim = _StockStatus.Trim();
        //                                bool _Tbt_Stock_Status_2 = false;

        //                                switch (_StockStatusTrim)
        //                                {
        //                                    case "IN STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE &":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "DISCONTINUED":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER AVAILABLE":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE NO ETA":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    default:
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                }

        //                                if (_Tbt_Stock_Status_2 == true)
        //                                {

        //                                    CartImgPath = "<table width=\"100%\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +
        //                                               "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 18px;\"   /> " +
        //                                             "</td><td width=\"2\">&nbsp;</td><td>" +
        //                                             "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +
        //                                        //"<div onmouseout=\"MM_swapImgRestore()\" onmouseover=\"MM_swapImage('Image"+ ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\" style=\"width:76px; height:25px; cursor:pointer; \">" +
        //                                             "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
        //                                             "</a></td></tr></table>";
        //                                }
        //                                else
        //                                {
        //                                    CartImgPath = "";
        //                                }

        //                                if (rowcolor == false)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px; \">" + CartImgPath + "</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px;  \">" + CartImgPath + "</TD>");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (rowcolor == false)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;   \">N/A</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"   Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">N/A</TD>");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        strBldr.Append("</TR>");
        //    }

        //    strBldr.Append("</TABLE></td></tr></table>");
        //    //if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
        //    //{
        //    //    strBldr = strBldr.Remove(0, strBldr.Length);
        //    //}
        //    return strBldr.ToString();
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRODUCT DETAILS DYNAMICALLY ON FAMILY PAGE ***/
        /********************************************************************************/
        public string GenerateHorizontalHTML(string _familyID, DataSet Ds)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
          //  string HypColumn = "";
            int Min_ord_qty = 0;
          //  int Qty_avail;
          //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";

            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            DataRow[] tempPriceDr;

            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id=HttpContext.Current.Request.QueryString["cid"] ;

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;

            strBldr.Append("<table width=\"100%\" border=\"0\" style=\" overflow:hidden;\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"left\" ><div Class=\"testscroll\"><TABLE width=\"99%\" border=0 cellspacing=1 Class=\"FamilyPageTable\" cellpadding=3>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");

           
            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
            if ((DisplayHeaders))
            {
                strBldrcost = new StringBuilder();
                strBldr.Append("<TR>");
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    
                    DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                        if (AttrType != 3)
                        {
                            if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper()=="COST")
                            {
                                strBldrcost.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200;  \" >");

                                if (pricecode == 1)
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldrcost.Append("</TD>");
                            }
                            else
                            {
                                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200;  \" >");

                                if (pricecode == 1)
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldr.Append("</TD>");
                            }
                        }                      
                        else
                        {
                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200;   \" >");
                            strBldr.Append("</TD>");
                        }
                    }
                }
                if (DsPreview.Tables[_familyID].Rows.Count > 0)
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >More Info</TD>");

                if (strBldrcost.ToString()!="" )
                {
                    strBldr.Append(strBldrcost);
                }
                if (EComState.ToUpper() == "YES" && DsPreview.Tables[_familyID].Rows.Count > 0)
                {
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width:120px;    \" >Cart</TD>");
                }
                strBldr.Append("</TR>");
            }
            string ValueFortag = string.Empty;
            bool rowcolor = false;

            if (_EA_Path == "" && _Category_id == "")
            {
                DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                if (tmpds != null && tmpds.Tables.Count > 0)
                {
                    _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                    _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                }
            }



            for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            {
                strBldr.Append("<TR>");
                if (!(rowcolor) && i != 0)
                {
                    rowcolor = true;
                }
                else if ((rowcolor))
                {
                    rowcolor = false;
                }
                tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                if(tempPriceDr.Length>0)
                    tempPriceDt=tempPriceDr.CopyToDataTable();
                else       
                    tempPriceDt=null;
                strBldrcost = new StringBuilder();
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    
                    string alignVal = "LEFT";

                    DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


                        //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
                        //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                        //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                        //DataSet DSS = oHelper.GetDataSet();
                        //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                        //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


                        //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                        if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
                        {
                            alignVal = "RIGHT";
                        }
                        if (AttrType == 3)
                        {
                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                            string ValueLargeImg = string.Empty;
                            if (ValueFortag != "" && ValueFortag != null)
                            {
                                FileInfo Fil;
                                string strFile = HttpContext.Current.Server.MapPath("ProdImages");
                               
                                Fil = new FileInfo(strFile + ValueFortag);
                                if (Fil.Exists)
                                {
                                    ValueFortag = "prodimages" + ValueFortag.Replace("\\", "/");
                                    ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200");
                                    //ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_images_200");
                                }
                                else
                                {
                                    ValueFortag = "prodimages/Images/NoImage.gif";
                                    ValueLargeImg = "";
                                }
                            }
                            else
                            {
                                ValueFortag = "prodimages/Images/NoImage.gif";
                                ValueLargeImg = "";
                            }
                            string Popupdiv="";
                            if (ValueLargeImg != "")
                            {

                                Popupdiv = "<div class=\"pro_img_popup\" id=\"pro_img_popup" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "\"><img alt=\"dummny img\" class=\"lazy\" data-original=\"" + ValueLargeImg + "\"></div>";
                            }
                            if (!(rowcolor))
                            {


                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag.Trim() + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" style=\"max-height:50px;max-width:50px;\" /></div></td>");
                            }
                            else if ((rowcolor))
                            {

                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag.Trim() + "\"  onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\"  style=\"max-height:50px;max-width:50px;\" /></div></td>");

                            }
                        }
                        
                        else  
                        {
                            if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                            {
                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else
                                {
                                    if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                    {
                                        if (AttrType == 4)
                                        {
                                            //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                            //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
                                            /* DB price 
                                            //if (tempPriceDt!=null)
                                            //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                            //else
                                            //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
                                             DB price */

                                            if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
                                                ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
                                            else
                                                ValueFortag = "";
                                            

                                        }
                                        else
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                        }
                                    }
                                    else
                                    {
                                        if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                        }
                                        else
                                        {
                                            ValueFortag = string.Empty;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                {
                                    ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                }
                                else
                                {
                                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                }
                            }
                            //if (DsPreview.Tables[_familyID].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
                            //{
                            //    ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i][0].ToString());
                            //    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
                            //    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
                            //    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                            //    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
                            //    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                            //    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

                            //    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
                            //}
                            if (AttrType == 4)
                            {
                                _StockStatus = "NO STATUS AVAILABLE";
                                _AvilableQty = "0";
                                string _ProCode = string.Empty;
                                string _eta = string.Empty;
                                if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                    _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                if (tempPriceDt != null)
                                {
                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                    _eta = tempPriceDt.Rows[0]["ETA"].ToString();
                                }
                                string _Buyer_Group = GetBuyerGroup(Convert.ToInt32 (userid));
                                if (Convert.ToInt32(userid) > 0)
                                {

                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                                }
                                else
                                {
                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                }

                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                        bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                        {
                                            ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                        }
                                    }
                                }
                                StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta);
                              //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                                ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" class=\"popupouterdiv2none\"><div class=\"popupaero\"></div>" + StrPriceTable + "</div><a class=\"poppricenone\" onMouseOut=\"javascript:Moutstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" onMouseOver=\"javascript:Moverstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" style=\"text-decoration:none;\">" + ValueFortag + " <br/> Price / Stock Status </a>";
                               // ValueFortag = "<div class=\"popupaero\"></div>";
                            }
                            if (!(rowcolor))
                            {
                                if (AttrType == 4 &&  DsPreview.Tables[_familyID].Columns[j].ToString()=="COST")
                                    //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
                                    strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
                                else
                                    strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200;   \" >" + ValueFortag + "</TD>");                                
                                    

                            }
                            else if ((rowcolor))
                            {
                                if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
                                    strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
                                    //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
                                else
                                    strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;   \" >" + ValueFortag + "</TD>");
                            }
                        }
                        //else
                        //{
                        //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
                        //}

                        //Add the Shipping and Cart Images
                        if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                        {

                            ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                            //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                            //int IsAvailable = oPro.GetProductAvailability(ProdID);
                            string ShipImgPath = string.Empty;
                            int IsAvailable = 0;
                             _StockStatus = "NO STATUS AVAILABLE";
                             _AvilableQty = "0";
                            Boolean IsShipping=false;
                            if (tempPriceDt != null)
                            {
                                IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
                                if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
                                    IsAvailable = 1;
                                else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
                                    IsAvailable = 0;
                                _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                            }                            
                                                      
                            if ((IsShipping))
                            {
                                ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
                                string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
                                ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                            }
                            else if (!(IsShipping))
                            {
                                ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                            }
                            string tempEAPath = string.Empty;
                            if (_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString()) ==true)
                                tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                            else
                                tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));

                            ShipImgPath = "<a href=\"productdetails.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
                                             // "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>See More Details </a>";
                                             "See More Details </a>";
                       
                            if (!(rowcolor))
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;   \">" + ShipImgPath + "</TD>");
                            }
                            else if ((rowcolor))
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;  \">" + ShipImgPath + "</TD>");
                            }
                            if (strBldrcost.ToString() != "")
                            {
                                strBldr.Append(strBldrcost);
                            }
                            if (EComState.ToUpper() == "YES")
                            {
                                //Add the Cart Image
                                string CartImgPath = string.Empty;
                                //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                if (Restricted.ToUpper() == "YES")
                                {
                                    CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                    string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                }
                                else
                                {
                                    if (IsAvailable == 1)
                                    {
                                        CartImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("CARTIMGPATH").ToString();

                                        //Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
                                        //string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
                                         _StockStatus = "NO STATUS AVAILABLE";
                                         _AvilableQty = "0";
                                        if (tempPriceDt != null)
                                        {
                                            Min_ord_qty =Convert.ToInt32(tempPriceDt.Rows[0]["MIN_ORD_QTY"].ToString());
                                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_"," ") ;
                                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                        }
                                        string CartUrl = objHelperServices.GetOptionValues("CARTURL").ToString();

                                        CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
                                        CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";

                                        
                                        
                                        
                                        string _StockStatusTrim = _StockStatus.Trim();
                                        bool _Tbt_Stock_Status_2 = false;

                                        switch (_StockStatusTrim)
                                        {
                                            case "IN STOCK":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "SPECIAL ORDER":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "SPECIAL ORDER PRICE &":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "DISCONTINUED":
                                                _Tbt_Stock_Status_2 = false;
                                                break;
                                            case "DISCONTINUED NO LONGER AVAILABLE":
                                                _Tbt_Stock_Status_2 = false;
                                                break;
                                            case "DISCONTINUED NO LONGER":
                                                _Tbt_Stock_Status_2 = false;
                                                break;
                                            case "TEMPORARY UNAVAILABLE":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "TEMPORARY UNAVAILABLE NO ETA":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "OUT OF STOCK":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            case "OUT OF STOCK ITEM WILL":
                                                _Tbt_Stock_Status_2 = true;
                                                break;
                                            default:
                                                _Tbt_Stock_Status_2 = false;
                                                break;
                                        }

                                        if ((_Tbt_Stock_Status_2))
                                        {
                                            //CartImgPath = "<table width=\"100%\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +
                                            //           "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;\"   /> " +
                                            //         "</td><td width=\"2\">&nbsp;</td><td>" +
                                            //         "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +
                                                
                                            //         "   <img src=\"images/button1.png\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"56\" height=\"26\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                            //         "</a></td></tr></table>";

                                            string productid = ProdID.ToString();

                                            CartImgPath = "<table width=\"100px\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +

                                                      "<div><input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" onkeydown=\"return keyct(event)\"  maxlength=\"6\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;\"/> " +
                                                      "<div class=\"costable\"><div class=\"pricepopup\"><div class=\"popupouterdivnone\" id=\"popupouterdiv" + ProdID.ToString() + "\"><div class=\"popupaero\"></div>  " + StrPriceTable + "</div>" +
                                                      "<a style=\"cursor:pointer;margin: 0 0 0 5px;\" onMouseOut=\"javascript:Mouseout('" + ProdID.ToString() + "');\" onMouseOver=\"javascript:test('" + ProdID.ToString() + "');\"  id=\"" + ProdID.ToString() + "\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\"  onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\">Buy  </a></div></div></div></td></tr></table>";

                                          
     

                                                      /*"  <a style=\"cursor:pointer;margin: 0 0 0 5px;\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\" onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" >Buy  </a></td></tr></table>";*/
                                                  
                                        }
                                        else
                                        {
                                            CartImgPath = "";
                                        }

                                        if (!(rowcolor))
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px; \">" + CartImgPath + "</TD>");
                                        }
                                        if ((rowcolor))
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px;  \">" + CartImgPath + "</TD>");
                                        }
                                    }
                                    else
                                    {
                                        if (!(rowcolor))
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;   \">N/A</TD>");
                                        }
                                        if ((rowcolor))
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"   Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">N/A</TD>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                strBldr.Append("</TR>");
            }

            strBldr.Append("</TABLE></div></td></tr></table>");
            //if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
            return strBldr.ToString();
        }

        public string GenerateHorizontalHTMLJson(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs)
        {

            //string rtnstr = "";
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;

            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;




         //   StringTemplate _stmpl_recordsrows = null;
            //  TBWDataList[] lstrecords = new TBWDataList[0];
            // TBWDataList[] lstrows = new TBWDataList[0];

         //   StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            //  TBWDataList1[] lstrows1 = new TBWDataList1[0];


            DataSet dsBgDisc = new DataSet();
          //  decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
          //  int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _prod_stk_Status = "0";
            string _prod_stk_Flag = "0";
            string _AvilableQty = "0";
            string _eta = string.Empty;
            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            DataRow[] tempPriceDr;

        //    DataTable tempPriceDt;
            //int ProdID;
         //   int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();



            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            string _ProCode = string.Empty;
            string family_name = string.Empty;

            if (userid == "")
                userid = "0";

            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;

            pricecode = GetPriceCode();

            string _SkinRootPath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);

            string HTMLAtts = string.Empty;
            string HTMLProducts = string.Empty;
            string HTMLHeaderStr = string.Empty;
           // int ictrecords = 0;
            string costtype = string.Empty;
            HTMLAtts = string.Empty;
            string prodcodedesc = string.Empty;

            string prodedesc = string.Empty;
            string dsprecolcaption = string.Empty;

            bool showheader = true;
            if (HttpContext.Current.Session["hfprevfid"] == null)
            {
                HttpContext.Current.Session["hfprevfid"] = _familyID;
            }
            else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
            {
                HttpContext.Current.Session["hfprevfid"] = _familyID;
            }
            else
            {
                showheader = false;
            }
            for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
            {
                dsprecolcaption = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
                if (dsprecolcaption != "COST" && dsprecolcaption != "CODE"
                    && dsprecolcaption != "TWEB IMAGE1"
                    && dsprecolcaption != "PRODUCT_ID"
                    && dsprecolcaption != "FAMILY_ID"
                    )
                {
                    if (showheader == true)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage\\ProcellHead");
                    }
                    else
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage\\ProcellHead_dyn");
                    }
                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
                    HTMLAtts = HTMLAtts + _stmpl_records.ToString();

                }

                //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
                //{
                if (dsprecolcaption == "COST")
                {
                    if (pricecode == 1)
                    {
                        costtype = " Inc GST";
                    }
                    else
                    {
                        costtype = " Ex GST";
                    }
                }

                //}

            }
            if (showheader == true)
            {
                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage\\ProrowHead");
            }
            else
            {

                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage\\ProrowHead_dyn");
            }
            _stmpl_records1.SetAttribute("INGST", costtype);
            _stmpl_records1.SetAttribute("ATTRIBUTE_HEADR", HTMLAtts);
            HTMLHeaderStr = _stmpl_records1.ToString();





            DisplayHeaders = true;
            if ((DisplayHeaders))
            {
                HTMLProducts = HTMLProducts + HTMLHeaderStr;
            }
            string ValueFortag = string.Empty;
          //  bool rowcolor = false;

            if (_EA_Path == "" && _Category_id == "")
            {
                DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                if (tmpds != null && tmpds.Tables.Count > 0)
                {
                    _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                    _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                }
            }

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");

            string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

            for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            {
                tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                //if (tempPriceDr.Length > 0)
                //    tempPriceDt = tempPriceDr.CopyToDataTable();
                //else
                //   tempPriceDt = null;

                strBldrcost = new StringBuilder();


                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage\\Prorow");

                _stmpl_records1.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                _stmpl_records1.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());


                //-------------------------------- cost
                _StockStatus = "NO STATUS AVAILABLE";
                _prod_stk_Status = "0";
                _prod_stk_Flag = "0";
                _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString() + " " + "<br/> Price / Stock Status");
                string ISSUBSTITUTE = "";
                //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
              //  _stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.CheckPriceValueDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()).ToString() + " " + "<br/> Price / Stock Status");
                if (tempPriceDr != null && tempPriceDr.Length > 0)
                {
                    _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                    _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
                    _prod_stk_Flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
                    _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
                    _eta = tempPriceDr[0]["ETA"].ToString();
                    ISSUBSTITUTE = tempPriceDr[0]["PROD_SUBSTITUTE"].ToString();

                }
                if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                    _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();

                StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid),_prod_stk_Flag, _eta, dsPriceTableAll);
                _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);

                string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=fl Id=" + _parentFamily_Id.ToString()));
                _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
                _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);




                //string ORIGINALURL = "pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;
                //string NEWURL = string.Empty;
                //string HREFURL = string.Empty;
                //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
                //{
                //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
                //    NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
                //    HREFURL = "/" + NEWURL + "/pd/";
                //}
                //else
                //{
                //    NEWURL = ORIGINALURL;
                //    HREFURL = ORIGINALURL;

                //}

                //HREFURL = "/" + NEWURL + "/pd/";

                tempEAPath = "";
                if ((_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString())))
                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                else
                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));

                _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
                _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);              
                _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
               

                bool _Tbt_Stock_Status_2 = false;
                switch (_StockStatus.Trim())
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "TEMPORARY UNAVAILABLE":
                      //  _Tbt_Stock_Status_2 = true;
                        //modified by indu Requirement Stock Status update date 22-Apr-2017
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                      //  _Tbt_Stock_Status_2 = true;
                        //modified by indu Requirement Stock Status update date 22-Apr-2017
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    default:
                        _Tbt_Stock_Status_2 = false;
                        break;
                }
             

                if ((_Tbt_Stock_Status_2))
                {
                    try
                    {

                        if (_StockStatus.ToUpper().Contains("OUT OF STOCK") == true && ISSUBSTITUTE.ToString().Trim() != "" &&  _prod_stk_Status =="0" && _prod_stk_Flag.ToString().Trim() == "-2")
                        {
                           // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(ISSUBSTITUTE.ToString().Trim(), Convert.ToInt32(userid));
                            DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode, Convert.ToInt32(userid));
                            if (rtntbl != null && rtntbl.Rows.Count > 0)
                            {

                                bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                {
                                   
                                    _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                                    _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                    string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                    _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                                }
                            }

                        }
                        else
                        {
                            _stmpl_records1.SetAttribute("SHOW_BUY", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString()); 
                    }
                }
                else
                {
                 try
                 {
                     if (ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "0" && _prod_stk_Flag.ToString().Trim() == "-2")
                    {
                        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode.ToString().Trim(), Convert.ToInt32(userid));

                       if (rtntbl != null && rtntbl.Rows.Count > 0)
                       {

                           bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                           bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                           if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                           {
                         

                               _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                               _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                               string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                               _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                           }
                       }
                    }
                 }
                 catch (Exception ex)
                 {
                     objErrorHandler.CreateLog(ex.ToString());
                 }
                }

                //-------------------------------- Code

                _stmpl_records1.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());

                //-------------------------------- Image

                ValueFortag = DsPreview.Tables[_familyID].Rows[i]["TWEB IMAGE1"].ToString();
                string ValueLargeImg = string.Empty;
                if (ValueFortag != "" && ValueFortag != null)
                {
                    FileInfo Fil;


                    Fil = new FileInfo(strFile + ValueFortag);
                    if (Fil.Exists)
                    {

                        ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
                        ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                    }
                    else
                    {
                        ValueFortag = "/prodimages/images/noimage.gif";
                        ValueLargeImg = "";
                    }
                }
                else
                {
                    ValueFortag = "/prodimages/images/noimage.gif";
                    ValueLargeImg = "";
                }
                _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
                if (ValueLargeImg != "")
                    _stmpl_records1.SetAttribute("SHOW_DIV", true);
                else
                    _stmpl_records1.SetAttribute("SHOW_DIV", false);


                HTMLAtts = "";
                prodedesc = "";
                bool flgdescchk = false;
                string dsprecolucaption = string.Empty;
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    dsprecolucaption = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
                    if (dsprecolucaption != "COST"
                        && dsprecolucaption != "CODE"
                        && dsprecolucaption != "TWEB IMAGE1"
                        && dsprecolucaption != "PRODUCT_ID"
                        && dsprecolucaption != "FAMILY_ID"
                        )
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage\\Procell");
                        _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));

                        HTMLAtts = HTMLAtts + _stmpl_records.ToString();
                        if (dsprecolucaption == "DESCRIPTION")
                        {
                            _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
                            prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                            flgdescchk = true;
                        }
                      

                    }

                }

                if (!(flgdescchk))
                    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name);

                if (prodedesc == "")
                {
                    if (tempPriceDr != null && tempPriceDr.Length > 0)
                        prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();

                    
                }

                _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);


                HTMLProducts = HTMLProducts + _stmpl_records1.ToString();

                if (prodedesc.Length > 0)
                    prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
                else
                    prodcodedesc = prodcodedesc + _ProCode + "|";


            }
            HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;
            _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage\\Promain");
            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts);


            return _stmpl_container.ToString();
            

            //if (_EA_Path == "" && _Category_id == "")
            //{
            //    DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
            //    if (tmpds != null && tmpds.Tables.Count > 0)
            //    {
            //        _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            //        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
            //        _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
            //    }
            //}



            //for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            //{
            //    strBldr.Append("<TR>");
            //    if (rowcolor == false && i != 0)
            //    {
            //        rowcolor = true;
            //    }
            //    else if (rowcolor == true)
            //    {
            //        rowcolor = false;
            //    }
            //    tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
            //    if (tempPriceDr.Length > 0)
            //        tempPriceDt = tempPriceDr.CopyToDataTable();
            //    else
            //        tempPriceDt = null;
            //    strBldrcost = new StringBuilder();
            //    for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
            //    {

            //        string alignVal = "LEFT";

            //        DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
            //        if (tempdr.Length > 0)
            //        {
            //            ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
            //            AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


            //            //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
            //            //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
            //            //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
            //            //DataSet DSS = oHelper.GetDataSet();
            //            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
            //            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


            //            //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
            //            if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
            //            {
            //                alignVal = "RIGHT";
            //            }
            //            if (AttrType == 3)
            //            {
            //                ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
            //                string ValueLargeImg = "";
            //                if (ValueFortag != "" && ValueFortag != null)
            //                {
            //                    FileInfo Fil;
            //                    string strFile = HttpContext.Current.Server.MapPath("ProdImages");

            //                    Fil = new FileInfo(strFile + ValueFortag);
            //                    if (Fil.Exists)
            //                    {
            //                        ValueFortag = "prodimages" + ValueFortag.Replace("\\", "/");
            //                        ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200");
            //                        //ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_images_200");
            //                    }
            //                    else
            //                    {
            //                        ValueFortag = "prodimages/Images/NoImage.gif";
            //                        ValueLargeImg = "";
            //                    }
            //                }
            //                else
            //                {
            //                    ValueFortag = "prodimages/Images/NoImage.gif";
            //                    ValueLargeImg = "";
            //                }
            //                string Popupdiv = "";
            //                if (ValueLargeImg != "")
            //                {

            //                    Popupdiv = "<div class=\"pro_img_popup\" id=\"pro_img_popup" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "\"><img alt=\"dummny img\" class=\"lazy\" data-original=\"" + ValueLargeImg + "\"></div>";
            //                }
            //                if (rowcolor == false)
            //                {


            //                    strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag.Trim() + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" style=\"max-height:50px;max-width:50px;\" /></div></td>");
            //                }
            //                else if (rowcolor == true)
            //                {

            //                    strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag.Trim() + "\"  onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\"  style=\"max-height:50px;max-width:50px;\" /></div></td>");

            //                }
            //            }

            //            else
            //            {
            //                if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
            //                {
            //                    if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
            //                    {
            //                        ValueFortag = ReplaceText;
            //                    }
            //                    else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
            //                    {
            //                        ValueFortag = ReplaceText;
            //                    }
            //                    else
            //                    {
            //                        if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
            //                        {
            //                            if (AttrType == 4)
            //                            {
            //                                //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
            //                                //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
            //                                /* DB price 
            //                                //if (tempPriceDt!=null)
            //                                //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
            //                                //else
            //                                //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
            //                                 DB price */

            //                                if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
            //                                    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
            //                                else
            //                                    ValueFortag = "";


            //                            }
            //                            else
            //                            {
            //                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
            //                            {
            //                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
            //                            }
            //                            else
            //                            {
            //                                ValueFortag = string.Empty;
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
            //                    {
            //                        ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
            //                    }
            //                    else
            //                    {
            //                        ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
            //                    }
            //                }
            //                //if (DsPreview.Tables[_familyID].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
            //                //{
            //                //    ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i][0].ToString());
            //                //    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
            //                //    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
            //                //    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
            //                //    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
            //                //    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
            //                //    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

            //                //    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
            //                //}
            //                if (AttrType == 4)
            //                {
            //                    _StockStatus = "NO STATUS AVAILABLE";
            //                    _AvilableQty = "0";
            //                    string _ProCode = "";
            //                    string _eta = "";
            //                    if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
            //                        _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
            //                    if (tempPriceDt != null)
            //                    {
            //                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
            //                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
            //                        _eta = tempPriceDt.Rows[0]["ETA"].ToString();
            //                    }
            //                    string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
            //                    if (Convert.ToInt32(userid) > 0)
            //                    {

            //                        dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
            //                    }
            //                    else
            //                    {
            //                        dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
            //                    }

            //                    if (dsBgDisc != null)
            //                    {
            //                        if (dsBgDisc.Tables[0].Rows.Count > 0)
            //                        {
            //                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
            //                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
            //                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
            //                            untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
            //                            bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
            //                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
            //                            {
            //                                ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

            //                            }
            //                        }
            //                    }
            //                    StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta);
            //                    //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
            //                    ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" class=\"popupouterdiv2none\"><div class=\"popupaero\"></div>" + StrPriceTable + "</div><a class=\"poppricenone\" onMouseOut=\"javascript:Moutstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" onMouseOver=\"javascript:Moverstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" style=\"text-decoration:none;\">" + ValueFortag + " <br/> Price / Stock Status </a>";
            //                    // ValueFortag = "<div class=\"popupaero\"></div>";
            //                }
            //                if (rowcolor == false)
            //                {
            //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
            //                        //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
            //                        strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
            //                    else
            //                        strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200;   \" >" + ValueFortag + "</TD>");


            //                }
            //                else if (rowcolor == true)
            //                {
            //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
            //                        strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
            //                    //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
            //                    else
            //                        strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;   \" >" + ValueFortag + "</TD>");
            //                }
            //            }
            //            //else
            //            //{
            //            //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
            //            //}

            //            //Add the Shipping and Cart Images
            //            if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
            //            {

            //                ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
            //                //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
            //                //int IsAvailable = oPro.GetProductAvailability(ProdID);
            //                string ShipImgPath = "";
            //                int IsAvailable = 0;
            //                _StockStatus = "NO STATUS AVAILABLE";
            //                _AvilableQty = "0";
            //                Boolean IsShipping = false;
            //                if (tempPriceDt != null)
            //                {
            //                    IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
            //                    if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
            //                        IsAvailable = 1;
            //                    else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
            //                        IsAvailable = 0;
            //                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
            //                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
            //                }

            //                if (IsShipping == true)
            //                {
            //                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
            //                    string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
            //                    ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
            //                }
            //                else if (IsShipping == false)
            //                {
            //                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
            //                    ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
            //                }
            //                string tempEAPath = "";
            //                if (_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString()) == true)
            //                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
            //                else
            //                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));

            //                ShipImgPath = "<a href=\"productdetails.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
            //                    // "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>See More Details </a>";
            //                                 "See More Details </a>";

            //                if (rowcolor == false)
            //                {
            //                    strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;   \">" + ShipImgPath + "</TD>");
            //                }
            //                else if (rowcolor == true)
            //                {
            //                    strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width:200;  \">" + ShipImgPath + "</TD>");
            //                }
            //                if (strBldrcost.ToString() != "")
            //                {
            //                    strBldr.Append(strBldrcost);
            //                }
            //                if (EComState.ToUpper() == "YES")
            //                {
            //                    //Add the Cart Image
            //                    string CartImgPath = "";
            //                    //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
            //                    if (Restricted.ToUpper() == "YES")
            //                    {
            //                        CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
            //                        string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
            //                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
            //                    }
            //                    else
            //                    {
            //                        if (IsAvailable == 1)
            //                        {
            //                            CartImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("CARTIMGPATH").ToString();

            //                            //Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
            //                            //string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
            //                            _StockStatus = "NO STATUS AVAILABLE";
            //                            _AvilableQty = "0";
            //                            if (tempPriceDt != null)
            //                            {
            //                                Min_ord_qty = Convert.ToInt32(tempPriceDt.Rows[0]["MIN_ORD_QTY"].ToString());
            //                                _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
            //                                _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
            //                            }
            //                            string CartUrl = objHelperServices.GetOptionValues("CARTURL").ToString();

            //                            CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
            //                            CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
            //                            CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";




            //                            string _StockStatusTrim = _StockStatus.Trim();
            //                            bool _Tbt_Stock_Status_2 = false;

            //                            switch (_StockStatusTrim)
            //                            {
            //                                case "IN STOCK":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "SPECIAL ORDER":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "SPECIAL ORDER PRICE &":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "DISCONTINUED":
            //                                    _Tbt_Stock_Status_2 = false;
            //                                    break;
            //                                case "DISCONTINUED NO LONGER AVAILABLE":
            //                                    _Tbt_Stock_Status_2 = false;
            //                                    break;
            //                                case "DISCONTINUED NO LONGER":
            //                                    _Tbt_Stock_Status_2 = false;
            //                                    break;
            //                                case "TEMPORARY UNAVAILABLE":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "TEMPORARY UNAVAILABLE NO ETA":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "OUT OF STOCK":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                case "OUT OF STOCK ITEM WILL":
            //                                    _Tbt_Stock_Status_2 = true;
            //                                    break;
            //                                default:
            //                                    _Tbt_Stock_Status_2 = false;
            //                                    break;
            //                            }

            //                            if (_Tbt_Stock_Status_2 == true)
            //                            {
            //                                //CartImgPath = "<table width=\"100%\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +
            //                                //           "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;\"   /> " +
            //                                //         "</td><td width=\"2\">&nbsp;</td><td>" +
            //                                //         "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +

            //                                //         "   <img src=\"images/button1.png\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"56\" height=\"26\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
            //                                //         "</a></td></tr></table>";

            //                                string productid = ProdID.ToString();

            //                                CartImgPath = "<table width=\"100px\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +

            //                                          "<div><input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" onkeydown=\"return keyct(event)\"  maxlength=\"6\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;\"/> " +
            //                                          "<div class=\"costable\"><div class=\"pricepopup\"><div class=\"popupouterdivnone\" id=\"popupouterdiv" + ProdID.ToString() + "\"><div class=\"popupaero\"></div>  " + StrPriceTable + "</div>" +
            //                                          "<a style=\"cursor:pointer;margin: 0 0 0 5px;\" onMouseOut=\"javascript:Mouseout('" + ProdID.ToString() + "');\" onMouseOver=\"javascript:test('" + ProdID.ToString() + "');\"  id=\"" + ProdID.ToString() + "\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\"  onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\">Buy  </a></div></div></div></td></tr></table>";




            //                                /*"  <a style=\"cursor:pointer;margin: 0 0 0 5px;\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\" onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" >Buy  </a></td></tr></table>";*/

            //                            }
            //                            else
            //                            {
            //                                CartImgPath = "";
            //                            }

            //                            if (rowcolor == false)
            //                            {
            //                                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px; \">" + CartImgPath + "</TD>");
            //                            }
            //                            if (rowcolor == true)
            //                            {
            //                                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px;  \">" + CartImgPath + "</TD>");
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (rowcolor == false)
            //                            {
            //                                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;   \">N/A</TD>");
            //                            }
            //                            if (rowcolor == true)
            //                            {
            //                                strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"   Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">N/A</TD>");
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    strBldr.Append("</TR>");
            //}

            //strBldr.Append("</TABLE></div></td></tr></table>");
            ////if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
            ////{
            ////    strBldr = strBldr.Remove(0, strBldr.Length);
            ////}
            //return strBldr.ToString();
        }
        public string GenerateVerticalHTML_print(string _familyID, DataSet Ds, string withprice, string withdetails)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            StringBuilder strBldr = new StringBuilder();
            try{
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
            int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";

            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            DataRow[] tempPriceDr;

            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
           
            StringBuilder strBldrcost_header = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;


            strBldr.Append("<table width='100%' height='100%' border='1' style='border-style:solid'  ><tr> <td align='left' ><div ><table width='99%' border=1 cellspacing=1 style='border:1 solid #E8E8E8 ; border-left:1 solid #E8E8E8;'>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");


            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
             
            int count = DsPreview.Tables[_familyID].Columns.Count;
            int mody = (count-2) % 5;
            int newcount = (count - mody) / 5;
            int newj=0;
            int colcount=0;
            int lastj = 0;
            strBldrcost_header = new StringBuilder();
            for (int k = 0; k <= newcount; k++)
            {
                if ((DisplayHeaders))
                {



                    if (k == 0)
                    {
                        newj = 2;
                        colcount =7;


                    }

                    else if (k == newcount)
                    {
                        newj = colcount;
                        colcount = count;

                    }
                    else
                    {
                        newj = colcount;
                        //colcount = (5 * (k + 1)) + 1;
                        colcount = colcount + 5;

                    }
                    strBldr.Append("<tr>");
                    try
                    {
                        for (int j = newj; j < colcount; j++)
                        {
                            //objErrorHandler.CreateLog(DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() + "j" + j.ToString() + "colcount" + colcount.ToString());  
                            lastj = j;
                            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());

                            DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                            if (tempdr.Length > 0)
                            {
                                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                                if (AttrType != 3)
                                {

                                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
                                    {

                                        strBldrcost_header.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\"  style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>");
                                        colcount = colcount + 1;
                                        if (pricecode == 1)
                                        {
                                            strBldrcost_header.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                        }
                                        else
                                        {
                                            strBldrcost_header.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                        }

                                        strBldrcost_header.Append("</td>");

                                    }
                                    else
                                    {
                                        strBldr.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\"  style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>");

                                        if (pricecode == 1)
                                        {
                                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                        }
                                        else
                                        {
                                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                        }

                                        strBldr.Append("</td>");
                                    }
                                }

                                else
                                {
                                    strBldr.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\" style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>");
                                    strBldr.Append("</td>");
                                }
                            }

                        }
                    }
                    catch
                    { 
                    
                    }


                  

                    strBldr.Append("</tr>");
                }
                string ValueFortag = string.Empty;
                bool rowcolor = false;

                if (_EA_Path == "" && _Category_id == "")
                {
                    DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {
                        _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                        _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                    }
                }


                //int inewj = 0;
                //int ilastj = 0;
                //int icolcount = 0;
                for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
                {

                    //if (k == 0)
                    //{

                    //    inewj = 1;
                    //    icolcount = 6;

                    //}

                    //else if (k == newcount)
                    //{

                    //    inewj = icolcount;
                    //    icolcount = count - 2;
                    //}
                    //else
                    //{

                    //    inewj = icolcount;
                    //    //colcount = (5 * (k + 1)) + 1;
                    //    icolcount = icolcount + 5;
                    //}
                    strBldr.Append("<tr>");
                    if (!(rowcolor) && i != 0)
                    {
                        rowcolor = true;
                    }
                    else if ((rowcolor))
                    {
                        rowcolor = false;
                    }
                    tempPriceDr = DsPreview.Tables["ProductPrice_pdf"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                    if (tempPriceDr.Length > 0)
                        tempPriceDt = tempPriceDr.CopyToDataTable();
                    else
                        tempPriceDt = null;
                  //  strBldrcost = new StringBuilder();
                    bool chkload = false;





                    try
                    {
                        for (int j = newj; j < colcount; j++)
                        {
                            lastj = j;
                            string alignVal = "LEFT";

                            DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                            if (tempdr.Length > 0)
                            {
                                ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


                                //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
                                //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                                //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                                //DataSet DSS = oHelper.GetDataSet();
                                //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                                //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


                                //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                                if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
                                {
                                    alignVal = "RIGHT";
                                }
                                if (AttrType == 3)
                                {
                                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                                    string ValueLargeImg = string.Empty;
                                    if (ValueFortag != "" && ValueFortag != null)
                                    {
                                        FileInfo Fil;
                                        string strFile = HttpContext.Current.Server.MapPath("ProdImages");

                                        Fil = new FileInfo(strFile + ValueFortag);
                                        if (Fil.Exists)
                                        {
                                            ValueFortag = "prodimages\\" + ValueFortag.Replace("/", "\\");
                                            ValueLargeImg = HttpContext.Current.Server.MapPath(ValueFortag.ToLower().Replace("_th", "_Images_200"));
                                        }
                                        else
                                        {
                                            ValueFortag = "images/noimage.gif";
                                            ValueLargeImg = "";
                                        }
                                    }
                                    else
                                    {
                                        ValueFortag = "images/noimage.gif";
                                        ValueLargeImg = "";
                                    }
                                    string Popupdiv = string.Empty;


                                    Popupdiv = "<img alt='' src='" + ValueFortag + "' />";

                                    //if (rowcolor == false)
                                    //{
                                    // strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'   \"><div>" + ValueFortag + "</div></td>");

                                    strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'>" + Popupdiv + "</td>");
                                    //}
                                    //else if (rowcolor == true)
                                    //{

                                    //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  style='width: 200px'  \"><div class=\"pro_thum_outer\">" + Popupdiv + "</div></td>");

                                    //}
                                }

                                else
                                {
                                    if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                                    {
                                        if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
                                        {
                                            ValueFortag = ReplaceText;
                                        }
                                        else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
                                        {
                                            ValueFortag = ReplaceText;
                                        }
                                        else
                                        {
                                            if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                            {
                                                if (AttrType == 4)
                                                {
                                                    //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                                    //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
                                                    /* DB price 
                                                    //if (tempPriceDt!=null)
                                                    //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                                    //else
                                                    //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
                                                     DB price */

                                                    if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
                                                        ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
                                                    else
                                                        ValueFortag = "";


                                                }
                                                else
                                                {
                                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                                }
                                            }
                                            else
                                            {
                                                if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
                                                {
                                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                                }
                                                else
                                                {
                                                    ValueFortag = string.Empty;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                        {
                                            ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                        }
                                        else
                                        {
                                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                        }
                                    }

                                    if (AttrType == 4)
                                    {
                                        _StockStatus = "NO STATUS AVAILABLE";
                                        _AvilableQty = "0";
                                        string _ProCode = string.Empty;
                                        string _eta = string.Empty;
                                        if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                            _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                        if (tempPriceDt != null)
                                        {
                                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                            _eta = tempPriceDt.Rows[0]["ETA"].ToString();
                                        }
                                        string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
                                        if (Convert.ToInt32(userid) > 0)
                                        {

                                            dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                                        }
                                        else
                                        {
                                            dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                        }

                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                                bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                                {
                                                    ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                                }
                                            }
                                        }
                                        if (withdetails.ToLower() == "true")
                                        {
                                            StrPriceTable = AssemblePriceTable_Print(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta, withprice, withdetails);
                                        }
                                        //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                                        // ValueFortag =   ValueFortag ;

                                        // ValueFortag = "<div class=\"popupaero\"></div>";
                                    }
                                    //if (rowcolor == false)
                                    //{
                                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
                                    //strBldrcost.Append("<td ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></td>");
                                    {
                                        if (withprice.ToLower() == "true" && !(chkload))
                                        {


                                            strBldrcost.Append("<td ALIGN=center VALIGN=Middle  style='width: 150;'><div>" + ValueFortag + "</Div></td>");
                                            strBldrcost.Append("<td width=40% height=100%>" + StrPriceTable + "</td>");
                                            chkload = true;
                                        }
                                        if (withdetails.ToLower() == "true" && withprice.ToLower() == "false")
                                        {
                                            strBldrcost.Append("<td width=40% height=100%>" + StrPriceTable + "</td>");
                                            chkload = true;
                                        }
                                    }
                                    else
                                        strBldr.Append("<td ALIGN=center VALIGN=middle  style=\"width: 200;   \" >" + ValueFortag + "</td>");



                                }

                                //Add the Shipping and Cart Images
                                if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                                {

                                    ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                    //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                                    //int IsAvailable = oPro.GetProductAvailability(ProdID);
                                    string ShipImgPath = string.Empty;
                                    int IsAvailable = 0;
                                    _StockStatus = "NO STATUS AVAILABLE";
                                    _AvilableQty = "0";
                                    Boolean IsShipping = false;
                                    if (tempPriceDt != null)
                                    {
                                        IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
                                        if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
                                            IsAvailable = 1;
                                        else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
                                            IsAvailable = 0;
                                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                    }

                                    if ((IsShipping))
                                    {
                                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
                                        string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
                                        ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                    }
                                    else if (!(IsShipping))
                                    {
                                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                        ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                    }
                                    string tempEAPath = string.Empty;
                                    if ((_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString())))
                                        tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                                    else
                                        tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));



                                    //if (rowcolor == false)
                                    //{
                                    // strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</td>");
                                    //}
                                    //else if (rowcolor == true)
                                    //{
                                    //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</td>");
                                    //}

                                    if (EComState.ToUpper() == "YES")
                                    {
                                        //Add the Cart Image
                                        string CartImgPath = string.Empty;
                                        //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                        if (Restricted.ToUpper() == "YES")
                                        {
                                            CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                            string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                            CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                        }

                                    }
                                }
                            }

                        }
                    }
                    catch
                    { }
                  
                    strBldr.Append("</tr>");
                }


               
                  
               
            }
            if (withdetails.ToLower() == "true")
            {

                //strBldrcost_header.Append("<td ALIGN=Center VALIGN=\"Middle\" style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>Details");

                //strBldrcost_header.Append("</td>");

            }
            if (strBldrcost_header.ToString() != "")
            {
                strBldr.Append("<tr>");
                strBldr.Append(strBldrcost_header);
              
            }

            if (strBldrcost.ToString() != "")
            {
               
                strBldr.Append(strBldrcost);
                strBldr.Append("</tr>");
            }

            strBldr.Append("</table></div></td></tr></table>");
            //if (strBldr.ToString().Contains("<table border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><tr></tr></table>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
        }
            catch 
            {}
            return strBldr.ToString();
        }
        public string GenerateHorizontalHTML_print(string _familyID, DataSet Ds,string withprice,string withdetails)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
           // int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";

            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            DataRow[] tempPriceDr;

            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;


            strBldr.Append("<table width='99%' border='0' style='margin-left:2px;' ><tr> <td align='left' ><div ><table width='100%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");


            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
            if ((DisplayHeaders))
            {
                strBldrcost = new StringBuilder();
                strBldr.Append("<tr>");
                
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());

                    DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                        if (AttrType != 3)
                        {
                            if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
                            {
                                strBldrcost.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\" width='15%'  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:29px; font-size:11px;font-family:Arial;'>");

                                if (pricecode == 1)
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldrcost.Append("</td>");
                            }
                            else
                            {
                                strBldr.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\" width='15%' style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:29px; font-size:11px;font-family:Arial;'>");

                                if (pricecode == 1)
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldr.Append("</td>");
                            }
                        }
                        else
                        {
                            strBldr.Append("<td  width='8%'  style='background-color: #007BDB;color: #007BDB;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; border-top:none ;border-left:none;font-size:11px;font-family:Arial;'>");
                            strBldr.Append("image</td>");
                        }
                    }
                }

                
                if (withprice.ToLower() == "true")
                {
                    if (strBldrcost.ToString() != "")
                    {
                        strBldr.Append(strBldrcost);
                    }
                }

               // strBldr.Append("<td ALIGN='Center' VALIGN='Middle' style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1 solid #E8E8E8; border-right: 1 solid #E8E8E8; height:23;width:40%; font-size:10;'>Price Table");
                  //  strBldr.Append("</td>");
               
                   
                strBldr.Append("</tr>");
            }
            string ValueFortag = string.Empty;
            bool rowcolor = false;

            if (_EA_Path == "" && _Category_id == "")
            {
                DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                if (tmpds != null && tmpds.Tables.Count > 0)
                {
                    _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                    _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                }
            }



            for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            {
                strBldr.Append("<tr>");
                if (!(rowcolor) && i != 0)
                {
                    rowcolor = true;
                }
                else if ((rowcolor))
                {
                    rowcolor = false;
                }
                tempPriceDr = DsPreview.Tables["ProductPrice_pdf"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                if (tempPriceDr.Length > 0)
                    tempPriceDt = tempPriceDr.CopyToDataTable();
                else
                    tempPriceDt = null;
                strBldrcost = new StringBuilder();
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {

                    string alignVal = "LEFT";

                    DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


                        //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
                        //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                        //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                        //DataSet DSS = oHelper.GetDataSet();
                        //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                        //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


                        //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                        if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
                        {
                            alignVal = "RIGHT";
                        }
                        if (AttrType == 3)
                        {
                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                            string ValueLargeImg = "";
                            if (ValueFortag != "" && ValueFortag != null)
                            {
                                FileInfo Fil;
                                string strFile = HttpContext.Current.Server.MapPath("ProdImages");

                                Fil = new FileInfo(strFile + ValueFortag);
                                if (Fil.Exists)
                                {
                                    ValueFortag =  "prodimages\\"+ValueFortag.Replace("/","\\");
                                   // ValueLargeImg = HttpContext.Current.Server.MapPath(ValueFortag.ToLower().Replace("_th", "_Images_200"));
                                }
                                else
                                {
                                    ValueFortag ="/images/noimage.gif";
                                    ValueLargeImg = "";
                                }
                            }
                            else
                            {
                                ValueFortag= "images/noimage.gif";
                                ValueLargeImg = "";
                            }
                            string Popupdiv = "";


                            Popupdiv = "<img width='40px' alt='' src='" + ValueFortag + "' class='imgsize' />";
                           
                            //if (rowcolor == false)
                            //{
                           // strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'   \"><div>" + ValueFortag + "</div></td>");

                            strBldr.Append("<td  ALIGN='center' VALIGN='Middle' width='8%'  style='border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;font-family:Arial; '>" + Popupdiv + "</td>");
                            //}
                            //else if (rowcolor == true)
                            //{

                            //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  style='width: 200px'  \"><div class=\"pro_thum_outer\">" + Popupdiv + "</div></td>");

                            //}
                        }

                        else
                        {
                            if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                            {
                                if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
                                {
                                    ValueFortag = ReplaceText;
                                }
                                else
                                {
                                    if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) ))
                                    {
                                        if (AttrType == 4)
                                        {
                                            //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                            //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
                                            /* DB price 
                                            //if (tempPriceDt!=null)
                                            //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                            //else
                                            //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
                                             DB price */

                                            if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
                                                ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
                                            else
                                                ValueFortag = "";


                                        }
                                        else
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                        }
                                    }
                                    else
                                    {
                                        if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
                                        {
                                            ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                        }
                                        else
                                        {
                                            ValueFortag = string.Empty;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                {
                                    ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                }
                                else
                                {
                                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                }
                            }
                         
                            if (AttrType == 4)
                            {
                                _StockStatus = "NO STATUS AVAILABLE";
                                _AvilableQty = "0";
                                string _ProCode = string.Empty;
                                string _eta = string.Empty;
                                if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                    _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                if (tempPriceDt != null)
                                {
                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                    _eta = tempPriceDt.Rows[0]["ETA"].ToString();
                                }
                                string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
                                if (Convert.ToInt32(userid) > 0)
                                {

                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                                }
                                else
                                {
                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                }

                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                        bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                        {
                                            ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                        }
                                    }
                                }
                               
                                   // StrPriceTable = AssemblePriceTable_Print(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta, withprice, withdetails);
                               
                                    //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                               // ValueFortag =   ValueFortag ;
                               
                                // ValueFortag = "<div class=\"popupaero\"></div>";
                            }
                            //if (rowcolor == false)
                            //{
                            if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
                            //strBldrcost.Append("<td ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></td>");
                            {
                                if (withprice.ToLower() == "true")
                                {
                                    strBldrcost.Append("<td ALIGN=center VALIGN=Middle  width='15%' style='border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;font-size:13px;font-family:Arial; '>" + ValueFortag + "</td>");
                                    //strBldrcost.Append("<td style='width=100%;height=100%'>" + StrPriceTable + "</td>");
                                }
                                if (withprice.ToLower() == "false")
                                {
                                   // strBldrcost.Append("<td style='width=100%;height=100%'>" + StrPriceTable + "</td>");

                                }
                                }
                            else
                                strBldr.Append("<td ALIGN=center VALIGN=middle  width='15%' style='border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;font-size:13px;font-family:Arial; '>" + ValueFortag + "</td>");

                          
                         
                        }
                     
                        //Add the Shipping and Cart Images
                        if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                        {

                            ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                            //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                            //int IsAvailable = oPro.GetProductAvailability(ProdID);
                            string ShipImgPath = string.Empty;
                            int IsAvailable = 0;
                            _StockStatus = "NO STATUS AVAILABLE";
                            _AvilableQty = "0";
                            Boolean IsShipping = false;
                            if (tempPriceDt != null)
                            {
                                IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
                                if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
                                    IsAvailable = 1;
                                else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
                                    IsAvailable = 0;
                                _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                            }

                            if ((IsShipping))
                            {
                                ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
                                string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
                                ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                            }
                            else if (!(IsShipping))
                            {
                                ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                            }
                            string tempEAPath = string.Empty;
                            if ((_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString())))
                                tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                            else
                                tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));

                           

                            //if (rowcolor == false)
                            //{
                               // strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</td>");
                            //}
                            //else if (rowcolor == true)
                            //{
                            //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</td>");
                            //}
                            if (strBldrcost.ToString() != "")
                            {
                                strBldr.Append(strBldrcost);
                            }
                            if (EComState.ToUpper() == "YES")
                            {
                                //Add the Cart Image
                                string CartImgPath = "";
                                //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                if (Restricted.ToUpper() == "YES")
                                {
                                    CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                    string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                }
                               
                            }
                        }
                    }
                   
                }
            
            }

            strBldr.Append("</tr></table></div></td></tr></table>");
            //if (strBldr.ToString().Contains("<table border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><tr></tr></table>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
            return strBldr.ToString();
        }



        public string GenerateVerticalHTML_printnew(string _familyID, DataSet Ds, string withprice, string withdetails)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
            int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";

            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            bool isfirst = false;
            DataRow[] tempPriceDr;

            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            StringBuilder strBldrcost_price = new StringBuilder();
            StringBuilder strBldrcost1 = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;

            //<tr><td><table width='100%' border='1' style='border-style:solid' <tr> <td align='left' ><div > >
           // strBldr.Append("<table width='99%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");
            strBldr.Append("<table width='99%' border='0' style='margin-left:2px;' ><tr> <td align='left' ><div ><table width='100%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");

            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
            if ((DisplayHeaders))
            {
                strBldrcost = new StringBuilder();
                strBldrcost_price = new StringBuilder();
                strBldrcost1 = new StringBuilder();
              

                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    strBldr.Append("<tr>");
                    DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                        if (AttrType != 3)
                        {
                            if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
                            {
                                strBldrcost.Append("<td ALIGN='Center' VALIGN='Middle'  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%;'>");

                                if (pricecode == 1)
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldrcost.Append("</td>");
                            }
                            else
                            {
                                strBldr.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\"  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%'>");

                                if (pricecode == 1)
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldr.Append("</td>");
                            }
                        }
                        else
                        {
                            strBldr.Append("<td ALIGN='Center' VALIGN='Middle' style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%;'>");
                            strBldr.Append("</td>");
                        }
                    }
                    // }


                    // strBldr.Append("</tr>");
                    // }
                    string ValueFortag = string.Empty;
                    bool rowcolor = false;

                    if (_EA_Path == "" && _Category_id == "")
                    {
                        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {
                            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                        }
                    }

                    //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                    //{

                    for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
                    {
                        // strBldr.Append("<tr>");
                        if (!(rowcolor) && i != 0)
                        {
                            rowcolor = true;
                        }
                        else if ((rowcolor))
                        {
                            rowcolor = false;
                        }
                        tempPriceDr = DsPreview.Tables["ProductPrice_pdf"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                        if (tempPriceDr.Length > 0)
                            tempPriceDt = tempPriceDr.CopyToDataTable();
                        else
                            tempPriceDt = null;
                        //strBldrcost = new StringBuilder();
                        //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                        //{

                        string alignVal = "LEFT";

                        // DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                        if (tempdr.Length > 0)
                        {
                            ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                            AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


                            //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
                            //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                            //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                            //DataSet DSS = oHelper.GetDataSet();
                            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


                            //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                            if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
                            {
                                alignVal = "RIGHT";
                            }
                            if (AttrType == 3)
                            {
                                ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                                string ValueLargeImg = string.Empty;
                                if (ValueFortag != "" && ValueFortag != null)
                                {
                                    FileInfo Fil;
                                    string strFile = HttpContext.Current.Server.MapPath("ProdImages");

                                    Fil = new FileInfo(strFile + ValueFortag);
                                    if (Fil.Exists)
                                    {
                                        ValueFortag = "prodimages\\" + ValueFortag.Replace("/", "\\");
                                        // ValueLargeImg = HttpContext.Current.Server.MapPath(ValueFortag.ToLower().Replace("_th", "_Images_200"));
                                    }
                                    else
                                    {
                                        ValueFortag = "/images/noimage.gif";
                                        ValueLargeImg = "";
                                    }
                                }
                                else
                                {
                                    ValueFortag = "images/noimage.gif";
                                    ValueLargeImg = "";
                                }
                                string Popupdiv = string.Empty;


                                Popupdiv = "<div><img alt='' src='" + ValueFortag + "' /></div>";

                                //if (rowcolor == false)
                                //{
                                // strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'   \"><div>" + ValueFortag + "</div></td>");

                                strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 150;'>" + Popupdiv + "</td>");
                                //}
                                //else if (rowcolor == true)
                                //{

                                //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  style='width: 200px'  \"><div class=\"pro_thum_outer\">" + Popupdiv + "</div></td>");

                                //}
                            }

                            else
                            {
                                if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                                {
                                    if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
                                    {
                                        ValueFortag = ReplaceText;
                                    }
                                    else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
                                    {
                                        ValueFortag = ReplaceText;
                                    }
                                    else
                                    {
                                        if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                        {
                                            if (AttrType == 4)
                                            {
                                                //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                                //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
                                                /* DB price 
                                                //if (tempPriceDt!=null)
                                                //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                                //else
                                                //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
                                                 DB price */

                                                if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
                                                    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
                                                else
                                                    ValueFortag = "";


                                            }
                                            else
                                            {
                                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                            }
                                        }
                                        else
                                        {
                                            if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
                                            {
                                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                            }
                                            else
                                            {
                                                ValueFortag = string.Empty;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                    {
                                        ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                    }
                                    else
                                    {
                                        ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                    }
                                }

                                if (AttrType == 4)
                                {
                                    _StockStatus = "NO STATUS AVAILABLE";
                                    _AvilableQty = "0";
                                    string _ProCode = string.Empty;
                                    string _eta = string.Empty;
                                    if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                        _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                    if (tempPriceDt != null)
                                    {
                                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                        _eta = tempPriceDt.Rows[0]["ETA"].ToString();
                                    }
                                    string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
                                    if (Convert.ToInt32(userid) > 0)
                                    {

                                        dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                                    }
                                    else
                                    {
                                        dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                    }

                                    if (dsBgDisc != null)
                                    {
                                        if (dsBgDisc.Tables[0].Rows.Count > 0)
                                        {
                                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                            untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                            bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                            {
                                                ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                            }
                                        }
                                    }
                                    if (withdetails.ToLower() == "true")
                                    {
                                        StrPriceTable = AssemblePriceTable_Print(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta, withprice, withdetails);
                                    }
                                    //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                                    // ValueFortag =   ValueFortag ;

                                    // ValueFortag = "<div class=\"popupaero\"></div>";
                                }
                                //if (rowcolor == false)
                                //{
                                if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
                                //strBldrcost.Append("<td ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></td>");
                                {
                                    if (withprice.ToLower() == "true")
                                    {
                                        if (!(isfirst))
                                        {
                                            isfirst = true;
                                            strBldrcost_price.Append(strBldrcost);
                                           // strBldrcost1.Append("<td ALIGN=Center VALIGN=\"Middle\" style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>Price Table</td>");
                                        }
                                        strBldrcost_price.Append("<td ALIGN='center' VALIGN='Middle'  style='width:200;font-family:arial;'><div>" + ValueFortag + "</Div></td>");

                                        
                                        
                                       // strBldrcost1.Append("<td style='width=100%;height=100%'>" + StrPriceTable + "</td>");
                                    }
                                }



                                else
                                {
                                    strBldr.Append("<td ALIGN='center' VALIGN='middle'  style='width:200;font-family:arial;' >" + ValueFortag + "</td>");

                                }

                            }

                            //Add the Shipping and Cart Images
                            if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                            {

                                ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                                //int IsAvailable = oPro.GetProductAvailability(ProdID);
                                string ShipImgPath = string.Empty;
                                int IsAvailable = 0;
                                _StockStatus = "NO STATUS AVAILABLE";
                                _AvilableQty = "0";
                                Boolean IsShipping = false;
                                if (tempPriceDt != null)
                                {
                                    IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
                                    if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
                                        IsAvailable = 1;
                                    else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
                                        IsAvailable = 0;
                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                }

                                if ((IsShipping))
                                {
                                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
                                    string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
                                    ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                }
                                else if (!(IsShipping))
                                {
                                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                    ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                }
                                string tempEAPath = string.Empty;
                                if ((_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString())))
                                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                                else
                                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));



                                //if (rowcolor == false)
                                //{
                                // strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</td>");
                                //}
                                //else if (rowcolor == true)
                                //{
                                //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</td>");
                                //}
                                //if (strBldrcost.ToString() != "")
                                //{
                                //    strBldr.Append(strBldrcost);
                                //}
                                if (EComState.ToUpper() == "YES")
                                {
                                    //Add the Cart Image
                                    string CartImgPath = string.Empty;
                                    //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                    if (Restricted.ToUpper() == "YES")
                                    {
                                        CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                        string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                    }

                                }
                            }
                            // }

                            //}

                        }
                    }
                    strBldr.Append("</tr>");
                }
            }
            if (strBldrcost_price.ToString() != "")
            {
                strBldr.Append("<tr>" + strBldrcost_price + "</tr>");
            }
            if (strBldrcost1.ToString() != "")
            {
                strBldr.Append("<tr>" + strBldrcost1 + "</tr>");
            }
            strBldr.Append("</table></td></tr></table>");
            //</div></td></tr>if (strBldr.ToString().Contains("<table border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><tr></tr></table>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
            return strBldr.ToString();
        }

        public string GenerateVerticalHTML_blocks(string _familyID, DataSet Ds, string withprice, string withdetails,int startcnt,int endcount)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
            int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";

            string _Category_id = string.Empty;
            string _EA_Path = string.Empty;
            string StrPriceTable = string.Empty;
            bool isfirst = false;
            DataRow[] tempPriceDr;

            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();
            StringBuilder strBldrcost_price = new StringBuilder();
            StringBuilder strBldrcost1 = new StringBuilder();
            //Modified by Indu
            //if (HttpContext.Current.Request.QueryString["path"] != null)            
            //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
            if (HttpContext.Current.Session["EA"] != null)
            {
                _EA_Path = HttpContext.Current.Session["EA"].ToString();

            }
            else
            {
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
            }
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;

            //<tr><td><table width='100%' border='1' style='border-style:solid' <tr> <td align='left' ><div > >
            // strBldr.Append("<table width='99%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");
            strBldr.Append("<table width='99%' border='0' style='margin-left:2px;' ><tr> <td align='left' ><div ><table width='100%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");

            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
            if ((DisplayHeaders))
            {
                strBldrcost = new StringBuilder();
                strBldrcost_price = new StringBuilder();
                strBldrcost1 = new StringBuilder();


                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    strBldr.Append("<tr>");
                    DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                        if (AttrType != 3)
                        {
                            if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
                            {
                                strBldrcost.Append("<td ALIGN='Center' VALIGN='Middle'  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%;'>");

                                if (pricecode == 1)
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldrcost.Append("</td>");
                            }
                            else
                            {
                                strBldr.Append("<td ALIGN=\"Center\" VALIGN=\"Middle\"  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%'>");

                                if (pricecode == 1)
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
                                }
                                else
                                {
                                    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
                                }

                                strBldr.Append("</td>");
                            }
                        }
                        else
                        {
                            strBldr.Append("<td ALIGN='Center' VALIGN='Middle' style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:15%;'>");
                            strBldr.Append("</td>");
                        }
                    }
                    // }


                    // strBldr.Append("</tr>");
                    // }
                    string ValueFortag = string.Empty;
                    bool rowcolor = false;

                    if (_EA_Path == "" && _Category_id == "")
                    {
                        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {
                            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
                            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                        }
                    }

                    //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                    //{

                    for (int i = startcnt; i < endcount; i++)
                    {
                        // strBldr.Append("<tr>");
                        if (!(rowcolor) && i != 0)
                        {
                            rowcolor = true;
                        }
                        else if ((rowcolor))
                        {
                            rowcolor = false;
                        }
                        tempPriceDr = DsPreview.Tables["ProductPrice_pdf"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                        if (tempPriceDr.Length > 0)
                            tempPriceDt = tempPriceDr.CopyToDataTable();
                        else
                            tempPriceDt = null;
                        //strBldrcost = new StringBuilder();
                        //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                        //{

                        string alignVal = "LEFT";

                        // DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                        if (tempdr.Length > 0)
                        {
                            ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                            AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


                            //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
                            //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
                            //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
                            //DataSet DSS = oHelper.GetDataSet();
                            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


                            //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
                            if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
                            {
                                alignVal = "RIGHT";
                            }
                            if (AttrType == 3)
                            {
                                ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                                string ValueLargeImg = string.Empty;
                                if (ValueFortag != "" && ValueFortag != null)
                                {
                                    FileInfo Fil;
                                    string strFile = HttpContext.Current.Server.MapPath("ProdImages");

                                    Fil = new FileInfo(strFile + ValueFortag);
                                    if (Fil.Exists)
                                    {
                                        ValueFortag = "prodimages\\" + ValueFortag.Replace("/", "\\");
                                        // ValueLargeImg = HttpContext.Current.Server.MapPath(ValueFortag.ToLower().Replace("_th", "_Images_200"));
                                    }
                                    else
                                    {
                                        ValueFortag = "/images/noimage.gif";
                                        ValueLargeImg = "";
                                    }
                                }
                                else
                                {
                                    ValueFortag = "images/noimage.gif";
                                    ValueLargeImg = "";
                                }
                                string Popupdiv = string.Empty;


                                Popupdiv = "<div><img alt='' src='" + ValueFortag + "' /></div>";

                                //if (rowcolor == false)
                                //{
                                // strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'   \"><div>" + ValueFortag + "</div></td>");

                                strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 150;'>" + Popupdiv + "</td>");
                                //}
                                //else if (rowcolor == true)
                                //{

                                //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  style='width: 200px'  \"><div class=\"pro_thum_outer\">" + Popupdiv + "</div></td>");

                                //}
                            }

                            else
                            {
                                if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
                                {
                                    if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
                                    {
                                        ValueFortag = ReplaceText;
                                    }
                                    else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
                                    {
                                        ValueFortag = ReplaceText;
                                    }
                                    else
                                    {
                                        if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                        {
                                            if (AttrType == 4)
                                            {
                                                //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                                //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
                                                /* DB price 
                                                //if (tempPriceDt!=null)
                                                //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                                //else
                                                //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
                                                 DB price */

                                                if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
                                                    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
                                                else
                                                    ValueFortag = "";


                                            }
                                            else
                                            {
                                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                            }
                                        }
                                        else
                                        {
                                            if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
                                            {
                                                ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
                                            }
                                            else
                                            {
                                                ValueFortag = string.Empty;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"))))
                                    {
                                        ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
                                    }
                                    else
                                    {
                                        ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                    }
                                }

                                if (AttrType == 4)
                                {
                                    _StockStatus = "NO STATUS AVAILABLE";
                                    _AvilableQty = "0";
                                    string _ProCode = string.Empty;
                                    string _eta = string.Empty;
                                    if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                        _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                    if (tempPriceDt != null)
                                    {
                                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                        _eta = tempPriceDt.Rows[0]["ETA"].ToString();
                                    }
                                    string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
                                    if (Convert.ToInt32(userid) > 0)
                                    {

                                        dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                                    }
                                    else
                                    {
                                        dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                    }

                                    if (dsBgDisc != null)
                                    {
                                        if (dsBgDisc.Tables[0].Rows.Count > 0)
                                        {
                                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                            untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                            bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                            {
                                                ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                            }
                                        }
                                    }
                                    if (withdetails.ToLower() == "true")
                                    {
                                        StrPriceTable = AssemblePriceTable_Print(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta, withprice, withdetails);
                                    }
                                    //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                                    // ValueFortag =   ValueFortag ;

                                    // ValueFortag = "<div class=\"popupaero\"></div>";
                                }
                                //if (rowcolor == false)
                                //{
                                if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
                                //strBldrcost.Append("<td ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></td>");
                                {
                                    if (withprice.ToLower() == "true")
                                    {
                                        if (!(isfirst))
                                        {
                                            isfirst = true;
                                            strBldrcost_price.Append(strBldrcost);
                                            // strBldrcost1.Append("<td ALIGN=Center VALIGN=\"Middle\" style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>Price Table</td>");
                                        }
                                        strBldrcost_price.Append("<td ALIGN='center' VALIGN='Middle'  style='width:200;font-family:arial;'><div>" + ValueFortag + "</Div></td>");



                                        // strBldrcost1.Append("<td style='width=100%;height=100%'>" + StrPriceTable + "</td>");
                                    }
                                }



                                else
                                {
                                    strBldr.Append("<td ALIGN='center' VALIGN='middle'  style='width:200;font-family:arial;' >" + ValueFortag + "</td>");

                                }

                            }

                            //Add the Shipping and Cart Images
                            if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                            {

                                ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                                //int IsAvailable = oPro.GetProductAvailability(ProdID);
                                string ShipImgPath = string.Empty;
                                int IsAvailable = 0;
                                _StockStatus = "NO STATUS AVAILABLE";
                                _AvilableQty = "0";
                                Boolean IsShipping = false;
                                if (tempPriceDt != null)
                                {
                                    IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
                                    if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
                                        IsAvailable = 1;
                                    else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
                                        IsAvailable = 0;
                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                }

                                if ((IsShipping))
                                {
                                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
                                    string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
                                    ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                }
                                else if (!(IsShipping))
                                {
                                    ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                    ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                }
                                string tempEAPath = string.Empty;
                                if ((_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString())))
                                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
                                else
                                    tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));



                                //if (rowcolor == false)
                                //{
                                // strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</td>");
                                //}
                                //else if (rowcolor == true)
                                //{
                                //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</td>");
                                //}
                                //if (strBldrcost.ToString() != "")
                                //{
                                //    strBldr.Append(strBldrcost);
                                //}
                                if (EComState.ToUpper() == "YES")
                                {
                                    //Add the Cart Image
                                    string CartImgPath = string.Empty;
                                    //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                    if (Restricted.ToUpper() == "YES")
                                    {
                                        CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                        string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                    }

                                }
                            }
                            // }

                            //}

                        }
                    }
                    strBldr.Append("</tr>");
                }
            }
            if (strBldrcost_price.ToString() != "")
            {
                strBldr.Append("<tr>" + strBldrcost_price + "</tr>");
            }
            if (strBldrcost1.ToString() != "")
            {
                strBldr.Append("<tr>" + strBldrcost1 + "</tr>");
            }
            strBldr.Append("</table></td></tr></table>");
            //</div></td></tr>if (strBldr.ToString().Contains("<table border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><tr></tr></table>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
            return strBldr.ToString();
        }


        //public string GenerateVerticalHTML_2Line(string _familyID, DataSet Ds, string withprice, string withdetails)
        //{
        //    //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
        //    DataSet dsBgDisc = new DataSet();
        //    decimal untPrice = 0;
        //    string AttrID = string.Empty;
        //    //  string HypColumn = "";
        //    int Min_ord_qty = 0;
        //    //  int Qty_avail;
        //    //  int flagtemp = 0;
        //    string _StockStatus = "NO STATUS AVAILABLE";
        //    string _AvilableQty = "0";

        //    string _Category_id = "";
        //    string _EA_Path = "";
        //    string StrPriceTable = "";
        //    bool isfirst = false;
        //    DataRow[] tempPriceDr;

        //    DataTable tempPriceDt;
        //    //int ProdID;
        //    int AttrType;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();

        //    string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //    string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //    string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //    string _parentFamily_Id = "0";
        //    if (EComState == "YES")
        //        if (!objHelperServices.GetIsEcomEnabled(userid))
        //            EComState = "NO";
        //    StringBuilder strBldr = new StringBuilder();
        //    StringBuilder strBldrcost = new StringBuilder();
        //    StringBuilder strBldrcost_price = new StringBuilder();
        //    StringBuilder strBldrcost1 = new StringBuilder();
        //    //Modified by Indu
        //    //if (HttpContext.Current.Request.QueryString["path"] != null)            
        //    //    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   
        //    if (HttpContext.Current.Session["EA"] != null)
        //    {
        //        _EA_Path = HttpContext.Current.Session["EA"].ToString();

        //    }
        //    else
        //    {
        //        _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());
        //    }
        //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //        _Category_id = HttpContext.Current.Request.QueryString["cid"];

        //    DsPreview = Ds;
        //    if (DsPreview.Tables[_familyID] == null)
        //        return "";


        //    DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //    if (_parentFamily_Id == "0")
        //        _parentFamily_Id = _familyID;

        //    //<tr><td><table width='100%' border='1' style='border-style:solid' <tr> <td align='left' ><div > >
        //    // strBldr.Append("<table width='99%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");
        //    //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");
        //    strBldr.Append("<table width='99%' border='0' style='margin-left:2px;' ><tr> <td align='left' ><div ><table width='100%' border=1 cellspacing=1 style='border-bottom: none;border-right: none; border-top:1px solid #E8E8E8 ; border-left:1px solid #E8E8E8;'>");

        //    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //    //oHelper.SQLString = sSQL;
        //    //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //    pricecode = GetPriceCode();
        //    DisplayHeaders = true;
        //    if (DisplayHeaders == true)
        //    {
        //        strBldrcost = new StringBuilder();
        //        strBldrcost_price = new StringBuilder();
        //        strBldrcost1 = new StringBuilder();


        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
        //            strBldr.Append("<tr>");
        //            DataRow[] tempdr = DsPreview.Tables["Attribute_pdf"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
        //                if (AttrType != 3)
        //                {
        //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
        //                    {
        //                        strBldrcost.Append("<td><table><tr><td ALIGN='Center' VALIGN='Middle'  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:30px;'>");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldrcost.Append("</td></tr></table></td>");
        //                    }
        //                    else
        //                    {
        //                        strBldr.Append("<td><table><tr><td ALIGN=\"Center\" VALIGN=\"Middle\"  style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:30px'>");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldr.Append("</td></tr></table></td>");
        //                    }
        //                }
        //                else
        //                {
        //                    strBldr.Append("<td><table><tr><td ALIGN='Center' VALIGN='Middle' style='background-color: #007BDB;color: #FFFFFF;font-weight: bold;text-align: center;border-bottom: 1px solid #E8E8E8;border-right: 1px solid #E8E8E8; border-top:none ; border-left:none;height:30px; font-size:11px;font-family:Arial;width:30px;'>");
        //                    strBldr.Append("</td></tr></table></td>");
        //                }
        //            }
        //            // }


        //            // strBldr.Append("</tr>");
        //            // }
        //            string ValueFortag = string.Empty;
        //            bool rowcolor = false;

        //            if (_EA_Path == "" && _Category_id == "")
        //            {
        //                DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //                if (tmpds != null && tmpds.Tables.Count > 0)
        //                {
        //                    _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + _familyID.ToString();
        //                    _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //                }
        //            }

        //            //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //            //{

        //            for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //            {
        //                // strBldr.Append("<tr>");
        //                if ((i == 0)&& (tempdr.Length > 0))
        //                {
        //                    strBldr.Append("<td><table width='700'><tr height='20'>");

        //                }
        //                if (rowcolor == false && i != 0)
        //                {
        //                    rowcolor = true;
        //                }
        //                else if (rowcolor == true)
        //                {
        //                    rowcolor = false;
        //                }
        //                tempPriceDr = DsPreview.Tables["ProductPrice_pdf"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //                if (tempPriceDr.Length > 0)
        //                    tempPriceDt = tempPriceDr.CopyToDataTable();
        //                else
        //                    tempPriceDt = null;
        //                //strBldrcost = new StringBuilder();
        //                //for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //                //{

        //                string alignVal = "LEFT";

        //                // DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //                if (tempdr.Length > 0)
        //                {
        //                    ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
        //                    AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


        //                    //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
        //                    //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
        //                    //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
        //                    //DataSet DSS = oHelper.GetDataSet();
        //                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


        //                    //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                    if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                    {
        //                        alignVal = "RIGHT";
        //                    }
        //                    if (AttrType == 3)
        //                    {
        //                        ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
        //                        string ValueLargeImg = "";
        //                        if (ValueFortag != "" && ValueFortag != null)
        //                        {
        //                            FileInfo Fil;
        //                            string strFile = HttpContext.Current.Server.MapPath("ProdImages");

        //                            Fil = new FileInfo(strFile + ValueFortag);
        //                            if (Fil.Exists)
        //                            {
        //                                ValueFortag = "prodimages\\" + ValueFortag.Replace("/", "\\");
        //                                // ValueLargeImg = HttpContext.Current.Server.MapPath(ValueFortag.ToLower().Replace("_th", "_Images_200"));
        //                            }
        //                            else
        //                            {
        //                                ValueFortag = "/images/noimage.gif";
        //                                ValueLargeImg = "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = "images/noimage.gif";
        //                            ValueLargeImg = "";
        //                        }
        //                        string Popupdiv = "";


        //                        Popupdiv = "<div><img alt='' src='" + ValueFortag + "' /></div>";

        //                        //if (rowcolor == false)
        //                        //{
        //                        // strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 200;'   \"><div>" + ValueFortag + "</div></td>");

        //                        strBldr.Append("<td  ALIGN='center' VALIGN='Middle' style='width: 150;'>" + Popupdiv + "</td>");
        //                        //}
        //                        //else if (rowcolor == true)
        //                        //{

        //                        //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  style='width: 200px'  \"><div class=\"pro_thum_outer\">" + Popupdiv + "</div></td>");

        //                        //}
        //                    }

        //                    else
        //                    {
        //                        if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                        {
        //                            if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
        //                            {
        //                                ValueFortag = ReplaceText;
        //                            }
        //                            else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
        //                            {
        //                                ValueFortag = ReplaceText;
        //                            }
        //                            else
        //                            {
        //                                if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                                {
        //                                    if (AttrType == 4)
        //                                    {
        //                                        //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                                        //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
        //                                        /* DB price 
        //                                        //if (tempPriceDt!=null)
        //                                        //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
        //                                        //else
        //                                        //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
        //                                         DB price */

        //                                        if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
        //                                            ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
        //                                        else
        //                                            ValueFortag = "";


        //                                    }
        //                                    else
        //                                    {
        //                                        ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
        //                                    {
        //                                        ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                    }
        //                                    else
        //                                    {
        //                                        ValueFortag = string.Empty;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                            {
        //                                ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                            }
        //                            else
        //                            {
        //                                ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                            }
        //                        }

        //                        if (AttrType == 4)
        //                        {
        //                            _StockStatus = "NO STATUS AVAILABLE";
        //                            _AvilableQty = "0";
        //                            string _ProCode = "";
        //                            string _eta = "";
        //                            if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                                _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
        //                            if (tempPriceDt != null)
        //                            {
        //                                _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                                _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                                _eta = tempPriceDt.Rows[0]["ETA"].ToString();
        //                            }
        //                            string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
        //                            if (Convert.ToInt32(userid) > 0)
        //                            {

        //                                dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
        //                            }
        //                            else
        //                            {
        //                                dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
        //                            }

        //                            if (dsBgDisc != null)
        //                            {
        //                                if (dsBgDisc.Tables[0].Rows.Count > 0)
        //                                {
        //                                    decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
        //                                    DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
        //                                    string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
        //                                    untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                                    bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
        //                                    if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
        //                                    {
        //                                        ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

        //                                    }
        //                                }
        //                            }
        //                            if (withdetails.ToLower() == "true")
        //                            {
        //                                StrPriceTable = AssemblePriceTable_Print(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _eta, withprice, withdetails);
        //                            }
        //                            //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
        //                            // ValueFortag =   ValueFortag ;

        //                            // ValueFortag = "<div class=\"popupaero\"></div>";
        //                        }
        //                        //if (rowcolor == false)
        //                        //{
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                        //strBldrcost.Append("<td ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></td>");
        //                        {
        //                            if (withprice.ToLower() == "true")
        //                            {
        //                                if (isfirst == false)
        //                                {
        //                                    isfirst = true;
        //                                    strBldrcost_price.Append(strBldrcost);
        //                                    // strBldrcost1.Append("<td ALIGN=Center VALIGN=\"Middle\" style='background-color: #007BDB;color: #FFFFFF; font-weight: bold; text-align: center;border-bottom: 1px solid #E8E8E8; border-right: 1px solid #E8E8E8; height:23; font-size:11px;'>Price Table</td>");
        //                                }
        //                                strBldrcost_price.Append("<td ALIGN='center' VALIGN='Middle'  style='width:200;font-family:arial;'><div>" + ValueFortag + "</Div></td>");



        //                                // strBldrcost1.Append("<td style='width=100%;height=100%'>" + StrPriceTable + "</td>");
        //                            }
        //                        }



        //                        else
        //                        {
        //                            strBldr.Append("<td ALIGN='center' VALIGN='middle'  style='width:200;font-family:arial;' >" + ValueFortag + "</td>");

        //                        }

        //                    }

        //                    //Add the Shipping and Cart Images
        //                    if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
        //                    {

        //                        ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                        //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
        //                        //int IsAvailable = oPro.GetProductAvailability(ProdID);
        //                        string ShipImgPath = "";
        //                        int IsAvailable = 0;
        //                        _StockStatus = "NO STATUS AVAILABLE";
        //                        _AvilableQty = "0";
        //                        Boolean IsShipping = false;
        //                        if (tempPriceDt != null)
        //                        {
        //                            IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
        //                            if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
        //                                IsAvailable = 1;
        //                            else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
        //                                IsAvailable = 0;
        //                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                        }

        //                        if (IsShipping == true)
        //                        {
        //                            ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
        //                            string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
        //                            ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
        //                        }
        //                        else if (IsShipping == false)
        //                        {
        //                            ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
        //                            ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
        //                        }
        //                        string tempEAPath = "";
        //                        if (_EA_Path.Contains("Family Id=" + _parentFamily_Id.ToString()) == true)
        //                            tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path));
        //                        else
        //                            tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch1=Family Id=" + _parentFamily_Id.ToString()));



        //                        //if (rowcolor == false)
        //                        //{
        //                        // strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</td>");
        //                        //}
        //                        //else if (rowcolor == true)
        //                        //{
        //                        //    strBldr.Append("<td  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</td>");
        //                        //}
        //                        //if (strBldrcost.ToString() != "")
        //                        //{
        //                        //    strBldr.Append(strBldrcost);
        //                        //}
        //                        if (EComState.ToUpper() == "YES")
        //                        {
        //                            //Add the Cart Image
        //                            string CartImgPath = "";
        //                            //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
        //                            if (Restricted.ToUpper() == "YES")
        //                            {
        //                                CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
        //                                string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
        //                                CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
        //                            }

        //                        }
        //                    }
        //                    // }

        //                    //}

        //                }
        //                if (i == 7)
        //                {
        //                    strBldr.Append("</tr><tr>");
        //                }
        //            }
        //            strBldr.Append("</tr></table></td>");

        //        }
        //        strBldr.Append("</tr>");
                



        //    }
        //    if (strBldrcost_price.ToString() != "")
        //    {
        //        strBldr.Append("<tr>" + strBldrcost_price + "</tr>");
        //    }
        //    if (strBldrcost1.ToString() != "")
        //    {
        //        strBldr.Append("<tr>" + strBldrcost1 + "</tr>");
        //    }
        //    strBldr.Append("</table></td></tr></table>");
        //    //</div></td></tr>if (strBldr.ToString().Contains("<table border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><tr></tr></table>"))
        //    //{
        //    //    strBldr = strBldr.Remove(0, strBldr.Length);
        //    //}
        //    return strBldr.ToString();
        //}
        /*********************************** OLD CODE ***********************************/
        //private bool IsEcomenabled()
        //{
        //    bool retvalue = false;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //    string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //    oHelper.SQLString = sSQL;
        //    int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
        //    if (iROLE <= 3)
        //        retvalue = true;
        //    return retvalue;
        //}

        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CURRENCY FORMAT EXTRACTION DETAILS ***/
        /********************************************************************************/
        private void ExtractCurrenyFormat(string AttributeValue)
        {
            //AppLoader.DBConnection Oocn = new DBConnection();
            string XMLstr = string.Empty;
           // DataSet dscURRENCY = new DataSet();
            //oHelper.SQLString = " SELECT ATTRIBUTE_DATARULE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID  =" + AttributeID + " ";
            //dscURRENCY = oHelper.GetDataSet();
            Prefix = string.Empty; Suffix = string.Empty; EmptyCondition = string.Empty; ReplaceText = string.Empty; Headeroptions = string.Empty;
            //if (dscURRENCY.Tables[0].Rows.Count > 0)
            //{
                if (AttributeValue!= string.Empty)
                {
                    XMLstr = AttributeValue; //dscURRENCY.Tables[0].Rows[0].ItemArray[0].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(XMLstr);
                    XmlNode rootNode = xmlDOc.DocumentElement;
                    {
                        XmlNodeList xmlNodeList;
                        xmlNodeList = rootNode.ChildNodes;

                        for (int xmlNode = 0; xmlNode < xmlNodeList.Count; xmlNode++)
                        {
                            if (xmlNodeList[xmlNode].ChildNodes.Count > 0)
                            {
                                if (xmlNodeList[xmlNode].ChildNodes[0].LastChild != null)
                                {
                                    Prefix = xmlNodeList[xmlNode].ChildNodes[0].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[1].LastChild != null)
                                {
                                    Suffix = xmlNodeList[xmlNode].ChildNodes[1].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[2].LastChild != null)
                                {
                                    EmptyCondition = xmlNodeList[xmlNode].ChildNodes[2].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[3].LastChild != null)
                                {
                                    ReplaceText = xmlNodeList[xmlNode].ChildNodes[3].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[4].LastChild != null)
                                {
                                    Headeroptions = xmlNodeList[xmlNode].ChildNodes[4].LastChild.Value;
                                }

                            }
                        }
                    }



               }
            //}
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK STRING VALUE IS MATCHED WITH NUMBERS OR NOT USING REGULAR EXPRESSION VALIDATOR ***/
        /********************************************************************************/ 
        public bool Isnumber(string RefValue)
        {
            //string StrRegx = "^[0-9.]";
            //string StrRegx =@"(^-?\d\d*$)"; jai
            //string StrRegx = @"^d[0-9.]+$";
            string StrRegx = @"^[0-9]*(\.)?[0-9]+$";
            bool Retval = false;
            Regex re = new Regex(StrRegx);
            if (re.IsMatch(RefValue))
            {
                Retval = true;
            }
            else
            {
                Retval = false;
            }
            return Retval;
        }

        /*********************************** OLD CODE ***********************************/
        //private decimal GetMyPrice(int ProductID)
        //{
        //    decimal retval = 0.00M;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    if (!string.IsNullOrEmpty(userid))
        //    {
        //        string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //        oHelper.SQLString = sSQL;
        //        int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

        //        string strquery = "";
        //        if (pricecode == 1)
        //        {
        //            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //        }
        //        else
        //        {
        //            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //        }

        //        DataSet DSprice = new DataSet();
        //        oHelper.SQLString = strquery;
        //        retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
        //    }
        //    return retval;
        //}
        //private string GetStockStatus(int ProductID)
        //{
        //    string Retval = "NO STATUS AVAILABLE";
        //    try
        //    {
        //        string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
        //        oHelper.SQLString = sSQL;
        //        Retval = oHelper.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
        //    }
        //    catch
        //    {
        //    }
        //    return Retval;
        //}
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //private string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus)
        //{
        //    string _sPriceTable = "";
        //    SqlConnection oSQLCon = null;
        //    try
        //    {

        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        DataSet dsPriceTable = new DataSet();
        //        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //        //oHelper.SQLString = sSQL;
        //        //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //        //DataSet dsPriceTable = new DataSet();
        //        //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //        //oSQLCon.Open();
        //        //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
        //        //oCmd.Parameters.Clear();
        //        //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
        //        //string _sCODE = oCmd.ExecuteScalar().ToString();

        //        //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
        //        //oCmd.Parameters.Clear();
        //        //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
        //        //string stkstatus = oCmd.ExecuteScalar().ToString();
        //        int pricecode = _priceCode;
        //        string _sCODE = _ProCode;
        //        string stkstatus = _ProStkStatus;
        //        string _Tbt_Stock_Status = "";
        //        string _Tbt_Stock_Status_1 = "";
        //        bool _Tbt_Stock_Status_2 = false;
        //        string _Tbt_Stock_Status_3 = "";
        //        string _Colorcode1 = "";
        //        string _Colorcode;
        //        string StockStatus = stkstatus.Replace("_", " ");
        //        string _StockStatusTrim = StockStatus.Trim();

        //        switch (_StockStatusTrim)
        //        {
        //            case "IN STOCK":
        //                _Tbt_Stock_Status = "<span style='color:#43A246'><b>INSTOCK</b></span><br>";
        //                _Tbt_Stock_Status_2 = true;
        //                break;
        //            case "SPECIAL ORDER":
        //                _Colorcode = "#43A246";
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE &":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "DISCONTINUED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER AVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE NO ETA":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "OUT OF STOCK":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'> <b>ITEM WILL BE BACK ORDERED</b> </span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL":
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            default:
        //                _Colorcode = "Black";
        //                _Tbt_Stock_Status = _StockStatusTrim;
        //                break;
        //        }

        //        //SqlDataAdapter oDa = new SqlDataAdapter();
        //        //oDa.SelectCommand = new SqlCommand();
        //        //oDa.SelectCommand.Connection = oSQLCon;
        //        //oDa.SelectCommand.CommandText = "GetPriceTable";
        //        //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        //oDa.SelectCommand.Parameters.Clear();
        //        //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
        //        //oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
        //        //oDa.Fill(dsPriceTable, "PriceTable");
        //        dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
        //        dsPriceTable.Tables[0].TableName = "PriceTable";

        //        _sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
        //        _sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
        //        _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
        //        _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
        //        if (_Tbt_Stock_Status != "")
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
        //        }
        //        else
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
        //        }
        //        _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

        //        int TotalCount = 0;
        //        int RowCount = 0;

        //        if (pricecode == 3)
        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
        //            }
        //        else
        //        {
        //            bool bLastRow = false;
        //            TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
        //            RowCount = 0;

        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                RowCount = RowCount + 1;
        //                if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
        //                {
        //                    bLastRow = true;
        //                }

        //                string _color = bLastRow ? "bg_grey31" : "bg_grey3";
        //                _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
        //            }
        //        }

        //        _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    return _sPriceTable;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE TABLE DETAILS BASED ON THE MATCH CASES  ***/
        /********************************************************************************/
        public string AssemblePriceTable(string userid,int ProductID, int _priceCode, string _ProCode, string _ProStkStatus, string _Eta)
        {
            string _sPriceTable = string.Empty;
            SqlConnection oSQLCon = null;
            try
            {
                /*********************************** OLD CODE ***********************************/
                ////////  string userid = HttpContext.Current.Session["USER_ID"].ToString();
                ////////  DataSet dsPriceTable = new DataSet();
                ////////  //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                ////////  //oHelper.SQLString = sSQL;
                ////////  //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                ////////  //DataSet dsPriceTable = new DataSet();
                ////////  //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                ////////  //oSQLCon.Open();
                ////////  //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                ////////  //oCmd.Parameters.Clear();
                ////////  //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////  //string _sCODE = oCmd.ExecuteScalar().ToString();

                ////////  //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                ////////  //oCmd.Parameters.Clear();
                ////////  //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////  //string stkstatus = oCmd.ExecuteScalar().ToString();
                ////////  int pricecode = _priceCode;
                ////////  string _sCODE = _ProCode;
                ////////  string stkstatus = _ProStkStatus;
                ////////  string _Tbt_Stock_Status = "";
                ////////  string _Tbt_Stock_Status_1 = "";
                ////////  bool _Tbt_Stock_Status_2 = false;
                ////////  string _Tbt_Stock_Status_3 = "";
                ////////  string _Colorcode1 = "";
                ////////  string _Colorcode;
                ////////  string StockStatus = stkstatus.Replace("_", " ");
                ////////  string _StockStatusTrim = StockStatus.Trim();

                ////////  switch (_StockStatusTrim)
                ////////  {
                ////////      case "IN STOCK":
                ////////          _Tbt_Stock_Status = "<span style='color:#43A246'><b>INSTOCK</b></span><br>";
                ////////          _Tbt_Stock_Status_2 = true;
                ////////          break;
                ////////      case "SPECIAL ORDER":
                ////////          _Colorcode = "#43A246";
                ////////          _Tbt_Stock_Status_2 = true;
                ////////          _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                ////////          break;
                ////////      case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                ////////          _Tbt_Stock_Status_2 = true;
                ////////          _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                ////////          break;
                ////////      case "SPECIAL ORDER PRICE &":
                ////////          _Tbt_Stock_Status_2 = true;
                ////////          _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                ////////          break;
                ////////      case "DISCONTINUED":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                ////////          break;
                ////////      case "DISCONTINUED NO LONGER AVAILABLE":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                ////////          break;
                ////////      case "DISCONTINUED NO LONGER":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                ////////          break;
                ////////      case "TEMPORARY UNAVAILABLE":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                ////////          break;
                ////////      case "TEMPORARY UNAVAILABLE NO ETA":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                ////////          break;
                ////////      case "OUT OF STOCK":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                ////////          break;
                ////////      case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
                ////////          break;
                ////////      case "OUT OF STOCK ITEM WILL":
                ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                ////////          _Tbt_Stock_Status_2 = false;
                ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
                ////////          break;
                ////////      default:
                ////////          _Colorcode = "Black";
                ////////          _Tbt_Stock_Status = _StockStatusTrim;
                ////////          break;
                ////////  }


                ////////  dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(  userid));
                ////////  dsPriceTable.Tables[0].TableName = "PriceTable";

                ////////  //_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                ////////  //_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b><br />";
                ////////  //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                ////////  //_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";

                ////////  //if (_Tbt_Stock_Status != "")
                ////////  //{
                ////////  //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                ////////  //}
                ////////  //else
                ////////  //{
                ////////  //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                ////////  //}
                ////////  _sPriceTable = "<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr class=\" success\">";
                ////////  _sPriceTable += "<td width=\"100\" height=\"20\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b></td><td width=\"100\" valign=\"top\" class=\"table_bdr\"><b>STOCK STATUS</b></td></tr>";
                ////////  _sPriceTable += "<tr><td>";
                ////////  _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                //////////  _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b>";

                ////////  if (_Tbt_Stock_Status != "")
                ////////  {
                ////////      _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                ////////  }
                ////////  else
                ////////  {
                ////////      _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                ////////  }



                ////////  _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

                ////////  int TotalCount = 0;
                ////////  int RowCount = 0;

                ////////  if (pricecode == 3)
                ////////      foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                ////////      {
                ////////          _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                ////////      }
                ////////  else
                ////////  {
                ////////      bool bLastRow = false;
                ////////      TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                ////////      RowCount = 0;

                ////////      foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                ////////      {
                ////////          RowCount = RowCount + 1;
                ////////          if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                ////////          {
                ////////              bLastRow = true;
                ////////          }

                ////////          string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                ////////          _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                ////////      }
                ////////  }

                //////// // _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                ////////  _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table>";
                ////////  _sPriceTable += "<div class=\"popupaero\"></div>";
                /*********************************** OLD CODE ***********************************/


           
                DataSet dsPriceTable = new DataSet();
                //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //oHelper.SQLString = sSQL;
                //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                //DataSet dsPriceTable = new DataSet();
                //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                //oSQLCon.Open();
                //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                //oCmd.Parameters.Clear();
                //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                //string _sCODE = oCmd.ExecuteScalar().ToString();

                //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                //oCmd.Parameters.Clear();
                //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                //string stkstatus = oCmd.ExecuteScalar().ToString();
                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatus;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span >INSTOCK</span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        //_Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span >SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span >SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED</span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED</span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }


                dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
                dsPriceTable.Tables[0].TableName = "PriceTable";


                //_sPriceTable = "<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"table table-striped  table-bordered table-condensed\"><tr class=\" success\">";
                //_sPriceTable += "<td width=\"100\" height=\"20\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b></td><td width=\"100\" valign=\"top\" class=\"table_bdr\"><b>STOCK STATUS</b></td></tr>";
                //_sPriceTable += "<tr><td>";
                //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);


                //if (_Tbt_Stock_Status != "")
                //{
                //    _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //}
                //else
                //{
                //    _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //}

                _sPriceTable += "<table  class=\"table table-striped  table-bordered table-condensed\" style=\"margin:0; background:#FFF\">";
                _sPriceTable += "<tr class=\"success\"><td width=\"28%\">ORDER CODE:</td><td colspan=\"2\">STOCK STATUS</td></tr>";
                _sPriceTable += string.Format("<tr><td width=\"28%\">{0}</td>", _sCODE);
                if (_Tbt_Stock_Status != "")
                {
                    _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                }
                else
                {
                    _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                }

                if (_Eta != "")
                {
                    _sPriceTable += string.Format("<tr ><td><b>ETA</b></td><td colspan=\"2\"><b>" + _Eta + "</b></td></tr>");

                }

                _sPriceTable += "<tr class=\"success\"><td>Qty</td><td width=\"38%\">Cost Inc GST</td><td width=\"34%\">Cost Ex GST</td></tr>";

                int TotalCount = 0;
                int RowCount = 0;
                string[] P1 = null;
                string[] P2 = null;
                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        P1 = oDr["Price1"].ToString().Split('.');
                        P2 = oDr["Price2"].ToString().Split('.');
                        if (P1[1].Length >= 4 && P2[1].Length >= 4)
                        {
                            if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.0000}</td><td align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                            else
                                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                        }
                        else
                            _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);

                    }
                else
                {
                    bool bLastRow = false;
                    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                    RowCount = 0;

                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        RowCount = RowCount + 1;
                        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                        {
                            bLastRow = true;
                        }

                        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                        P1 = oDr["Price1"].ToString().Split('.');
                        P2 = oDr["Price2"].ToString().Split('.');
                        if (P1[1].Length >= 4 && P2[1].Length >= 4)
                        {
                            if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            {
                                _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.0000}</td><td class=\"{3}\" align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            }
                            else
                                _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);

                        }
                        else
                            _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                }
                _sPriceTable += "</table>";
                // _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                //_sPriceTable += "</table><div class=\"clear\"></div>";
                //_sPriceTable += "<div class=\"popupaero\"></div>";



            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable;
        }

        public string AssemblePriceTable_Print(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus, string _Eta, string withprice, string withdetails)
        {
            string _sPriceTable = string.Empty;
            SqlConnection oSQLCon = null;
            try
            {


                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                DataSet dsPriceTable = new DataSet();

                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatus;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span><b>INSTOCK</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span ><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }


                dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
                dsPriceTable.Tables[0].TableName = "PriceTable";



                _sPriceTable += "<table width='100%' cellpadding='1' border='1' cellspacing='0' style=' background:#FFF;font-size:9;'>";
                _sPriceTable += "<tr style='border:1;border-style:solid'><td width='100'>ORDER CODE:</td><td colspan='2'>STOCK STATUS</td></tr>";
                _sPriceTable += string.Format("<tr><td width='28%'>{0}</td>", _sCODE);
                if (_Tbt_Stock_Status != "")
                {
                    _sPriceTable += string.Format("<td colspan='2'>{0}</td></tr>", _Tbt_Stock_Status);
                }
                else
                {
                    _sPriceTable += string.Format("<td colspan='2'>{0}</td></tr>", _Tbt_Stock_Status_1);
                }

                if (_Eta != "")
                {
                    _sPriceTable += string.Format("<tr><td  style='font-weight :bold' >ETA</td><td colspan='2' style='font-weight :bold'>" + _Eta + "</td></tr>");

                }
                else
                {
                   // _sPriceTable += string.Format("<tr><td style='font-weight :bold' ></td><td style='font-weight :bold'></td></tr>");
                }
                if (withprice.ToLower() == "true")
                {
                    _sPriceTable += "<tr style='background-color: #dff0d8;'><td>Qty</td><td width='38'>Cost Inc GST</td><td width='34%'>Cost Ex GST</td></tr>";
                }
                
                int TotalCount = 0;
                int RowCount = 0;

                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        //if (withprice == "true")
                        //{
                            _sPriceTable += string.Format("<tr><td style='background-color: #dff0d8;'>{0}</td><td>${1:0.00}</td><td align='center'>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                        //}

                    }
                else
                {
                    bool bLastRow = false;
                    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                    RowCount = 0;

                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        RowCount = RowCount + 1;
                        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                        {
                            bLastRow = true;
                        }

                        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                        //if (withprice == "true")
                        //{
                            _sPriceTable += string.Format("<tr><td>{0}</td><td align=center>${1:0.00}</td><td  align='center'>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        //}
                    }
                }
                _sPriceTable += "</table>";
                // _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                //_sPriceTable += "</table><div class=\"clear\"></div>";
                //_sPriceTable += "<div class=\"popupaero\"></div>";



            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }

            return _sPriceTable;
        }
        //public string AssemblePriceTable_Print(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus, string _Eta)
        //{
        //    string _sPriceTable = "";
        //    if (_priceCode <= 0)
        //        return "";
        //    SqlConnection oSQLCon = null;
        //    try
        //    {

        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        DataSet dsPriceTable = new DataSet();

        //        int pricecode = _priceCode;
        //        string _sCODE = _ProCode;
        //        string stkstatus = _ProStkStatus;
        //        string _Tbt_Stock_Status = "";
        //        string _Tbt_Stock_Status_1 = "";
        //        bool _Tbt_Stock_Status_2 = false;
        //        string _Tbt_Stock_Status_3 = "";
        //        string _Colorcode1 = "";
        //        string _Colorcode;
        //        string StockStatus = stkstatus.Replace("_", " ");
        //        string _StockStatusTrim = StockStatus.Trim();

        //        switch (_StockStatusTrim)
        //        {
        //            case "IN STOCK":
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
        //                _Tbt_Stock_Status_2 = true;
        //                break;
        //            case "SPECIAL ORDER":
        //                _Colorcode = "#43A246";
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE &":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "DISCONTINUED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER AVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE NO ETA":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "OUT OF STOCK":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL":
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            default:
        //                _Colorcode = "Black";
        //                _Tbt_Stock_Status = _StockStatusTrim;
        //                break;
        //        }

        //        dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
        //        dsPriceTable.Tables[0].TableName = "PriceTable";

        //        // _sPriceTable = "<table style=' background:#FFF;border:0;border-style:solid;font-size:11px;'><tr><td><table width='100%' border='1' cellpadding='4' cellspacing='0' ><tr>";
        //        _sPriceTable = "<table width='100%' border='1' style=' background:#FFF;font-size:11px;' cellpadding='1' cellspacing='0' ><tr>";
        //        _sPriceTable += "<td width='100' height='39' valign='top' style='font-size:11px;' ><b>ORDER CODE:</b><br />";
        //        _sPriceTable += string.Format("<span><b>{0}</b></span></td>", _sCODE);
        //        _sPriceTable += "<td width='100' style='font-size:11px;' valign='top'><b>STOCK STATUS</b><br />";
        //        if (_Tbt_Stock_Status != "")
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr>", _Tbt_Stock_Status);
        //        }
        //        else
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr>", _Tbt_Stock_Status_1);
        //        }

        //        if (_Eta != "")
        //        {
        //            _sPriceTable += string.Format("<tr><td style='font-size:11px;'>ETA</td><td style='font-size:11px;'>" + _Eta + "</td></tr>");
        //        }

        //        _sPriceTable += "<tr><td colspan='2' valign='top' style='font-size:11px;'>";

        //        _sPriceTable += "<table cellpadding='1' cellspacing='0' border='1' width='100%' style='font-size:11px;'><tr style='background-color: #dff0d8;font-size:11px;' ><td><b>Qty</b></td><td style='font-size:11px;'><b>Cost Inc GST</b></td><td style='font-size:11px;'><b>Cost Ex GST</b></td></tr>";

        //        int TotalCount = 0;
        //        int RowCount = 0;

        //        if (pricecode == 3)
        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                _sPriceTable += string.Format("<tr><td style='font-size:11px;' ><b>{0}</b></td><td align='center' style='font-size:11px;'>${1:0.00}</td><td align='center' style='font-size:11px;'>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
        //            }
        //        else
        //        {
        //            bool bLastRow = false;
        //            TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
        //            RowCount = 0;

        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                RowCount = RowCount + 1;
        //                if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
        //                {
        //                    bLastRow = true;
        //                }

        //                string _color = bLastRow ? "bg_grey31" : "bg_grey3";
        //                _sPriceTable += string.Format("<tr><td style='font-size:11px;' ><b>{0}</b></td><td style='font-size:11px;'  align='center'>${1:0.00}</td><td style='font-size:11px;' align='center'>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
        //            }
        //        }

        //        //_sPriceTable += "</table></td></tr><tr><td colspan='2' height='4' style='font-size:11px;'></td></tr></table></td></tr></table>";
        //        _sPriceTable += "</table></td></tr><tr><td colspan='2' height='4' style='font-size:11px;'></td></tr></table>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    return _sPriceTable;


        //}
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE TABLE BASED ON THE MATCH CASES  ***/
        /********************************************************************************/
        public string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatusdesc, string _Pro_stock_Status,string CustomerType,int user_id, string _Pro_stock_Flag, string _Eta, DataSet Ds)
        {
            string _sPriceTable = string.Empty;
            SqlConnection oSQLCon = null;
            try
            {
                /*********************************** OLD CODE ***********************************/
                //////string userid = HttpContext.Current.Session["USER_ID"].ToString();
                //////DataSet dsPriceTable = new DataSet();
                ////////string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                ////////oHelper.SQLString = sSQL;
                ////////int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                ////////DataSet dsPriceTable = new DataSet();
                ////////oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                ////////oSQLCon.Open();
                ////////SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                ////////oCmd.Parameters.Clear();
                ////////oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////string _sCODE = oCmd.ExecuteScalar().ToString();

                ////////oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                ////////oCmd.Parameters.Clear();
                ////////oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////string stkstatus = oCmd.ExecuteScalar().ToString();
                //////int pricecode = _priceCode;
                //////string _sCODE = _ProCode;
                //////string stkstatus = _ProStkStatus;
                //////string _Tbt_Stock_Status = "";
                //////string _Tbt_Stock_Status_1 = "";
                //////bool _Tbt_Stock_Status_2 = false;
                //////string _Tbt_Stock_Status_3 = "";
                //////string _Colorcode1 = "";
                //////string _Colorcode;
                //////string StockStatus = stkstatus.Replace("_", " ");
                //////string _StockStatusTrim = StockStatus.Trim();

                //////switch (_StockStatusTrim)
                //////{
                //////    case "IN STOCK":
                //////        _Tbt_Stock_Status = "<span style='color:#43A246'><b>INSTOCK</b></span><br>";
                //////        _Tbt_Stock_Status_2 = true;
                //////        break;
                //////    case "SPECIAL ORDER":
                //////        _Colorcode = "#43A246";
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "SPECIAL ORDER PRICE &":
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style='color:#43A246'><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "DISCONTINUED":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "DISCONTINUED NO LONGER AVAILABLE":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "DISCONTINUED NO LONGER":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "TEMPORARY UNAVAILABLE":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                //////        break;
                //////    case "TEMPORARY UNAVAILABLE NO ETA":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                //////        break;
                //////    case "OUT OF STOCK":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                //////        break;
                //////    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
                //////        break;
                //////    case "OUT OF STOCK ITEM WILL":
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style='color:#43A246'><b>ITEM WILL BE BACK ORDERED</b></span>";
                //////        break;
                //////    default:
                //////        _Colorcode = "Black";
                //////        _Tbt_Stock_Status = _StockStatusTrim;
                //////        break;
                //////}

                ////////SqlDataAdapter oDa = new SqlDataAdapter();
                ////////oDa.SelectCommand = new SqlCommand();
                ////////oDa.SelectCommand.Connection = oSQLCon;
                ////////oDa.SelectCommand.CommandText = "GetPriceTable";
                ////////oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                ////////oDa.SelectCommand.Parameters.Clear();
                ////////oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                ////////oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                ////////oDa.Fill(dsPriceTable, "PriceTable");
                //////if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                //////{
                //////    DataRow[] dr = Ds.Tables[0].Select("PRODUCT_ID='" + ProductID.ToString() + "'");
                //////    if (dr.Length > 0)
                //////    {
                //////        dsPriceTable.Tables.Add(dr.CopyToDataTable().Copy());
                //////        dsPriceTable.Tables[0].TableName = "PriceTable";
                //////    }
                //////}
                //////else
                //////    return "";

                ////////dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
                ////////dsPriceTable.Tables[0].TableName = "PriceTable";

                //////_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //////_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                //////_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                //////_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                //////if (_Tbt_Stock_Status != "")
                //////{
                //////    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //////}
                //////else
                //////{
                //////    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //////}
                //////_sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

                //////int TotalCount = 0;
                //////int RowCount = 0;

                //////if (pricecode == 3)
                //////    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                //////    {
                //////        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                //////    }
                //////else
                //////{
                //////    bool bLastRow = false;
                //////    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                //////    RowCount = 0;

                //////    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                //////    {
                //////        RowCount = RowCount + 1;
                //////        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                //////        {
                //////            bLastRow = true;
                //////        }

                //////        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                //////        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                //////    }
                //////}

                //////_sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                ////////if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
                /*********************************** OLD CODE ***********************************/

                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                DataSet dsPriceTable = new DataSet();
               
                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatusdesc;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                bool isProductReplace = true;



                //if ((_StockStatusTrim.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatusTrim.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED")))//&& CustomerType.ToLower() == "dealer"
                //    isProductReplace = false ;
                //else
                //{
                //  if (_Pro_stock_Status.ToLower() == "true" || _Pro_stock_Status.ToLower() == "1")
                //      isProductReplace = false;
                //  else if(_Pro_stock_Flag=="0")
                //      isProductReplace = false;
                //}
                if (_Pro_stock_Flag == "0")
                    isProductReplace = false;

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span>INSTOCK</span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        //_Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED</span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span>ITEM WILL BE BACK ORDERED</span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }

                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = Ds.Tables[0].Select("PRODUCT_ID='" + ProductID.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        dsPriceTable.Tables.Add(dr.CopyToDataTable().Copy());
                        dsPriceTable.Tables[0].TableName = "PriceTable";
                    }
                }
                else
                    return "";

              

                //_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                //_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                //if (_Tbt_Stock_Status != "")
                //{
                //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //}
                //else
                //{
                //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //}
                //_sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";
                _sPriceTable += "<table  class=\"table table-striped  table-bordered table-condensed\" style=\"margin:0; background:#FFF\">";
                _sPriceTable += "<tr class=\"success\"><td width=\"28%\">ORDER CODE:</td><td colspan=\"2\">STOCK STATUS</td></tr>";

                if (isProductReplace == true)
                {
                    string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid="";
                    bool samecodesubproduct = false;
                    bool samecodenotFound = false;
                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_sCODE, user_id);
                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                    {

                        _catid = rtntbl.Rows[0]["CatId"].ToString();
                        pfid = rtntbl.Rows[0]["Pfid"].ToString();
                        Ea_Path = rtntbl.Rows[0]["Ea_Path"].ToString();
                        samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                        samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                        wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                        SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                    }
                    else
                    {
                        samecodesubproduct = true;
                        samecodenotFound = false;
                    }
                  
                     if (samecodenotFound == false && samecodesubproduct == false)
                    {
                        _sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);
                        _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Not Available.");
                        _sPriceTable += "<tr class=\"success\"><td colspan=\"3\"><br>RECOMMENDED REPLACEMENT<br><br></td></tr>";
                        _sPriceTable += "<tr><td colspan=\"3\">";
                        _sPriceTable += "<br>Order Code : " + "<span  style=\"color:green;font-weight: bold;\">" + wag_product_code + "</span> <br>";
                        string strurl="ProductDetails.aspx?Pid="+ SubstuyutePid +"&amp;fid="+pfid+"&amp;Cid="+_catid+"&amp;path="+ Ea_Path;
                        _sPriceTable += "<br><a href =\"" + strurl + "\" style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" > View Replacement Product </a>";
                        _sPriceTable += "<br><br></td></tr>";
                        
                    }
                    else // if (samecodenotFound == false && samecodesubproduct == true)
                    {
                        _sPriceTable += string.Format("<tr><td style=\"width=\"28%\">{0}</td>", _sCODE);
                        //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);
                        //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
                        if (_Tbt_Stock_Status != "")
                        {                            
                            //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                            _sPriceTable += string.Format("<td  colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                        }
                        else
                        {
                            //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                            _sPriceTable += string.Format("<td  colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                        }
                        _sPriceTable += pricetablebinding(ProductID, _priceCode, _ProCode, _ProStkStatusdesc, _Pro_stock_Status, CustomerType, user_id, _Pro_stock_Flag, _Eta, Ds, dsPriceTable);


                    }
                    //else
                    //{
                    //    _sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);
                    //    if (_Tbt_Stock_Status != "")
                    //    {
                    //        _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                    //    }
                    //    else
                    //    {
                    //        _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                    //    }
                    //}


                }
                else
                {

                    _sPriceTable += string.Format("<tr><td width=\"28%\">{0}</td>", _sCODE);
                    if (_Tbt_Stock_Status != "")
                    {
                        _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                    }
                    else
                    {
                        _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                    }
                    _sPriceTable += pricetablebinding(ProductID, _priceCode, _ProCode, _ProStkStatusdesc, _Pro_stock_Status, CustomerType, user_id, _Pro_stock_Flag, _Eta, Ds, dsPriceTable);

                    //if (_Eta != "")
                    //{
                    //    _sPriceTable += string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + _Eta + "</b></td></tr>");

                    //}

                    //_sPriceTable += "<tr class=\"success\"><td>Qty</td><td width=\"38%\">Cost Inc GST</td><td width=\"34%\">Cost Ex GST</td></tr>";

                    //int TotalCount = 0;
                    //int RowCount = 0;
                    //string[] P1 = null;
                    //string[] P2 = null;
                    //if (pricecode == 3)
                    //    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    //    {
                    //        // _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    //        P1 = oDr["Price1"].ToString().Split('.');
                    //        P2 = oDr["Price2"].ToString().Split('.');
                    //        if (P1[1].Length >= 4 && P2[1].Length >= 4)
                    //        {
                    //            if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                    //            {
                    //                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.0000}</td><td align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    //            }
                    //            else
                    //                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    //        }
                    //        else
                    //            _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);

                    //    }
                    //else
                    //{
                    //    bool bLastRow = false;
                    //    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                    //    RowCount = 0;

                    //    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    //    {
                    //        RowCount = RowCount + 1;
                    //        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                    //        {
                    //            bLastRow = true;
                    //        }

                    //        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                    //        P1 = oDr["Price1"].ToString().Split('.');
                    //        P2 = oDr["Price2"].ToString().Split('.');
                    //        if (P1[1].Length >= 4 && P2[1].Length >= 4)
                    //        {
                    //            if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                    //            {
                    //                _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.0000}</td><td class=\"{3}\" align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    //            }
                    //            else
                    //                _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    //        }
                    //        else
                    //            _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    //    }
                    //}
                }
                _sPriceTable += "</table>";
               // _sPriceTable += "</table><div class=\"clear\"></div>";
              //  _sPriceTable += "<div class=\"popupaero\"></div>";
               

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = string.Empty;//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable;
        }

        private string pricetablebinding(int ProductID, int _priceCode, string _ProCode, string _ProStkStatusdesc, string _Pro_stock_Status, string CustomerType, int user_id, string _Pro_stock_Flag, string _Eta, DataSet Ds, DataSet dsPriceTable)
        {

            string _sPriceTable = "";
            if (_Eta != "")
            {
                _sPriceTable += string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + _Eta + "</b></td></tr>");

            }

            _sPriceTable += "<tr class=\"success\"><td>Qty</td><td width=\"38%\">Cost Inc GST</td><td width=\"34%\">Cost Ex GST</td></tr>";

            int TotalCount = 0;
            int RowCount = 0;
            string[] P1 = null;
            string[] P2 = null;
            if (pricecode == 3)
                foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                {
                    // _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    P1 = oDr["Price1"].ToString().Split('.');
                    P2 = oDr["Price2"].ToString().Split('.');
                    if (P1[1].Length >= 4 && P2[1].Length >= 4)
                    {
                        if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                        {
                            _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.0000}</td><td align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                        }
                        else
                            _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    }
                    else
                        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);

                }
            else
            {
                bool bLastRow = false;
                TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                RowCount = 0;

                foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                {
                    RowCount = RowCount + 1;
                    if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                    {
                        bLastRow = true;
                    }

                    string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                    P1 = oDr["Price1"].ToString().Split('.');
                    P2 = oDr["Price2"].ToString().Split('.');
                    if (P1[1].Length >= 4 && P2[1].Length >= 4)
                    {
                        if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                        {
                            _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.0000}</td><td class=\"{3}\" align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        }
                        else
                            _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                    else
                        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                }
            }

            return _sPriceTable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT DETAILS BASED ON THE BUYER GROUP  ***/
        /********************************************************************************/
        public DataSet GetBuyerGroupBasedDiscountDetails(string BuyerGroup)
        {
            try
            {
                //string sSQL = " SELECT DISCOUNT =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT";
                //sSQL = sSQL + " ELSE DISC_AMT";
                //sSQL = sSQL + " END, VALID_DATE =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT_TILL_DT";
                //sSQL = sSQL + " ELSE DISC_AMT_VALID_TILL_DT";
                //sSQL = sSQL + " END,DISC_METHOD =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN '" + DefaultBG.PERCENTAGEMETHOD.ToString() + "'";
                //sSQL = sSQL + " ELSE '" + DefaultBG.AMOUNTMETHOD.ToString() + "'";
                //sSQL = sSQL + " END FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = '" + BuyerGroup + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objHelperDb.GetGenericDataDB("", BuyerGroup, DefaultBG.PERCENTAGEMETHOD.ToString(), DefaultBG.AMOUNTMETHOD.ToString(), "GET_BUYER_GROUP_BASED_DISCOUNT", HelperDB.ReturnType.RTDataSet);
  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE BUYER GROUP DETAILS ***/
        /********************************************************************************/
        public string GetBuyerGroup(int UserID)
        {
            string retVal;
            try
            {
                if (UserID > 0)
                {
                    //string sSQL = " SELECT TBG.BUYER_GROUP FROM TBWC_BUYER_GROUP TBG,TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB";
                    //sSQL = sSQL + " WHERE TBG.BUYER_GROUP = TC.BUYER_GROUP";
                    //sSQL = sSQL + " AND TC.COMPANY_ID = TCB.COMPANY_ID AND USER_ID =" + UserID;
                    //oHelper.SQLString = sSQL;
                    //retVal = oHelper.GetValue("BUYER_GROUP");
                    retVal=(string)objHelperDb.GetGenericDataDB(UserID.ToString(), "GET_BUYER_GROUP", HelperDB.ReturnType.RTString);
                }
                else
                {
                    retVal = DefaultBG.DEFAULTBG.ToString();
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = "";
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT PRICE FOR BUYER GROUP ***/
        /********************************************************************************/
        public decimal CalculateBGDiscountPrice(decimal CurrentPrice, decimal DiscountValue, string DiscoundMethod)
        {
            decimal retPrice = 0;
            try
            {
                if (DiscoundMethod == DefaultBG.PERCENTAGEMETHOD.ToString())
                {
                    decimal DiscountPrice = (CurrentPrice * DiscountValue) / 100;
                    retPrice = CurrentPrice - DiscountPrice;
                }
                else
                {
                    retPrice = CurrentPrice - DiscountValue;

                }
                if (retPrice < 0)
                {
                    retPrice = 0;
                }
                else
                {
                    retPrice = objHelperServices.CDEC(retPrice.ToString("N2"));
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retPrice = 0;
            }

            return retPrice;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  CHECK THE CATALOG ID BELONGS TO THE BUYER GROUP OR NOT ***/
        /********************************************************************************/
        public bool IsBGCatalogProduct(int ProductCatalogID, string BuyerGroupName)
        {
            bool retVal = false;
            string tempstr = string.Empty;
            try
            {
                //string sSQL = "SELECT CATALOG_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP='" + BuyerGroupName + "'";
                //oHelper.SQLString = sSQL;
                //int BGCatalogID = oHelper.CI(oHelper.GetValue("CATALOG_ID").ToString());
                int BGCatalogID=0;
                tempstr=(string)objHelperDb.GetGenericDataDB(BuyerGroupName, "GET_BUYER_GROUP_CATALOG_ID", HelperDB.ReturnType.RTString);  
                if (tempstr!=null && tempstr!="")
                    BGCatalogID = objHelperServices.CI(tempstr);

                if (ProductCatalogID == BGCatalogID)
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = false;
                return retVal;
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  RETRIVE THE DETAILS OF FAMILY PAGE PRODUCTS ***/
        /********************************************************************************/
        public DataSet GetFamilyPageProduct(string familyid, string Option)
        {
            DataSet ds = new DataSet();
            SqlCommand objSqlCommand;
            SqlDataAdapter da;
            try
            {
                //SqlConnection Gcon = new SqlConnection();
                //Gcon.ConnectionString = conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1);

                objSqlCommand = new SqlCommand("STP_TBWC_PICKFAMILYPAGEPRODUCT", objConnectionDB.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add("@FamilyID", familyid);
                objSqlCommand.Parameters.Add("@OPTION", Option);
                da = new SqlDataAdapter(objSqlCommand);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
               // objErrorHandler.CreateLog(e.ToString() + familyid + Option);
            }
            finally
            {
                objSqlCommand = null;
                da = null;
            }

            return ds;
        }



       
    }
    /*********************************** J TECH CODE ***********************************/   
}