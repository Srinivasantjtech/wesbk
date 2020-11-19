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
using System.ComponentModel;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_ProductAttributes : System.Web.UI.UserControl
{
    ErrorHandler oErr;
    HelperDB oHelper = new HelperDB();
    Product oProduct;
    int _ProductID;
    string _CssClass;
    string _HeaderCssClass;
    int _CellWidth;
    int _UserId;
    CustomPrice oCustomPrice = new CustomPrice();
    public enum DisplayType
    {
        SpecificationType = 1,
    //    DescriptionType = 2,
        PriceType = 3,
        CustomPrice =4
    }
    DisplayType _DisplayText;
    string _CusPriceAttrName;
    decimal _CustomPriceValue;
    int _CatalogID;

    #region "Property"
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public int ProductID
        {
            get
            {
                return _ProductID;
            }
            set
            {
                _ProductID = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public int CatalogID
        {
            get
            {
                return _CatalogID;
            }
            set
            {
                _CatalogID = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Display Type")
        ]
        public DisplayType Display
        {
            get
            {
                return _DisplayText;
            }
            set
            {
                _DisplayText = value;
            }
        }

        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute value CssClass")
        ]
        public string HeaderCssClass
        {
            get
            {
                return _HeaderCssClass;
            }
            set
            {
                _HeaderCssClass = value;
            }
        }
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
    public int UserId
    {
        get
        {
            return _UserId;
        }
        set
        {
            _UserId = value;
        }

    }

        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute value CssClass")
        ]
        public string CssClass
        {
            get
            {
                return _CssClass;
            }
            set
            {
                _CssClass = value;
            }
        }

    [
    Browsable(true),
    Category("TradingBell"),
    DefaultValue(""),
    Description("Set the Cell width")
    ]
    public int CellWidth
    {
        get
        {
            return _CellWidth;
        }
        set
        {
            _CellWidth = value;
        }
    }

    [Browsable(true),
      Category("TradingBell"),
      DefaultValue("")]
    public string CustomPriceAttributeName
    {

        get
        {
            return _CusPriceAttrName;
        }
        set
        {
            _CusPriceAttrName = value;
        }

    }

    [Browsable(true),
   Category("TradingBell"),
   DefaultValue("")]
    public decimal CustomPriceValue
    {

        get
        {
            return _CustomPriceValue;
        }
        set
        {
            _CustomPriceValue = value;
        }

    }

    #endregion 
    protected void Page_Load(object sender, EventArgs e)
    {
        HelperDB oHelper = new HelperDB();
        _CatalogID = oHelper.CI(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
        if (Request["Pid"] != null)
        {
            _ProductID =  oHelper.CI(Request["PId"].ToString());
        }
        if (_DisplayText == DisplayType.SpecificationType)
            LoadProductSpecAttributes();
        //else if (_DisplayText == DisplayType.DescriptionType)
        //    LoadProdDescAttributes();
        else if (_DisplayText == DisplayType.PriceType)
        {
            LoadProdPriceAttributes();
        }
        else if (_DisplayText == DisplayType.CustomPrice)
        {
            LoadCustomPrice();
        }
    }
    #region "Functions"
    public void LoadProductSpecAttributes()
    {
        if (!DesignMode)
        {
            //Load Core Attributes.
            oErr = new ErrorHandler();
            oProduct = new Product();
            //ProductAttribute oAttribute = new ProductAttribute();
            DataSet dsCoreAttrName = new DataSet();
            DataSet dsCoreAttrVal = new DataSet();

            Table TblProd = new Table();
            TblProd.ID = "TblCoreAttribute";
            TblProd.CellPadding = 0;
            TblProd.CellSpacing = 5;

            try
            {
               
                //Load product spec attributes.
                TblProd = new Table();
                DataSet dsSpec = new DataSet();

                TblProd.ID = "TblSpecAttribute";
                TblProd.CellPadding = 0;
                TblProd.CellSpacing = 3;
                dsSpec = oProduct.GetProductsAllAttributesValues(_ProductID,_CatalogID);
                if (dsSpec != null)
                {
                    foreach (DataRow rSpec in dsSpec.Tables[0].Rows)
                    {
                        TableRow RowAttrName = new TableRow();
                        TableRow RowAttrValue = new TableRow();
                        TableRow RowAttr = new TableRow();
                        TableCell CellAttrName = new TableCell();
                        TableCell CellAttrVal = new TableCell();
                        TableCell CellAttr = new TableCell();
                        CellAttrName.Width = _CellWidth;
                        CellAttrName.Font.Bold=true;
                        CellAttrVal.Font.Bold=false;
                        CellAttrVal.Font.Size = 9;
                        //CellAttrName.Text = rSpec["ATTRIBUTE_NAME"].ToString();
                        CellAttrName.Text = rSpec["ATTRIBUTE_NAME"].ToString().ToUpper() ;
                        CellAttrVal.Text = ": " + rSpec["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "  ") + "";
                        RowAttr.Cells.Add(CellAttrName);
                        RowAttr.Cells.Add(CellAttrVal);
                        
                            //rSpec["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>");
                       CellAttrName.CssClass = _HeaderCssClass;
                        CellAttrVal.CssClass = _CssClass;
                       // CellAttr.Text = CellAttrName.Text + CellAttrVal.Text;
                       // RowAttrName.Cells.Add(CellAttrName);
                        //RowAttr.Cells.Add(CellAttrVal);
                       // RowAttr.Cells.Add(RowAttrName + RowAttrValue);
                        TblProd.Rows.Add(RowAttr);
                        //TblProd.Rows.Add(RowAttrValue);
                    }
                    dsSpec = null;
                }
                Controls.Add(TblProd);

            }
            catch (Exception e)
            {
                oErr.ErrorMsg = e;
            }
        }
    }
    //public void LoadProdDescAttributes()
    //{
    //    if (!DesignMode)
    //    {
    //        Table tblProdDesc = new Table();
    //        DataSet dsDesc = new DataSet();
    //        oProduct = new Product();

    //        tblProdDesc.ID = "TblDescAttribute";
    //        tblProdDesc.CellPadding = 0;
    //        tblProdDesc.CellSpacing = 3;
    //        try
    //        {
    //            dsDesc = oProduct.GetProductDescValues(_ProductID, true);
    //            if (dsDesc != null)
    //            {
    //                foreach (DataRow rDesc in dsDesc.Tables[0].Rows)
    //                {
    //                    TableRow RowAttrName = new TableRow();
    //                    TableRow RowAttrValue = new TableRow();
    //                    TableCell CellAttrName = new TableCell();
    //                    TableCell CellAttrVal = new TableCell();
    //                    CellAttrName.Text = rDesc["ATTRIBUTE_NAME"].ToString();
    //                    CellAttrVal.Text = rDesc["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>");
    //                    CellAttrName.CssClass = _HeaderCssClass;
    //                    CellAttrVal.CssClass = _CssClass;
    //                    RowAttrName.Cells.Add(CellAttrName);
    //                    RowAttrValue.Cells.Add(CellAttrVal);
    //                    tblProdDesc.Rows.Add(RowAttrName);
    //                    tblProdDesc.Rows.Add(RowAttrValue);
    //                }
    //                dsDesc = null;
    //                Controls.Add(tblProdDesc);
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            oErr.ErrorMsg = e;
    //        }

    //    }
    //}

      
    public void LoadCustomPrice()
        {
            try
            {
                Table tblCustPrice = new Table();
                TableRow RowAttr = new TableRow();
                TableCell CellAttrName = new TableCell();
                TableCell CellAttrVal = new TableCell();
                CellAttrName.Width = _CellWidth;
                CellAttrName.Text = _CusPriceAttrName + "&nbsp;";
                CellAttrVal.Text = ": " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() +  oHelper.CDEC(oHelper.FixDecPlace( _CustomPriceValue));
                CellAttrName.CssClass = _HeaderCssClass;
                CellAttrVal.CssClass = _CssClass;
                RowAttr.Cells.Add(CellAttrName);
                RowAttr.Cells.Add(CellAttrVal);
                tblCustPrice.Rows.Add(RowAttr);
                this.Controls.Add(tblCustPrice);
               
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; // oErr.CreateLog();
            }
        }
    public void LoadProdPriceAttributes()
    {
        if (!DesignMode)
        {
            HelperDB oHelper = new HelperDB();
            ProductPromotion oProdPro = new ProductPromotion();
            BuyerGroup oBG = new BuyerGroup();
            Table tblProdPrice = new Table();
            DataSet dsPrice = new DataSet();
            DataSet dsBgDisc = null;
            oProduct = new Product();
            int BuyerGroupAttrID = 0;
            string BGName = "";
            decimal untPrice = 0;
            bool ChkPromotion = false;
            

            tblProdPrice.ID = "TblPriceAttribute";
            tblProdPrice.CellPadding = 0;
            tblProdPrice.CellSpacing = 0;

            try
            {

                ChkPromotion = oProdPro.CheckPromotion(ProductID);
                

                    if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
                    {


                        //Check the user buyer group and get the price attribute id.
                        BuyerGroupAttrID = oBG.GetBuyerGroupPriceID(oHelper.CI(Session["USER_ID"].ToString()));
                        dsPrice = oProduct.GetProductPriceValue(ProductID, BuyerGroupAttrID);
                    }
                    else
                    {
                        //Get the price type attribute id for default buyer group(DEFAULTBG).
                        BuyerGroupAttrID = oBG.GetBuyerGroupPriceID(0);
                        dsPrice = oProduct.GetProductPriceValue(ProductID, BuyerGroupAttrID);
                    }



                    if (dsPrice != null)
                    {
                        foreach (DataRow rPrice in dsPrice.Tables[0].Rows)
                        {
                            TableRow RowAttr = new TableRow();
                            //TableRow RowAttrValue = new TableRow();
                            TableCell CellAttrName = new TableCell();
                            TableCell CellAttrVal = new TableCell();
                            CellAttrName.Width = _CellWidth;
                            if (rPrice["ATTRIBUTE_VALUE"].ToString() != "")
                            {

                                //Check the promotion table.
                                if (oProdPro.CheckPromotion(ProductID))
                                {
                                    Session["DiscountPrice"] = "";
                                    decimal DiscPrice = oHelper.CDEC(oProdPro.GetProductPromotionDiscValue(ProductID));
                                    untPrice = oHelper.CDEC(oProduct.GetProductBasePrice(ProductID));
                                    DiscPrice = (untPrice * DiscPrice) / 100;
                                    untPrice = untPrice - DiscPrice;
                                    untPrice = oHelper.CDEC(untPrice.ToString("N2"));
                                    CellAttrName.Text = "&nbsp;" + "DISCOUNT PRICE";
                                    Session["DiscountPrice"] = untPrice;

                                    CellAttrVal.Text = ": " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + oHelper.FixDecPlace(untPrice).ToString();
                                }

                                else
                                {
                                    CellAttrName.Text = "&nbsp;" + rPrice["ATTRIBUTE_NAME"].ToString();
                                    BGName = oBG.GetBuyerGroup(oHelper.CI(Session["USER_ID"].ToString()));
                                    dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                    if (dsBgDisc != null)
                                    {
                                        if (dsBgDisc.Tables[0].Rows.Count > 0)
                                        {
                                            decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                            DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                            untPrice = oHelper.CDEC(rPrice["ATTRIBUTE_VALUE"].ToString());
                                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0)
                                            {
                                                untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                            }

                                        }

                                    }
                                    //if(rPrice["ATTRIBUTE_NAME"].ToString().Length==5)
                                    //CellAttrVal.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + oHelper.FixDecPlace(untPrice).ToString();
                                    CellAttrVal.Text = ": " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + oHelper.FixDecPlace(untPrice).ToString();


                                }
                            }
                            //CellAttrVal.Text = rPrice["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>");
                            //CellAttrVal.Text = ":&nbsp;" + oHelper.FixDecPlace(untPrice).ToString();
                            CellAttrName.CssClass = _HeaderCssClass;
                            CellAttrVal.CssClass = _CssClass;
                            // CellAttrName.Text = CellAttrName.Text + "&nbsp;&nbsp;&nbsp;&nbsp;";
                            RowAttr.Cells.Add(CellAttrName);
                            RowAttr.Cells.Add(CellAttrVal);
                            tblProdPrice.Rows.Add(RowAttr);
                        }
                        dsPrice = null;
                        Controls.Add(tblProdPrice);
                    }
                
            }
                 
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; oErr.CreateLog();
            }

        }

    }



    #endregion  
}
