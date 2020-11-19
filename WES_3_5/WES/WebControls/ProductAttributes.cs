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
using System;

/// <summary>
///This web control is used to get all the Product Attributes and its its Values.
/// </summary>

[assembly: TagPrefix("TradingBell.WebServices.UI", "WebCat")]
[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace TradingBell.WebServices.UI
{
    public class ProductAttributes : WebControl
    {
        #region "Declarations.." 
        

        Helper oHelper;
        ErrorHandler oErr;
        Product oProduct;
        int _ProductID;
        int Pid;
        int _UserId;
        int _CellWidth;
        string _Skin;
        int _CatalogID;
        public enum DispayType
        {
            SpecificationType = 1,
           // DescriptionType = 2,
            PriceType = 3,
            CustomPrice =4
        }
        string _HeaderCssClass;
        string _CssClass;
        Decimal _DiscountPrice;
        string _CusPriceAttrName;
        decimal _CustomPriceValue;

        Table tblFAttr = new Table();
        DataSet dsFam = new DataSet();
        DispayType _DisplayType;
        #endregion
        #region "Property"
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
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public decimal DiscountPrice
        {
            get
            {
                return _DiscountPrice;
            }
            set
            {
                _DiscountPrice = value;
            }

        }

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
        [Browsable(true),
        Category("TradingBell"),
        DefaultValue("")
        ]
        public DispayType Display
        {
            get
            {
                return _DisplayType;
            }
            set
            {
                _DisplayType = value;
            }
        }
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute Header CssClass")
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
        [
        Browsable(true),
        Category("TradingBell"),
        DefaultValue(""),
        Description("Attribute value CssClass")
        ]
        public override string CssClass
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
        public override string SkinID
        {

            get
            {
                return _Skin;
            }
            set
            {
                _Skin = value;
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
       public decimal  CustomPriceValue
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

        protected override void RenderContents(HtmlTextWriter output)
        {

            if (DesignMode)
            {
                Image oImg = new Image();
                oImg.ImageUrl = "~/Images/ProductAttributes.gif";
                oImg.ID = "imgDefault";
                oImg.Width = 125;
                this.Controls.Add(oImg);
                oImg.RenderControl(output);
            }
            else
            {
                oHelper = new Helper();

                if (_ProductID !=0)
                {
                    Pid = oHelper.CI(_ProductID.ToString());
                }
                if (_DisplayType == DispayType.SpecificationType)
                {
                    LoadProductSpecAttributes(output);
                }
                //else if (_DisplayType == DispayType.DescriptionType)
                //{
                //    LoadProdDescAttributes(output);
                //}
                else if (_DisplayType == DispayType.PriceType)
                {
                    LoadProdPriceAttributes(output);
                }
                else if (_DisplayType == DispayType.CustomPrice)
                {
                    LoadCustomPrice(output);
                }
            }

        }

        //#region "Functions.."
        #region "Functions"
        public void LoadProductSpecAttributes(HtmlTextWriter oWriter)
        {

            //Load Core Attributes.
            oErr = new ErrorHandler();
            oProduct = new Product();
            //ProductAttribute oAttribute = new ProductAttribute();
           // DataSet dsCoreAttrName = new DataSet();
            //DataSet dsCoreAttrVal = new DataSet();
            DataSet dsSpec = new DataSet();
            Table TblProd = new Table();
            TblProd.ID = "TblCoreAttribute";
            TblProd.CellPadding = 0;
            TblProd.CellSpacing = 5;
          //  dsCoreAttrName = oProduct.GetCoreAttributesName();
           // dsCoreAttrVal = oProduct.GetProductList(_ProductID, dsCoreAttrName.Tables[0].Rows[0].ItemArray[0].ToString(), dsCoreAttrName.Tables[0].Rows[1].ItemArray[0].ToString(), dsCoreAttrName.Tables[0].Rows[2].ItemArray[0].ToString());
           // if (dsCoreAttrVal != null)
            dsSpec = oProduct.GetProductsAllAttributesValues(Pid,_CatalogID);
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
                    //TblProd.BorderWidth = 2;
                    CellAttrName.Width = _CellWidth;
                    CellAttrName.Font.Bold = true;
                    CellAttrVal.Font.Bold = false;                  
                    CellAttrName.Text = rSpec["ATTRIBUTE_NAME"].ToString().ToUpper();
                    CellAttrVal.Text = ":&nbsp;" + rSpec["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>\t\t") + "\n\n";
                    RowAttr.Cells.Add(CellAttrName);
                    RowAttr.Cells.Add(CellAttrVal);                    
                    CellAttrName.CssClass = _HeaderCssClass;
                    CellAttrVal.CssClass = _CssClass;
                    TblProd.SkinID = _Skin;
                   // RowAttr.BorderWidth = 2;
                    //CellAttrVal.BorderWidth = 2;
                    //CellAttrName.BorderWidth = 2;
                    TblProd.Rows.Add(RowAttr);
                   
                }
                dsSpec = null;
            }
            TblProd.RenderControl(oWriter);     
        }
        //public void LoadProdDescAttributes(HtmlTextWriter oWriter)
        //{
        //    Table tblProdDesc = new Table();
        //    DataSet dsDesc = new DataSet();
        //    oProduct = new Product();

        //    tblProdDesc.ID = "TblDescAttribute";
        //    tblProdDesc.CellPadding = 0;
        //    tblProdDesc.CellSpacing = 3;
        //    dsDesc = oProduct.GetProductDescValues(_ProductID, true);
        //    if (dsDesc != null)
        //    {
        //        foreach (DataRow rDesc in dsDesc.Tables[0].Rows)
        //        {
        //            TableRow RowAttrName = new TableRow();
        //            TableRow RowAttrValue = new TableRow();
        //            TableCell CellAttrName = new TableCell();
        //            TableCell CellAttrVal = new TableCell();
        //            CellAttrName.Text = rDesc["ATTRIBUTE_NAME"].ToString();
        //            CellAttrVal.Text = rDesc["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>");
        //            CellAttrName.CssClass = _HeaderCssClass;
        //            CellAttrVal.CssClass = _CssClass;
        //            RowAttrName.Cells.Add(CellAttrName);
        //            RowAttrValue.Cells.Add(CellAttrVal);
        //            tblProdDesc.Rows.Add(RowAttrName);
        //            tblProdDesc.Rows.Add(RowAttrValue);
        //        }
        //        dsDesc = null;
        //        //Controls.Add(tblProdDesc);
        //        tblProdDesc.RenderControl(oWriter);
        //    }

        //}
        //Modified as per New Database on may 9 2007
        public void LoadProdPriceAttributes(HtmlTextWriter oWriter)
        {
            if (!DesignMode)
            {
                Helper oHelper = new Helper();
                ProductPromotion oProdPro = new ProductPromotion();
                BuyerGroup oBG = new BuyerGroup();
                Table tblProdPrice = new Table();
                DataSet dsPrice = new DataSet();
                DataSet dsBgDisc = null;
                oProduct = new Product();
                //tblProdPrice.BorderWidth = 2;
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
                        //  if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
                        if (_UserId != 0)
                        {
                            //Check the user buyer group and get the price attribute id.
                            BuyerGroupAttrID = oBG.GetBuyerGroupPriceID(_UserId);
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
                                        _DiscountPrice = 0;
                                        decimal DiscPrice = oHelper.CDEC(oProdPro.GetProductPromotionDiscValue(ProductID));
                                        untPrice = oHelper.CDEC(oProduct.GetProductBasePrice(ProductID));
                                        DiscPrice = (untPrice * DiscPrice) / 100;
                                        untPrice = untPrice - DiscPrice;
                                        untPrice = oHelper.CDEC(untPrice.ToString("N2"));
                                        CellAttrName.Text = "DISCOUNT PRICE ";
                                        _DiscountPrice = untPrice;
                                        //CellAttrVal.Text = ":&nbsp;" + oHelper.FixDecPlace(untPrice).ToString();
                                        CellAttrVal.Text = ": " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + oHelper.FixDecPlace(untPrice).ToString();

                                    }
                                    else
                                    {
                                        CellAttrName.Text = "&nbsp;&nbsp;" + rPrice["ATTRIBUTE_NAME"].ToString();
                                        BGName = oBG.GetBuyerGroup(_UserId);
                                        dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = oHelper.CDEC(rPrice["ATTRIBUTE_VALUE"].ToString());
                                                bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UserId).ToString());
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                                {
                                                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                                }
                                            }
                                        }
                                        //CellAttrVal.Text = "&nbsp;:&nbsp;" + oHelper.FixDecPlace(untPrice).ToString();
                                        CellAttrVal.Text = ": " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + oHelper.FixDecPlace(untPrice).ToString();

                                    }
                                }
                                //CellAttrVal.Text = rPrice["ATTRIBUTE_VALUE"].ToString().Replace("\r\n", "<br/>");
                                CellAttrName.CssClass = _HeaderCssClass;
                                CellAttrVal.CssClass = _CssClass;
                                RowAttr.Cells.Add(CellAttrName);
                                //RowAttr.BorderWidth = 2;
                                // CellAttrName.BorderWidth = 2;
                                //CellAttrVal.BorderWidth = 2;
                                RowAttr.Cells.Add(CellAttrVal);
                                tblProdPrice.SkinID = _Skin;
                                tblProdPrice.Rows.Add(RowAttr);
                            }
                            dsPrice = null;
                            tblProdPrice.RenderControl(oWriter);
                            // Controls.Add(tblProdPrice);
                        }
                    }//End Try
                catch (Exception ex)
                {
                    oErr.ErrorMsg = ex; // oErr.CreateLog();
                }

            }

        }
        public void LoadCustomPrice(HtmlTextWriter oWriter)
        {
            try
            {
                Table tblCustPrice = new Table();
                TableRow RowAttr = new TableRow();
                TableCell CellAttrName = new TableCell();
                TableCell CellAttrVal = new TableCell();
                CellAttrName.Width = _CellWidth;
                CellAttrName.Text = _CusPriceAttrName + "&nbsp;";
                CellAttrVal.Text = ":" + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() +  oHelper.CDEC(oHelper.FixDecPlace( _CustomPriceValue));
                CellAttrName.CssClass = _HeaderCssClass;
                CellAttrVal.CssClass = _CssClass;
                RowAttr.Cells.Add(CellAttrName);
                RowAttr.Cells.Add(CellAttrVal);
                tblCustPrice.Rows.Add(RowAttr);

                tblCustPrice.RenderControl(oWriter);   
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex; // oErr.CreateLog();
            }
        }

        #endregion

    }
}
