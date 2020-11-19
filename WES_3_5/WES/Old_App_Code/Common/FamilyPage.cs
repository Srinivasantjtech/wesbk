using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using TradingBell.Common;
using TradingBell.WebServices;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.Common
{
    public class FamilyPage
    {
        ConnectionDB oCon = new ConnectionDB();
        HelperDB oHelper = new HelperDB();
        ErrorHandler oErr = new ErrorHandler();
        DataSet DsPreview = new DataSet();
       // Order oOrder = new Order();
        //BuyerGroup oBG = new BuyerGroup();
        Product oPro = new Product();
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
        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        public int CatalogID
        {
            get
            {
                return _catalogID;
            }
            set
            {
                _catalogID = value;

            }
        }
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
        public int GetPriceCode()
        {
            int pc = -1;
            DataTable Sqltb = new DataTable();
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                Sqltb = oHelper.GetDataTable(sSQL);
                if (Sqltb != null && Sqltb.Rows.Count > 0)
                    pc = Convert.ToInt32(Sqltb.Rows[0]["price_code"]);
            }
            return pc;
        }
        public string GenerateHorizontalHTML(string _familyID, DataSet Ds)
        {
            //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            string HypColumn = "";
            int Min_ord_qty = 0;
            int Qty_avail;
            int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _AvilableQty = "0";
            DataRow[] tempPriceDr;
            DataTable tempPriceDt;
            //int ProdID;
            int AttrType;


            string NavColumn = oHelper.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
            if (EComState == "YES")
                if (!IsEcomenabled())
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";

            strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=0 cellspacing=1 Class=\"FamilyPageTable\" cellpadding=3>");
            //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");

            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //oHelper.SQLString = sSQL;
            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
            pricecode = GetPriceCode();
            DisplayHeaders = true;
            if (DisplayHeaders == true)
            {
                strBldr.Append("<TR>");
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
                    //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());
                    DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        AttrType = oHelper.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
                        if (AttrType != 3)
                        {
                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

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
                        else
                        {
                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >");
                            strBldr.Append("</TD>");
                        }
                    }
                }
                if (DsPreview.Tables[_familyID].Rows.Count > 0)
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >More Info</TD>");
                if (EComState.ToUpper() == "YES" && DsPreview.Tables[_familyID].Rows.Count > 0)
                {
                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;    \" >Cart</TD>");
                }
                strBldr.Append("</TR>");
            }
            string ValueFortag = string.Empty;
            bool rowcolor = false;
            for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            {
                strBldr.Append("<TR>");
                if (rowcolor == false && i != 0)
                {
                    rowcolor = true;
                }
                else if (rowcolor == true)
                {
                    rowcolor = false;
                }
                tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                if(tempPriceDr.Length>0)
                    tempPriceDt=tempPriceDr.CopyToDataTable();
                else       
                    tempPriceDt=null;

                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    string alignVal = "LEFT";

                    DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    if (tempdr.Length > 0)
                    {
                        ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
                        AttrType = oHelper.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


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
                            if (ValueFortag != "" && ValueFortag != null)
                            {
                                FileInfo Fil;
                                string strFile = HttpContext.Current.Server.MapPath("ProdImages");
                                Fil = new FileInfo(strFile + ValueFortag);
                                if (Fil.Exists)
                                {
                                    ValueFortag = "prodimages" + ValueFortag;
                                }
                                else
                                {
                                    ValueFortag = "Images/NoImage.gif";
                                }
                            }
                            else
                            {
                                ValueFortag = "Images/NoImage.gif";
                            }

                            if (rowcolor == false)
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");
                            }
                            else if (rowcolor == true)
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");

                            }
                        }
                        // strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[_familyID].Rows[i][j].ToString()));
                        else  //if (chkAttrType[j] == 4)
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
                                    if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
                                    {
                                        if (AttrType == 4)
                                        {
                                            //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                                            //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            

                                            if (tempPriceDt!=null)                                            
                                                ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
                                            else
                                                ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        


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
                                if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
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
                                string _ProCode = "";
                                if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                                    _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
                                if (tempPriceDt != null)
                                {
                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                   
                                } 
    
                                if (UserID > 0)
                                {

                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails(GetBuyerGroup(UserID));
                                }
                                else
                                {
                                    dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                }

                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        untPrice = oHelper.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                        bool IsBGCatProd = IsBGCatalogProduct(CatalogID, GetBuyerGroup(UserID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                        }
                                    }
                                }
                                ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                                //ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
                            }
                            if (rowcolor == false)
                            {
                                strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
                            }
                            else if (rowcolor == true)
                            {
                                strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
                            }
                        }
                        //else
                        //{
                        //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
                        //}

                        //Add the Shipping and Cart Images
                        if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
                        {

                            ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                            //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
                            //int IsAvailable = oPro.GetProductAvailability(ProdID);
                            string ShipImgPath = "";
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
                                                      
                            if (IsShipping == true)
                            {
                                ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                                string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                                ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                            }
                            else if (IsShipping == false)
                            {
                                ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                            }

                            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                            {
                                ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request["sl2"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                          "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";

                            }
                            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
                            {
                                ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[1].Rows[0]["FAMILY_ID"].ToString() + "&byp=2&qf=1&cid=" + HttpContext.Current.Request["cid"].ToString() + "&sl1=" + HttpContext.Current.Request["sl1"].ToString() + "&tf=1\"  class=\"tx_3\">" +
                                         "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                            }
                            else
                            {
                                if (HttpContext.Current.Request["pcr"] != null && HttpContext.Current.Request["pcr"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request["byp"] != null && HttpContext.Current.Request["byp"].ToString() != "")
                                    {
                                        if (HttpContext.Current.Request["cid"] != null && HttpContext.Current.Request["cid"].ToString() != null)
                                        {
                                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[_familyID].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "&cid=" + HttpContext.Current.Request["cid"].ToString() + "&byp=" + HttpContext.Current.Request["byp"].ToString() + "&qf=1\"  class=\"tx_3\">" +
                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                        }
                                        else
                                        {
                                            ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[_familyID].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "&byp=" + HttpContext.Current.Request["byp"].ToString() + "&qf=1\"  class=\"tx_3\">" +
                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                        }
                                    }
                                    else
                                    {
                                        ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[_familyID].Rows[0]["FAMILY_ID"].ToString() + "&pcr=" + HttpContext.Current.Request["pcr"].ToString() + "\"  class=\"tx_3\">" +
                                  "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> Details </a>";
                                    }
                                }
                                else
                                    ShipImgPath = "<a href=\"productdetails.aspx?&pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString() + "\"  class=\"tx_3\">" +
                                              "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";
                            }

                            if (rowcolor == false)
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</TD>");
                            }
                            else if (rowcolor == true)
                            {
                                strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</TD>");
                            }

                            if (EComState.ToUpper() == "YES")
                            {
                                //Add the Cart Image
                                string CartImgPath = "";
                                //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
                                if (Restricted.ToUpper() == "YES")
                                {
                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                }
                                else
                                {
                                    if (IsAvailable == 1)
                                    {
                                        CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();

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
                                        string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();

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

                                        if (_Tbt_Stock_Status_2 == true)
                                        {

                                            CartImgPath = "<table><tr><td>" +
                                                       "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"5\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;height=23;\"   /> " +
                                                     "</td><td>" +
                                                     "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +
                                                //"<div onmouseout=\"MM_swapImgRestore()\" onmouseover=\"MM_swapImage('Image"+ ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\" style=\"width:76px; height:25px; cursor:pointer; \">" +
                                                     "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
                                                     "</a></td></tr></table>";
                                        }
                                        else
                                        {
                                            CartImgPath = "";
                                        }

                                        if (rowcolor == false)
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px; \">" + CartImgPath + "</TD>");
                                        }
                                        if (rowcolor == true)
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;  \">" + CartImgPath + "</TD>");
                                        }
                                    }
                                    else
                                    {
                                        if (rowcolor == false)
                                        {
                                            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;   \">N/A</TD>");
                                        }
                                        if (rowcolor == true)
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

            strBldr.Append("</TABLE></td></tr></table>");
            //if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
            //{
            //    strBldr = strBldr.Remove(0, strBldr.Length);
            //}
            return strBldr.ToString();
        }
        private bool IsEcomenabled()
        {
            bool retvalue = false;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
            string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
            oHelper.SQLString = sSQL;
            int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
            if (iROLE <= 3)
                retvalue = true;
            return retvalue;
        }
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
        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                oHelper.SQLString = sSQL;
                int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

                string strquery = "";
                if (pricecode == 1)
                {
                    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                }
                else
                {
                    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                }

                DataSet DSprice = new DataSet();
                oHelper.SQLString = strquery;
                retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
            }
            return retval;
        }
        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                oHelper.SQLString = sSQL;
                Retval = oHelper.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
            }
            catch
            {
            }
            return Retval;
        }
        private string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus)
        {
            string _sPriceTable = "";
            SqlConnection oSQLCon = null;
            try
            {
                                
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                DataSet dsPriceTable = new DataSet();
                //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //oHelper.SQLString = sSQL;
                //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                //DataSet dsPriceTable = new DataSet();
                oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                oSQLCon.Open();
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
                string _Tbt_Stock_Status = "";
                string _Tbt_Stock_Status_1 = "";
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = "";
                string _Colorcode1 = "";
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }

                SqlDataAdapter oDa = new SqlDataAdapter();
                oDa.SelectCommand = new SqlCommand();
                oDa.SelectCommand.Connection = oSQLCon;
                oDa.SelectCommand.CommandText = "GetPriceTable";
                oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                oDa.SelectCommand.Parameters.Clear();
                oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                oDa.Fill(dsPriceTable, "PriceTable");
                _sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                _sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                if (_Tbt_Stock_Status != "")
                {
                    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                }
                else
                {
                    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                }
                _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

                int TotalCount = 0;
                int RowCount = 0;

                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
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
                        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                }

                _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            catch (Exception)
            {
                _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable;
        }
        public DataSet GetBuyerGroupBasedDiscountDetails(string BuyerGroup)
        {
            try
            {
                string sSQL = " SELECT DISCOUNT =";
                sSQL = sSQL + " CASE";
                sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT";
                sSQL = sSQL + " ELSE DISC_AMT";
                sSQL = sSQL + " END, VALID_DATE =";
                sSQL = sSQL + " CASE";
                sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT_TILL_DT";
                sSQL = sSQL + " ELSE DISC_AMT_VALID_TILL_DT";
                sSQL = sSQL + " END,DISC_METHOD =";
                sSQL = sSQL + " CASE";
                sSQL = sSQL + " WHEN USE_PCT = 1 THEN '" + DefaultBG.PERCENTAGEMETHOD.ToString() + "'";
                sSQL = sSQL + " ELSE '" + DefaultBG.AMOUNTMETHOD.ToString() + "'";
                sSQL = sSQL + " END FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = '" + BuyerGroup + "'";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                return null;
            }

        }
        public string GetBuyerGroup(int UserID)
        {
            string retVal;
            try
            {
                if (UserID > 0)
                {
                    string sSQL = " SELECT TBG.BUYER_GROUP FROM TBWC_BUYER_GROUP TBG,TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB";
                    sSQL = sSQL + " WHERE TBG.BUYER_GROUP = TC.BUYER_GROUP";
                    sSQL = sSQL + " AND TC.COMPANY_ID = TCB.COMPANY_ID AND USER_ID =" + UserID;
                    oHelper.SQLString = sSQL;
                    retVal = oHelper.GetValue("BUYER_GROUP");
                }
                else
                {
                    retVal = DefaultBG.DEFAULTBG.ToString();
                }

            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                retVal = "";
            }
            return retVal;
        }
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
                    retPrice = oHelper.CDEC(retPrice.ToString("N2"));
                }
            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
                //oErr.CreateLog();
                retPrice = 0;
            }

            return retPrice;
        }
        public bool IsBGCatalogProduct(int ProductCatalogID, string BuyerGroupName)
        {
            bool retVal = false;
            try
            {
                string sSQL = "SELECT CATALOG_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP='" + BuyerGroupName + "'";
                oHelper.SQLString = sSQL;
                int BGCatalogID = oHelper.CI(oHelper.GetValue("CATALOG_ID").ToString());
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
                oErr.ErrorMsg = e;
                oErr.CreateLog();
                retVal = false;
                return retVal;
            }
            return retVal;
        }
    }
   
}